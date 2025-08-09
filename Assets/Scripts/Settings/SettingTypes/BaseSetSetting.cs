using SimpleJSONFixed;
using System;

namespace Settings
{
    abstract class BaseSetSetting : BaseSettingsContainer
    {
        public StringSetting Name = new StringSetting("Set 1");
        public BoolSetting Preset = new BoolSetting(false);
        public StringSetting UniqueId = new StringSetting(Guid.NewGuid().ToString());
    }
}
