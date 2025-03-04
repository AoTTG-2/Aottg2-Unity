using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ApplicationManagers;
using SimpleJSONFixed;
using Utility;

namespace Anticheat
{
    static class ChatFilter
    {
        static List<Regex> _bannedRegex = new List<Regex>();
        static List<char> _bannedChar = new List<char>();

        public static void Init()
        {
            var info = JSON.Parse(ResourceManager.TryLoadText(ResourcePaths.Info, "ChatFilterInfo"));
            foreach (JSONNode item in info["banned_regex"].AsArray)
            {
                var regex = new Regex(item.Value, RegexOptions.Compiled);
                _bannedRegex.Add(regex);
            }
            foreach (JSONNode item in info["banned_char"].AsArray)
                _bannedChar.Add(item.Value.ToCharArray()[0]);
        }

        public static string FilterBadWords(this string text)
        {
            foreach (var regex in _bannedRegex)
                text = regex.Replace(text, match => new string('*', match.Length));
            foreach (var character in _bannedChar)
                text = text.Replace(character, '*');
            return text;
        }

        public static string FilterSizeTag(this string text)
        {
            MatchCollection matches = Regex.Matches(text.ToLower(), @"(<size=(.*?>))");
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
    }
}
