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

namespace Systematic.NUnit.Util
{
    /// <summary/>
    public static class Int32Extensions
    {
        /// <summary>
        /// Creates a new TimeSpan from the provided int by invoking <see cref="TimeSpan.FromHours"/>
        /// </summary>
        public static TimeSpan Hours(this int value)
        {
            return TimeSpan.FromHours(value);
        }

        /// <summary>
        /// Creates a new TimeSpan from the provided int by invoking <see cref="TimeSpan.FromMinutes"/>
        /// </summary>
        public static TimeSpan Minutes(this int value)
        {
            return TimeSpan.FromMinutes(value);
        }

        /// <summary>
        /// Creates a new TimeSpan from the provided int by invoking <see cref="TimeSpan.FromSeconds"/>
        /// </summary>
        public static TimeSpan Seconds(this int value)
        {
            return TimeSpan.FromSeconds(value);
        }

        /// <summary>
        /// Creates a new TimeSpan from the provided int by invoking <see cref="TimeSpan.FromMilliseconds"/>
        /// </summary>
        public static TimeSpan Milliseconds(this int value)
        {
            return TimeSpan.FromMilliseconds(value);
        }
    }
}
