namespace Settings
{
    class HumanInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "HumanInput.json"; } }
        public KeybindSetting AttackDefault = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting AttackSpecial = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting HookLeft = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting HookRight = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting HookBoth = new KeybindSetting(new string[] { "None", "None" });
        public KeybindSetting Dash = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting ReelIn = new KeybindSetting(new string[] { "WheelDown", "None" });
        public KeybindSetting ReelOut = new KeybindSetting(new string[] { "WheelUp", "None" });
        public KeybindSetting Dodge = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Jump = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting Reload = new KeybindSetting(new string[] { "R", "None" });
        public KeybindSetting HorseMount = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting HorseWalk = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting HorseJump = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting NapeLock = new KeybindSetting(new string[] { "None", "None" });
        public BoolSetting DashDoubleTap = new BoolSetting(true);
        public FloatSetting ReelOutScrollSmoothing = new FloatSetting(0.2f, minValue: 0f, maxValue: 1f);
        public BoolSetting SwapTSAttackSpecial = new BoolSetting(false);
    }
}
