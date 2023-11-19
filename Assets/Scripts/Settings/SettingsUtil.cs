using System;
using UnityEngine;
using Utility;

namespace Settings
{
    class SettingsUtil
    {
        public static void SetSettingValue(BaseSetting setting, SettingType type, object value)
        {
            if (type == SettingType.Bool)
                ((BoolSetting)setting).Value = (bool)value;
            else if (type == SettingType.Color)
                ((ColorSetting)setting).Value = (Color255)value;
            else if (type == SettingType.Float)
                ((FloatSetting)setting).Value = (float)value;
            else if (type == SettingType.Int)
                ((IntSetting)setting).Value = (int)value;
            else if (type == SettingType.String)
                ((StringSetting)setting).Value = (string)value;
            else if (type == SettingType.Vector3)
                ((Vector3Setting)setting).Value = (Vector3)value;
            else
                Debug.Log("Attempting to set invalid setting value.");
        }

        public static SettingType GetSettingType(BaseSetting setting)
        {
            Type t = setting.GetType();
            if (t == typeof(IntSetting))
                return SettingType.Int;
            if (t == typeof(FloatSetting))
                return SettingType.Float;
            if (t == typeof(StringSetting) || t == typeof(NameSetting))
                return SettingType.String;
            if (t == typeof(BoolSetting))
                return SettingType.Bool;
            if (t == typeof(KeybindSetting))
                return SettingType.Keybind;
            if (t == typeof(ColorSetting))
                return SettingType.Color;
            if (t == typeof(Vector3Setting))
                return SettingType.Vector3;
            throw new ArgumentException("Invalid setting type found.");
        }

        public static object DeserializeValueFromJson(SettingType type, string json)
        {
            BaseSetting setting = CreateBaseSetting(type);
            if (setting == null)
                return setting;
            setting.DeserializeFromJsonString(json);
            return setting;
        }

        public static BaseSetting CreateBaseSetting(SettingType type)
        {
            switch (type)
            {
                case SettingType.Bool:
                    return new BoolSetting();
                case SettingType.Int:
                    return new IntSetting();
                case SettingType.Float:
                    return new FloatSetting();
                case SettingType.Color:
                    return new ColorSetting();
                case SettingType.String:
                    return new StringSetting();
                case SettingType.Vector3:
                    return new Vector3Setting();
                default:
                    return null;
            }
        }
    }
}
