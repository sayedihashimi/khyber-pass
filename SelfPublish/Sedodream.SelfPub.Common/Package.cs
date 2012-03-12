namespace Sedodream.SelfPub.Common {
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

    public class Package {
        public MongoDB.Bson.ObjectId Id { get; set; }
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
    }
}
