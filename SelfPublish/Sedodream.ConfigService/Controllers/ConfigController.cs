using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sedodream.SelfPub.ConfigService.Controllers {
    public class ConfigController : ApiController {

        #region Actions which the creating side will invoke (i.e. developer/build server/etc)
        /// <summary>
        /// This is what clients will call to add new packages.
        /// </summary>
        /// <param name="jsonPackage">This object will be converted to an IPackage object</param>
        public void PostPackage(string jsonPackage) {
            if (string.IsNullOrEmpty(jsonPackage)) { throw new HttpResponseException("jsonPackage parameter cannot be null", HttpStatusCode.BadRequest); }

            // we need to convert this to an IPackage




            throw new NotImplementedException();
        }
        #endregion

        public string GetConfig() {
            string result = null;

            var somObj = new {
                Name = "Foo",
                Bar = new {
                    Message = "bar string Here",
                    Time = DateTime.Now,
                    InnerBar = new {
                        IIB = "string here",
                        ObjHere = new {
                            SampleStr = "sample string here",
                            SomeInt = 1
                        }
                    }
                }
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(somObj, Newtonsoft.Json.Formatting.None);
        }

        public string GetConfig(string id) {
            var someObj = new {
                Id=id
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(someObj);
        }

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