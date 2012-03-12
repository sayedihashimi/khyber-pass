using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sedodream.SelfPub.ConfigService.Controllers {
    public class BaseApiController {

        protected object Desearlize(string objString) {
            if (string.IsNullOrEmpty(objString)) { throw new ArgumentNullException("object"); }

            // TODO: Add IOC so that we can switch out desearlizer here
            return Newtonsoft.Json.JsonConvert.DeserializeObject(objString);
        }

        protected T Desearlize<T>(string objString) {
            if (string.IsNullOrEmpty(objString)) { throw new ArgumentNullException("objString"); }

            // TODO: Add IOC so that we can switch out desearlizer here
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objString);
        }
    }
}