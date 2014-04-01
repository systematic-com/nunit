#region Copyright (c) 2014 Systematic
// **********************************************************************************
// The MIT License (MIT)
// 
// Copyright (c) 2014 Systematic
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// **********************************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Systematic.NUnit.Constraints
{
    /// <summary>
    /// Constraint for checking Enumerables inside Properties asserts.
    /// </summary>
    public class EnumerableEqualsConstraint : Constraint
    {
        private readonly IEnumerable<object> expected;

        private string expectedMessage;
        private string actualMessage;

        public EnumerableEqualsConstraint(IEnumerable expected)
        {
            if (expected != null)
                this.expected = expected.Cast<object>();
        }

        public override bool Matches(object actualObject)
        {
            if (ReferenceEquals(actualObject, expected))
                return true;

            //Note: If actual is null and we have passed the first if, we know that expected is not null.
            if (actualObject == null)
            {
                expectedMessage = "Collection with " + expected.Count() + " elements.";
                actualMessage = "<null>";
                return false;
            }

            IEnumerable<object> actualEnummerable = (actualObject as IEnumerable).Cast<object>();
            actual = actualEnummerable;

            //Note: If expected is null and we have passed the first if, we know that actual is not null.
            if (expected == null)
            {
                actualMessage = "Collection with " + actualEnummerable.Count() + " elements.";
                expectedMessage = "<null>";
                return false;
            }

            int actualCount = actualEnummerable.Count();
            int expectedCount = expected.Count();

            if (actualCount != expectedCount)
            {
                expectedMessage = "Collection with " + expected.Count() + " elements.";
                actualMessage = "Collection with " + actualEnummerable.Count() + " elements.";
                return false;
            }

            for (int i = 0; i < actualCount; i++)
            {
                object actualItem = actualEnummerable.ElementAt(i);
                object expectedItem = expected.ElementAt(i);
                if (HAS.Properties.EqualTo(expectedItem).Matches(actualItem))
                    continue;

                expectedMessage = "Element at [" + i + "] should be: \"" + expectedItem + "\"";
                actualMessage = actualItem.ToString();
                return false;
            }
            return true;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate(expectedMessage);
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(actualMessage);
        }
    }
}