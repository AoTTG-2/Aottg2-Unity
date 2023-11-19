using SimpleJSONFixed;
using System.Collections.Generic;

namespace Settings
{
    class InGameModeSettings : BaseSettingsContainer
    {
        public Dictionary<string, BaseSetting> Current = new Dictionary<string, BaseSetting>();
        public ListSetting<StringSetting> Names = new ListSetting<StringSetting>();
        public ListSetting<StringSetting> Values = new ListSetting<StringSetting>();
        public ListSetting<StringSetting> Types = new ListSetting<StringSetting>();

        public override JSONNode SerializeToJsonObject()
        {
            Names.Clear();
            Values.Clear();
            Types.Clear();
            foreach (string name in Current.Keys)
            {
                Names.AddItem(new StringSetting(name));
                Values.AddItem(new StringSetting(Current[name].SerializeToJsonString()));
                Types.AddItem(new StringSetting(SettingsUtil.GetSettingType(Current[name]).ToString()));
            }
            return base.SerializeToJsonObject();
        }

        public override void DeserializeFromJsonObject(JSONNode json)
        {
            base.DeserializeFromJsonObject(json);
            Current.Clear();
            for (int i = 0; i < Names.GetCount(); i++)
            {
                BaseSetting value = (BaseSetting)SettingsUtil.DeserializeValueFromJson(Types.Value[i].Value.ToEnum<SettingType>(), Values.Value[i].Value);
                Current.Add(Names.Value[i].Value, value);
            }
        }
    }
}