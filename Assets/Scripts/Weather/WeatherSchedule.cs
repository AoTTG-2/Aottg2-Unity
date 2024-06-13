using Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Utility;

namespace Weather
{
    class WeatherSchedule
    {
        static Dictionary<string, WeatherAction> NameToWeatherAction = Util.EnumToDict<WeatherAction>();
        static Dictionary<string, WeatherEffect> NameToWeatherEffect = Util.EnumToDict<WeatherEffect>();
        static Dictionary<string, WeatherValueSelectType> NameToWeatherValueSelectType = Util.EnumToDict<WeatherValueSelectType>();
        public List<WeatherEvent> Events = new List<WeatherEvent>();

        public WeatherSchedule()
        {
        }

        public WeatherSchedule(string csv)
        {
            DeserializeFromCSV(csv);
        }

        public string SerializeToCSV()
        {
            List<string> lines = new List<string>();
            foreach (WeatherEvent ev in Events)
            {
                List<string> items = new List<string>();
                items.Add(ev.Action.ToString());
                if (ev.Effect != WeatherEffect.None)
                    items.Add(ev.Effect.ToString());
                if (ev.ValueSelectType != WeatherValueSelectType.None)
                {
                    if (ev.Action != WeatherAction.Label)
                        items.Add(ev.ValueSelectType.ToString());
                    if (ev.ValueSelectType == WeatherValueSelectType.RandomFromList)
                    {
                        for (int i = 0; i < ev.Values.Count; i++)
                            items.Add(SerializeRandomListValue(ev.GetValueType(), ev.Values[i], ev.Weights[i]));
                    }
                    else
                    {
                        foreach (object value in ev.Values)
                            items.Add(SerializeValue(ev.GetValueType(), value));
                    }
                }
                lines.Add(string.Join(",", items.ToArray()));
            }
            return string.Join(";\n", lines.ToArray());
        }

        public string DeserializeFromCSV(string csv)
        {
            Events.Clear();
            string[] lines = csv.Split(';');
            int lineCount = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string trim = lines[i].Trim();
                    lineCount += lines[i].Split('\n').Length - 1;
                    if (trim != string.Empty && !trim.StartsWith("//"))
                        Events.Add(DeserializeLine(trim));
                }
                catch (Exception)
                {
                    return string.Format("Import failed at line {0}", lineCount);
                }
            }
            return "";
        }

        private string SerializeValue(WeatherValueType type, object value)
        {
            string str = "";
            switch (type)
            {
                case WeatherValueType.String:
                    str = (string)value;
                    break;
                case WeatherValueType.Float:
                    str = ((float)value).ToString();
                    break;
                case WeatherValueType.Int:
                    str = ((int)value).ToString();
                    break;
                case WeatherValueType.Bool:
                    str = Convert.ToInt32((bool)value).ToString();
                    break;
                case WeatherValueType.Color:
                    str = SerializeColor((Color)value);
                    break;
                case WeatherValueType.Vector3:
                    str = SerializeVector3((Vector3)value);
                    break;
            }
            str = str.Replace(",", string.Empty);
            str = str.Replace(";", string.Empty);
            return str;
        }

        private string SerializeRandomListValue(WeatherValueType type, object value, float weight)
        {
            return SerializeValue(type, value) + "-" + weight.ToString();
        }

        private string SerializeColor(Color color)
        {
            string[] str = new string[4];
            str[0] = SerializeColorValue(color.r);
            str[1] = SerializeColorValue(color.g);
            str[2] = SerializeColorValue(color.b);
            str[3] = SerializeColorValue(color.a);
            if (color.a == 1f && color.r == color.g && color.r == color.b)
                return str[0];
            return string.Join("-", str);
        }

        private string SerializeColorValue(float value)
        {
            return ((int)(value * 255f)).ToString();
        }

        private string SerializeVector3(Vector3 v)
        {
            string[] str = new string[3];
            str[0] = v.x.ToString();
            str[1] = v.y.ToString();
            str[2] = v.z.ToString();
            return string.Join("-", str);
        }

        private WeatherEvent DeserializeLine(string line)
        {
            WeatherEvent ev = new WeatherEvent();
            string[] items = line.Split(',');
            int index = 0;
            ev.Action = NameToWeatherAction[items[index++]];
            if (ev.SupportsWeatherEffects())
                ev.Effect = NameToWeatherEffect[items[index++]];
            if (ev.Action == WeatherAction.Label)
                ev.ValueSelectType = WeatherValueSelectType.Constant;
            else if (ev.SupportsWeatherValueSelectTypes())
                ev.ValueSelectType = NameToWeatherValueSelectType[items[index++]];
            if (ev.ValueSelectType == WeatherValueSelectType.RandomFromList)
            {
                for (int i = index; i < items.Length; i++)
                {
                    string[] weightedValue = items[i].Split('-');
                    ev.Values.Add(DeserializeValue(ev.GetValueType(), weightedValue[0]));
                    if (weightedValue.Length > 1)
                        ev.Weights.Add(float.Parse(weightedValue[1]));
                    else
                        ev.Weights.Add(1f);
                }
            }
            else
            {
                for (int i = index; i < items.Length; i++)
                    ev.Values.Add(DeserializeValue(ev.GetValueType(), items[i]));
            }
            return ev;
        }

        private object DeserializeValue(WeatherValueType type, string item)
        {
            switch (type)
            {
                case WeatherValueType.String:
                    return item;
                case WeatherValueType.Float:
                    return float.Parse(item);
                case WeatherValueType.Int:
                    return int.Parse(item);
                case WeatherValueType.Bool:
                    return Convert.ToBoolean(int.Parse(item));
                case WeatherValueType.Color:
                    return DeserializeColor(item);
                case WeatherValueType.Vector3:
                    return DeserializeVector3(item);
            }
            return null;
        }

        private Color255 DeserializeColor(string item)
        {
            string[] nums = item.Split('-');
            if (nums.Length == 1)
            {
                int num = DeserializeColorValue(nums[0]);
                return new Color255(num, num, num, 255);
            }    
            return new Color255(DeserializeColorValue(nums[0]), DeserializeColorValue(nums[1]), DeserializeColorValue(nums[2]), DeserializeColorValue(nums[3]));
        }

        private int DeserializeColorValue(string str)
        {
            return int.Parse(str);
        }

        private Vector3 DeserializeVector3(string item)
        {
            string[] nums = item.Split('-');
            return new Vector3(float.Parse(nums[0]), float.Parse(nums[1]), float.Parse(nums[2]));
        }
    }
}
