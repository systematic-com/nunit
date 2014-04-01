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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Systematic.NUnit.Util
{
    /// <summary>
    /// <br/>Parameter Format Options:
    /// <br/> - f|format : Sets the format to use for the object property
    /// <br/> - a|align  : Sets the alignment to use for the format string
    /// <br/> - d|default : Sets the default value
    /// <br/> - f|type : Sets the type
    /// <br/>
    /// <br/>Type Options:
    /// <br/> - f|float : 128 bit Floating point value (decimal)
    /// <br/> - n|number : 64 bit Number (long)
    /// <br/> - d|date : DateTime
    /// <br/>
    /// <br/>if no type is set, string is presumed.
    /// </summary>
    /// <example>
    /// <br/> $(key) 
    /// <br/> $(key|format|default|type)
    /// <br/> $(key|format=000.000|default=50000|type=n)
    /// </example>
    public class AdvPropertyBag : IEnumerable<KeyValuePair<string, object>>
    {
        private const char ESCAPE = '\\';
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        private readonly string startPattern;
        private readonly string endPattern;

        public int FormatIndex { get; set; }
        public int DefaultIndex { get; set; }
        public int TypeIndex { get; set; }

        public object this[string key]
        {
            get { return values[key]; }
            set { values[key] = value; }
        }

        public AdvPropertyBag()
            : this("@(", ")")
        { }

        public AdvPropertyBag(string start, string end)
        {
            startPattern = start;
            endPattern = end;
            FormatIndex = 1;
            DefaultIndex = 2;
            TypeIndex = 3;
        }

        public string Replace(string input)
        {
            Parameter param;
            StringBuilder output = new StringBuilder(input);
            for (int i = 0; (param = RetreiveParameterInString(i, output)) != null; i = param.Start)
            {
                output = param.Replace(output, this);
            }
            return output.ToString();
        }

        private Parameter RetreiveParameterInString(int index, StringBuilder result)
        {
            int start = StartOfParameter(result, index);
            if (start != -1)
            {
                int end = EndOfParameter(result, start);
                if (end != -1)
                {
                    Parameter param = new Parameter(result.ToString().Substring(start + 2, (end - start - 2)), start, end, this);
                    if (values.ContainsKey(param.Key))
                    {
                        param.Value = values[param.Key];
                    }
                    return param;
                }
            }
            return null;
        }

        private int StartOfParameter(StringBuilder result, int index)
        {
            while ((index = result.ToString().IndexOf(startPattern, index)) != -1)
            {
                if (index > 1 && result[index - 1] == ESCAPE)
                {
                    if (result[index - 2] != ESCAPE)
                    {
                        result.Remove(index - 1, 1);
                    }
                    else if (result[index - 2] == ESCAPE)
                    {
                        result.Remove(index - 1, 1);
                    }
                    index++;
                    continue;
                }
                if (index > 0 && result[index - 1] == ESCAPE)
                {
                    result.Remove(index - 1, 1);
                    index++;
                    continue;
                }
                return index;
            }
            return -1;
        }

        private int EndOfParameter(StringBuilder result, int index)
        {
            while ((index = result.ToString().IndexOf(endPattern, index)) != -1)
            {
                if (index > 0 && result[index - 1] == ESCAPE)
                    continue;
                return index;
            }
            return -1;
        }

        /// <summary>
        /// Sets a value in the property bag for the given key, the value is checked for references to it self, if such referece occures a exception is thrown.
        /// </summary>
        public void Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot set value when Key is null or empty", "key");
            }

            if (value is string)
            {
                Parameter param;
                StringBuilder output = new StringBuilder(value.ToString());
                for (int i = 0; (param = RetreiveParameterInString(i, output)) != null; i = param.End)
                {
                    if (param.Key == key)
                    {
                        throw new ArgumentException("Value cannot contain a reference to it self");
                    }
                }
            }
            values[key] = value;
        }

        public bool Contains(string key)
        {
            return values.ContainsKey(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (KeyValuePair<string, object> pair in values)
                yield return pair;
        }


        private void ApplyConfiguration(Parameter param, string[] args)
        {
            string defaultType = string.Empty, defaultValue = string.Empty;
            for (int i = 1; i < args.Length; i++)
            {
                string[] config = args[i].Split('=');
                if (config.Length > 1)
                {
                    switch (config[0].ToLower())
                    {
                        case "f":
                        case "format":
                            param.Format = config[1];
                            break;
                        case "a":
                        case "align":
                            param.Align = int.Parse(config[1]);
                            break;
                        case "d":
                        case "default":
                            defaultValue = config[1];
                            break;
                        case "t":
                        case "type":
                            defaultType = config[1];
                            break;
                    }
                }
                else
                {
                    if (i == FormatIndex)
                        param.Format = config[0];
                    if (i == DefaultIndex)
                        param.Format = config[0];
                    if (i == TypeIndex)
                        param.Format = config[0];
                }
            }
            param.Default = ParseDefault(defaultType, defaultValue);
        }

        private static object ParseDefault(string type, string value)
        {
            switch (type.ToLower())
            {
                case "":
                    return value;
                case "f":
                case "float":
                    if (value == string.Empty)
                        return 0m;
                    return decimal.Parse(value);
                case "n":
                case "number":
                    if (value == string.Empty)
                        return 0;
                    return long.Parse(value);
                case "d":
                case "date":
                    if (value == string.Empty)
                        return DateTime.Now;
                    return DateTime.Parse(value);
                default:
                    throw new ArgumentException("Type '" + type + "' is unknown.");
            }
        }

        #region Nested type: Parameter

        private class Parameter
        {
            public int Start { get; private set; }
            public int End { get; private set; }

            public string Key { get; set; }
            public string Format { get; set; }

            public object Value { get; set; }
            public object Default { get; set; }

            public int Align { get; set; }
            public int Lenght { get; private set; }

            public Parameter(string key, int start, int end, AdvPropertyBag bag)
            {
                Lenght = key.Length + 3;
                string[] args = key.Split('|');
                Key = args[0];
                End = end;
                Start = start;

                bag.ApplyConfiguration(this, args);
            }

            public StringBuilder Replace(StringBuilder param, AdvPropertyBag properties)
            {
                if (Value != null)
                {
                    return InternalReplace(param, properties, Value);
                }
                return InternalReplace(param, properties, Default);
            }

            private StringBuilder InternalReplace(StringBuilder param, AdvPropertyBag properties, object value)
            {
                if (value is string)
                {
                    value = properties.Replace(value.ToString());
                }
                param.Remove(Start, Lenght);
                param.Insert(Start, string.Format(GennerateFormatString(), value));
                return param;
            }

            private string GennerateFormatString()
            {
                string format = "0";
                if (Align != 0) format += "," + Align;
                if (Format != string.Empty) format += ":" + Format; //TODO: Support more complex formats.
                return "{" + format + "}";
            }

            public override string ToString()
            {
                return Key;
            }
        }

        #endregion
    }
}

