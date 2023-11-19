using Settings;
using UnityEngine;

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
        public IntSetting Skin = new IntSetting(0);
        public IntSetting Costume = new IntSetting(0);
        public IntSetting Cape = new IntSetting(0);
        public IntSetting Logo = new IntSetting(0);
        public ColorSetting HairColor = new ColorSetting();

        // stats
        public IntSetting Speed = new IntSetting(110, minValue: 100, maxValue: 150);
        public IntSetting Gas = new IntSetting(115, minValue: 100, maxValue: 150);
        public IntSetting Blade = new IntSetting(110, minValue: 100, maxValue: 150);
        public IntSetting Acceleration = new IntSetting(115, minValue: 100, maxValue: 150);

        protected override bool Validate()
        {
            return Speed.Value + Gas.Value + Blade.Value + Acceleration.Value <= 450;
        }
    }

    public enum HumanSex
    {
        Male,
        Female
    }
}
