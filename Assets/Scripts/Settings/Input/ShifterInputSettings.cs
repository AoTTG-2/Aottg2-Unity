namespace Settings
{
    class ShifterInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "ShifterInput.json"; } }
        public KeybindSetting Attack = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Kick = new KeybindSetting(new string[] { "LeftControl", "None" });
    }
}
