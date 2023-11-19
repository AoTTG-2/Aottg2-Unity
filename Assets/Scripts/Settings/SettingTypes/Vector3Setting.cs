using System.Collections.Generic;
using UnityEngine;
using SimpleJSONFixed;
using Utility;

namespace Settings
{
    class Vector3Setting: TypedSetting<Vector3>
    {
        public Vector3Setting() : base(new Vector3(0, 0, 0))
        { 
        }

        public Vector3Setting(Vector3 defaultValue)
        {
            DefaultValue = SanitizeValue(defaultValue);
            Value = DefaultValue;
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONArray array = new JSONArray();
            array.Add(new JSONNumber(Value.x));
            array.Add(new JSONNumber(Value.y));
            array.Add(new JSONNumber(Value.z));
            return array;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            JSONArray array = json.AsArray;
            Value = new Vector3(array[0].AsFloat, array[1].AsFloat, array[2].AsFloat);
        }
    }
}
