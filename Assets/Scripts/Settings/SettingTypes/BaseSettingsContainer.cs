using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleJSONFixed;
using System.Collections.Specialized;

namespace Settings
{
    abstract class BaseSettingsContainer: BaseSetting
    {
        public OrderedDictionary Settings = new OrderedDictionary();
        public Dictionary<string, BaseSetting> TypedSettings = new Dictionary<string, BaseSetting>();

        public BaseSettingsContainer()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            RegisterSettings();
            Apply();
        }

        protected void RegisterSettings()
        {
            foreach (FieldInfo field in GetType().GetFields().Where(field => field.FieldType.IsSubclassOf(typeof(BaseSetting))))
            {
                var setting = (BaseSetting)field.GetValue(this);
                Settings.Add(field.Name, setting);

                if (IsTypedSetting(field.FieldType))
                {
                    TypedSettings.Add(field.Name, setting);
                }
            }

            static bool IsTypedSetting(System.Type settingType)
            {
                var generic = typeof(TypedSetting<>);
                return settingType.BaseType.IsGenericType && settingType.BaseType.GetGenericTypeDefinition() == generic;
            }
        }

        public override void SetDefault()
        {
            foreach (BaseSetting setting in Settings.Values)
                setting.SetDefault();
        }

        public virtual void Apply()
        {
        }

        public override JSONNode SerializeToJsonObject()
        {
            JSONObject root = new JSONObject();
            foreach (string name in Settings.Keys)
                root.Add(name, ((BaseSetting)Settings[name]).SerializeToJsonObject());
            return root;
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            JSONObject root = (JSONObject)json;
            foreach (string name in Settings.Keys)
            {
                if (root[name] != null)
                    ((BaseSetting)Settings[name]).DeserializeFromJsonObject(root[name]);
            }
            if (!Validate())
                SetDefault();
        }

        protected virtual bool Validate()
        {
            return true;
        }
    }
}
