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

            string tagString = CustomAssert.ConvertTagsToString(package.Tags);
            Assert.AreEqual(packagePageModel.Tags.Replace(" ", string.Empty), tagString);
        }

        public static void AreEqual(Package package1, Package package2,bool includeOptionalFields = false) {
            if (package1 == null) { throw new ArgumentNullException("package1"); }
            if (package2 == null) { throw new ArgumentNullException("package2"); }

            if (includeOptionalFields) {
                Assert.AreEqual(package1.Id, package2.Id);
                Assert.AreEqual(package1.DateCreated, package2.DateCreated);
            }
            
            Assert.AreEqual(package1.Name, package2.Name);
            Assert.AreEqual(package1.PackageLocation, package2.PackageLocation);
            Assert.AreEqual(package1.PackageManifest, package2.PackageManifest);
            Assert.AreEqual(package1.PackageType, package2.PackageType);
            Assert.AreEqual(package1.Version, package2.Version);

            string package1Tags = string.Empty;
            string package2Tags = string.Empty;

            if (package1.Tags != null) {
                package1Tags = CustomAssert.ConvertTagsToString(package1.Tags);
            }
            if (package2.Tags != null) {
                package2Tags = CustomAssert.ConvertTagsToString(package2.Tags);
            }
        }

        public static void AreEqual(IList<Package> packagesList1, IList<Package> packagesList2) {
            if (packagesList1 == null) { throw new ArgumentNullException("packagesList1"); }
            if (packagesList2 == null) { throw new ArgumentNullException("packagesList2"); }

            Assert.AreEqual(packagesList1.Count, packagesList2.Count);
            for (int i = 0; i < packagesList1.Count; i++) {
                CustomAssert.AreEqual(packagesList1[i], packagesList2[i]);
            }
        }

        private static string ConvertTagsToString(IEnumerable<string>tags) {
            if (tags == null) { throw new ArgumentNullException("tags"); }

            StringBuilder flattenedTags = new StringBuilder();
            tags.ToList().ForEach(t => {
                flattenedTags.Append(t.Replace(" ", string.Empty));
            });

            return flattenedTags.ToString();
        }
    }
}
