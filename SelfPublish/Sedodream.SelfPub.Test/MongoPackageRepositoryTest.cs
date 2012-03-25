namespace Sedodream.SelfPub.Test {
    using System;
    using System.Configuration;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using System.Linq;
    using MongoDB.Driver;
    using System.Diagnostics;
    using Sedodream.SelfPub.Test.Helpers;
    using System.Collections.Generic;

    [TestClass]
    public class MongoPackageRepositoryTest {
        private static Process mongoDbProcess;

        [ClassInitialize]
        public static void Initalize(TestContext testContext) {
            // we have to start mongo DB
            DirectoryInfo mongoDbDir = GetMongoDbDir(testContext);
            var mongodbexe = mongoDbDir.GetFiles("mongod.exe").Single();

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(ConfigurationManager.AppSettings[CommonTestStrings.MongodbDir]);

            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.AddPackage(null);        
        }

        [TestMethod]
        public void Test_GetLatestPackageByName_1Package() {
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
            MongoUrlBuilder mub = new MongoUrlBuilder(connectionString);
            MongoPackageRepository repo = new MongoPackageRepository(connectionString);
            repo.Reset();

            Package package1 = RandomDataHelper.Instance.CreateRandomePackage();
            Package package2 = RandomDataHelper.Instance.CreateRandomePackage();
            package2.Name = package1.Name;

            repo.AddPackage(package1);
            repo.AddPackage(package2);
            Package latestPackage = repo.GetLatestPackageByName(package2.Name);

            CustomAssert.AreEqual(package1, latestPackage);
        }

        [TestMethod]        
        public void Test_GetPackagesByName_1Package() {
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CommonStrings.Database.ConnectionStringName].ConnectionString;
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
