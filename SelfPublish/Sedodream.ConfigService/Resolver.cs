namespace Sedodream.SelfPub.ConfigService {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Http.Services;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Controllers;

    public class Resolver : IDependencyResolver {

        public object GetService(Type serviceType) {
            if (serviceType == null) { throw new ArgumentNullException("serviceType"); }
            
            object result = null;
            if (serviceType == typeof(IPackageRepository)) {
                result = this.CreatePackageRepository();
            }
            else if (serviceType == typeof(IJsonSearlizer)) {
                result = new JsonNetSearlizer();
            }
            else if (serviceType == typeof(ConfigController)) {
                result = new ConfigController(this.CreatePackageRepository(), this.CreateJsonSearlizer());
            }
            else if (serviceType == typeof(HomeController)) {
                result = new HomeController(this.CreatePackageRepository());
            }

            return result;
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return new List<object>();
        }

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