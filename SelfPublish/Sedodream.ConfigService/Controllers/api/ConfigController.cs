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

        public IQueryable<Package> GetFoo() {
            // TODO: For now this returns all packages but this should be filtered

            return this.PackageRepository.GetPackages();
        }

        //public IEnumerable<IPackage> GetAllPacakges() {
        //    // TODO: For now this returns all packages but this should be filtered
        //    var result = from p in this.PackageRepository.GetPackages()
        //                 select p;

        //    return result;
        //}
        
        //public string GetInfo() {
        //    return DateTime.UtcNow.ToString();
        //}



        public string GetPublisherConfig(string publisherName) {
            //if (string.IsNullOrEmpty(publisherName)) {
            //    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest,
            //}
            throw new NotImplementedException();
        }


        //// GET /api/values
        //public IEnumerable<string> Get() {
        //    return new string[] { "value1", "value2" };
        //}

        //// GET /api/values/5
        //public string Get(int id) {
        //    return "value";
        //}

        //// POST /api/values
        //public void Post(string value) {
        //}

        //// PUT /api/values/5
        //public void Put(int id, string value) {
        //}

        //// DELETE /api/values/5
        //public void Delete(int id) {
        //}
    }
}