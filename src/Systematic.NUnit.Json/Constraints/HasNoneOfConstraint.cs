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
using System.Linq;
using Systematic.NUnit.Constraints;
using Newtonsoft.Json.Linq;

namespace Systematic.NUnit.Json.Constraints
{
    public class HasNoneOfConstraint<T> : AbstractConstraint where T : JToken
    {
        private readonly IEnumerable<T> expectedCollection;

        public HasNoneOfConstraint(IEnumerable<T> expected)
        {
            this.expectedCollection = expected;
        }

        protected override void DoMatches(object actual)
        {
            IEnumerable<T> actualCollection = actual as IEnumerable<T>;
            if (actualCollection == null) 
                return;

            foreach (var item in actualCollection.Where(item => expectedCollection.Contains(item, new JObjectEqualityComparer())))
            {
                FailWithMessage("Actual collection did contain same item: '{0}' as the expected collection", item);
            }
        }

        public class JObjectEqualityComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return JToken.DeepEquals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}