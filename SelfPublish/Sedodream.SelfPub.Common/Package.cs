namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPackage {
        MongoDB.Bson.ObjectId Id { get; set; }
        /// <summary>
        /// The name of the pacakge
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The package type, for example:
        ///     MSDeploy
        ///     Zip
        ///     git
        /// </summary>
        // string PackageType { get; set; }
        /// <summary>
        /// A string representing where the package is located. This is typically a URI.<br/>
        /// Since this is a URI it also describes the protocol which is needed to download the package.
        /// </summary>
        Uri PackageLocation { get; set; }
        /// <summary>
        /// This is a JSON string which fully represents the package, it will contain all the properties described
        /// in this object as well as potentially others.
        /// </summary>
        string PackageManifest { get; set; }
        /// <summary>
        /// Represents the version of the item which the package contains.
        /// </summary>
        string Version { get; set; }
    }

    public class Package : IPackage {
        public MongoDB.Bson.ObjectId Id { get; set; }

        public string Name { get; set; }

        // public string PackageType { get; set; }

        public Uri PackageLocation { get; set; }

        public string PackageManifest { get; set; }

        public string Version { get; set; }
    }
}
