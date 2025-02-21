using Settings;
using UnityEngine;
using Characters;
using Utility;

namespace Settings
{
    class TitanCustomSet: BaseSetSetting
    {
        public IntSetting Head = new IntSetting(0);
        public IntSetting Body = new IntSetting(0);
        public IntSetting Eye = new IntSetting(0);
        public StringSetting Hair = new StringSetting("HairM0");
        public ColorSetting SkinColor = new ColorSetting(new Color255(255, 255, 255));
        public ColorSetting HairColor = new ColorSetting(new Color255(128, 128, 128));
    }
}
