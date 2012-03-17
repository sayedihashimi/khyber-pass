using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sedodream.SelfPub.ConfigService.Models;

namespace Sedodream.SelfPub.ConfigService {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterWebApiRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start() {
            this.RegisterDependencies();

            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }

        protected void RegisterDependencies() {            
            // this is for ASP.NET MVC
            DependencyResolver.SetResolver(new CustomDeendencyResolver());

            // this is for Web API
            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(new CustomDeendencyResolver());
        }

        protected static void RegisterWebApiRoutes(RouteCollection routes) {
            if (routes == null) { throw new ArgumentNullException("routes"); }

            // http://localhost:12914/api/Config

            routes.MapHttpRoute(
                name: "allPackages",
                routeTemplate: "api/config/packages",
                defaults: new { controller = "Config", action = "GetAllPackages" });

            routes.MapHttpRoute(
                name: "GetPackagesByTag",
                routeTemplate: "api/config/packages/tag/{tag}",
                defaults: new { controller = "Config", action = "GetPackagesByTag" });

            routes.MapHttpRoute(
                name: "foo",
                routeTemplate: "api/config",
                defaults: new { controller = "Config", action = "Get" });

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}