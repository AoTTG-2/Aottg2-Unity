using SimpleJSONFixed;
using System;

namespace Settings
{
    class IntSetting: TypedSetting<int>
    {
        public int MinValue = int.MinValue;
        public int MaxValue = int.MaxValue;

        public IntSetting(): base(0)
        {
        }

        public IntSetting(int defaultValue, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = SanitizeValue(defaultValue);
            SetDefault();
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            Value = json.AsInt;
        }

        public override JSONNode SerializeToJsonObject()
        {
            return new JSONNumber(Value);
        }

        protected override int SanitizeValue(int value)
        {
            value = Math.Min(value, MaxValue);
            value = Math.Max(value, MinValue);
            return value;
        }
    }
}
