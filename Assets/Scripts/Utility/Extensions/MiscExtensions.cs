using GameManagers;
using Photon.Realtime;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

static class MiscExtensions
{
    static readonly string HexPattern = @"(\[)[\w]{6}(\])";
    static readonly string ColorTagPattern = @"(<color=#)[\w]{6}(\>)";
    static readonly string TagPattern = @"<\/?[^>]+>";
    static readonly string SizePattern = @"<\/?size.*?>";
    static readonly string MaterialPattern = @"<\/?material.*?>";
    static readonly string QuadPattern = @"<\/?quad.*?>";
    static readonly Regex HexRegex = new Regex(HexPattern);
    static readonly Regex ColorTagRegex = new Regex(ColorTagPattern);
    static readonly Regex TagRegex = new Regex(TagPattern);
    static readonly Regex IllegalStyleRegex = new Regex(SizePattern + "|" + MaterialPattern + "|" + QuadPattern);

    public static bool GetActive(this GameObject target)
    {
        return target.activeInHierarchy;
    }

    public static string ToDisplayString(this Vector3 vector)
    {
        return "(" + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString() + ")";
    }

    public static double UnboxToDouble(this object obj)
    {
        if (obj is int)
            return (double)(int)obj;
        return (double)obj;
    }

    public static float UnboxToFloat(this object obj)
    {
        if (obj is int)
            return (float)(int)obj;
        return (float)obj;
    }

    public static int UnboxToInt(this object obj)
    {
        if (obj is float)
            return (int)(float)obj;
        return (int)obj;
    }

    public static float MaxComponent(this Vector3 v)
    {
        return Mathf.Max(v.x, v.y, v.z);
    }

    public static string UpperFirstLetter(this string text)
    {
        if (text == string.Empty)
            return text;
        if (text.Length > 1)
            return char.ToUpper(text[0]) + text.Substring(1);
        return text.ToUpper();
    }

    public static string StripHex(this string text)
    {
        return HexRegex.Replace(text, "");
    }

    public static List<string> Tokenize(string input)
    {
        List<string> tokens = new List<string>();
        int lastIndex = 0;

        foreach (Match match in TagRegex.Matches(input))
        {
            // Add text before the tag as individual characters
            for (int i = lastIndex; i < match.Index; i++)
            {
                tokens.Add(input[i].ToString());
            }

            // Add the tag itself
            tokens.Add(match.Value);
            lastIndex = match.Index + match.Length;
        }

        // Add any remaining text after the last tag as individual characters
        for (int i = lastIndex; i < input.Length; i++)
        {
            tokens.Add(input[i].ToString());
        }

        return tokens;
    }

    public static string StripRichText(this string text)
    {
        // Remove all tags
        return TagRegex.Replace(text, "");
    }

    public static string StripIllegalRichText(this string text)
    {
        // Remove all tags that are not i, b, or color
        return IllegalStyleRegex.Replace(text, "");
    }

    public static string ForceWhiteColorTag(this string text)
    {
        return ColorTagRegex.Replace(text, "<color=#FFFFFF>");
    }

    public static string TruncateRichText(this string text, int length)
    {
        text = text.StripIllegalRichText();

        // Tokenize the text to preserve tags
        List<string> tokens = Tokenize(text);

        // Create stacks for <i>, <b>, and <color> tags
        Stack<string> openTags = new Stack<string>();

        // Iterate over the tokens, only counting the character tokens (length = 1) towards the length limit.
        int charCount = 0;
        string result = string.Empty;

        foreach (string token in tokens)
        {
            if (token.Length == 1)
            {
                charCount++;
                if (charCount > length)
                {
                    // If the length limit is reached, break out of the loop
                    break;
                }
                result += token;
            }
            else
            {
                // If the token is a tag, push its name onto the stack <i> = i, <b> = b, <color=...> = color
                if (token.StartsWith("</"))
                {
                    // If the token is a closing tag, peek at the top of the stack an check if it matches
                    string closeTag = token.Substring(2, token.Length - 3);
                    if (openTags.Count > 0 && openTags.Peek() == closeTag)
                    {
                        openTags.Pop();
                        result += token;
                    }
                }
                else if (token.StartsWith("<"))
                {
                    string openTag = token.Substring(1, token.IndexOfAny(new char[] { '=', '>' }) - 1);
                    openTags.Push(openTag);
                    result += token;
                }
            }
        }

        // Add closing tags for any open tags
        while (openTags.Count > 0)
        {
            result += "</" + openTags.Pop() + ">";
        }

        return result;
    }

    public static string HexColor(this string text)
    {
        // strip existing html
        text = text.StripRichText();

        if (text.Contains("]"))
        {
            text = text.Replace("]", ">");
        }
        bool flag2 = false;
        while (text.Contains("[") && !flag2)
        {
            int index = text.IndexOf("[");
            if (text.Length >= (index + 7))
            {
                string str = text.Substring(index + 1, 6);
                text = text.Remove(index, 7).Insert(index, "<color=#" + str);
                int length = text.Length;
                if (text.Contains("["))
                {
                    length = text.IndexOf("[");
                }
                text = text.Insert(length, "</color>");
            }
            else
            {
                flag2 = true;
            }
        }
        if (flag2)
        {
            return string.Empty;
        }
        return text;
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        if (Enum.IsDefined(typeof(T), value))
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        return default(T);
    }

    public static float ParseFloat(string str)
    {
        return float.Parse(str);
    }

    public static bool IsGray(this Color color)
    {
        return color.r == color.g && color.r == color.b && color.a == 1f;
    }

    public static bool IsGray(this Color255 color)
    {
        return color.R == color.G && color.R == color.B && color.A == 255;
    }

    public static T GetRandomItem<T>(this List<T> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static JSONNode GetRandomItem(this JSONNode list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static Color ToColor(this JSONNode array)
    {
        Color255 color = new Color255(array[0].AsInt, array[1].AsInt, array[2].AsInt, array[3].AsInt);
        return color.ToColor();
    }

    public static PlayerInfo GetPlayerInfo(this Player player)
    {
        if (InGameManager.AllPlayerInfo.ContainsKey(player.ActorNumber))
            return InGameManager.AllPlayerInfo[player.ActorNumber];
        return null;
    }

    public static string ReverseString(this string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
