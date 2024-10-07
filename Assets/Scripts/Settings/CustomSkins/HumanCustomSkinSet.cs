namespace Settings
{
    class HumanCustomSkinSet : BaseSetSetting
    {
        public StringSetting Hair = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Eye = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Glass = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Face = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Skin = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Costume = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Logo = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting GearL = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting GearR = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Gas = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Hoodie = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting WeaponTrail = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Horse = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting ThunderspearL = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting ThunderspearR = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting HookL = new StringSetting(string.Empty, maxLength: 200);
        public FloatSetting HookLTiling = new FloatSetting(1f);
        public StringSetting HookR = new StringSetting(string.Empty, maxLength: 200);
        public FloatSetting HookRTiling = new FloatSetting(1);
    }
}
