namespace Sedodream.SelfPub.ConfigService {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Controllers;

    public class CustomDeendencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Services.IDependencyResolver {
        protected object CoreGetService(Type serviceType) {
            if (serviceType == null) { throw new ArgumentNullException("serviceType"); }

            object result = null;
            if (serviceType == typeof(IPackageRepository)) {
                result = this.CreatePackageRepository();
            }
            else if (serviceType == typeof(IJsonSearlizer)) {
                result = new JsonNetSearlizer();
            }
            // MVC controllers
            else if (serviceType == typeof(ConfigController)) {
                result = new ConfigController(this.CreatePackageRepository(), this.CreateJsonSearlizer());
            }
            else if (serviceType == typeof(HomeController)) {
                result = new HomeController(this.CreatePackageRepository());
            }
            // API Controllers
            else if (serviceType == typeof(ConfigController)) {
                result = new ConfigController(this.CreatePackageRepository(), this.CreateJsonSearlizer());
            }
            else if (serviceType == typeof(HomeController)) {
                result = new HomeController(this.CreatePackageRepository());
            }

            return result;
        }

        #region System.Web.Mvc.IDependencyResolver
        object System.Web.Mvc.IDependencyResolver.GetService(Type serviceType) {
            return this.CoreGetService(serviceType);
        }

        IEnumerable<object> System.Web.Mvc.IDependencyResolver.GetServices(Type serviceType) {
            return new List<object>();
        }
        #endregion

        #region System.Web.Http.Services
        object System.Web.Http.Services.IDependencyResolver.GetService(Type serviceType){
            return this.CoreGetService(serviceType);
        }

        IEnumerable<object> System.Web.Http.Services.IDependencyResolver.GetServices(Type serviceType) {
            return new List<object>();
        }
        #endregion

        private IPackageRepository CreatePackageRepository() {
            
            // TODO: Switch out for an IoC
            return new MongoPackageRepository(ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString);
        }

        private IJsonSearlizer CreateJsonSearlizer() {
            // TODO: Switch out for an IoC
            return new JsonNetSearlizer();
        }
    }

}