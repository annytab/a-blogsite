using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Blogsite.Controllers
{
    /// <summary>
    /// This class controls the front pages of the website
    /// </summary>
    public class homeController : Controller
    {
        #region View methods
        
        // Get the default page
        // GET: /home/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain and the home page
            Domain currentDomain = Tools.GetCurrentDomain();
            StaticPage staticPage = StaticPage.GetOneByConnectionId(1, currentDomain.front_end_language);
            staticPage = staticPage != null ? staticPage : new StaticPage();

            // Set form values
            ViewBag.CurrentCategory = new Category();
            ViewBag.BreadCrumbs = new List<BreadCrumb>(0);
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.StaticPage = staticPage;
            ViewBag.UserSettings = (Dictionary<string, string>)Session["UserSettings"];
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/home.cshtml");

        } // End of the index method

        // Get the search result page
        // GET: /home/search/
        [HttpGet]
        public ActionResult search()
        {
            // Get the current domain and the home page
            Domain currentDomain = Tools.GetCurrentDomain();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(tt.Get("search_result"), "/home/search"));

            // Set form values
            ViewBag.CurrentCategory = new Category();
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.UserSettings = (Dictionary<string, string>)Session["UserSettings"];
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/search.cshtml");

        } // End of the search method

        // Get the category page
        // GET: /home/category/
        [HttpGet]
        public ActionResult category(string id = "")
        {
            // Get the domain and the current category
            Domain currentDomain = Tools.GetCurrentDomain();
            Category currentCategory = Category.GetOneByPageName(id, currentDomain.front_end_language);

            // Make sure that the category not is null
            if (currentCategory == null)
            {
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.Write(Tools.GetHttpNotFoundPage());
                return new EmptyResult();
            }
                
            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Get a chain of parent categories
            List<Category> parentCategoryChain = Category.GetParentCategoryChain(currentCategory, currentDomain.front_end_language);

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            for (int i = 0; i < parentCategoryChain.Count; i++)
            {
                breadCrumbs.Add(new BreadCrumb(parentCategoryChain[i].title, "/home/category/" + parentCategoryChain[i].page_name));
            }

            // Update page views
            if (currentCategory.page_views <= Int32.MaxValue - 1)
            {
                Category.UpdatePageviews(currentCategory.id, currentDomain.front_end_language, currentCategory.page_views + 1);
            }

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.MainCategories = Category.GetActiveChildCategories(0, currentDomain.front_end_language, "title", "ASC");
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.CurrentCategory = currentCategory;
            ViewBag.UserSettings = (Dictionary<string, string>)Session["UserSettings"];
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/category.cshtml");

        } // End of the category method

        // Get the post page
        // GET: /home/post/
        [HttpGet]
        public ActionResult post(string id = "")
        {
            // Get the domain and the post
            Domain currentDomain = Tools.GetCurrentDomain();
            Post post = Post.GetOneByPageName(id, currentDomain.front_end_language);
            
            // Make sure that the post not is null
            if (post == null)
            {
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.Write(Tools.GetHttpNotFoundPage());
                return new EmptyResult();
            }

            // Get additional data
            Category currentCategory = Category.GetOneById(post.category_id, currentDomain.front_end_language);
            currentCategory = currentCategory != null ? currentCategory : new Category();
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");
            List<Category> parentCategoryChain = Category.GetParentCategoryChain(currentCategory, currentDomain.front_end_language);
            Administrator author = Administrator.GetOneById(post.administrator_id, currentDomain.front_end_language);
            Administrator user = Administrator.GetSignedInAdministrator(currentDomain.front_end_language);

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            for (int i = 0; i < parentCategoryChain.Count; i++)
            {
                breadCrumbs.Add(new BreadCrumb(parentCategoryChain[i].title, "/home/category/" + parentCategoryChain[i].page_name));
            }
            breadCrumbs.Add(new BreadCrumb(post.title, "/home/post/" + post.page_name));

            // Update page views
            if (post.page_views <= Int32.MaxValue - 1)
            {
                Post.UpdatePageviews(post.id, currentDomain.front_end_language, post.page_views + 1);
            }

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.CurrentCategory = currentCategory;
            ViewBag.Post = post;
            ViewBag.Author = author != null ? author : new Administrator();
            ViewBag.User = user;
            ViewBag.UserSettings = (Dictionary<string, string>)Session["UserSettings"];
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/post.cshtml");

        } // End of the product method

        // Get the information page
        // GET: /home/information/
        [HttpGet]
        public ActionResult information(string id = "")
        {
            // Get the current domain and the static page
            Domain currentDomain = Tools.GetCurrentDomain();
            StaticPage staticPage = StaticPage.GetOneByPageName(id, currentDomain.front_end_language);

            // Make sure that the static page not is null
            if(staticPage == null)
            {
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.Write(Tools.GetHttpNotFoundPage());
                return new EmptyResult();
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(staticPage.link_name, "/home/information/" + staticPage.page_name));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.StaticPage = staticPage;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/information.cshtml");

        } // End of the information method

        // Get author information
        // GET: /home/author/1
        [HttpGet]
        public ActionResult author(Int32 id = 0)
        {
            // Get the current domain and the author
            Domain currentDomain = Tools.GetCurrentDomain();
            Administrator author = Administrator.GetOneById(id, currentDomain.front_end_language);

            // Make sure that the author not is null
            if (author == null)
            {
                Response.StatusCode = 404;
                Response.Status = "404 Not Found";
                Response.Write(Tools.GetHttpNotFoundPage());
                return new EmptyResult();
            }

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(author.author_name, "/home/author/" + id.ToString()));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.Author = author;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/author.cshtml");

        } // End of the author method

        // Get the error page
        // GET: /home/error/
        [HttpGet]
        public ActionResult error(string id = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();

            // Set the connection id
            byte connectionId = 3;
            if (id == "invalid_input")
            {
                connectionId = 4;
            }
            else if (id == "404")
            {
                connectionId = 5;
            }            
            else
            {
                id = "general";
            }
               
            // Get the error page
            StaticPage staticPage = StaticPage.GetOneByConnectionId(connectionId, currentDomain.front_end_language);
            staticPage = staticPage != null ? staticPage : new StaticPage();

            // Get the translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.front_end_language, "id", "ASC");

            // Create the bread crumb list
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>(2);
            breadCrumbs.Add(new BreadCrumb(tt.Get("start_page"), "/"));
            breadCrumbs.Add(new BreadCrumb(staticPage.link_name, "/home/error/" + id.ToString()));

            // Set form values
            ViewBag.BreadCrumbs = breadCrumbs;
            ViewBag.CurrentCategory = new Category();
            ViewBag.TranslatedTexts = tt;
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.CurrentLanguage = Language.GetOneById(currentDomain.front_end_language);
            ViewBag.StaticPage = staticPage;
            ViewBag.CultureInfo = Tools.GetCultureInfo(ViewBag.CurrentLanguage);

            // Return the view
            return currentDomain.custom_theme_id == 0 ? View() : View("/Views/theme/error.cshtml");

        } // End of the error method

        #endregion

        #region Post methods

        // Search for posts
        // POST: /home/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];

            // Return the url with search parameters
            return Redirect("/home/search?kw=" + Server.UrlEncode(keywordString));

        } // End of the search method

        // Sort categories on the index page
        // POST: /home/sort_home
        [HttpPost]
        public ActionResult sort_home(FormCollection collection)
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            string layoutType = "standard";
            if (Request.Cookies["LayoutType"] != null)
            {
                layoutType = Request.Cookies["LayoutType"].Value;
            }

            // Get the form data
            string sort_field = collection["selectSortField"] != null ? collection["selectSortField"] : "id";
            string sort_order = collection["selectSortOrder"] != null ? collection["selectSortOrder"] : "ASC";
            string page_size = collection["selectPageSize"] != null ? collection["selectPageSize"] : "10";

            // Create a new dictionary
            Dictionary<string, string> userSettings = new Dictionary<string, string>(3);
            userSettings.Add("sort_field", sort_field);
            userSettings.Add("sort_order", sort_order);
            userSettings.Add("page_size", page_size);

            // Save the dictionary to a session
            Session["UserSettings"] = userSettings;

            // Redirect the user to the start page
            return RedirectToAction("index", "home");

        } // End of the sort_home method

        // Sort categories and posts in a category
        // POST: /home/sort_category
        [HttpPost]
        public ActionResult sort_category(FormCollection collection)
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            string layoutType = "standard";
            if (Request.Cookies["LayoutType"] != null)
            {
                layoutType = Request.Cookies["LayoutType"].Value;
            }

            // Get the form data
            string page_name = collection["hiddenPageName"] != null ? collection["hiddenPageName"] : "";
            string sort_field = collection["selectSortField"] != null ? collection["selectSortField"] : "id";
            string sort_order = collection["selectSortOrder"] != null ? collection["selectSortOrder"] : "ASC";
            string page_size = collection["selectPageSize"] != null ? collection["selectPageSize"] : "10";

            // Create a new dictionary
            Dictionary<string, string> userSettings = new Dictionary<string, string>(3);
            userSettings.Add("sort_field", sort_field);
            userSettings.Add("sort_order", sort_order);
            userSettings.Add("page_size", page_size);

            // Save the dictionary to a session
            Session["UserSettings"] = userSettings;

            // Redirect the user to the category page
            return RedirectToAction("category", "home", new { id = page_name });

        } // End of the sort_category method

        // Sort the search result
        // POST: /home/sort_search
        [HttpPost]
        public ActionResult sort_search(FormCollection collection)
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            string layoutType = "standard";
            if (Request.Cookies["LayoutType"] != null)
            {
                layoutType = Request.Cookies["LayoutType"].Value;
            }

            // Get the form data
            string keywords = collection["txtFormSearchBox"] != null ? collection["txtFormSearchBox"] : "";
            string sort_field = collection["selectSortField"] != null ? collection["selectSortField"] : "id";
            string sort_order = collection["selectSortOrder"] != null ? collection["selectSortOrder"] : "ASC";
            string page_size = collection["selectPageSize"] != null ? collection["selectPageSize"] : "10";

            // Create a new dictionary
            Dictionary<string, string> userSettings = new Dictionary<string, string>(3);
            userSettings.Add("sort_field", sort_field);
            userSettings.Add("sort_order", sort_order);
            userSettings.Add("page_size", page_size);

            // Save the dictionary to a session
            Session["UserSettings"] = userSettings;
            
            // Redirect the user to the category page
            return RedirectToAction("search", "home", new { kw = Server.UrlEncode(keywords) });

        } // End of the sort_search method

        // Sort the the list of posts by author
        // POST: /home/sort_author
        [HttpPost]
        public ActionResult sort_author(FormCollection collection)
        {
            // Get the current domain
            Domain domain = Tools.GetCurrentDomain();

            string layoutType = "standard";
            if (Request.Cookies["LayoutType"] != null)
            {
                layoutType = Request.Cookies["LayoutType"].Value;
            }

            // Get the form data
            Int32 author_id = 0;
            Int32.TryParse(collection["hiddenAuthorId"], out author_id);
            string sort_field = collection["selectSortField"] != null ? collection["selectSortField"] : "id";
            string sort_order = collection["selectSortOrder"] != null ? collection["selectSortOrder"] : "ASC";
            string page_size = collection["selectPageSize"] != null ? collection["selectPageSize"] : "10";

            // Create a new dictionary
            Dictionary<string, string> userSettings = new Dictionary<string, string>(3);
            userSettings.Add("sort_field", sort_field);
            userSettings.Add("sort_order", sort_order);
            userSettings.Add("page_size", page_size);

            // Save the dictionary to a session
            Session["UserSettings"] = userSettings;

            // Redirect the user to the author page
            return RedirectToAction("author", "home", new { id = author_id });

        } // End of the sort_author method

        #endregion

        #region Helper methods

        // Set the layout of the page
        // GET: /home/layout/mobile
        [HttpGet]
        public ActionResult layout(string id = "")
        {
            // Create a new cookie
            HttpCookie aCookie = new HttpCookie("LayoutType");
            aCookie.Value = id;

            // Set the expiration and add the cookie
            aCookie.Expires = DateTime.Now.AddDays(1);
            aCookie.HttpOnly = true;
            Response.Cookies.Add(aCookie);

            // Redirect the user to the new url
            return Redirect("/");

        } // End of the layout method

        #endregion

    } // End of the class

} // End of the namespace