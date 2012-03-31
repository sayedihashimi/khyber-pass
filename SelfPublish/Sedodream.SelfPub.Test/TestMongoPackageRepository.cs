namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using log4net.Config;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestMongoPackageRepository {
        private static Process mongoDbProcess;

        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            XmlConfigurator.Configure();
            // we have to start mongo DB
            DirectoryInfo mongoDbDir = GetMongoDbDir(testContext);
            var mongodbexe = mongoDbDir.GetFiles("mongod.exe").Single();

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(new Config().GetAppSetting<string>(CommonTestStrings.MongodbDir));

            string connectionString =new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            string args = string.Format(@"--dbpath ""{0}"" --port {1}", dataDbDir.FullName, mub.Server.Port);

            var psi = new ProcessStartInfo {
                FileName = mongodbexe.FullName,
                WorkingDirectory = testCtxDirectory.FullName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            mongoDbProcess = Process.Start(psi);
        }

        [ClassCleanup]
        public static void Cleanup() {
            // we have to stop mongo DB
            mongoDbProcess.CloseMainWindow();
            mongoDbProcess.WaitForExit(5 * 1000);
            if (!mongoDbProcess.HasExited) {
                mongoDbProcess.Kill();
            }
        }

        [TestMethod]
        public void Test_AddPackage() {            
            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);

            repo.Reset();

            repo.AddPackage(package);

            var r = from p in repo.GetPackages()
                    where string.Compare(p.Name, package.Name, StringComparison.OrdinalIgnoreCase) == 0
                    select p;

            Assert.IsNotNull(r);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_AddPackage_Null() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.AddPackage(null);        
        }

        [TestMethod]
        public void Test_GetLatestPackageByName_1Package() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            Package package = RandomDataHelper.Instance.CreateRandomePackage();

            repo.AddPackage(package);
            Package latestPackage = repo.GetLatestPackageByName(package.Name);

            CustomAssert.AreEqual(package, latestPackage);
        }

        [TestMethod]
        public void Test_GetLatestPackageByName_2Packages() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            Package package1 = RandomDataHelper.Instance.CreateRandomePackage();
            Package package2 = RandomDataHelper.Instance.CreateRandomePackage();
            package2.Name = package1.Name;

            repo.AddPackage(package1);
            // wait a second before adding the next one
            Thread.Sleep(1000);

            repo.AddPackage(package2);
            Package latestPackage = repo.GetLatestPackageByName(package2.Name);

            CustomAssert.AreEqual(package2, latestPackage);
        }

        [TestMethod]        
        public void Test_GetPackagesByName_1Package() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            repo.AddPackage(package);

            IList<Package> result = repo.GetPackagesByName(package.Name).ToList();

            Assert.IsTrue(result.Count == 1);
            CustomAssert.AreEqual(package, result[0]);
        }

        [TestMethod]
        public void Test_GetPackagesByName_2Packages() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            Package package1 = RandomDataHelper.Instance.CreateRandomePackage();
            Package package2 = RandomDataHelper.Instance.CreateRandomePackage();
            package2.Name = package1.Name;
            repo.AddPackage(package1);
            repo.AddPackage(package2);

            IList<Package> result = repo.GetPackagesByName(package1.Name).ToList();

            Assert.IsTrue(result.Count == 2);
            CustomAssert.AreEqual(package1, result[0]);
            CustomAssert.AreEqual(package2, result[1]);
        }

        [TestMethod]
        public void Test_GetPackage_ById_1PackageInCollection() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            // first add a package and then ask try and get it back
            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            repo.AddPackage(package);

            Package foundPackage = repo.GetPackage(package.Id);
            Assert.IsNotNull(foundPackage);
            CustomAssert.AreEqual(package, foundPackage);
        }

        [TestMethod]
        public void Test_GetPackage_ById_ManyPackagesInCollection() {
            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            // add a random # of packages before the one we want
            int numToAdd = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
            for (int i = 0; i < numToAdd; i++) {
                repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage());
            }

            Package package = RandomDataHelper.Instance.CreateRandomePackage();
            repo.AddPackage(package);

            // add a random # of packages after the one we want
            numToAdd = RandomDataHelper.Instance.Primitives.GetRandomInt(10);
            for (int i = 0; i < numToAdd; i++) {
                repo.AddPackage(RandomDataHelper.Instance.CreateRandomePackage());
            }

            Package foundPackage = repo.GetPackage(package.Id);
            Assert.IsNotNull(foundPackage);
            CustomAssert.AreEqual(package, foundPackage);
        }

        private static DirectoryInfo GetMongoDbDir(TestContext testContext) {
            if (testContext == null) { throw new ArgumentNullException("testContext"); }

            var mongoDbBindir =
                new DirectoryInfo(testContext.TestDir)
                    .Parent.Parent.Parent
                    .GetDirectories("lib").Single()
                    .GetDirectories("mongodb*").Single()
                    .GetDirectories("bin").Single();

            return mongoDbBindir;
        }
    }
}
