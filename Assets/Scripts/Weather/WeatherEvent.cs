using System.Collections.Generic;
using System.Linq;
using System.Text;
using Settings;
using UnityEngine;
using Utility;

namespace Weather
{
    class WeatherEvent
    {
        static string[] AllWeatherEffects = Util.EnumToStringArrayExceptNone<WeatherEffect>();
        static string[] AllWeatherValueSelectTypes = Util.EnumToStringArrayExceptNone<WeatherValueSelectType>();

        public WeatherAction Action;
        public WeatherEffect Effect;
        public WeatherValueSelectType ValueSelectType;
        public List<object> Values = new List<object>();
        public List<float> Weights = new List<float>();

        public WeatherEvent()
        {
        }

        public object GetValue()
        {
            WeatherValueType valueType = GetValueType();
            WeatherValueSelectType selectType = ValueSelectType;
            if (selectType == WeatherValueSelectType.Constant)
                return Values[0];
            else if (selectType == WeatherValueSelectType.RandomBetween)
            {
                if (valueType == WeatherValueType.Float)
                    return Random.Range((float)Values[0], (float)Values[1]);
                else if (valueType == WeatherValueType.Int)
                    return Random.Range((int)Values[0], (int)Values[1] + 1);
                else if (valueType == WeatherValueType.Color)
                {
                    Color255 color1 = (Color255)Values[0];
                    Color255 color2 = (Color255)Values[1];
                    if (color1.IsGray() && color2.IsGray())
                    {
                        int gray = Random.Range(color1.R, color2.R);
                        return new Color255(gray, gray, gray);
                    }
                    else
                    {
                        int r = Random.Range(Mathf.Min(color1.R, color2.R), Mathf.Max(color1.R, color2.R));
                        int g = Random.Range(Mathf.Min(color1.G, color2.G), Mathf.Max(color1.G, color2.G));
                        int b = Random.Range(Mathf.Min(color1.B, color2.B), Mathf.Max(color1.B, color2.B));
                        int a = Random.Range(Mathf.Min(color1.A, color2.A), Mathf.Max(color1.A, color2.A));
                        return new Color255(r, g, b, a);
                    }
                }
                else if (valueType == WeatherValueType.Vector3)
                {
                    Vector3 v1 = (Vector3)Values[0];
                    Vector3 v2 = (Vector3)Values[1];
                    float x = Random.Range(Mathf.Min(v1.x, v2.x), Mathf.Max(v1.x, v2.x));
                    float y = Random.Range(Mathf.Min(v1.y, v2.y), Mathf.Max(v1.y, v2.y));
                    float z = Random.Range(Mathf.Min(v1.z, v2.z), Mathf.Max(v1.z, v2.z));
                    return new Vector3(x, y, z);
                }
            }
            else if (selectType == WeatherValueSelectType.RandomFromList)
            {
                return Util.GetRandomFromWeightedList(Values, Weights);
            }
            return null;
        }

        public WeatherValueType GetValueType()
        {
            switch (Action)
            {
                case WeatherAction.SetDefault:
                case WeatherAction.SetDefaultAll:
                case WeatherAction.Return:
                case WeatherAction.SetTargetDefault:
                case WeatherAction.SetTargetDefaultAll:
                case WeatherAction.BeginSchedule:
                case WeatherAction.EndSchedule:
                case WeatherAction.EndRepeat:
                    return WeatherValueType.None;
                case WeatherAction.SetTargetTime:
                case WeatherAction.SetTargetTimeAll:
                case WeatherAction.Wait:
                    return WeatherValueType.Float;
                case WeatherAction.Goto:
                case WeatherAction.Label:
                case WeatherAction.LoadSkybox:
                    return WeatherValueType.String;
                case WeatherAction.RepeatNext:
                case WeatherAction.BeginRepeat:
                    return WeatherValueType.Int;
            }
            switch (Effect)
            {
                case WeatherEffect.Daylight:
                case WeatherEffect.AmbientLight:
                case WeatherEffect.FogColor:
                case WeatherEffect.Flashlight:
                case WeatherEffect.SkyboxColor:
                    return WeatherValueType.Color;
                case WeatherEffect.DaylightIntensity:
                case WeatherEffect.FogDensity:
                case WeatherEffect.Rain:
                case WeatherEffect.Thunder:
                case WeatherEffect.Snow:
                case WeatherEffect.Wind:
                case WeatherEffect.RainForce:
                case WeatherEffect.SnowForce:
                case WeatherEffect.WindForce:
                    return WeatherValueType.Float;
                case WeatherEffect.Skybox:
                    return WeatherValueType.String;
                case WeatherEffect.DaylightDirection:
                case WeatherEffect.WindDirection:
                    return WeatherValueType.Vector3;
                default:
                    return WeatherValueType.None;
            }
        }

        public SettingType GetSettingType()
        {
            switch (GetValueType())
            {
                case WeatherValueType.Bool:
                    return SettingType.Bool;
                case WeatherValueType.Color:
                    return SettingType.Color;
                case WeatherValueType.Float:
                    return SettingType.Float;
                case WeatherValueType.Int:
                    return SettingType.Int;
                case WeatherValueType.String:
                    return SettingType.String;
                case WeatherValueType.Vector3:
                    return SettingType.Vector3;
                default:
                    return SettingType.None;
            }
        }

        public string[] SupportedWeatherEffects()
        {
            switch (Action)
            {
                case WeatherAction.SetDefault:
                case WeatherAction.SetValue:
                case WeatherAction.SetTargetValue:
                case WeatherAction.SetTargetTime:
                case WeatherAction.SetTargetDefault:
                    return AllWeatherEffects;
                default:
                    return new string[0];
            }
        }

        public bool SupportsWeatherEffects()
        {
            return SupportedWeatherEffects().Length > 0;
        }

        public string[] SupportedWeatherValueSelectTypes()
        {
            switch (GetValueType())
            {
                case WeatherValueType.Float:
                case WeatherValueType.Color:
                case WeatherValueType.Int:
                case WeatherValueType.Vector3:
                    return AllWeatherValueSelectTypes;
                case WeatherValueType.Bool:
                case WeatherValueType.String:
                    if (Action == WeatherAction.LoadSkybox || Action == WeatherAction.Label)
                        return new string[] { WeatherValueSelectType.Constant.ToString() };
                    return new string[] { WeatherValueSelectType.Constant.ToString(), WeatherValueSelectType.RandomFromList.ToString() };
                default:
                    return new string[0];
            }
        }

        public bool SupportsWeatherValueSelectTypes()
        {
            return SupportedWeatherValueSelectTypes().Length > 0;
        }
    }

    public enum WeatherAction
    {
        BeginSchedule,
        EndSchedule,
        RepeatNext,
        BeginRepeat,
        EndRepeat,
        SetDefaultAll,
        SetDefault,
        SetValue,
        SetTargetDefaultAll,
        SetTargetDefault,
        SetTargetValue,
        SetTargetTimeAll,
        SetTargetTime,
        Wait,
        Goto,
        Label,
        Return,
        LoadSkybox
    }

    public enum WeatherEffect
    {
        None,
        Daylight,
        DaylightIntensity,
        DaylightDirection,
        AmbientLight,
        Skybox,
        SkyboxColor,
        Flashlight,
        FogDensity,
        FogColor,
        Rain,
        Thunder,
        Snow,
        Wind,
        WindDirection,
        RainForce,
        SnowForce,
        WindForce
    }

    public enum WeatherValueSelectType
    {
        None,
        Constant,
        RandomBetween,
        RandomFromList
    }

    public enum WeatherValueType
    {
        None,
        Float,
        Int,
        String,
        Color,
        Bool,
        Vector3
    }

    public enum WeatherSkybox
    {
        Day1,
        Dawn1,
        Dusk1,
        Night1,
        Night2,
        Night3,
        Cloudy1,
        Storm1
    }
}
