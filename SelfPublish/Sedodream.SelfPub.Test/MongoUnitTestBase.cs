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

    public static class MongoUnitTestBase {
        private static Process mongoDbProcess;

        public static void Initalize(TestContext testContext) {
            XmlConfigurator.Configure();
            // we have to start mongo DB
            DirectoryInfo mongoDbDir = GetMongoDbDir(testContext);
            var mongodbexe = mongoDbDir.GetFiles("mongod.exe").Single();

            DirectoryInfo testCtxDirectory = new DirectoryInfo(testContext.TestDir);
            DirectoryInfo dataDbDir = testCtxDirectory.CreateSubdirectory(new Config().GetAppSetting<string>(CommonTestStrings.MongodbDir));

            string connectionString = new Config().GetConnectionString(CommonStrings.Database.ConnectionStringName).ConnectionString;
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

        public static void Cleanup() {
            // we have to stop mongo DB
            mongoDbProcess.CloseMainWindow();
            mongoDbProcess.WaitForExit(5 * 1000);
            if (!mongoDbProcess.HasExited) {
                mongoDbProcess.Kill();
            }
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
