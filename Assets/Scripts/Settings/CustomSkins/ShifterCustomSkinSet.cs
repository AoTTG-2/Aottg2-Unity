namespace Settings
{
    class ShifterCustomSkinSet: BaseSetSetting
    {
        public StringSetting Eren = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Annie = new StringSetting(string.Empty, maxLength: 200);
        public StringSetting Colossal = new StringSetting(string.Empty, maxLength: 200);
    }
}
