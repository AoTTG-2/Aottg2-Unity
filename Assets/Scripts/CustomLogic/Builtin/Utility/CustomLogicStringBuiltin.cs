using System;
using System.Collections.Generic;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "String", Static = true, Abstract = true, Description = "String manipulation functions.")]
    partial class CustomLogicStringBuiltin : BuiltinClassInstance
    {
        [CLConstructor("Creates a new String instance.")]
        public CustomLogicStringBuiltin() { }

        [CLProperty("Returns the newline character.")]
        public static string Newline => "\n";

        [CLMethod("Formats a float to a string with the specified number of decimal places.")]
        public static string FormatFloat(
            [CLParam("The float value to format.")]
            float val,
            [CLParam("The number of decimal places.")]
            int decimals)
        {
            return Util.FormatFloat(val, decimals);
        }

        [CLMethod("Equivalent to C# string.format(string, List<string>).")]
        public static string FormatFromList(
            [CLParam("The format string.")]
            string str,
            [CLParam("The list of values to format.", Type = "List<string>")]
            CustomLogicListBuiltin list)
        {
            return string.Format(str, list.List.ToArray());
        }

        [CLMethod(ReturnTypeArguments = new[] { "string" }, Description = "Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.")]
        public static CustomLogicListBuiltin Split(
            [CLParam("The string to split.")]
            string toSplit,
            [CLParam("The separator string or list of separator strings.", Type = "string|List<string>")]
            object splitter,
            [CLParam("Whether to remove empty entries from the result.")]
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

        [CLMethod("Joins a list of strings into a single string with the specified separator.")]
        public static string Join(
            [CLParam("The list of strings to join.", Type = "List<string>")]
            CustomLogicListBuiltin list,
            [CLParam("The separator string.")]
            string separator)
        {
            var strList = new List<string>();
            foreach (var obj in list.List)
            {
                strList.Add((string)obj);
            }
            return string.Join(separator, strList.ToArray());
        }

        [CLMethod("Returns a substring starting from the specified index.")]
        public static string Substring(
            [CLParam("The string to get a substring from.")]
            string str,
            [CLParam("The starting index.")]
            int startIndex) => str.Substring(startIndex);

        [CLMethod("Returns a substring of the specified length starting from the specified start index.")]
        public static string SubstringWithLength(
            [CLParam("The string to get a substring from.")]
            string str,
            [CLParam("The starting index.")]
            int startIndex,
            [CLParam("The length of the substring.")]
            int length) => str.Substring(startIndex, length);

        [CLMethod("Length of the string.")]
        public static int Length(
            [CLParam("The string to get the length of.")]
            string str) => str.Length;

        [CLMethod("Replaces all occurrences of a substring with another substring.")]
        public static string Replace(
            [CLParam("The string to perform replacement on.")]
            string str,
            [CLParam("The substring to replace.")]
            string replace,
            [CLParam("The replacement substring.")]
            string with) => str.Replace(replace, with);

        [CLMethod("Checks if the string contains the specified substring.")]
        public static bool Contains(
            [CLParam("The string to check.")]
            string str,
            [CLParam("The substring to search for.")]
            string match) => str.Contains(match);

        [CLMethod("Checks if the string starts with the specified substring.")]
        public static bool StartsWith(
            [CLParam("The string to check.")]
            string str,
            [CLParam("The substring to check for.")]
            string match) => str.StartsWith(match);

        [CLMethod("Checks if the string ends with the specified substring.")]
        public static bool EndsWith(
            [CLParam("The string to check.")]
            string str,
            [CLParam("The substring to check for.")]
            string match) => str.EndsWith(match);

        [CLMethod("Trims whitespace from the start and end of the string.")]
        public static string Trim(
            [CLParam("The string to trim.")]
            string str) => str.Trim();

        [CLMethod("Inserts a substring at the specified index.")]
        public static string Insert(
            [CLParam("The string to insert into.")]
            string str,
            [CLParam("The substring to insert.")]
            string insert,
            [CLParam("The index to insert at.")]
            int index) => str.Insert(index, insert);

        [CLMethod("Capitalizes the first letter of the string.")]
        public static string Capitalize(
            [CLParam("The string to capitalize.")]
            string str) => str.UpperFirstLetter();

        [CLMethod("Converts the string to uppercase.")]
        public static string ToUpper(
            [CLParam("The string to convert.")]
            string str) => str.ToUpper();

        [CLMethod("Converts the string to lowercase.")]
        public static string ToLower(
            [CLParam("The string to convert.")]
            string str) => str.ToLower();

        [CLMethod("Returns the index of the given string.")]
        public static int IndexOf(
            [CLParam("The string to search in.")]
            string str,
            [CLParam("The substring to find.")]
            string substring) => str.IndexOf(substring, StringComparison.Ordinal);
    }
}
