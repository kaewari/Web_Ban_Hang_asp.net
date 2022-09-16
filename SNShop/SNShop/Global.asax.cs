using SNShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SNShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["Visiter"] = 0;
            Application["Sessions"] = 0;
            
        }
        protected void Application_End()
        {
            Application["Visiter"] = 0;
            Application["Sessions"] = 0;

        }
        protected void Session_Start()
        {
            Application["Sessions"] = (int)Application["Sessions"] + 1;
        }
        protected void Session_End()
        {
            Application["Sessions"] = (int)Application["Sessions"] -1;
        }
    }
}
