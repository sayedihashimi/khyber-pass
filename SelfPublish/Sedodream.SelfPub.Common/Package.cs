namespace Sedodream.SelfPub.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPackage {
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
        string PackageType { get; set; }
        /// <summary>
        /// A string representing where the package is located. This is typically a URI.
        /// </summary>
        string PackageLocation { get; set; }
    }

    public class Package : IPackage {
        public string Name { get; set; }

        public string PackageType { get; set; }

        public string PackageLocation { get; set; }
    }
}
