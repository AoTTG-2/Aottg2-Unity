using Settings;

namespace GameProgress
{
    class GameStatContainer : BaseSettingsContainer
    {
        public IntSetting Level = new IntSetting(1);
        public IntSetting Exp = new IntSetting(0);
        public FloatSetting PlayTime = new FloatSetting(0f);
        public FloatSetting HighestSpeed = new FloatSetting(0f, minValue: 0f, maxValue: 100000f);
    }
}
