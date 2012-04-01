namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class BaseTest {
        protected IList<string> FilesToDeleteAfterTest { get; set; }

        public BaseTest() {
            this.SetupFilesToDeleteList();
        }

        ~BaseTest() {
            this.CleanUpFilesToDeleteList();
        }

        public virtual void SetupFilesToDeleteList() {
            this.FilesToDeleteAfterTest = new List<string>();
        }

        public virtual void CleanUpFilesToDeleteList() {
            if (this.FilesToDeleteAfterTest != null && this.FilesToDeleteAfterTest.Count > 0) {
                foreach (string filename in this.FilesToDeleteAfterTest) {
                    if (File.Exists(filename)) {
                        File.Delete(filename);
                    }
                }
            }

            this.FilesToDeleteAfterTest = null;
        }

        protected virtual string WriteTextToTempFile(string content) {
            if (string.IsNullOrEmpty(content)) { throw new ArgumentNullException("content"); }

            string tempFile = this.GetTempFilename(true);
            File.WriteAllText(tempFile, content);
            return tempFile;
        }

        protected virtual string GetTempFilename(bool ensureFileDoesntExist) {
            string path = Path.GetTempFileName();
            if (ensureFileDoesntExist && File.Exists(path)) {
                File.Delete(path);
            }
            this.FilesToDeleteAfterTest.Add(path);
            return path;
        }
    }
}
