using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of administrators
    /// </summary>
    [ValidateInput(false)]
    public class admin_administratorsController : Controller
    {
        #region View methods

        // Get the list of administrators
        // GET: /admin_administrators/
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
        // POST: /admin_administrators/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_administrators?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_administrators/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "/admin_administrators")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.Administrator = Administrator.GetOneById(id, adminLanguageId);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty administrator post if the administrator does not exist
            if (ViewBag.Administrator == null)
            {
                // Add data to the view
                ViewBag.Administrator = new Administrator();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the images form
        // GET: /admin_administrators/images
        [HttpGet]
        public ActionResult images(Int32 id = 0, string returnUrl = "/admin_administrators")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.Administrator = Administrator.GetOneById(id, adminLanguageId);
            ViewBag.MainImageUrl = GetMainImageUrl(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Administrator != null)
            {
                return View("images");
            }
            else
            {
                return Redirect(returnUrl);
            }

        } // End of the images method

        // Get the translate form
        // GET: /admin_administrators/translate/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "/admin_administrators")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.StandardAdministrator = Administrator.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedAdministrator = Administrator.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardAdministrator != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect(returnUrl);
            }

        } // End of the translate method

        #endregion

        #region Post methods

        // Update the administrator
        // POST: /admin_administrators/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the return url
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
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

            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            string user_name = collection["txtUserName"];
            string password = collection["txtPassword"];
            string role = collection["selectAdminRole"];
            string email = collection["txtEmail"];
            string author_name = collection["txtAuthorName"];
            string author_description = collection["txtAuthorDescription"];
            string facebook_user_id = collection["txtFacebookUserId"];
            string google_user_id = collection["txtGoogleUserId"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the administrator
            Administrator administrator = Administrator.GetOneById(id, adminLanguageId);
            bool postExists = true;

            // Check if the administrator exists
            if (administrator == null)
            {
                // Create an empty administrator
                administrator = new Administrator();
                postExists = false;
            }

            // Update values
            administrator.admin_user_name = user_name;
            administrator.admin_role = role;
            administrator.email = email;
            administrator.author_name = author_name;
            administrator.author_description = author_description;
            administrator.facebook_user_id = facebook_user_id;
            administrator.google_user_id = google_user_id;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a administrator on user name
            Administrator adminOnUserName = Administrator.GetOneByUserName(user_name);

            // Check for errors in the administrator
            if (adminOnUserName != null && administrator.id != adminOnUserName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_unique"), tt.Get("user_name")) + "<br/>";
            }
            if (administrator.admin_user_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("user_name"), "50") + "<br/>";
            }
            if (administrator.author_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (AnnytabDataValidation.IsEmailAddressValid(administrator.email) == null)
            {
                errorMessage += "&#149; " + tt.Get("error_email_valid") + "<br/>";
            }
            if (administrator.facebook_user_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Facebook user id", "50") + "<br/>";
            }
            if (administrator.google_user_id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), "Google user id", "50") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the administrator
                if (postExists == false)
                {
                    // Add the administrator
                    Int32 insertId = (Int32)Administrator.AddMasterPost(administrator);
                    administrator.id = insertId;
                    Administrator.AddLanguagePost(administrator, adminLanguageId);
                    Administrator.UpdatePassword(insertId, PasswordHash.CreateHash(password));
                }
                else
                {
                    // Update the administrator
                    Administrator.UpdateMasterPost(administrator);
                    Administrator.UpdateLanguagePost(administrator, adminLanguageId);

                    // Only update the password if it has changed
                    if (password != "")
                    {
                        Administrator.UpdatePassword(administrator.id, PasswordHash.CreateHash(password));
                    }
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Administrator = administrator;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Update images for the administrator
        // POST: /admin_administrators/images
        [HttpPost]
        public ActionResult images(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
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

            // Get form values
            Int32 administrator_id = Convert.ToInt32(collection["txtId"]);
            HttpPostedFileBase mainImage = Request.Files["uploadMainImage"];
         
            // Update images
            UpdateImages(administrator_id, mainImage);

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the images method

        // Translate the administrator
        // POST: /admin_administrators/translate
        [HttpPost]
        public ActionResult translate(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the return url
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
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

            // Get the admin default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 id = Convert.ToInt32(collection["hiddenAdministratorId"]);
            string author_name = collection["txtTranslatedAuthorName"];
            string author_description = collection["txtTranslatedAuthorDescription"];

            // Create the translated administrator
            Administrator translatedAdministrator = new Administrator();
            translatedAdministrator.id = id;
            translatedAdministrator.author_name = author_name;
            translatedAdministrator.author_description = author_description;

            // Create a error message
            string errorMessage = string.Empty;

            // Check the translated author name
            if (translatedAdministrator.author_name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved administrator
                Administrator administrator = Administrator.GetOneById(id, translationLanguageId);

                if (administrator == null)
                {
                    // Add a new translated administrator
                    Administrator.AddLanguagePost(translatedAdministrator, translationLanguageId);
                }
                else
                {
                    // Update the translated administrator
                    administrator.author_name = translatedAdministrator.author_name;
                    administrator.author_description = translatedAdministrator.author_description;
                    Administrator.UpdateLanguagePost(administrator, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardAdministrator = Administrator.GetOneById(id, adminLanguageId);
                ViewBag.TranslatedAdministrator = translatedAdministrator;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the administrator
        // POST: /admin_administrators/delete/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "/admin_administrators")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
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
                // Delete the administrator and all the connected posts (CASCADE)
                errorCode = Administrator.DeleteOnId(id);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all files
                DeleteAllFiles(id);
            }
            else
            {
                // Delete the language post
                errorCode = Administrator.DeleteLanguagePostOnId(id, languageId);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Get the main image url for the administrator
        /// </summary>
        /// <param name="id">The administrator id</param>
        /// <returns>An image url as a string</returns>
        public string GetMainImageUrl(Int32 id)
        {
            // Create the string to return
            string imageUrl = "/Content/images/annytab_design/no_user_image.jpg";

            // Create the main image url
            string mainImageUrl = "/Content/administrators/" + (id / 100).ToString() + "/" + id.ToString() + "/main_image.jpg";

            // Check if the main image exists
            if (System.IO.File.Exists(Server.MapPath(mainImageUrl)))
            {
                imageUrl = mainImageUrl;
            }

            // Return the image url
            return imageUrl;

        } // End of the GetMainImageUrl method

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="id">The administrator id</param>
        /// <param name="mainImage">The posted main image file</param>
        private void UpdateImages(Int32 id, HttpPostedFileBase mainImage)
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

        } // End of the UpdateImages method

        /// <summary>
        /// Delete all the files for the administrator
        /// </summary>
        /// <param name="id">The administrator id</param>
        private void DeleteAllFiles(Int32 id)
        {
            // Define the directory url
            string directory = Server.MapPath("/Content/administrators/" + (id / 100).ToString() + "/" + id.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.Delete(directory, true);
            }

        } // End of the DeleteAllFiles method

        #endregion

    } // End of the class

} // End of the namespace