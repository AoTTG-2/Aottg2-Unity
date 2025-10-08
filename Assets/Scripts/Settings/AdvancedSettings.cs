using System.Collections.Generic;

namespace Settings
{
    internal class AdvancedSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "AdvancedSettings.json"; } }

        /// <summary>
        /// new StringSetting("https://ip:port"),
        /// new StringSetting("https://meowfacts.herokuapp.com/"),
        /// </summary>
        public ListSetting<StringSetting> Services = new ListSetting<StringSetting>(new List<StringSetting>()
        {
        });
    }
}
