namespace Settings
{
    class MapEditorInputSettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "MapEditorInput.json"; } }
        public KeybindSetting Forward = new KeybindSetting(new string[] { "W", "None" });
        public KeybindSetting Back = new KeybindSetting(new string[] { "S", "None" });
        public KeybindSetting Left = new KeybindSetting(new string[] { "A", "None" });
        public KeybindSetting Right = new KeybindSetting(new string[] { "D", "None" });
        public KeybindSetting Up = new KeybindSetting(new string[] { "E", "None" });
        public KeybindSetting Down = new KeybindSetting(new string[] { "Q", "None" });
        public KeybindSetting Slow = new KeybindSetting(new string[] { "LeftShift", "None" });
        public KeybindSetting Fast = new KeybindSetting(new string[] { "LeftAlt", "None" });
        public KeybindSetting Select = new KeybindSetting(new string[] { "Mouse0", "None" });
        public KeybindSetting Multiselect = new KeybindSetting(new string[] { "LeftControl", "None" });
        public KeybindSetting Deselect = new KeybindSetting(new string[] { "Escape", "None" });
        public KeybindSetting RotateCamera = new KeybindSetting(new string[] { "Mouse1", "None" });
        public KeybindSetting AddObject = new KeybindSetting(new string[] { "Space", "None" });
        public KeybindSetting ChangeGizmo = new KeybindSetting(new string[] { "C", "None" });
        public KeybindSetting ToggleSnap = new KeybindSetting(new string[] { "G", "None" });
        public KeybindSetting Delete = new KeybindSetting(new string[] { "Delete", "None" });
        public KeybindSetting CopyObjects = new KeybindSetting(new string[] { "LeftControl+C", "None" });
        public KeybindSetting Paste = new KeybindSetting(new string[] { "LeftControl+V", "None" });
        public KeybindSetting Cut = new KeybindSetting(new string[] { "LeftControl+X", "None" });
        public KeybindSetting Undo = new KeybindSetting(new string[] { "LeftControl+Z", "None" });
        public KeybindSetting Redo = new KeybindSetting(new string[] { "LeftControl+Y", "None" });
        public KeybindSetting SaveMap = new KeybindSetting(new string[] { "LeftControl+S", "None" });
    }
}
