using CustomLogic;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Anticheat
{
    static class ChatFilter
    {
        // Case insensitive regex patterns to match rich text tags
        const string boldTagPattern = "<b>";
        const string colorTagPattern = @"<color=(.*?>)";
        const string italicsTagPattern = "<i>";
        const string sizeTagPattern = @"<size=(.*?>)";

        // Default tags to use when missing tags
        const string defaultBoldTag = "<b>";
        const string defaultColorTag = "<color=white>";
        const string defaultitalicsTag = "<i>";
        const string defaultSizeTag = "<size=18>";

        // Rich text closing tags
        const string boldCloseTag = "</b>";
        const string colorCloseTag = "</color>";
        const string italicsCloseTag = "</i>";
        const string sizeCloseTag = "</size>";

        public static string FilterText(this string text)
        {
            text = text.FilterSizeTag();
            text = text.BalanceTags();
            return text;
        }

        public static string FilterSizeTag(this string text)
        {
            MatchCollection matches = Regex.Matches(text, sizeTagPattern, RegexOptions.IgnoreCase);
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            foreach (Match match in matches)
            {
                if (list.Any(p => p.Key == match.Index))
                {
                    continue;
                }
                list.Add(new KeyValuePair<int, string>(match.Index, match.Value));
            }
            foreach (KeyValuePair<int, string> pair in list)
            {
                if (pair.Value.StartsWith("<size="))
                {
                    if (pair.Value.Length > 9)
                    {
                        text = text.Remove(pair.Key, pair.Value.Length);
                        text = text.Substring(0, pair.Key) + "<size=20>" + text.Substring(pair.Key, text.Length - pair.Key);
                    }
                }
            }
            return text;
        }

        // Ensure that all tags are balanced by adding new tags to the beginning or end of the text
        public static string BalanceTags(this string text)
        {
            text = BalanceTags(text, boldTagPattern, defaultBoldTag, boldCloseTag);
            text = BalanceTags(text, colorTagPattern, defaultColorTag, colorCloseTag);
            text = BalanceTags(text, italicsTagPattern, defaultitalicsTag, italicsCloseTag);
            text = BalanceTags(text, sizeTagPattern, defaultSizeTag, sizeCloseTag);
            return text;
        }

        private static string BalanceTags(string text, string openingTagPattern, string defaultTag, string closingTag)
        {
            int openingTagCount = Regex.Matches(text, openingTagPattern, RegexOptions.IgnoreCase).Count;
            int closingTagCount = Regex.Matches(text, closingTag, RegexOptions.IgnoreCase).Count;

            if (closingTagCount > openingTagCount)
            {
                int missingTagCount = closingTagCount - openingTagCount;
                string newTagsString = string.Concat(Enumerable.Repeat(defaultTag, missingTagCount));
                return string.Concat(newTagsString, text);
            }
            else if (closingTagCount < openingTagCount)
            {
                int missingTagCount = openingTagCount - closingTagCount;
                string newTagsString = string.Concat(Enumerable.Repeat(closingTag, missingTagCount));
                return string.Concat(text, newTagsString);
            }
            else
            {
                return text;
            }
        }
    }
}
