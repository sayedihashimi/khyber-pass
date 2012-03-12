namespace Sedodream.SelfPub.ConfigService.Models.PageModels {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sedodream.SelfPub.Common;

    public class HomePageModel {
        public HomePageModel()
            : this(new List<Package>()) {
        }

        public HomePageModel(IList<Package> packages) {
            if (packages == null) {
                packages = new List<Package>();
            }
            this.Packages = packages;
        }
        public IList<Package> Packages { get; set; }
    }
}