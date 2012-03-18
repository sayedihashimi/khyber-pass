namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common.Deploy;
    using System.Xml.XPath;
    using System.Collections;


    [TestClass]
    public class TestMSDeployHandler {
        [TestMethod]
        public void Test_CreateSetParametersXml() {
            MSDeployDeploymentParameters parameters = new MSDeployDeploymentParameters();
            parameters.Parameters.Add(@"IIS Web Application Name", @"Default Web Site/Foobar");
            parameters.Parameters.Add(@"ApplicationServices-Web.config Connection String",@"data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\aspnetdb.mdf;User Instance=true");

            MSDeployHandler handler = new MSDeployHandler();
            string filePath = handler.CreateSetParametersXml(parameters);

            Assert.IsNotNull(filePath);
            // load the xml file and make sure the parameters made it
            XDocument doc = XDocument.Load(filePath);

            Assert.IsNotNull(doc);
            var xmlParams = from e in doc.Root.Descendants("setParameter")
                            select e;

            Assert.AreEqual(2, xmlParams.Count());
            Assert.IsTrue(
                string.Compare(@"Default Web Site/Foobar",
                GetSingleValuedStringFromXPath(doc,@"/parameters/setParameter[@name='IIS Web Application Name']/@value"), StringComparison.OrdinalIgnoreCase) == 0);
            Assert.IsTrue(
                string.Compare(@"data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\aspnetdb.mdf;User Instance=true",
                GetSingleValuedStringFromXPath(doc, @"/parameters/setParameter[@name='ApplicationServices-Web.config Connection String']/@value"), StringComparison.OrdinalIgnoreCase) == 0);
        }

        internal string GetSingleValuedStringFromXPath(XDocument document, string xpath) {
            if (document == null) { throw new ArgumentNullException("document"); }
            if (string.IsNullOrEmpty(xpath)) { throw new ArgumentNullException("xpath"); }

            IEnumerable attributes = document.XPathEvaluate(xpath) as IEnumerable;
            return attributes.Cast<XAttribute>().Single().Value;

            //var result = (from r in document.XPathEvaluate(xpath) as IEnumerable
            //              select r).Single();


            throw new NotImplementedException();
            // return result;
        }
    }
}
