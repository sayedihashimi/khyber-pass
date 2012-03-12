namespace Sedodream.SelfPub.ConfigService.Models.PageModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;

    public class AddPackagePageModel {

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string PackageLocation { get; set; }
        public string PackageManifest { get; set; }
        public string Tags { get; set; }

        public void Test() {
            Package package = null;
            //package.Id;
            //package.Name;
            //package.PackageLocation;
            //package.PackageManifest;
            //package.Tags;
        }
    }


}