using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of categories
    /// </summary>
    [ValidateInput(false)]
    public class admin_categoriesController : Controller
    {
        #region View methods

        // Get the list of categories
        // GET: /admin_categories/
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
        // POST: /admin_categories/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_categories?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_categories/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "/admin_categories")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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

            // Get the administrator default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.Category = Category.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Check if the category is null
            if (ViewBag.Category == null)
            {
                // Create a new empty category
                ViewBag.Category = new Category();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the translate form
        // GET: /admin_categories/translate/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "/admin_categories")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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

            // Get the default language id
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
            ViewBag.StandardCategory = Category.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedCategory = Category.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardCategory != null)
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

        // Update the category
        // POST: /admin_categories/edit
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
            Int32 id = Convert.ToInt32(collection["txtId"]);
            Int32 parentCategoryId = Convert.ToInt32(collection["selectCategory"]);
            string title = collection["txtTitle"];
            string description = collection["txtDescription"];
            string metaDescription = collection["txtMetaDescription"];
            string metaKeywords = collection["txtMetaKeywords"];
            string pageName = collection["txtPageName"];
            string metaRobots = collection["selectMetaRobots"];
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtDateAdded"], out date_added);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the category
            Category category = Category.GetOneById(id, adminLanguageId);

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Check if the category exists
            if (category == null)
            {
                // Create a new category
                category = new Category();
            }

            // Set values for the category
            category.parent_category_id = parentCategoryId;
            category.title = title;
            category.main_content = description;
            category.meta_description = metaDescription;
            category.meta_keywords = metaKeywords;
            category.page_name = pageName;
            category.meta_robots = metaRobots;
            category.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            category.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(category.page_name, adminLanguageId);

            // Check the page name
            if (categoryOnPageName != null && category.id != categoryOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (category.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(category.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (category.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (category.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (category.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (category.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the category
                if (category.id != 0)
                {
                    // Update the category
                    Category.UpdateMasterPost(category);
                    Category.UpdateLanguagePost(category, adminLanguageId);
                }
                else
                {
                    // Add the category
                    Int64 insertId = Category.AddMasterPost(category);
                    category.id = Convert.ToInt32(insertId);
                    Category.AddLanguagePost(category, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Category = category;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Translate the category
        // POST: /admin_categories/translate
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 categoryId = Convert.ToInt32(collection["hiddenCategoryId"]);
            string title = collection["txtTranslatedTitle"];
            string description = collection["txtTranslatedDescription"];
            string metadescription = collection["txtTranslatedMetadescription"];
            string metakeywords = collection["txtTranslatedMetakeywords"];
            string pagename = collection["txtTranslatedPagename"];
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtTranslatedDateAdded"], out date_added);
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated category
            Category translatedCategory = new Category();
            translatedCategory.id = categoryId;
            translatedCategory.title = title;
            translatedCategory.main_content = description;
            translatedCategory.meta_description = metadescription;
            translatedCategory.meta_keywords = metakeywords;
            translatedCategory.page_name = pagename;
            translatedCategory.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            translatedCategory.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a category on page name
            Category categoryOnPageName = Category.GetOneByPageName(translatedCategory.page_name, translationLanguageId);

            // Check for errors
            if (categoryOnPageName != null && translatedCategory.id != categoryOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedCategory.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(translatedCategory.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedCategory.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (translatedCategory.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (translatedCategory.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (translatedCategory.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved category
                Category category = Category.GetOneById(categoryId, translationLanguageId);

                if (category == null)
                {
                    // Add a new translated category
                    Category.AddLanguagePost(translatedCategory, translationLanguageId);
                }
                else
                {
                    // Update values for the saved category
                    category.title = translatedCategory.title;
                    category.main_content = translatedCategory.main_content;
                    category.meta_description = translatedCategory.meta_description;
                    category.meta_keywords = translatedCategory.meta_keywords;
                    category.page_name = translatedCategory.page_name;
                    category.date_added = translatedCategory.date_added;
                    category.inactive = translatedCategory.inactive;

                    // Update the category translation
                    Category.UpdateLanguagePost(category, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardCategory = Category.GetOneById(categoryId, adminLanguageId);
                ViewBag.TranslatedCategory = translatedCategory;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the category
        // POST: /admin_categories/delete/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "/admin_categories")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
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
                // Delete the category and all the posts connected to this category (CASCADE)
                errorCode = Category.DeleteOnId(id);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }
            }
            else
            {
                // Delete the translated post
                errorCode = Category.DeleteLanguagePostOnId(id, languageId);

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

	} // End of the class

} // End of the namespace