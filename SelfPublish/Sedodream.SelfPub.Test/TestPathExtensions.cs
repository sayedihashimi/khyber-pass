namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common.Extensions;

    [TestClass]
    public class TestPathExtensions {
        [TestMethod]
        public void Test_GetTempFileWithExtension_WithPeriod() {
            string filePath = PathExtensions.GetTempFileWithExtension(".xml");

            Assert.IsNotNull(filePath);
            Assert.IsTrue(filePath.EndsWith(@".xml"));
            Assert.IsFalse(filePath.EndsWith(@"..xml"));
        }

        [TestMethod]
        public void Test_GetTempFileWithExtension_WithoutPeriod() {
            string filePath = PathExtensions.GetTempFileWithExtension("xml");

            Assert.IsNotNull(filePath);
            Assert.IsTrue(filePath.EndsWith(@".xml"));
            Assert.IsFalse(filePath.EndsWith(@"..xml"));
        }

        [TestMethod]
        public void Test_GetTempFileWithExtension_BasePath_WithPeriod() {
            string basePath = Path.Combine(@"C:\temp\foobar\", Guid.NewGuid().ToString());

            string filePath = PathExtensions.GetTempFileWithExtension(basePath, ".xml");

            Assert.IsNotNull(filePath);
            Assert.IsTrue(filePath.StartsWith(basePath));
            Assert.IsTrue(filePath.EndsWith(".xml"));
            Assert.IsFalse(filePath.EndsWith("..xml"));
        }

        [TestMethod]
        public void Test_GetTempFileWithExtension_BasePath_WithoutPeriod() {
            string basePath = Path.Combine(@"C:\temp\foobar\", Guid.NewGuid().ToString());

            string filePath = PathExtensions.GetTempFileWithExtension(basePath, "xml");

            Assert.IsNotNull(filePath);
            Assert.IsTrue(filePath.StartsWith(basePath));
            Assert.IsTrue(filePath.EndsWith(".xml"));
            Assert.IsFalse(filePath.EndsWith("..xml"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullExceptio))]
        public void Test_GetTempFileWithExtension_NullBasePath() {
            PathExtensions.GetTempFileWithExtension(null, ".zip");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_GetTempFileWithExtension_NullExtension() {
            PathExtensions.GetTempFileWithExtension(@"C:\temp", null);
        }
    }
}
