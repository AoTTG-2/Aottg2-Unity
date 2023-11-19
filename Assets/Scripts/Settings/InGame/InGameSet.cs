using System.Collections.Generic;

namespace Settings
{
    class InGameSet : BaseSetSetting
    {
        public InGameGeneralSettings General = new InGameGeneralSettings();
        public InGameModeSettings Mode = new InGameModeSettings();
        public InGameTitanSettings Titan = new InGameTitanSettings();
        public InGameMiscSettings Misc = new InGameMiscSettings();
        public InGameInternalSettings Internal = new InGameInternalSettings();
        public IntSetting WeatherIndex = new IntSetting(0);
    }
}