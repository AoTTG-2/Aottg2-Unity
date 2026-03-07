namespace Settings
{
    class GeneralInputSettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "GeneralInput.json"; } }
        public KeybindSetting Forward = new KeybindSetting(new string[] { "W", "None" });
        public KeybindSetting Back = new KeybindSetting(new string[] { "S", "None" });
        public KeybindSetting Left = new KeybindSetting(new string[] { "A", "None" });
        public KeybindSetting Right = new KeybindSetting(new string[] { "D", "None" });
        public KeybindSetting Up = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting Down = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting Modifier = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting Autorun = new KeybindSetting(new string[] { "Period", "None" });
        public KeybindSetting Pause = new KeybindSetting(new string[] { "P", "None" });
        public KeybindSetting ChangeCharacter = new KeybindSetting(new string[] { "T", "None" });
        public KeybindSetting RestartGame = new KeybindSetting(new string[] { "F5", "None" });
        public KeybindSetting ToggleScoreboard = new KeybindSetting(new string[] { "Tab", "None" });
        public KeybindSetting ToggleMap = new KeybindSetting(new string[] { "M", "None" });
        public KeybindSetting Chat = new KeybindSetting(new string[] { "Return", "None" });
        public KeybindSetting PushToTalk = new KeybindSetting(new string[] { "V", "None" });
        public KeybindSetting ChangeCamera = new KeybindSetting(new string[] { "C", "None" });
        public KeybindSetting HideCursor = new KeybindSetting(new string[] { "X", "None" });
        //public KeybindSetting MinimapMaximize = new KeybindSetting(new string[] { "M", "None" });
        public KeybindSetting SpectatePreviousPlayer = new KeybindSetting(new string[] { "1", "None" });
        public KeybindSetting SpectateNextPlayer = new KeybindSetting(new string[] { "2", "None" });
        public KeybindSetting SkipCutscene = new KeybindSetting(new string[] { "Y", "None" });
        public BoolSetting TapScoreboard = new BoolSetting(true);
        public BoolSetting TapMap = new BoolSetting(true);
        public KeybindSetting HideUI = new KeybindSetting(new string[] { "F6", "None" });
        public KeybindSetting DebugWindow = new KeybindSetting(new string[] { "F11", "None" });
    }
}
