namespace Settings
{
    class InteractionInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "InteractionInput.json"; } }
        public KeybindSetting Interact = new KeybindSetting(new string[] { "G", "None" });
        public KeybindSetting ItemMenu = new KeybindSetting(new string[] { "F", "None" });
        public KeybindSetting EmoteMenu = new KeybindSetting(new string[] { "N", "None" });
        public KeybindSetting MenuNext = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting QuickSelect1 = new KeybindSetting(new string[] { "Alpha1", "None" });
        public KeybindSetting QuickSelect2 = new KeybindSetting(new string[] { "Alpha2", "None" });
        public KeybindSetting QuickSelect3 = new KeybindSetting(new string[] { "Alpha3", "None" });
        public KeybindSetting QuickSelect4 = new KeybindSetting(new string[] { "Alpha4", "None" });
        public KeybindSetting QuickSelect5 = new KeybindSetting(new string[] { "Alpha5", "None" });
        public KeybindSetting QuickSelect6 = new KeybindSetting(new string[] { "Alpha6", "None" });
        public KeybindSetting QuickSelect7 = new KeybindSetting(new string[] { "Alpha7", "None" });
        public KeybindSetting QuickSelect8 = new KeybindSetting(new string[] { "Alpha8", "None" });
    }
}
