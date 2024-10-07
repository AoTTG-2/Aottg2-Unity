using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
    class FloatSetting : TypedSetting<float>
    {
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;

        public FloatSetting(): base(0f)
        {
        }

        public FloatSetting(float defaultValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            DefaultValue = SanitizeValue(defaultValue);
            SetDefault();
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            Value = json.AsFloat;
        }

        public override JSONNode SerializeToJsonObject()
        {
            return new JSONNumber(Value);
        }

        protected override float SanitizeValue(float value)
        {
            return Mathf.Clamp(value, MinValue, MaxValue);
        }
    }
}
