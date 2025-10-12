namespace Settings
{
    class HumanCustomSkinSettings:  BaseCustomSkinSettings<HumanCustomSkinSet>
    {
        public BoolSetting GasEnabled = new BoolSetting(true);
        public BoolSetting HookEnabled = new BoolSetting(true);
        public BoolSetting SetSpecificSkinsEnabled = new BoolSetting(true);
        public BoolSetting GlobalSkinOverridesEnabled = new BoolSetting(true);
        public IntSetting SkinMode = new IntSetting(0);
        public IntSetting SelectedCharacterIndex = new IntSetting(0);
        public IntSetting LastGlobalPresetIndex = new IntSetting(0);
    }
}
