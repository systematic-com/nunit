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
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Systematic.NUnit.Test.Constraints
{
    [TestFixture]
    public class ObjectPropertiesEqualsConstraintTest
    {
        #region No Modifyers

        [Test]
        public void Matches_PrimitiveEqual_ReturnsTrue()
        {
            Constraint constraint = HAS.Properties.EqualTo("one");
            Assert.That(constraint.Matches("one"), Is.True);
        }

        [Test]
        public void Matches_PrimitiveNotEqual_ReturnsTrue()
        {
            //Note: Because primitives has no properties as such, might wan't to implement this as "Is.EqualTo" instead, but for now we accept this.
            Constraint constraint = HAS.Properties.EqualTo("one");
            Assert.That(constraint.Matches("two"), Is.True);
        }

        [Test]
        public void Matches_SimpleEqual_ReturnsTrue()
        {
            SimpleObject actual = new SimpleObject { StringValue = "Hello", Int32Value = 42 };
            SimpleObject expected = new SimpleObject { StringValue = "Hello", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected);
            Assert.That(constraint.Matches(actual), Is.True);
        }

        [Test]
        public void Matches_SimpleNotEqual_ReturnsFalse()
        {
            SimpleObject actual = new SimpleObject { StringValue = "Hello", Int32Value = 42 };
            SimpleObject expected = new SimpleObject { StringValue = "Hello World", Int32Value = 4200 };

            Constraint constraint = HAS.Properties.EqualTo(expected);
            Assert.That(constraint.Matches(actual), Is.False);
        }

        [Test]
        public void Matches_ComplexEqual_ReturnsTrue()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected);
            Assert.That(constraint.Matches(actual), Is.True);
        }

        [Test]
        public void Matches_ComplexNotEqual_ReturnsFalse()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello World", Int32Value = 4200 }, StringValue = "Hello World", Int32Value = 4200 };

            Constraint constraint = HAS.Properties.EqualTo(expected);
            Assert.That(constraint.Matches(actual), Is.False);
        } 

        #endregion

        #region Using Ignore

        [Test]
        public void Matches_SimpleEqualWithIgnore_ReturnsTrue()
        {
            SimpleObject actual = new SimpleObject { StringValue = "Hello", Int32Value = 42 };
            SimpleObject expected = new SimpleObject { StringValue = "Hello World", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.StringValue);
            Assert.That(constraint.Matches(actual), Is.True);
        }

        [Test]
        public void Matches_SimpleNotEqualWithIgnore_ReturnsFalse()
        {
            SimpleObject actual = new SimpleObject { StringValue = "Hello", Int32Value = 42 };
            SimpleObject expected = new SimpleObject { StringValue = "Hello World", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.Int32Value);
            Assert.That(constraint.Matches(actual), Is.False);
        }

        [Test]
        public void Matches_ComplexEqualWithIgnore_ReturnsTrue()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello World", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.StringValue);
            Assert.That(constraint.Matches(actual), Is.True);
        }

        [Test]
        public void Matches_ComplexNotEqualWithIgnore_ReturnsFalse()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello World", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.Int32Value);
            Assert.That(constraint.Matches(actual), Is.False);
        }

        [Test]
        public void Matches_ComplexEqualWithIgnoreComplex_ReturnsTrue()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello World", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.Simple);
            Assert.That(constraint.Matches(actual), Is.True);
        }

        [Test]
        public void Matches_ComplexNotEqualWithIgnoreComplex_ReturnsFalse()
        {
            ComplexObject actual = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello", Int32Value = 42 };
            ComplexObject expected = new ComplexObject { Simple = new SimpleObject { StringValue = "Hello", Int32Value = 42 }, StringValue = "Hello World", Int32Value = 42 };

            Constraint constraint = HAS.Properties.EqualTo(expected).Ignore(x => x.Simple);
            Assert.That(constraint.Matches(actual), Is.False);
        } 

        #endregion
    }
}