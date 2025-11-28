using System;
using System.Collections.Generic;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// String manipulation functions.
    /// </summary>
    [CLType(Name = "String", Static = true, Abstract = true)]
    partial class CustomLogicStringBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicStringBuiltin() { }

        [CLProperty("Returns the newline character.")]
        public static string Newline => "\n";

        [CLMethod("Formats a float to a string with the specified number of decimal places.")]
        public static string FormatFloat(float val, int decimals)
        {
            return Util.FormatFloat(val, decimals);
        }

        [CLMethod("Equivalent to C# string.format(string, List<string>).")]
        public static string FormatFromList(string str, CustomLogicListBuiltin list)
        {
            return string.Format(str, list.List.ToArray());
        }

        [CLMethod(Description = "Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.", ReturnTypeArguments = new[] { "string" })]
        public static CustomLogicListBuiltin Split(string toSplit, object splitter, bool removeEmptyEntries = false)
        {
            var options = removeEmptyEntries
                ? System.StringSplitOptions.RemoveEmptyEntries
                : System.StringSplitOptions.None;

            var list = new CustomLogicListBuiltin();
            if (splitter is string separator)
            {
                if (separator.Length == 1)
                {
                    foreach (var str in toSplit.Split(separator[0], options))
                        list.List.Add(str);
                }
                else
                {
                    foreach (var str in toSplit.Split(separator, options))
                        list.List.Add(str);
                }
            }
            else
            {
                var separatorList = (CustomLogicListBuiltin)splitter;
                var separators = new string[separatorList.List.Count];
                for (var i = 0; i < separatorList.List.Count; i++)
                    separators[i] = (string)separatorList.List[i];
                foreach (var str in toSplit.Split(separators, options))
                    list.List.Add(str);
            }
            return list;
        }

        [CLMethod("Joins a list of strings into a single string with the specified separator.")]
        public static string Join(CustomLogicListBuiltin list, string separator)
        {
            var strList = new List<string>();
            foreach (var obj in list.List)
            {
                strList.Add((string)obj);
            }
            return string.Join(separator, strList.ToArray());
        }

        [CLMethod("Returns a substring starting from the specified index.")]
        public static string Substring(string str, int startIndex) => str.Substring(startIndex);

        [CLMethod("Returns a substring of the specified length starting from the specified start index.")]
        public static string SubstringWithLength(string str, int startIndex, int length) => str.Substring(startIndex, length);

        [CLMethod("Length of the string.")]
        public static int Length(string str) => str.Length;

        [CLMethod("Replaces all occurrences of a substring with another substring.")]
        public static string Replace(string str, string replace, string with) => str.Replace(replace, with);

        [CLMethod("Checks if the string contains the specified substring.")]
        public static bool Contains(string str, string match) => str.Contains(match);

        [CLMethod("Checks if the string starts with the specified substring.")]
        public static bool StartsWith(string str, string match) => str.StartsWith(match);

        [CLMethod("Checks if the string ends with the specified substring.")]
        public static bool EndsWith(string str, string match) => str.EndsWith(match);

        [CLMethod("Trims whitespace from the start and end of the string.")]
        public static string Trim(string str) => str.Trim();

        [CLMethod("Inserts a substring at the specified index.")]
        public static string Insert(string str, string insert, int index) => str.Insert(index, insert);

        [CLMethod("Capitalizes the first letter of the string.")]
        public static string Capitalize(string str) => str.UpperFirstLetter();

        [CLMethod("Converts the string to uppercase.")]
        public static string ToUpper(string str) => str.ToUpper();

        [CLMethod("Converts the string to lowercase.")]
        public static string ToLower(string str) => str.ToLower();

        [CLMethod("Returns the index of the given string.")]
        public static int IndexOf(string str, string substring) => str.IndexOf(substring, StringComparison.Ordinal);
    }
}
