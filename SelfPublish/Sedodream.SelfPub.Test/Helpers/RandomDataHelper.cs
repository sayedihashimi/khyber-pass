namespace Sedodream.SelfPub.Test.Helpers {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using MongoDB.Bson;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Models;
    using Sedodream.SelfPub.ConfigService.Models.PageModels;

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

        public PackagePageModel CreateRandomPackagePageModel() {
            // this is kind of cheating but it works
            PackagePageModel ppm = ObjectMapper.Instance.Map<Package, PackagePageModel>(this.CreateRandomePackage());

            return ppm;
        }

        public IList<T> CreateRandomListOf<T>(Func<T> creator, int maxNumElements) {
            if (creator == null) { throw new System.ArgumentNullException("creator"); }

            int numElements = this.Primitives.GetRandomInt(maxNumElements);

            IList<T> result = new List<T>();
            for (int i = 0; i < maxNumElements; i++) {
                result.Add(creator());
            }
            return result;
        }
    }
}
