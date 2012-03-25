namespace Sedodream.SelfPub.ConfigService.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Sedodream.SelfPub.Common;

    public class ConfigController : ApiController {
        protected IPackageRepository PackageRepository { get; set; }
        protected IJsonSearlizer JsonSearlizer { get; set; }

        public ConfigController(IPackageRepository pkgRepositiry, IJsonSearlizer jsonSearlizer) {
            if (pkgRepositiry == null) { throw new ArgumentNullException("pkgRepositiry"); }
            if (jsonSearlizer == null) { throw new ArgumentNullException("jsonSearlizer"); }

            this.PackageRepository = pkgRepositiry;
            this.JsonSearlizer = jsonSearlizer;
        }

        /// <summary>
        /// This is what clients will call to add new packages.
        /// </summary>
        /// <param name="jsonPackage">This object will be converted to an IPackage object</param>
        public void PostPackage(string jsonPackage) {
            if (string.IsNullOrEmpty(jsonPackage)) { throw new HttpResponseException("jsonPackage parameter cannot be null", HttpStatusCode.BadRequest); }
            // we need to convert this to an IPackage
            Package pkg = this.JsonSearlizer.Desearlize<Package>(jsonPackage);

            // add the package to the repositry
            this.PackageRepository.AddPackage(pkg);
        }
        
        // Sample OData queries that callers can use here
        //  http://localhost:12914/api/config/allPackages?$filter=startswith(Name,'PkgN')
        //  http://localhost:12914/api/config/allPackages?$filter=(Name eq 'PkgName')
        public IQueryable<Package> GetAllPackages() {
            return this.PackageRepository.GetPackages();
        }

        public IQueryable<Package> GetPackagesByTag(string tag) {
            if (string.IsNullOrWhiteSpace(tag)) { throw new ArgumentNullException("tag"); }

            return this.PackageRepository.GetPackagesByTag(tag);
        }

        public IQueryable<Package> GetPackagesByName(string name) {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            return this.PackageRepository.GetPackagesByName(name);
        }

        public Package GetLatestPackageByName(string name) {
            return this.PackageRepository.GetLatestPackageByName(name);
        }


    }
}