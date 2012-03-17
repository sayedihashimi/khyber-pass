namespace Sedodream.SelfPub.ConfigService.Models.PageModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;

    public class PackagePageModel {
        
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string PackageLocation { get; set; }
        public string PackageManifest { get; set; }
        public string Tags { get; set; }
        public string Version { get; set; }

    }

    public class PackageListPageModel {
        public PackageListPageModel()
            : this(new List<PackagePageModel>()) {
        }

        public PackageListPageModel(IList<Package> packages)
            : this(ObjectMapper.Instance.Map<IList<Package>, IList<PackagePageModel>>(packages)) {
        }

        public PackageListPageModel(IEnumerable<PackagePageModel> packages) {
            if (packages == null) { packages = new List<PackagePageModel>(); }

            this.Packages = new List<PackagePageModel>();

            foreach (var ppm in packages) {
                this.Packages.Add(ppm);
            }
        }
        public IList<PackagePageModel> Packages { get; set; }
    }

}