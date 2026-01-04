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
        public CustomLogicStringBuiltin(){}

        /// <summary>
        /// Returns the newline character.
        /// </summary>
        [CLProperty]
        public static string Newline => "\n";

        /// <summary>
        /// Formats a float to a string with the specified number of decimal places.
        /// </summary>
        /// <param name="val">The float value to format.</param>
        /// <param name="decimals">The number of decimal places.</param>
        /// <returns>The formatted string.</returns>
        [CLMethod]
        public static string FormatFloat(float val, int decimals)
        {
            return Util.FormatFloat(val, decimals);
        }

        /// <summary>
        /// Equivalent to C# string.format(string, List&lt;string&gt;).
        /// </summary>
        /// <param name="str">The format string.</param>
        /// <param name="list">The list of values to format.</param>
        /// <returns>The formatted string.</returns>
        [CLMethod]
        public static string FormatFromList(string str, [CLParam(Type = "List<string>")] CustomLogicListBuiltin list)
        {
            return string.Format(str, list.List.ToArray());
        }

        /// <summary>
        /// Split the string into a list. Can pass in either a string to split on or a list of strings to split on,
        /// the last optional param can remove all empty entries.
        /// </summary>
        /// <param name="toSplit">The string to split.</param>
        /// <param name="splitter">The separator string or list of separator strings.</param>
        /// <param name="removeEmptyEntries">Whether to remove empty entries from the result.</param>
        /// <returns>A list of strings.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "string" })]
        public static CustomLogicListBuiltin Split(
            string toSplit,
            [CLParam(Type = "string|List<string>")] object splitter,
            bool removeEmptyEntries = false)
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

        /// <summary>
        /// Joins a list of strings into a single string with the specified separator.
        /// </summary>
        /// <param name="list">The list of strings to join.</param>
        /// <param name="separator">The separator string.</param>
        /// <returns>The joined string.</returns>
        [CLMethod]
        public static string Join([CLParam(Type = "List<string>")] CustomLogicListBuiltin list, string separator)
        {
            var strList = new List<string>();
            foreach (var obj in list.List)
            {
                strList.Add((string)obj);
            }
            return string.Join(separator, strList.ToArray());
        }

        /// <summary>
        /// Returns a substring starting from the specified index.
        /// </summary>
        /// <param name="str">The string to get a substring from.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <returns>The substring.</returns>
        [CLMethod]
        public static string Substring(string str, int startIndex) => str.Substring(startIndex);

        /// <summary>
        /// Returns a substring of the specified length starting from the specified start index.
        /// </summary>
        /// <param name="str">The string to get a substring from.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="length">The length of the substring.</param>
        /// <returns>The substring.</returns>
        [CLMethod]
        public static string SubstringWithLength(string str, int startIndex, int length) => str.Substring(startIndex, length);

        /// <summary>
        /// Length of the string.
        /// </summary>
        /// <param name="str">The string to get the length of.</param>
        /// <returns>The length of the string.</returns>
        [CLMethod]
        public static int Length(string str) => str.Length;

        /// <summary>
        /// Replaces all occurrences of a substring with another substring.
        /// </summary>
        /// <param name="str">The string to perform replacement on.</param>
        /// <param name="replace">The substring to replace.</param>
        /// <param name="with">The replacement substring.</param>
        /// <returns>The string with replacements.</returns>
        [CLMethod]
        public static string Replace(string str, string replace, string with) => str.Replace(replace, with);

        /// <summary>
        /// Checks if the string contains the specified substring.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="match">The substring to search for.</param>
        /// <returns>True if the string contains the substring, false otherwise.</returns>
        [CLMethod]
        public static bool Contains(string str, string match) => str.Contains(match);

        /// <summary>
        /// Checks if the string starts with the specified substring.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="match">The substring to check for.</param>
        /// <returns>True if the string starts with the substring, false otherwise.</returns>
        [CLMethod]
        public static bool StartsWith(string str, string match) => str.StartsWith(match);

        /// <summary>
        /// Checks if the string ends with the specified substring.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="match">The substring to check for.</param>
        /// <returns>True if the string ends with the substring, false otherwise.</returns>
        [CLMethod]
        public static bool EndsWith(string str, string match) => str.EndsWith(match);

        /// <summary>
        /// Trims whitespace from the start and end of the string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        [CLMethod]
        public static string Trim(string str) => str.Trim();

        /// <summary>
        /// Inserts a substring at the specified index.
        /// </summary>
        /// <param name="str">The string to insert into.</param>
        /// <param name="insert">The substring to insert.</param>
        /// <param name="index">The index to insert at.</param>
        /// <returns>The string with the substring inserted.</returns>
        [CLMethod]
        public static string Insert(string str, string insert, int index) => str.Insert(index, insert);

        /// <summary>
        /// Capitalizes the first letter of the string.
        /// </summary>
        /// <param name="str">The string to capitalize.</param>
        /// <returns>The capitalized string.</returns>
        [CLMethod]
        public static string Capitalize(string str) => str.UpperFirstLetter();

        /// <summary>
        /// Converts the string to uppercase.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The uppercase string.</returns>
        [CLMethod]
        public static string ToUpper(string str) => str.ToUpper();

        /// <summary>
        /// Converts the string to lowercase.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The lowercase string.</returns>
        [CLMethod]
        public static string ToLower(string str) => str.ToLower();

        /// <summary>
        /// Returns the index of the given string.
        /// </summary>
        /// <param name="str">The string to search in.</param>
        /// <param name="substring">The substring to find.</param>
        /// <returns>The index of the substring, or -1 if not found.</returns>
        [CLMethod]
        public static int IndexOf(string str, string substring) => str.IndexOf(substring, StringComparison.Ordinal);
    }
}
