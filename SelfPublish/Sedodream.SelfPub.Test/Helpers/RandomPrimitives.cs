namespace Sedodream.SelfPub.Test.Helpers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    public class RandomPrimitives {
        RandomStringGenerator StringGenerator = new RandomStringGenerator();

        public TList CreateRandomListOf<TElement, TList>(Func<TElement> elementCreator, int maxNumElements, Func<TList> listCreator)
            where TList : ICollection<TElement> {
            if (elementCreator == null) { throw new System.ArgumentNullException("elementCreator"); }
            if (listCreator == null) { throw new System.ArgumentNullException("listCreator"); }

            int numElements = this.GetRandomInt(maxNumElements);
            TList result = listCreator();
            for (int i = 0; i < numElements; i++) {
                result.Add(elementCreator());
            }

            return result;
        }

        public DateTime GetRandomDateTime() {
            // TODO: This could be better
            return DateTime.Now;
        }

        public string CreateRandomEmail(int maxLength) {
            if (maxLength < 3) { throw new ArgumentException("maxLength"); }

            int lengthFirstPart = (int)(Math.Floor(maxLength / 2m));
            // -1 to accomodate the '@'
            int lengthSecondPart = maxLength - lengthFirstPart - 1;

            string firstPart = this.GetRandomString(lengthFirstPart);
            string secondPart = this.GetRandomString(lengthSecondPart);

            return string.Format(@"{0}@{1}", firstPart, secondPart);
        }

        public bool GetRandomBool() {
            int randomNumber = this.GetRandomInt(1000);
            bool retValue = randomNumber % 2 == 1;
            return retValue;
        }

        public long GetRandomLong(long max) {
            Random random = new Random(DateTime.Now.Millisecond);

            return (long)random.Next((int)max);
        }

        public int GetRandomInt(int max) {
            Random random = new Random(DateTime.Now.Millisecond);

            return random.Next(1, max);
        }

        public string GetRandomString(int maxLength) {
            int length = this.GetRandomInt(maxLength);
            return StringGenerator.NextString(length);

            //int length = this.GetRandomInt(maxLength);
            //StringBuilder builder = new StringBuilder();
            //Random random = new Random(DateTime.Now.Millisecond);
            //char ch;
            //for (int i = 0; i < length; i++) {
            //    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            //    builder.Append(ch);
            //}

            //return builder.ToString();
        }

        public string GetRandomEmail(int maxLength) {
            Debug.Assert(maxLength > 4);
            int maxLengthPart = (int)Math.Floor(maxLength / 2.0m);

            string email = string.Format(
                "{0}@{1}.com",
                this.GetRandomString(maxLengthPart),
                this.GetRandomString(maxLengthPart));

            return email;
        }
    }
}
