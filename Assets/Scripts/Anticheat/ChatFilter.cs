using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Anticheat
{
    static class ChatFilter
    {
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
