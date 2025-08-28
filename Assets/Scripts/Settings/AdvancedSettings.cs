using System.Collections.Generic;

namespace Settings
{
    internal class AdvancedSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "AdvancedSettings.json"; } }

        public ListSetting<StringSetting> Services = new ListSetting<StringSetting>(new List<StringSetting>()
        {
            new StringSetting("https://ip:port"),
        });
    }
}
