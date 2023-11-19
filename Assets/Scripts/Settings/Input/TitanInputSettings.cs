namespace Settings
{
    class TitanInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "TitanInput.json"; } }
        public KeybindSetting AttackPunch = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackBody = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting AttackSlap = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting AttackGrab = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting Kick = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Sit = new KeybindSetting(new string[] { "X", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftShift", "None" });
    }
}
