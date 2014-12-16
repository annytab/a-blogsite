using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of ads
    /// </summary>
    [ValidateInput(false)] 
    public class admin_adsController : Controller
    {
        #region View methods

        // Get the list of ads
        // GET: /admin_ads/
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
        // POST: /admin_ads/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_ads?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_ads/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "/admin_ads")
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
            ViewBag.Keywords = "";
            ViewBag.CurrentPage = 1;
            ViewBag.Ad = Ad.GetOneById(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty ad post if the ad does not exist
            if (ViewBag.Ad == null)
            {
                // Add data to the view
                ViewBag.Ad = new Ad();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the ad
        // POST: /admin_ads/edit
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
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            string name = collection["txtName"];
            string url = collection["txtUrl"];
            string ad_slot = collection["selectAdSlot"];
            string ad_code = collection["txtDescription"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);
            string keywords = collection["txtSearch"];
            Int32 currentPage = Convert.ToInt32(collection["hiddenPage"]);

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the ad
            Ad ad = Ad.GetOneById(id);
            bool postExists = true;

            // Check if the ad exists
            if (ad == null)
            {
                // Create an empty ad
                ad = new Ad();
                postExists = false;
            }

            // Update values
            ad.name = name;
            ad.language_id = language_id;
            ad.ad_slot = ad_slot;
            ad.ad_code = ad_code;
            ad.inactive = inactive;

            // Check if the user wants to do a search
            if (collection["btnSearch"] != null)
            {
                // Set form values
                ViewBag.Keywords = keywords;
                ViewBag.CurrentPage = currentPage;
                ViewBag.Ad = ad;
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
                ViewBag.Ad = ad;
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
                ViewBag.Ad = ad;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the ad
            if (ad.language_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("language").ToLower()) + "<br/>";
            }
            if (ad.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the ad
                if (postExists == false)
                {
                    // Add the ad
                    Ad.Add(ad);
                }
                else
                {
                    // Update the ad
                    Ad.Update(ad);
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
                ViewBag.TranslatedTexts = tt;
                ViewBag.Ad = ad;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the ad
        // POST: /admin_ads/delete/1?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "/admin_ads")
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

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the ad post and all the connected posts (CASCADE)
            errorCode = Ad.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace