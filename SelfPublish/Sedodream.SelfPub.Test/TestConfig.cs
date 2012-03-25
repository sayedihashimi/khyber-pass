namespace Sedodream.SelfPub.Test {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;

    [TestClass]
    public class TestConfig {
        [TestMethod]
        public void Test_GetAString() {
            const string keyName = "configTestString";

            string expectedValue = ConfigurationManager.AppSettings[keyName];
            string actualValue = new Config().GetConfigSetting<string>(keyName);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Test_GetAsString_DefaultValue() {
            const string keyName = "non-existant-key";

            string defaultValue = "some default value";
            string actualValue = new Config().GetConfigSetting<string>(keyName, defaultValue);

            Assert.AreEqual(defaultValue, actualValue);
        }

        [TestMethod]
        public void Test_GetAsInt() {
            const string keyName = "configTestInt";

            int expectedValue = Convert.ToInt32(ConfigurationManager.AppSettings[keyName]);
            int actualValue = new Config().GetConfigSetting<int>(keyName);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Test_GetAsInt_DefaultValue() {
            const string keyName = "non-existant-key";

            int defaultValue = 586;
            int actualValue = new Config().GetConfigSetting<int>(keyName, defaultValue);

            Assert.AreEqual(defaultValue, actualValue);
        }

        [TestMethod]
        public void Test_GetAsLong() {
            const string keyName = "configTestInt";

            long expectedValue = Convert.ToInt64(ConfigurationManager.AppSettings[keyName]);
            long actualValue = new Config().GetConfigSetting<long>(keyName);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Test_GetAsBool() {
            const string keyName = "configTestBool";

            bool expectedValue = Convert.ToBoolean(ConfigurationManager.AppSettings[keyName]);
            bool actualValue = new Config().GetConfigSetting<bool>(keyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException))]
        public void Test_Required_NoValue_NoDefault() {
            const string keyName = "non-existant-key";

            new Config().GetConfigSetting<string>(keyName, required: true);
        }

        [TestMethod]
        public void Test_Required_NoValue_Default() {
            const string keyName = "non-existant-key";

            string defaultValue = "default value here";
            string actualValue = new Config().GetConfigSetting<string>(keyName, defaultValue: defaultValue, required: true);

            Assert.AreEqual(defaultValue, actualValue);
        }

        [TestMethod]
        public void Test_Required_NoValue_Default_Int() {
            const string keyName = "non-existant-key";

            int defaultValue = 789;
            int actualValue = new Config().GetConfigSetting<int>(keyName, defaultValue: defaultValue, required: true);

            Assert.AreEqual(defaultValue, actualValue);
        }
    }
}
