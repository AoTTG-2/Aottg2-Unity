namespace Settings
{
    class HumanCustomSkinSettings:  BaseCustomSkinSettings<HumanCustomSkinSet>
    {
        public BoolSetting GasEnabled = new BoolSetting(true);
        public BoolSetting HookEnabled = new BoolSetting(true);
    }
}
