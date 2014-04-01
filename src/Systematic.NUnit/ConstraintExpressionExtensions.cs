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
using System;
using System.Linq.Expressions;
using Systematic.NUnit.Util;
using NUnit.Framework.Constraints;

namespace Systematic.NUnit
{
    /// <summary/>
    public static class ConstraintExpressionExtensions
    {
        /// <summary/>
        public static ResolvableConstraintExpression Property<T>(this ConstraintExpression self, Expression<Func<T, object>> expression)
        {
            return self.Property(expression.GetPropertyInfo().Name);
        }

        /// <summary/>
        public static Constraint Matches<T>(this ConstraintExpression self, Predicate<T> predicate)
        {
            return self.Matches(predicate);
        }

        /// <summary/>
        public static Constraint That(this ConstraintExpression self, IResolveConstraint constraint)
        {
            return self.Matches(constraint.Resolve());
        }
    }
}