using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of posts
    /// </summary>
    [ValidateInput(false)]
    public class admin_postsController : Controller
    {
        #region View methods

        // Get the list of posts
        // GET: /admin_posts/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.AdminSession = true;
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_posts/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_posts?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_posts/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "/admin_posts")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" &&
            (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Add data to the view
            ViewBag.Keywords = "";
            ViewBag.CurrentPage = 1;
            ViewBag.Post = post;
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Make sure that post not is null
            if (ViewBag.Post == null)
            {
                // Create a new empty post
                ViewBag.Post = new Post();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_posts/translate/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "/admin_posts")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post standardPost = Post.GetOneById(id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" &&
                (standardPost == null || standardPost.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.Keywords = "";
            ViewBag.CurrentPage = 1;
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.StandardPost = standardPost;
            ViewBag.TranslatedPost = Post.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardPost != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect(returnUrl);
            }

        } // End of the translate method

        // Get the files form
        // GET: /admin_posts/files
        [HttpGet]
        public ActionResult files(Int32 id = 0, string returnUrl = "/admin_posts")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.Post = post;
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Post != null)
            {
                return View("files");
            }
            else
            {
                return Redirect(returnUrl);
            }

        } // End of the files method

        #endregion

        #region Post methods

        // Update the post
        // POST: /admin_posts/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            Int32 category_id = Convert.ToInt32(collection["selectCategory"]);
            Int32 administrator_id = Convert.ToInt32(collection["hiddenAdministratorId"]);
            string meta_robots = collection["selectMetaRobots"];
            string title = collection["txtTitle"];
            string main_content = collection["txtDescription"];
            string meta_description = collection["txtMetaDescription"];
            string meta_keywords = collection["txtMetaKeywords"];
            string page_name = collection["txtPageName"];
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtDateAdded"], out date_added);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);
            string keywords = collection["txtSearch"];
            Int32 currentPage = Convert.ToInt32(collection["hiddenPage"]);
            string returnUrl = collection["returnUrl"];

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Check if the post exists
            if (post == null)
            {
                // Create a new post
                post = new Post();
            }

            // Set values for the post
            post.category_id = category_id;
            post.administrator_id = administrator_id;
            post.meta_robots = meta_robots;
            post.title = title;
            post.main_content = main_content;
            post.meta_description = meta_description;
            post.meta_keywords = meta_keywords;
            post.page_name = page_name;
            post.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            post.date_updated = DateTime.Now;
            post.inactive = inactive;

            // Check if the user wants to do a search
            if (collection["btnSearch"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage;
                ViewBag.Post = post;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Check if the user wants to do a search
            if (collection["btnPreviousPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage - 1;
                ViewBag.Post = post;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Check if the user wants to do a search
            if (collection["btnNextPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage + 1;
                ViewBag.Post = post;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Create a error message
            string errorMessage = "";

            // Get a post on page name
            Post postOnPageName = Post.GetOneByPageName(post.page_name, currentDomain.back_end_language);

            // Check for errors
            if (postOnPageName != null && post.id != postOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (post.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(post.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (post.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (post.category_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("category").ToLower()) + "<br/>";
            }
            if (post.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (post.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (post.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Check if we should add or update the post
                if (post.id != 0)
                {
                    // Update the post
                    Post.UpdateMasterPost(post);
                    Post.UpdateLanguagePost(post, currentDomain.back_end_language);
                }
                else
                {
                    // Add the post
                    Int64 insertId = Post.AddMasterPost(post);
                    post.id = Convert.ToInt32(insertId);
                    Post.AddLanguagePost(post, currentDomain.back_end_language);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage;
                ViewBag.Post = post;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the post
        // POST: /admin_posts/translate
        [HttpPost]
        public ActionResult translate(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 post_id = Convert.ToInt32(collection["hiddenPostId"]);
            string title = collection["txtTranslatedTitle"];
            string description = collection["txtTranslatedDescription"];
            string meta_description = collection["txtTranslatedMetadescription"];
            string meta_keywords = collection["txtTranslatedMetakeywords"];
            string page_name = collection["txtTranslatedPagename"];
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtTranslatedDateAdded"], out date_added);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);
            string returnUrl = collection["returnUrl"];
            string keywords = collection["txtSearch"];
            Int32 currentPage = Convert.ToInt32(collection["hiddenPage"]);

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post standardPost = Post.GetOneById(post_id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" &&
                (standardPost == null || standardPost.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Create the translated post
            Post translatedPost = new Post();
            translatedPost.id = post_id;
            translatedPost.title = title;
            translatedPost.main_content = description;
            translatedPost.meta_description = meta_description;
            translatedPost.meta_keywords = meta_keywords;
            translatedPost.page_name = page_name;
            translatedPost.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            translatedPost.date_updated = DateTime.Now;
            translatedPost.inactive = inactive;

            // Check if the user wants to do a search
            if (collection["btnSearch"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
                ViewBag.StandardPost = standardPost;
                ViewBag.TranslatedPost = translatedPost;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Check if the user wants to do a search
            if (collection["btnPreviousPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage - 1;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
                ViewBag.StandardPost = standardPost;
                ViewBag.TranslatedPost = translatedPost;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Check if the user wants to do a search
            if (collection["btnNextPage"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage + 1;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
                ViewBag.StandardPost = standardPost;
                ViewBag.TranslatedPost = translatedPost;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Create a error message
            string errorMessage = string.Empty;

            // Get a post on page name
            Post postOnPageName = Post.GetOneByPageName(translatedPost.page_name, translationLanguageId);

            // Check for errors
            if (postOnPageName != null && translatedPost.id != postOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedPost.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(translatedPost.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedPost.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (translatedPost.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (translatedPost.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (translatedPost.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == "")
            {
                // Get the saved post
                Post post = Post.GetOneById(post_id, translationLanguageId);

                if (post == null)
                {
                    // Add a new translated post
                    Post.AddLanguagePost(translatedPost, translationLanguageId);
                }
                else
                {
                    // Update values for the saved post
                    post.title = translatedPost.title;
                    post.main_content = translatedPost.main_content;
                    post.meta_description = translatedPost.meta_description;
                    post.meta_keywords = translatedPost.meta_keywords;
                    post.page_name = translatedPost.page_name;
                    post.date_added = translatedPost.date_added;
                    post.date_updated = translatedPost.date_updated;
                    post.inactive = translatedPost.inactive;

                    // Update the post translation
                    Post.UpdateLanguagePost(post, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage;
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
                ViewBag.StandardPost = standardPost;
                ViewBag.TranslatedPost = translatedPost;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Add files
        // POST: /admin_posts/add_file
        [HttpPost]
        public ActionResult add_file(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get form values
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            Int32 post_id = Convert.ToInt32(collection["txtId"]);
            string title = collection["txtFileTitle"];
            string returnUrl = collection["returnUrl"];
            HttpPostedFileBase uploadedFile = Request.Files["uploadFile"];

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(post_id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" &&
                (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Create the directory string
            string filesDirectory = "/Content/posts/" + (post_id / 100).ToString() + "/" + post_id.ToString() + "/" + language_id.ToString() + "/files/";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(filesDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(filesDirectory));
            }

            // Set the file path
            string filePath = filesDirectory + System.IO.Path.GetFileName(uploadedFile.FileName);

            // Create a new post file
            PostFile postFile = new PostFile();
            postFile.post_id = post_id;
            postFile.language_id = language_id;
            postFile.title = title;
            postFile.src = filePath;

            // Add the post file
            if(uploadedFile.ContentLength > 0)
            {
                PostFile.Add(postFile);
                uploadedFile.SaveAs(Server.MapPath(filePath));
            }

            // Return the files view
            return RedirectToAction("files", new { id = post_id, returnUrl = returnUrl, lang = language_id });

        } // End of the add_file method

        // Update or delete a file
        // POST: /admin_posts/update_file
        [HttpPost]
        public ActionResult update_file(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get form values
            Int32 id = Convert.ToInt32(collection["hiddenId"]);
            Int32 post_id = Convert.ToInt32(collection["hiddenPostId"]);
            Int32 language_id = Convert.ToInt32(collection["hiddenLanguageId"]);
            string title = collection["txtUpdateFileTitle"];
            string returnUrl = collection["returnUrl"];

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(post_id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the saved post file
            PostFile postFile = PostFile.GetOneById(id);

            // Make sure that the post file not is null
            if(postFile == null)
            {
                // Return the files view
                return RedirectToAction("files", new { id = post_id, returnUrl = returnUrl, lang = language_id });
            }

            // Check if we should update or delete the file
            if(collection["btnUpdate"] != null)
            {
                // Update the title
                postFile.title = title;

                // Update the post file
                PostFile.Update(postFile);
            }
            else
            {
                // Create the directory string
                string filesDirectory = "/Content/posts/" + (post_id / 100).ToString() + "/" + post_id.ToString() + "/" + language_id.ToString() + "/files/";

                // Set the filepath
                string filePath = Server.MapPath(filesDirectory + System.IO.Path.GetFileName(postFile.src));

                // Delete the post file
                PostFile.DeleteOnId(postFile.id);

                // Check if the file exists
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            
            // Return the files view
            return RedirectToAction("files", new { id = post_id, returnUrl = returnUrl, lang = language_id });

        } // End of the delete_file method

        // Delete the post
        // POST: /admin_posts/delete/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "/admin_posts")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post
            Post post = Post.GetOneById(id, currentDomain.back_end_language);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (post == null || post.administrator_id == administrator.id))
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the full post or just the translation
            if (languageId == 0 || languageId == currentDomain.back_end_language)
            {
                // Delete the post and all the posts connected to this post (CASCADE)
                errorCode = Post.DeleteOnId(id);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all the files
                DeleteAllFiles(id);
            }
            else
            {
                // Delete the translated post
                errorCode = Post.DeleteLanguagePostOnId(id, languageId);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all the language files
                DeleteLanguageFiles(id, languageId);
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Delete all the files for the post
        /// </summary>
        /// <param name="postId">The post id</param>
        private void DeleteAllFiles(Int32 postId)
        {
            // Define the directory url for a post
            string postDirectory = Server.MapPath("/Content/posts/" + (postId / 100).ToString() + "/" + postId.ToString());

            // Delete post files
            PostFile.DeleteOnPostId(postId);

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(postDirectory))
            {
                System.IO.Directory.Delete(postDirectory, true);
            }

        } // End of the DeleteAllFiles method

        /// <summary>
        /// Delete all the files for the language and the post
        /// </summary>
        /// <param name="postId">The post id</param>
        /// <param name="languageId">The language id</param>
        private void DeleteLanguageFiles(Int32 postId, Int32 languageId)
        {
            // Define the directory url for product images
            string postDirectory = Server.MapPath("/Content/posts/" + (postId / 100).ToString() + "/" + postId.ToString() + "/" + languageId.ToString());

            // Delete post files
            PostFile.DeleteOnPostId(postId, languageId);

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(postDirectory))
            {
                System.IO.Directory.Delete(postDirectory, true);
            }

        } // End of the DeleteLanguageFiles method

        #endregion

    } // End of the class

} // End of the namespace