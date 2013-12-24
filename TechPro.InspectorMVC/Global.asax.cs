using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TechPro.InspectorMVC.Controllers;
using TechPro.InspectorMVC.Models;

namespace TechPro.InspectorMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create fake database data
            for (int i = 0; i < 23; i++) {
                HomeController.Database.Add(new PersonModel());
            }
        }
    }
}
