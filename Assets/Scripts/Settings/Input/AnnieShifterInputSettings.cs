namespace Settings
{
    class AnnieShifterInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "AnnieShifterInput.json"; } }
        public KeybindSetting Kick = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting Walk = new KeybindSetting(new string[] { "LeftAlt", "None" });
        public KeybindSetting AttackCombo = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackSwing = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting AttackStomp = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting AttackBite = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting AttackHead = new KeybindSetting(new string[] { "Alpha1", "None" });
        public KeybindSetting AttackBrushBack = new KeybindSetting(new string[] { "Alpha2", "None" });
        public KeybindSetting AttackBrushFront = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackBrushHead = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabBottom = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabMid = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting AttackGrabUp = new KeybindSetting(new string[] { "None", "None" });
    }
}
