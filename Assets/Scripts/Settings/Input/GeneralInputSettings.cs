using UnityEngine;

namespace Settings
{
    class GeneralInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "GeneralInput.json"; } }
        public KeybindSetting Forward = new KeybindSetting(new string[] { "W", "None" });
        public KeybindSetting Back = new KeybindSetting(new string[] { "S", "None" });
        public KeybindSetting Left = new KeybindSetting(new string[] { "A", "None" });
        public KeybindSetting Right = new KeybindSetting(new string[] { "D", "None" });
        public KeybindSetting Pause = new KeybindSetting(new string[] { "P", "None" });
        public KeybindSetting ChangeCharacter = new KeybindSetting(new string[] { "T", "None" });
        public KeybindSetting RestartGame = new KeybindSetting(new string[] { "F5", "None" });
        public KeybindSetting ToggleScoreboard = new KeybindSetting(new string[] { "Tab", "None" });
        public KeybindSetting Chat = new KeybindSetting(new string[] { "Return", "None" });
        public KeybindSetting ChangeCamera = new KeybindSetting(new string[] { "C", "None" });
        public KeybindSetting HideCursor = new KeybindSetting(new string[] { "X", "None" });
        //public KeybindSetting MinimapMaximize = new KeybindSetting(new string[] { "M", "None" });
        public KeybindSetting SpectatePreviousPlayer = new KeybindSetting(new string[] { "1", "None" });
        public KeybindSetting SpectateNextPlayer = new KeybindSetting(new string[] { "2", "None" });
        public KeybindSetting SkipCutscene = new KeybindSetting(new string[] { "Y", "None" });
        public BoolSetting TapScoreboard = new BoolSetting(true);
    }
}
