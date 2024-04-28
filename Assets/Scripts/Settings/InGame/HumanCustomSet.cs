using Settings;
using UnityEngine;
using Characters;
using Utility;

namespace Settings
{
    class HumanCustomSet: BaseSetSetting
    {
        // costume
        public IntSetting Sex = new IntSetting(0);
        public IntSetting Eye = new IntSetting(0);
        public StringSetting Face = new StringSetting("FaceNone");
        public StringSetting Glass = new StringSetting("GlassNone");
        public StringSetting Hair = new StringSetting("HairM0");
        public IntSetting Costume = new IntSetting(0);
        public IntSetting Boots = new IntSetting(0);
        public IntSetting Cape = new IntSetting(0);
        public IntSetting Logo = new IntSetting(0);
        public ColorSetting SkinColor = new ColorSetting(new Color255(255, 220, 196));
        public ColorSetting HairColor = new ColorSetting(new Color255(128, 128, 128));
        public ColorSetting ShirtColor = new ColorSetting(new Color255(255, 255, 255));
        public ColorSetting StrapsColor = new ColorSetting(new Color255(98, 81, 65));
		public ColorSetting PantsColor = new ColorSetting(new Color255(255, 255, 255));
		public ColorSetting JacketColor = new ColorSetting(new Color255(183, 144, 107));
		public ColorSetting BootsColor = new ColorSetting(new Color255(49, 36, 33));
        public StringSetting Stats = new StringSetting(string.Empty);

        protected override bool Validate()
        {
            if (Sex.Value == 0 && Costume.Value >= HumanSetup.CostumeMCount)
                return false;
            if (Sex.Value == 1 && Costume.Value >= HumanSetup.CostumeFCount)
                return false;
            return true;
        }
    }

    public enum HumanSex
    {
        Male,
        Female
    }
}
