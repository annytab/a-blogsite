using System.Web.Mvc;
using System.Web.Routing;

namespace Annytab.Blogsite
{
    /// <summary>
    /// This class handles routes, user friendly urls for the website
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register routes for the application
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
            );

        } // End of the RegisterRoutes method

    } // End of the class

} // End of the namespace