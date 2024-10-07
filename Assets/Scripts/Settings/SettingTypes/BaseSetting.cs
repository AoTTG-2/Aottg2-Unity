using SimpleJSONFixed;
using UnityEngine;

namespace Settings
{
    abstract class BaseSetting
    {
        public abstract void SetDefault();
        public abstract JSONNode SerializeToJsonObject();
        public abstract void DeserializeFromJsonObject(JSONNode json);
        public virtual string SerializeToJsonString()
        {
            return SerializeToJsonObject().ToString(aIndent: 4);
        }
        public virtual void DeserializeFromJsonString(string json)
        {
            DeserializeFromJsonObject(JSON.Parse(json));
        }

        public virtual void Copy(BaseSetting other)
        {
            DeserializeFromJsonObject(other.SerializeToJsonObject());
        }
    }
}
