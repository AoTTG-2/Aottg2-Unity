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
        public StringSetting Hat = new StringSetting("HatNone");
        public StringSetting Head = new StringSetting("HeadNone");
        public StringSetting Back = new StringSetting("BackNone");
        public ColorSetting SkinColor = new ColorSetting(new Color255(255, 220, 196));
        public ColorSetting HairColor = new ColorSetting(new Color255(128, 128, 128));
        public ColorSetting ShirtColor = new ColorSetting(new Color255(255, 255, 255));
        public ColorSetting StrapsColor = new ColorSetting(new Color255(98, 81, 65));
		public ColorSetting PantsColor = new ColorSetting(new Color255(255, 255, 255));
		public ColorSetting JacketColor = new ColorSetting(new Color255(183, 144, 107));
		public ColorSetting BootsColor = new ColorSetting(new Color255(49, 36, 33));
        public StringSetting Stats = new StringSetting(string.Empty);

        // Preset skin URLs
        public StringSetting SkinHair = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinEye = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinGlass = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinFace = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinSkin = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinCostume = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinLogo = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinGearL = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinGearR = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinGas = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinHoodie = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinWeaponTrail = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinHorse = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinThunderspearL = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinThunderspearR = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinHookL = new StringSetting(string.Empty, maxLength: 200);
        public FloatSetting SkinHookLTiling = new FloatSetting(1f);
        public StringSetting SkinHookR = new StringSetting(string.Empty, maxLength: 200);
        public FloatSetting SkinHookRTiling = new FloatSetting(1f);
        public StringSetting SkinHat = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinHead = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting SkinBack = new StringSetting(string.Empty, maxLength: 200);

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
