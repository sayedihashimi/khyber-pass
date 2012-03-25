namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    public interface IJsonSearlizer {
        string Searlize(object obj);

        T Desearlize<T>(string objString);
    }

    public class JsonNetSearlizer : IJsonSearlizer {
        public string Searlize(object obj) {
            if (obj == null) { throw new ArgumentNullException("obj"); }

            return JsonConvert.SerializeObject(obj);
        }

        public T Desearlize<T>(string objString) {
            if (string.IsNullOrEmpty(objString)) { throw new ArgumentNullException("objString"); }

            return JsonConvert.DeserializeObject<T>(objString);           
        }
    }
}
