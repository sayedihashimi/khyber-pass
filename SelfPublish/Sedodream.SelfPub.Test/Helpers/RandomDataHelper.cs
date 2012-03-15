namespace Sedodream.SelfPub.Test.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;

    public class RandomDataHelper {
        private static RandomDataHelper instance = new RandomDataHelper();

        private RandomDataHelper() {
            this.Primitives = new RandomPrimitives();
        }

        public RandomPrimitives Primitives {
            get;
            private set;
        }

        public static RandomDataHelper Instance { get { return instance; } }

        public Package CreateRandomePackage() {
            Package package = new Package {
                Id = ObjectId.GenerateNewId(),
                Name = Guid.NewGuid().ToString(),
                PackageLocation = new Uri(Path.Combine(string.Format(@"C:\temp\{0}",Guid.NewGuid()), Guid.NewGuid().ToString())),
                PackageManifest = Guid.NewGuid().ToString(),
                Version = Guid.NewGuid().ToString()
            };

            for (int i = 0; i < this.Primitives.GetRandomInt(10); i++) {
                package.Tags.Add(Guid.NewGuid().ToString());
            }

            return package;
        }
    }
}
