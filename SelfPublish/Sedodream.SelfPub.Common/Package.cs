namespace Sedodream.SelfPub.Common {
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

    public class Package {
        public Package() {
            // this is the default package type
            this.PackageType = KnownPackageTypes.msdeploy.ToString();
            this.Tags = new List<string>();
        }

        public MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// This will get automatically set when the package is inserted.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Indicates what kind of package this is. For example:<br/>
        ///     msdeploy
        ///     git
        ///     tfs
        ///     fileSystem
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// The name of the pacakge
        /// </summary>
        // [Required]
        public string Name { get; set; }

        /// <summary>
        /// A string representing where the package is located. This is typically a URI.<br/>
        /// Since this is a URI it also describes the protocol which is needed to download the package.
        /// </summary>
        // [Required]
        public Uri PackageLocation { get; set; }
        
        /// <summary>
        /// This is a JSON string which fully represents the package, it will contain all the properties described
        /// in this object as well as potentially others.
        /// </summary>
        public string PackageManifest { get; set; }
        
        /// <summary>
        /// Represents the version of the item which the package contains.
        /// </summary>
        // [Required]
        public string Version { get; set; }
        
        /// <summary>
        /// An optional set of tags.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
