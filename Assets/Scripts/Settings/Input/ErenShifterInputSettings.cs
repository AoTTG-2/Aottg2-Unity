namespace Settings
{
    class ErenShifterInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "ErenShifterInput.json"; } }
        public KeybindSetting Kick = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftAlt", "None" });
        public KeybindSetting AttackCombo = new KeybindSetting(new string[] { "Mouse0", "None" });
    }
}
