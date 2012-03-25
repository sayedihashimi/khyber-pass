namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class CommonStrings {
        public static class Database {
            public static string ConnectionStringName = "mondodb";
            public static string DatabaseName = "self-publish";
            public static string CollectionName = "packages";
        }

        public static class Deployer {
            public static string ConfigServiceBaseUrl = "configServiceBaseUrl";
            public static string PackageNameToDeploy = "packageNameToDeploy";
            public static string DeployParameters = "deployParameters";
            public static string MsdeployTimeout = "msdeployTimeout";
            public static string GetLatestPackageTimeout = "getLatestPackageTimeout";
        }
    }
}
