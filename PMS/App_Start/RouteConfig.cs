using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           routes.MapRoute(
               name: "Customer",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Supplier",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Supplier", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Projects",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Projects", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "Receipts",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Receipts", action = "Index", id = UrlParameter.Optional }
         );

        routes.MapRoute(
             name: "Payments",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Payments", action = "Index", id = UrlParameter.Optional }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "login", id = UrlParameter.Optional }
            );
        }
    }
}
