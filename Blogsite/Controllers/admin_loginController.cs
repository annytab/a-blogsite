using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class represent the administrator login page
    /// </summary>
    public class admin_loginController : Controller
    {
        #region View methods

        // Get the login form
        // GET: /admin_login/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Set data for the form
            ViewBag.Administrator = new Administrator();
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Return the index view
            return View();

        } // End of the index method

        #endregion

        #region Post methods

        // Check for a correct login attempt
        // POST: /admin_login/index
        [HttpPost]
        public ActionResult index(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the data from the form
            string user_name = collection["txtUserName"];
            string password = collection["txtPassword"];

            // Get the administrator
            Administrator administrator = Administrator.GetOneByUserName(user_name);

            // Get the current language id for admins
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList translatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Check if the user name exists and if the password is correct
            if (administrator != null && Administrator.ValidatePassword(user_name, password) == true 
                && Administrator.IsAuthorized(Administrator.GetAllAdminRoles(), administrator.admin_role) == true)
            {
                // Get website settings
                KeyStringList websiteSettings = WebsiteSetting.GetAllFromCache();
                string redirectHttps = websiteSettings.Get("REDIRECT-HTTPS");

                // Create the administrator cookie
                HttpCookie adminCookie = new HttpCookie("Administrator");
                adminCookie.Value = Tools.ProtectCookieValue(administrator.id.ToString(), "Administration");
                adminCookie.Expires = DateTime.Now.AddDays(1);
                adminCookie.HttpOnly = true;
                adminCookie.Secure = redirectHttps.ToLower() == "true" ? true : false;
                Response.Cookies.Add(adminCookie);

                // Redirect the user to the default admin page
                return RedirectToAction("index", "admin_default");
            }
            else
            {
                // Create a new administrator
                Administrator admin = new Administrator();
                admin.admin_user_name = user_name;

                // Set the form data
                ViewBag.Administrator = admin;
                ViewBag.TranslatedTexts = translatedTexts;
                ViewBag.ErrorMessage = "&#149; " + translatedTexts.Get("error_login");

                // Return the index view
                return View("index");
            }

        } // End of the index method

        // Sign out the user
        // GET: /admin_login/logout
        [HttpGet]
        public ActionResult logout()
        {
            // Delete the administrator cookie
            HttpCookie adminCookie = new HttpCookie("Administrator");
            adminCookie.Value = "";
            adminCookie.Expires = DateTime.Now.AddDays(-1);
            adminCookie.HttpOnly = true;
            Response.Cookies.Add(adminCookie);

            // Redirect the user to the login page
            return RedirectToAction("index", "admin_login");

        } // End of the logout method

        #endregion

    } // End of the class

} // End of the namespace