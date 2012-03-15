namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Models.PageModels;

    public static class CustomAssert {
        public static void AreEqual(Package package, PackagePageModel packagePageModel) {
            if (package == null) { throw new ArgumentNullException("package"); }
            if (packagePageModel == null) { throw new ArgumentNullException("packagePageModel"); }

            Assert.AreEqual(package.Id, packagePageModel.Id);
            Assert.AreEqual(package.Name, packagePageModel.Name);
            Assert.AreEqual(package.PackageLocation, packagePageModel.PackageLocation);
            Assert.AreEqual(packagePageModel.PackageManifest, packagePageModel.PackageManifest);
            Assert.AreEqual(package.Version, packagePageModel.Version);

            StringBuilder flattenedTags = new StringBuilder();
            package.Tags.ForEach(t => {
                flattenedTags.Append(t.Replace(" ", string.Empty));
            });

            Assert.AreEqual(packagePageModel.Tags.Replace(" ", string.Empty), flattenedTags.ToString());
        }
    }
}
