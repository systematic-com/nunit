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
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Systematic.NUnit.Constraints
{
    public class ObjectPropertiesNotEqualsConstraint<T> : ObjectPropertiesEqualsConstraint<T>
    {
        private bool strict;

        #region Ctor

        public ObjectPropertiesNotEqualsConstraint(T expected) : base(expected)
        {
        }

        public ObjectPropertiesNotEqualsConstraint(T expected, HashSet<object> references) : base(expected, references)
        {
        }

        #endregion

        #region Initialise

        protected override Constraint SetupPrimitive(object expected)
        {
            return Is.Not.EqualTo(expected);
        }

        #endregion

        #region Constraint members

        public override bool Matches(object actualObject)
        {
            if(base.Matches(actualObject))
                return false;

            if(strict)
            {
                foreach (Property property in propertyMap.Values)
                {
                    try
                    {
                        property.Actual = property.Info.GetValue(actual, null);
                        property.Expected = property.Info.GetValue(Expected, null);
                        if (property.Matches)
                            return false;
                    }
                    catch (TargetException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Checks all properties and if any one of them matches the constraint fails.
        /// </summary>
        public ObjectPropertiesNotEqualsConstraint<T> Strict()
        {
            strict = true;
            return this;
        }
    }
}
