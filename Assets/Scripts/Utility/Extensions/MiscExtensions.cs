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
    static readonly Regex HexRegex = new Regex(HexPattern);
    static readonly Regex ColorTagRegex = new Regex(ColorTagPattern);

    public static bool GetActive(this GameObject target)
    {
        return target.activeInHierarchy;
    }

    public static string ToDisplayString(this Vector3 vector)
    {
        return "(" + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString() + ")";
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

    public static string ForceWhiteColorTag(this string text)
    {
        return ColorTagRegex.Replace(text, "<color=#FFFFFF>");
    }

    public static string HexColor(this string text)
    {
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
}
