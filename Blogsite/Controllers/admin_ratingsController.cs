using System;
using System.Web.Mvc;
using System.Globalization;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of ratings
    /// </summary>
    [ValidateInput(false)]
    public class admin_ratingsController : Controller
    {
        #region View methods

        // Get the list of ratings
        // GET: /admin_ratings/
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
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_ratings/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the search data
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("~/admin_ratings?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_ratings/edit/1?administratorId=1&languageId=2&returnUrl=55
        [HttpGet]
        public ActionResult edit(Int32 id = 0, Int32 administratorId = 0, Int32 languageId = 0, string returnUrl = "/admin_ratings")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the signed in administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post rating
            PostRating postRating = PostRating.GetOneById(id, administratorId, languageId);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (postRating == null || postRating.administrator_id == administrator.id))
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
            ViewBag.PostRating = postRating;
            ViewBag.ReturnUrl = returnUrl;

            // Return the user to the index page if the rating does not exist
            if (ViewBag.PostRating == null)
            {
                // Return the user to the index page
                return Redirect(returnUrl);
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the rating
        // POST: /admin_ratings/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get all the form values
            Int32 post_id = Convert.ToInt32(collection["hiddenPostId"]);
            Int32 administrator_id = Convert.ToInt32(collection["hiddenAdministratorId"]);
            Int32 language_id = Convert.ToInt32(collection["hiddenLanguageId"]);
            decimal rating = 0;
            decimal.TryParse(collection["userVote"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out rating);
            string returnUrl = collection["returnUrl"];

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the signed in administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post rating
            PostRating postRating = PostRating.GetOneById(post_id, administrator_id, language_id);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (postRating == null || postRating.administrator_id == administrator.id))
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

            // Update the post rating
            if (postRating != null)
            {
                // Update the rating for the post
                postRating.rating = rating;
                PostRating.Update(postRating);

                // Update the rating sum for the post
                Post.UpdateRating(post_id, language_id);
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the edit method

        // Delete the post rating
        // POST: /admin_ratings/delete
        [HttpGet]
        public ActionResult delete(Int32 id = 0, Int32 administratorId = 0, Int32 languageId = 0, string returnUrl = "/admin_ratings")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Get the signed in administrator
            Administrator administrator = Administrator.GetSignedInAdministrator();

            // Get the post rating
            PostRating postRating = PostRating.GetOneById(id, administratorId, languageId);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (administrator != null && administrator.admin_role == "Author" && 
                (postRating == null || postRating.administrator_id == administrator.id))
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

            // Get the rating post
            PostRating rating = PostRating.GetOneById(id, administratorId, languageId);

            // Create an error code variable
            Int32 errorCode = 0;

            // Make sure that the rating not is null
            if (rating != null)
            {
                // Delete the rating
                errorCode = PostRating.DeleteOnId(id, administratorId, languageId);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Update the post rating
                Post.UpdateRating(rating.post_id, rating.language_id);
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace