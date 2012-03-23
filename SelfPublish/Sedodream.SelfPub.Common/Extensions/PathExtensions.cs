namespace Sedodream.SelfPub.Common.Extensions {
    using System;
    using System.IO;
    using System.Linq;

    public static class PathExtensions {
        public static string GetTempFileWithExtension(string extension) {
            if (string.IsNullOrEmpty(extension)) { throw new ArgumentNullException("extension"); }

            extension = extension.Trim();

            string tempFilePath = Path.GetTempPath();

            return GetTempFileWithExtension(tempFilePath, extension);
        }

        public static string GetTempFileWithExtension(string basePath, string extension) {
            if (string.IsNullOrEmpty(basePath)) { throw new ArgumentNullException("basePath"); }
            if (string.IsNullOrEmpty(extension)) { throw new ArgumentNullException("extension"); }

            basePath = basePath.Trim();
            extension = extension.Trim();
            if (string.IsNullOrEmpty(basePath)) { throw new ArgumentNullException("basePath"); }
            if (string.IsNullOrEmpty(extension)) { throw new ArgumentNullException("extension"); }

            string extWithPeriod = extension.StartsWith(".") ? extension : string.Format(".{0}", extension);

            string filename = string.Format("{0}{1}",
                new string(
                     (from c in Guid.NewGuid().ToString().ToCharArray()
                      where !("{}-".Contains(c))
                      select c).ToArray()
                  ),
                  extWithPeriod);


            string newTempFile = Path.Combine(basePath, filename);

            return newTempFile;
        }
    }
}
