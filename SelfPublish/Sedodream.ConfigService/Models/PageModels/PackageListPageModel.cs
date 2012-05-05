namespace Sedodream.SelfPub.ConfigService.Models.PageModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;

    public class PackagePageModel {        
        public PackagePageModel() {
            this.Id = Guid.NewGuid();

            IList<SelectListItem> availableItems = new List<SelectListItem>();
            Enum.GetNames(typeof(KnownPackageTypes)).ToList().ForEach(name => {
                SelectListItem item = new SelectListItem {
                    Text = name,
                    Value = name,
                };
                availableItems.Add(item);

                this.AvailablePackageTypes = availableItems;
            });
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PackageLocation { get; set; }
        public string PackageManifest { get; set; }
        public string Tags { get; set; }
        public string Version { get; set; }
        public string PackageType { get; set; }

        public IEnumerable<SelectListItem> AvailablePackageTypes { get; set; }
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