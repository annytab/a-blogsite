using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the administration of media files
    /// </summary>
    [ValidateInput(false)]
    public class admin_media_filesController : Controller
    {
        #region View methods

        // Get the list of media files
        // GET: /admin_media_files/
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
        // POST: /admin_media_files/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_media_files?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_media_files/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "/admin_media_files")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Author" }) == true)
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
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.MediaFile = MediaFile.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty media file post if the media file does not exist
            if (ViewBag.MediaFile == null)
            {
                // Add data to the view
                ViewBag.MediaFile = new MediaFile();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        #endregion

        #region Post methods

        // Update the media file
        // POST: /admin_media_files/edit
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
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Author" }) == true)
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
            string title = collection["txtTitle"];
            string media_type = collection["selectMediaType"];
            HttpPostedFileBase media_file_content = Request.Files["uploadMediaFile"];

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the media file post
            MediaFile mediaFilePost = MediaFile.GetOneById(id);
            bool postExists = true;

            // Check if the media files exists
            if (mediaFilePost == null)
            {
                // Create an empty media file
                mediaFilePost = new MediaFile();
                postExists = false;
            }

            // Update values
            mediaFilePost.title = title;
            mediaFilePost.media_type = media_type;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the campaign
            if (mediaFilePost.title.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "100") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Update the media file
                if (media_file_content.ContentLength > 0)
                {
                    UpdateMediaFile(mediaFilePost, media_file_content);
                }

                // Check if we should add or update the media file
                if (postExists == false)
                {
                    // Add the media file
                    MediaFile.Add(mediaFilePost);
                }
                else
                {
                    // Update the media file
                    MediaFile.Update(mediaFilePost);
                }

                // Redirect the user to the list
                return Redirect(returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.MediaFile = mediaFilePost;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the media file
        // POST: /admin_media_files/delete/1?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "/admin_media_files")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Author" }) == true)
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

            // Get the media file post
            MediaFile mediaFilePost = MediaFile.GetOneById(id);

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the media file post and all the connected posts (CASCADE)
            errorCode = MediaFile.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Delete the media file
            if (mediaFilePost != null)
            {
                DeleteMediaFile(mediaFilePost.src);
            }

            // Redirect the user to the list
            return Redirect(returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Update the media file
        /// </summary>
        /// <param name="mediaFilePost">A reference to the media file post</param>
        /// <param name="mediaFile">The uploaded media file</param>
        private void UpdateMediaFile(MediaFile mediaFilePost, HttpPostedFileBase mediaFileContent)
        {
            // Create the directory string
            string directory = "/Content/media/" + (mediaFilePost.id / 1000).ToString() + "/";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(directory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(directory));
            }

            // Delete the old media file
            if (mediaFilePost.src != "")
            {
                DeleteMediaFile(mediaFilePost.src);
            }

            // Save the new media file
            mediaFilePost.src = directory + System.IO.Path.GetFileName(mediaFileContent.FileName);
            mediaFileContent.SaveAs(Server.MapPath(mediaFilePost.src));

        } // End of the UpdateMediaFile method

        /// <summary>
        /// Delete the media file
        /// </summary>
        /// <param name="src">The source for the media file</param>
        private void DeleteMediaFile(string src)
        {
            // Create the file url
            string url = Server.MapPath(src);

            // Delete the media file if it exists
            if (System.IO.File.Exists(url))
            {
                System.IO.File.Delete(url);
            }

        } // End of the DeleteMediaFile method

        #endregion

    } // End of the class

} // End of the namespace