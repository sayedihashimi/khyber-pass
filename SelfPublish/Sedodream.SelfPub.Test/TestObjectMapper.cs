﻿namespace Sedodream.SelfPub.Test {
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
        public void Test_PackageToAddPackagePageModel_Default() {
            Package package = new Package {
                Id = ObjectId.GenerateNewId(),
                Name = Guid.NewGuid().ToString(),
                PackageLocation = new Uri(Path.Combine(@"C:\temp", Guid.NewGuid().ToString())),
                PackageManifest = Guid.NewGuid().ToString(),
                Version = Guid.NewGuid().ToString()
            };

            int numTags = new Random().Next(10);
            for (int i = 0; i < numTags; i++) {
                package.Tags.Add(Guid.NewGuid().ToString());
            }

            AddPackagePageModel pageModel = ObjectMapper.Instance.Map<Package, AddPackagePageModel>(package);

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
    }
}
