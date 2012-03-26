namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Bson;
    using Moq;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Controllers;
    using Sedodream.SelfPub.Test.Helpers;
    
    [TestClass]
    public class TestConfigService {
        [TestMethod]
        public void Test_PostPackage() {
            var mockRepo = new Mock<IPackageRepository>();
            mockRepo.Setup(foo => foo
                .AddPackage(It.IsAny<Package>()))
                .Returns<Package>(p => {
                    p.Id = Guid.NewGuid();
                    return p;
                });

            IJsonSearlizer searlizer = this.CreateSearlizer();
            ConfigController controller = new ConfigController(mockRepo.Object, searlizer);

            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            controller.PostPackage(searlizer.Searlize(package));
            
            // make sure AddPackage is getting called only once
            mockRepo.Verify(
                pkg => pkg.AddPackage(It.IsAny<Package>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public void TestGetAllPackages() {
            IList<Package> allPackages = RandomDataHelper.Instance.CreateRandomListOf<Package>(
                RandomDataHelper.Instance.CreateRandomePackage,
                10);

            var mock = new Mock<IPackageRepository>();
            mock.Setup(repo => repo.GetPackages())
                .Returns(() => {
                    return allPackages.AsQueryable();
                });

            ConfigController controller = new ConfigController(mock.Object, this.CreateSearlizer());
            var result = controller.GetAllPackages();

            Assert.IsNotNull(result);
            IList<Package> expectedResult = allPackages.ToList();
            IList<Package> actualResult = result.ToList();
            CustomAssert.AreEqual(expectedResult, actualResult);

            // ensure GetPackages is called only once
            mock.Verify(
                res => res.GetPackages(),
                Times.Exactly(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_NullRepository() {
            ConfigController cc = new ConfigController(null, this.CreateSearlizer());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_NullSearlizer() {
            var mockRepo = new Mock<IPackageRepository>();
            ConfigController cc = new ConfigController(mockRepo.Object,null);
        }

        private IJsonSearlizer CreateSearlizer() {
            return new JsonNetSearlizer();
        }
    }
}
