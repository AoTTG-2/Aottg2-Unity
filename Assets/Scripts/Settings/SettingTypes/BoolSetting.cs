using SimpleJSONFixed;

namespace Settings
{
    class BoolSetting: TypedSetting<bool>
   {
        public BoolSetting(): base(false)
        {
        }

        public BoolSetting(bool defaultValue): base(defaultValue)
        {
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            Value = json.AsBool;
        }

        public override JSONNode SerializeToJsonObject()
        {
            return new JSONBool(Value);
        }
    }
}
