namespace Sedodream.SelfPub.Test.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Models;
    using Sedodream.SelfPub.ConfigService.Models.PageModels;

    [TestClass]
    public class TestObjectMapper {
        [TestMethod]
        public void Test_Package_ToPackagePageModel_Default() {
            Package package = RandomDataHelper.Instance.CreateRandomePackage();

            PackagePageModel pageModel = ObjectMapper.Instance.Map<Package, PackagePageModel>(package);

            Assert.IsNotNull(pageModel);
            Assert.AreEqual(package.Id, pageModel.Id);
            Assert.AreEqual(package.Name, pageModel.Name);
            Assert.AreEqual(package.PackageLocation, pageModel.PackageLocation);
            Assert.AreEqual(package.PackageManifest, pageModel.PackageManifest);
            Assert.IsNotNull(package.Tags);
            foreach (string tag in package.Tags) {
                Assert.IsTrue(pageModel.Tags.Contains(tag));
            }
        }

        [TestMethod]
        public void Test_AddPackagePageModel_Constructor_Default() {
            IList<Package> packages = RandomDataHelper.Instance.CreateRandomListOf<Package>(() => RandomDataHelper.Instance.CreateRandomePackage(), 10);
            IList<PackagePageModel> packageModelList = ObjectMapper.Instance.Map<IList<Package>,IList<PackagePageModel>>(packages);

            Assert.IsNotNull(packageModelList);
            for (int i = 0; i < packages.Count; i++) {
                CustomAssert.AreEqual(packages[i], packageModelList[i]);
            }
        }
    }
}
