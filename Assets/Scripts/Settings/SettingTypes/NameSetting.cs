using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settings
{
    class NameSetting: StringSetting
    {
        public int MaxStrippedLength = int.MaxValue;
        public NameSetting(): base(string.Empty)
        {
        }

        public NameSetting(string defaultValue, int maxLength = int.MaxValue, int maxStrippedLength = int.MaxValue) : base(defaultValue)
        {
            MaxLength = maxLength;
            MaxStrippedLength = maxStrippedLength;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            Value = json.Value;
        }

        public override JSONNode SerializeToJsonObject()
        {
            return new JSONString(Value);
        }

        protected override string SanitizeValue(string value)
        {
            if (value.Length > MaxLength)
                return value.Substring(0, MaxLength);
            string stripped = value.StripHex();
            if (stripped.Length > MaxStrippedLength)
                return stripped.Substring(0, MaxStrippedLength);
            return value;
        }
    }
}
