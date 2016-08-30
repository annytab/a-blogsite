using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls user interaction
    /// </summary>
    public class userController : Controller
    {
        #region View methods

        // Get the user start page (my pages)
        // GET: /user/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the signed-in administrator
            Administrator user = Administrator.GetSignedInAdministrator(currentDomain.front_end_language);

            // Check if the user is signed in
            if (user == null)
            {
                user = Administrator.GetSignedInAdministrator();
                if(user == null)
                {
                    return RedirectToAction("login", "user");
                } 
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.User = user;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/user_start_page.cshtml");

        } // End of the index method

        // Get the user login page
        // GET: /user/login
        [HttpGet]
        public ActionResult login()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("log_in"), "/user/login"));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.User = new Administrator();
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/user_login.cshtml");

        } // End of the login method

        // Redirect the user to the facebook login
        // GET: /user/facebook_login
        [HttpGet]
        public ActionResult facebook_login()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Create a random state
            string state = Tools.GeneratePassword();
            Session["FacebookState"] = state;

            // Create the url
            string url = "https://www.facebook.com/dialog/oauth?client_id=" + domain.facebook_app_id + "&state=" + state
                + "&response_type=code&redirect_uri=" + Server.UrlEncode(domain.web_address + "/user/facebook_login_callback");

            // Redirect the user
            return Redirect(url);

        } // End of the facebook_login method

        // Login the user with facebook
        // GET: /user/facebook_login_callback
        [HttpGet]
        public async Task<ActionResult> facebook_login_callback()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the state
            string state = "";
            if (Request.Params["state"] != null)
            {
                state = Server.UrlDecode(Request.Params["state"]);
            }

            // Get the state stored in the session
            string sessionState = "";
            if (Session["FacebookState"] != null)
            {
                sessionState = Session["FacebookState"].ToString();
            }

            // Get the code
            string code = "";
            if (Request.Params["code"] != null)
            {
                code = Server.UrlDecode(Request.Params["code"]);
            }

            // Make sure that the callback is valid
            if (state != sessionState || code == "")
            {
                return Redirect("/");
            }

            // Get website settings
            KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();
            string redirectHttps = websiteSettings.Get("REDIRECT-HTTPS");

            // Get the access token
            string access_token = await AnnytabExternalLogin.GetFacebookAccessToken(domain, code);

            // Get the facebook user
            Dictionary<string, object> facebookUser = await AnnytabExternalLogin.GetFacebookUser(domain, access_token);
                
            // Get the facebook data
            string facebookId = facebookUser.ContainsKey("id") == true ? facebookUser["id"].ToString() : "";
            string facebookName = facebookUser.ContainsKey("name") == true ? facebookUser["name"].ToString() : "";
            string facebookEmail = facebookId + "_facebook";

            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if the user exists or not
            if (facebookId != "" && user != null)
            {
                // Update the user
                user.facebook_user_id = facebookId;
                Administrator.UpdateMasterPost(user);

                // Redirect the user to his start page
                return RedirectToAction("index", "user");
            }
            else if (facebookId != "" && user == null)
            {
                // Check if we can find a user with the facebook id
                user = Administrator.GetOneByFacebookUserId(facebookId);

                // Check if the user exists
                if (user == null)
                {
                    // Create a new administrator
                    user = new Administrator();
                    user.admin_user_name = facebookEmail;
                    user.admin_password = PasswordHash.CreateHash(Tools.GeneratePassword());
                    user.admin_role = "User";
                    user.author_name = "-";
                    user.facebook_user_id = facebookId;

                    // Add the new Administrator
                    Int64 insertId = Administrator.AddMasterPost(user);
                    user.id = Convert.ToInt32(insertId);
                    Administrator.AddLanguagePost(user, domain.front_end_language);
                    Administrator.UpdatePassword(user.id, PasswordHash.CreateHash(user.admin_password));

                    // Create the administrator cookie
                    HttpCookie adminCookie = new HttpCookie("Administrator");
                    adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                    adminCookie.HttpOnly = true;
                    adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                    Response.Cookies.Add(adminCookie);

                    // Redirect the user to the edit user page
                    return Redirect("/user/edit");
                }
                else
                {
                    // Create the administrator cookie
                    HttpCookie adminCookie = new HttpCookie("Administrator");
                    adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                    adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                    adminCookie.HttpOnly = true;
                    adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(adminCookie);

                    // Redirect the user to the start page
                    return RedirectToAction("index");
                }
            }
            else
            {
                // Redirect the user to the login
                return RedirectToAction("login", "user");
            }

        } // End of the facebook_login_callback method

        // Redirect the user to the google login
        // GET: /user/google_login
        [HttpGet]
        public ActionResult google_login()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Create a random state
            string state = Tools.GeneratePassword();
            Session["GoogleState"] = state;

            // Create the url
            string url = "https://accounts.google.com/o/oauth2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fplus.me&state=" + state
                + "&redirect_uri=" + Server.UrlEncode(domain.web_address + "/user/google_login_callback") + "&response_type=code&client_id=" + domain.google_app_id
                + "&access_type=offline";

            // Redirect the user
            return Redirect(url);

        } // End of the google_login method

        // Handle the google login callback
        // GET: /user/google_login_callback
        [HttpGet]
        public async Task<ActionResult> google_login_callback()
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the state
            string state = "";
            if (Request.Params["state"] != null)
            {
                state = Server.UrlDecode(Request.Params["state"]);
            }

            // Get the state stored in the session
            string sessionState = "";
            if(Session["GoogleState"] != null)
            {
                sessionState = Session["GoogleState"].ToString();
            }

            // Get the code
            string code = "";
            if (Request.Params["code"] != null)
            {
                code = Server.UrlDecode(Request.Params["code"]);
            }

             // Check if this is a valid callback
            if (state != sessionState || code == "")
            {
                // Redirect the user
                return Redirect("/");
            }

            // Get website settings
            KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();
            string redirectHttps = websiteSettings.Get("REDIRECT-HTTPS");

            // Get the access token
            string access_token = await AnnytabExternalLogin.GetGoogleAccessToken(domain, code);

            // Get the google user
            Dictionary<string, object> googleUser = await AnnytabExternalLogin.GetGoogleUser(domain, access_token);

            // Get the google data
            string googleId = googleUser.ContainsKey("id") == true ? googleUser["id"].ToString() : "";
            string googleName = googleUser.ContainsKey("displayName") == true ? googleUser["displayName"].ToString() : "";

            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if the user exists or not
            if (googleId != "" && user != null)
            {
                // Update the user
                user.google_user_id = googleId;
                Administrator.UpdateMasterPost(user);

                // Redirect the user to his start page
                return RedirectToAction("index", "user");
            }
            else if (googleId != "" && user == null)
            {
                // Check if we can find a user with the google id
                user = Administrator.GetOneByGoogleUserId(googleId);

                // Check if the user exists
                if (user == null)
                {
                    // Create a new administrator
                    user = new Administrator();
                    user.admin_user_name = googleId + "_google";
                    user.admin_password = PasswordHash.CreateHash(Tools.GeneratePassword());
                    user.admin_role = "User";
                    user.author_name = "-";
                    user.google_user_id = googleId;

                    // Add the new Administrator
                    Int64 insertId = Administrator.AddMasterPost(user);
                    user.id = Convert.ToInt32(insertId);
                    Administrator.AddLanguagePost(user, domain.front_end_language);
                    Administrator.UpdatePassword(user.id, PasswordHash.CreateHash(user.admin_password));

                    // Create the administrator cookie
                    HttpCookie adminCookie = new HttpCookie("Administrator");
                    adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                    adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                    adminCookie.HttpOnly = true;
                    adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(adminCookie);

                    // Redirect the user to the edit user page
                    return Redirect("/user/edit");
                }
                else
                {
                    // Create the administrator cookie
                    HttpCookie adminCookie = new HttpCookie("Administrator");
                    adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                    adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                    adminCookie.HttpOnly = true;
                    adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(adminCookie);

                    // Redirect the user to the start page
                    return RedirectToAction("index");
                }
            }
            else
            {
                // Redirect the user to the login
                return RedirectToAction("login", "user");
            }

        } // End of the google_login_callback method

        // Get the user forgot password page
        // GET: /user/forgot_password
        [HttpGet]
        public ActionResult forgot_password()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("forgot") + " " + tt.Get("password"), "/user/forgot_password"));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.User = new Administrator();
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/forgot_password.cshtml");

        } // End of the forgot_password method

        // Get the edit form
        // GET: /user/edit
        [HttpGet]
        public ActionResult edit()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Create the user
            Administrator user = Administrator.GetSignedInAdministrator(currentDomain.front_end_language);

            // Check if the user exists but not are translated
            if(user == null)
            {
                user = Administrator.GetSignedInAdministrator();
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("user_details").ToLower(), "/user/edit"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.User = user;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Create a new empty user if the user does not exist
            if (ViewBag.User == null)
            {
                // Add data to the view
                ViewBag.User = new Administrator();
            }

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_user_details.cshtml");

        } // End of the edit method

        // Get the edit comments form
        // GET: /user/edit_comments
        [HttpGet]
        public ActionResult edit_comments()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if there is a signed in user
            if (user == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("comments").ToLower(), "/user/edit_comments"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.User = user;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_user_comments.cshtml");

        } // End of the edit_comments form

        // Get the edit ratings form
        // GET: /user/edit_ratings
        [HttpGet]
        public ActionResult edit_ratings()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if there is a signed in user
            if (user == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("ratings").ToLower(), "/user/edit_ratings"));

            // Add data to the view
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.TranslatedTexts = tt;
            ViewBag.User = user;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/edit_user_ratings.cshtml");

        } // End of the edit_ratings form


        #endregion

        #region Post methods

        // Update user details
        // POST: /user/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string user_name = collection["txtUserName"];
            string password = collection["txtPassword"];
            string email = collection["txtEmail"];
            string author_name = collection["txtAuthorName"];
            string author_description = collection["txtAuthorDescription"];
            HttpPostedFileBase authorImage = Request.Files["uploadMainImage"];

            // Modify the author description
            author_description = author_description.Replace(Environment.NewLine, "<br />");

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Get the user
            Administrator user = Administrator.GetOneById(id, domain.front_end_language);

            // Check if the user exists
            if (user == null)
            {
                // Check if the user exists but not are translated
                user = Administrator.GetOneById(id);
                if(user == null)
                {
                    // Create an empty user
                    user = new Administrator();
                }
            }

            // Update values
            user.admin_user_name = user_name;
            user.email = email;
            user.author_name = author_name;
            user.author_description = author_description;

            // Create a error message
            string errorMessage = string.Empty;

            // Get the user on user name
            Administrator userOnUserName = Administrator.GetOneByUserName(user.admin_user_name);

            // Check for errors
            if (userOnUserName != null && user.id != userOnUserName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_unique"), tt.Get("user_name")) + "<br/>";
            }
            if (user.admin_user_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("user_name"), "50") + "<br/>";
            }
            if (user.author_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (AnnytabDataValidation.IsEmailAddressValid(user.email) == null)
            {
                errorMessage += "&#149; " + tt.Get("error_email_valid") + "<br/>";
            }
            if (authorImage.ContentLength > 0 && Tools.IsImageJpeg(authorImage) == false)
            {
                errorMessage += "&#149; " + tt.Get("error_invalid_jpeg") + "<br/>";
            }
            if (authorImage.ContentLength > 262144)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_image_size"), "256 kb") + "<br/>"; ;
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the user
                if (user.id == 0)
                {
                    // Add the user
                    user.admin_role = "User";
                    Int64 insertId = Administrator.AddMasterPost(user);
                    user.id = Convert.ToInt32(insertId);
                    Administrator.AddLanguagePost(user, domain.front_end_language);
                    Administrator.UpdatePassword(user.id, PasswordHash.CreateHash(password));

                    // Get website settings
                    KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();
                    string redirectHttps = websiteSettings.Get("REDIRECT-HTTPS");

                    // Create the administrator cookie
                    HttpCookie adminCookie = new HttpCookie("Administrator");
                    adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                    adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                    adminCookie.HttpOnly = true;
                    adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                    Response.Cookies.Add(adminCookie);
                }
                else
                {
                    // Update the user
                    Administrator.UpdateMasterPost(user);

                    // Update or add the language post
                    if (Administrator.GetOneById(id, domain.front_end_language) != null)
                    {
                        Administrator.UpdateLanguagePost(user, domain.front_end_language);
                    }
                    else
                    {
                        Administrator.AddLanguagePost(user, domain.front_end_language);
                    }
                    

                    // Only update the password if it has changed
                    if (password != "")
                    {
                        Administrator.UpdatePassword(user.id, PasswordHash.CreateHash(password));
                    }
                }

                // Update the image
                if (authorImage.ContentLength > 0)
                {
                    UpdateImage(user.id, authorImage);
                }

                // Redirect the user to the start page
                return RedirectToAction("index");
            }
            else
            {
                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
                breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("edit") + " " + tt.Get("user_details").ToLower(), "/user/edit"));

                // Set form values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.CurrentCategory = new Category();
                ViewBag.CurrentDomain = domain;
                ViewBag.CurrentLanguage = Language.GetOneById(domain.front_end_language);
                ViewBag.TranslatedTexts = tt;
                ViewBag.User = user;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the edit view
                return domain.custom_theme_id == 0 ? View("edit") : View("/Views/theme/edit_user_details.cshtml");
            }

        } // End of the edit method

        // Add a comment
        // POST: /user/add_comment
        [HttpPost]
        public ActionResult add_comment(FormCollection collection)
        {
            // Make sure that the user is signed in
            Administrator user = Administrator.GetSignedInAdministrator();

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Check if the post request is valid
            if (user == null || collection == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the form data
            Int32 postId = Convert.ToInt32(collection["hiddenPostId"]);
            Int32 languageId = Convert.ToInt32(collection["hiddenLanguageId"]);
            string commentText = collection["txtCommentText"];

            // Get the post
            Post post = Post.GetOneById(postId, languageId);

            // Modify the comment text
            commentText = commentText.Replace(Environment.NewLine, "<br />");

            // Create a new post comment
            PostComment comment = new PostComment();
            comment.post_id = postId;
            comment.administrator_id = user.id;
            comment.language_id = languageId;
            comment.comment_date = DateTime.UtcNow;
            comment.comment_text = commentText;

            // Add the post comment
            Int64 insertId = PostComment.Add(comment);

            // Send a email to the administrator of the website
            string subject = tt.Get("comment") + " - " + domain.website_name;
            string message = tt.Get("post") + ": " + insertId.ToString() + "<br />"
                + tt.Get("language") + ": " + comment.language_id.ToString() + "<br />"
                + tt.Get("user_name") + ": " + user.admin_user_name + "<br /><br />" 
                + comment.comment_text;
            Tools.SendEmailToHost("", subject, message);

            // Redirect the user to the post
            return Redirect("/home/post/" + post.page_name + "#comments");

        } // End of the add_comment method

        // Edit a comment
        // POST: /user/edit_comment
        [HttpPost]
        public ActionResult edit_comment(FormCollection collection)
        {
            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if the user is signed in
            if (user == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Get form values
            Int32 commentId = Convert.ToInt32(collection["hiddenId"]);
            Int32 languageId = Convert.ToInt32(collection["hiddenLanguageId"]);
            string commentText = collection["txtCommentText"];

            // Modify the comment text
            commentText = commentText.Replace(Environment.NewLine, "<br />");

            // Get the comment
            PostComment comment = PostComment.GetOneById(commentId);

            // Get the post
            Post post = Post.GetOneById(comment.post_id, languageId);

            // Update the comment
            if(comment != null && comment.administrator_id == user.id)
            {
                // Update values
                comment.comment_date = DateTime.UtcNow;
                comment.comment_text = commentText;

                // Update the comment
                PostComment.Update(comment);

                // Send a email to the administrator of the website
                string subject = tt.Get("comment") + " - " + domain.website_name;
                string message = tt.Get("post") + ": " + comment.post_id.ToString() + "<br />"
                    + tt.Get("language") + ": " + comment.language_id.ToString() + "<br />"
                    + tt.Get("user_name") + ": " + user.admin_user_name + "<br /><br />" 
                    + comment.comment_text;
                Tools.SendEmailToHost("", subject, message);
            }

            // Redirect the user to the post
            return Redirect("/home/post/" + post.page_name + "#comments");

        } // End of the edit_comment method

        // Delete a comment
        // POST: /user/delete_comment
        [HttpGet]
        public ActionResult delete_comment(Int32 id = 0)
        {
            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if the request is valid
            if (user == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the comment
            PostComment comment = PostComment.GetOneById(id);

            // Delete the comment
            if (comment != null && comment.administrator_id == user.id)
            {
                // Delete the comment
                PostComment.DeleteOnId(comment.id);
            }

            // Return the edit comments view
            return RedirectToAction("edit_comments");

        } // End of the delete_comment method

        // Edit a rating
        // POST: /user/edit_rating
        [HttpPost]
        public ActionResult edit_rating(FormCollection collection)
        {
            // Make sure that the user is signed in
            Administrator user = Administrator.GetSignedInAdministrator();

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(domain.front_end_language, "id", "ASC");

            // Check if the post request is valid
            if (user == null || collection == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the form data
            Int32 post_id = Convert.ToInt32(collection["hiddenPostId"]);
            Int32 language_id = Convert.ToInt32(collection["hiddenLanguageId"]);
            decimal userVote = 0;
            decimal.TryParse(collection["userVote"], NumberStyles.Any, CultureInfo.InvariantCulture, out userVote);

            // Get the post
            Post post = Post.GetOneById(post_id, language_id);

            // Try to get a saved rating
            PostRating postRating = PostRating.GetOneById(post_id, user.id, language_id);

            // Add or update the rating
            if (postRating != null && postRating.administrator_id == user.id)
            {
                // Update values
                postRating.rating_date = DateTime.UtcNow;
                postRating.rating = userVote;

                // Update the rating
                PostRating.Update(postRating);
            }
            else
            {
                // Create a new rating
                postRating = new PostRating();

                // Update values
                postRating.post_id = post_id;
                postRating.administrator_id = user.id;
                postRating.language_id = language_id;
                postRating.rating_date = DateTime.UtcNow;
                postRating.rating = userVote;

                // Add the rating
                PostRating.Add(postRating);
            }

            // Send a email to the administrator of the website
            string subject = tt.Get("rating") + " - " + domain.website_name;
            string message = tt.Get("post") + ": " + postRating.post_id.ToString() + "<br />"
                + tt.Get("language") + ": " + postRating.language_id.ToString() + "<br />"
                + tt.Get("user_name") + ": " + user.admin_user_name + "<br />" 
                + tt.Get("rating") + ": " + postRating.rating.ToString();
            Tools.SendEmailToHost("", subject, message);

            // Update the rating for the post
            Post.UpdateRating(postRating.post_id, postRating.language_id);

            // Redirect the user to the post
            return Redirect("/home/post/" + post.page_name + "#comments");

        } // End of the edit_rating method

        // Delete a rating
        // POST: /user/delete_rating/1?languageId=1
        [HttpGet]
        public ActionResult delete_rating(Int32 id = 0, Int32 languageId = 0)
        {
            // Get the signed in user
            Administrator user = Administrator.GetSignedInAdministrator();

            // Check if the post request is valid
            if (user == null)
            {
                return RedirectToAction("login", "user");
            }

            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            // Get the rating
            PostRating postRating = PostRating.GetOneById(id, user.id, languageId);

            // Delete the rating
            if (postRating != null && postRating.administrator_id == user.id)
            {
                // Delete the rating
                PostRating.DeleteOnId(id, user.id, languageId);

                // Update the rating for the post
                Post.UpdateRating(postRating.post_id, postRating.language_id);
            }

            // Return the edit ratings view
            return RedirectToAction("edit_ratings");

        } // End of the delete_rating method

        // Sign in the user
        // POST: /user/login
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            // Get data from the form
            string returnUrl = collection["hiddenReturnUrl"];
            string user_name = collection["txtUserName"];
            string password = collection["txtPassword"];

            // Get the user
            Administrator user = Administrator.GetOneByUserName(user_name);

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Check if the user exists and if the password is correct
            if (user != null && Administrator.ValidatePassword(user_name, password) == true)
            {
                // Get website settings
                KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();
                string redirectHttps = websiteSettings.Get("REDIRECT-HTTPS");

                // Create the administrator cookie
                HttpCookie adminCookie = new HttpCookie("Administrator");
                adminCookie.Value = Tools.ProtectCookieValue(user.id.ToString(), "Administration");
                adminCookie.Expires = DateTime.UtcNow.AddDays(1);
                adminCookie.HttpOnly = true;
                adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                Response.Cookies.Add(adminCookie);

                // Redirect the user to the checkout page
                return Redirect(returnUrl);
            }
            else
            {
                // Create a new user
                user = new Administrator();
                user.admin_user_name = user_name;
                string error_message = "&#149; " + tt.Get("error_login");

                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
                breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("my_pages"), "/user"));
                breadCrumbs.Add(new BreadCrumb(tt.Get("log_in"), "/user/login"));

                // Set values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.CurrentCategory = new Category();
                ViewBag.TranslatedTexts = tt;
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.User = user;
                ViewBag.ErrorMessage = error_message;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the login view
                return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/user_login.cshtml");
            }

        } // End of the login method

        // Send an email to the user with a random password
        // GET: /user/forgot_password
        [HttpPost]
        public ActionResult forgot_password(FormCollection collection)
        {
            // Get form data
            string user_name = collection["txtUserName"];

            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Get the user
            Administrator user = Administrator.GetOneByUserName(user_name);

            // Create a random password
            string password = Tools.GeneratePassword();
            
            // Create a error message
            string error_message = "";

            // Check if the user exists
            if(user != null)
            {
                // Create the mail message
                string subject = translatedTexts.Get("forgot") + " " + translatedTexts.Get("password").ToLower() + " - " + currentDomain.website_name;
                string message = translatedTexts.Get("user_name") + ": " + user.admin_user_name + "<br />" 
                    + translatedTexts.Get("password") + ": " + password + "<br /><br />"
                    + "<a href=\"" + currentDomain.web_address + "/user/login\">" + translatedTexts.Get("log_in") + "</a><br />";

                // Try to send the email message
                if(Tools.SendEmailToUser(user.email, subject, message) == false)
                {
                    error_message += "&#149; " + translatedTexts.Get("error_send_email");
                }
            }
            else
            {
                error_message += "&#149; " + translatedTexts.Get("error_user_exists");
            }

            // Check if there is a error message
            if (error_message == "")
            {
                // Update the password
                Administrator.UpdatePassword(user.id, PasswordHash.CreateHash(password));

                // Redirect the user to the login page
                return RedirectToAction("login");
            }
            else
            {
                // Create the bread crumb list
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(3);
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("start_page"), "/"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("my_pages"), "/user"));
                breadCrumbs.Add(new BreadCrumb(translatedTexts.Get("forgot") + " " + translatedTexts.Get("password"), "/user/forgot_password"));

                // Set values
                ViewBag.BreadCrumbs = breadCrumbs;
                ViewBag.CurrentCategory = new Category();
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.CurrentDomain = currentDomain;
                ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
                ViewBag.User = new Administrator();
                ViewBag.ErrorMessage = error_message;
                ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

                // Return the view
                return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/forgot_password.cshtml");
            }

        } // End of the forgot_password method

        // Sign out the user
        // GET: /user/logout
        [HttpGet]
        public ActionResult logout()
        {
            // Delete the administrator cookie
            HttpCookie adminCookie = new HttpCookie("Administrator");
            adminCookie.Value = "";
            adminCookie.Expires = DateTime.UtcNow.AddDays(-1);
            adminCookie.HttpOnly = true;
            Response.Cookies.Add(adminCookie);

            // Redirect the user to the login page
            return RedirectToAction("index", "home");

        } // End of the logout method

        #endregion

        #region Helper methods

        /// <summary>
        /// Update the administrator image
        /// </summary>
        /// <param name="id">The administrator id</param>
        /// <param name="mainImage">The posted main image file</param>
        private void UpdateImage(Int32 id, HttpPostedFileBase mainImage)
        {
            // Create directory strings
            string directoryUrl = Server.MapPath("/Content/administrators/" + (id / 100).ToString() + "/" + id.ToString() + "/");
            string mainImageUrl = directoryUrl + "main_image.jpg";

            // Check if the directory exists
            if (System.IO.Directory.Exists(directoryUrl) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(directoryUrl);
            }

            // Save the main image
            if (mainImage != null)
            {
                mainImage.SaveAs(mainImageUrl);
            }

        } // End of the UpdateImage method

        #endregion

    } // End of the class

} // End of the namespace