namespace Sedodream.SelfPub.Test {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.Test.Helpers;

    [TestClass]
    public class TestJsonNetSearlizer {

        [TestMethod]
        public void TestJsonNetSearlizer_Searlize() {
            IJsonSearlizer searlizer = new JsonNetSearlizer();

            TestClass tc = new TestClass {
                Id = RandomDataHelper.Instance.Primitives.GetRandomLong(int.MaxValue),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };

            string expectedString = string.Format(
                @"{{""Id"":{0},""FirstName"":""{1}"",""LastName"":""{2}""}}",
                tc.Id,
                tc.FirstName,
                tc.LastName);

            string actualString = searlizer.Searlize(tc);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void TestJsonNetSearlizer_Desearlize_T() {
            TestClass expectedTc = new TestClass {
                Id = RandomDataHelper.Instance.Primitives.GetRandomLong(int.MaxValue),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };

            string actualTcAsString = string.Format(
                @"{{""Id"":{0},""FirstName"":""{1}"",""LastName"":""{2}""}}",
                expectedTc.Id,
                expectedTc.FirstName,
                expectedTc.LastName);

            IJsonSearlizer searlizer = new JsonNetSearlizer();
            TestClass actualTc = searlizer.Desearlize<TestClass>(actualTcAsString);

            Assert.AreEqual(expectedTc, actualTc);
        }
        
        private class TestClass {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public override bool Equals(object obj) {
                bool areEqual = false;
                TestClass other = obj as TestClass;
                if (other != null) {
                    if (this.Id == other.Id &&
                        string.Compare(this.FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase) == 0 &&
                        string.Compare(this.LastName, other.LastName, StringComparison.OrdinalIgnoreCase) == 0) {
                            areEqual = true;
                    }
                }

                return areEqual;
            }
            public override int GetHashCode() {
                return this.Id.GetHashCode() + FirstName.GetHashCode() + LastName.GetHashCode();
            }
        }
    }
}
