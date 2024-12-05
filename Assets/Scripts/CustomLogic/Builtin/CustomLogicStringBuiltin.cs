using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicStringBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicStringBuiltin() : base("String")
        {
        }

        [CLProperty("Gets the newline character.")]
        public static string Newline => "\n";

        [CLMethod("Formats a float to a string with the specified number of decimal places.")]
        public string FormatFloat(object value, int decimals)
        {
            return Util.FormatFloat(value.UnboxToFloat(), decimals);
        }

        [CLMethod("Splits a string by the specified separator.")]
        public CustomLogicListBuiltin Split(string toSplit, string splitStr)
        {
            char splitChar = splitStr[0];
            CustomLogicListBuiltin list = new CustomLogicListBuiltin();
            foreach (string str in toSplit.Split(splitChar))
                list.List.Add(str);
            return list;
        }

        [CLMethod("Joins a list of strings into a single string with the specified separator.")]
        public string Join(CustomLogicListBuiltin list, string separator)
        {
            List<string> strList = new List<string>();
            foreach (var obj in list.List)
            {
                strList.Add((string)obj);
            }
            return string.Join(separator, strList.ToArray());
        }

        [CLMethod("Gets a substring starting from the specified index.")]
        public string Substring(string str, int startIndex)
        {
            return str.Substring(startIndex);
        }

        [CLMethod("Gets a substring of the specified length starting from the specified index.")]
        public string SubstringWithLength(string str, int startIndex, int length)
        {
            return str.Substring(startIndex, length);
        }

        [CLMethod("Gets the length of the string.")]
        public int Length(string str)
        {
            return str.Length;
        }

        [CLMethod("Replaces all occurrences of a substring with another substring.")]
        public string Replace(string str, string replace, string with)
        {
            return str.Replace(replace, with);
        }

        [CLMethod("Checks if the string contains the specified substring.")]
        public bool Contains(string str, string contains)
        {
            return str.Contains(contains);
        }

        [CLMethod("Checks if the string starts with the specified substring.")]
        public bool StartsWith(string str, string startsWith)
        {
            return str.StartsWith(startsWith);
        }

        [CLMethod("Checks if the string ends with the specified substring.")]
        public bool EndsWith(string str, string endsWith)
        {
            return str.EndsWith(endsWith);
        }

        [CLMethod("Trims whitespace from the start and end of the string.")]
        public string Trim(string str)
        {
            return str.Trim();
        }

        [CLMethod("Inserts a substring at the specified index.")]
        public string Insert(string str, string insert, int index)
        {
            return str.Insert(index, insert);
        }

        [CLMethod("Capitalizes the first letter of the string.")]
        public string Capitalize(string str)
        {
            return str.UpperFirstLetter();
        }

        [CLMethod("Converts the string to uppercase.")]
        public string ToUpper(string str)
        {
            return str.ToUpper();
        }

        [CLMethod("Converts the string to lowercase.")]
        public string ToLower(string str)
        {
            return str.ToLower();
        }
    }
}
