using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the default page for administration
    /// </summary>
    [ValidateInput(false)]
    public class admin_defaultController : Controller
    {
        #region View methods

        // Get the default page
        // GET: /admin_default/
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
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the login page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Get the comments page
        // GET: /admin_default/comments
        public ActionResult comments()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the login page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the comments method

        // Get the ratings page
        // GET: /admin_default/ratings
        public ActionResult ratings()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the login page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the ratings method

        // Sort the list of posts
        // POST: /admin_default/sort_posts
        [HttpPost]
        public ActionResult sort_posts(FormCollection collection)
        {
            // Get the sort data
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_default?sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Sort the list of comments
        // POST: /admin_default/sort_comments
        [HttpPost]
        public ActionResult sort_comments(FormCollection collection)
        {
            // Get the sort data
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_default/comments?sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the sort_comments method

        // Sort the list of ratings
        // POST: /admin_default/sort_ratings
        [HttpPost]
        public ActionResult sort_ratings(FormCollection collection)
        {
            // Get the sort data
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_default/ratings?sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the sort_ratings method

        #endregion

    } // End of the class

} // End of the namespace