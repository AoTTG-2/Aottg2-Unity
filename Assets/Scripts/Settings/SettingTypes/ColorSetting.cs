using System.Collections.Generic;
using UnityEngine;
using SimpleJSONFixed;
using Utility;

namespace Settings
{
    class ColorSetting: TypedSetting<Color255>
    {
        public int MinAlpha = 0;

        public ColorSetting() : base(new Color255(255, 255, 255, 255))
        { 
        }

        public ColorSetting(Color255 defaultValue, int minAlpha = 0)
        {
            MinAlpha = minAlpha;
            DefaultValue = SanitizeValue(defaultValue);
            Value = DefaultValue;
        }

        protected override Color255 SanitizeValue(Color255 value)
        {
            value.R = Mathf.Clamp(value.R, 0, 255);
            value.G = Mathf.Clamp(value.G, 0, 255);
            value.B = Mathf.Clamp(value.B, 0, 255);
            value.A = Mathf.Clamp(value.A, MinAlpha, 255);
            return value;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            array.Add(new JSONNumber(Value.R));
            array.Add(new JSONNumber(Value.G));
            array.Add(new JSONNumber(Value.B));
            array.Add(new JSONNumber(Value.A));
            return array;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            JSONArray array = json.AsArray;
            Value = new Color255(array[0].AsInt, array[1].AsInt, array[2].AsInt, array[3].AsInt);
        }
    }
}
