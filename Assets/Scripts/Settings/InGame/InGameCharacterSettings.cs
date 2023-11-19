namespace Settings
{
    class InGameCharacterSettings : BaseSettingsContainer
    {
        public IntSetting ChooseStatus = new IntSetting(0);
        public StringSetting CharacterType = new StringSetting(string.Empty);
        public StringSetting Loadout = new StringSetting(string.Empty);
        public StringSetting Special = new StringSetting(string.Empty);
        public IntSetting CustomSet = new IntSetting(0);
        public IntSetting Costume = new IntSetting(0);
        public StringSetting Team = new StringSetting("Blue");
    }

    public enum ChooseCharacterStatus
    {
        Choosing,
        Spectating,
        Chosen
    }
}