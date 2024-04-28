namespace Settings
{
    class InGameMiscSettings : BaseSettingsContainer
    {
        public IntSetting PVP = new IntSetting(0);
        public BoolSetting EndlessRespawnEnabled = new BoolSetting(false);
        public FloatSetting EndlessRespawnTime = new FloatSetting(5f, minValue: 1f);
        public FloatSetting AllowSpawnTime = new FloatSetting(60f, minValue: 0f);
        public BoolSetting ThunderspearPVP = new BoolSetting(false);
        public BoolSetting APGPVP = new BoolSetting(false);
        public BoolSetting AllowBlades = new BoolSetting(true);
        public BoolSetting AllowAHSS = new BoolSetting(true);
        public BoolSetting AllowAPG = new BoolSetting(true);
        public BoolSetting AllowThunderspears = new BoolSetting(true);
        public BoolSetting AllowPlayerTitans = new BoolSetting(true);
        public BoolSetting AllowShifterSpecials = new BoolSetting(true);
        public BoolSetting AllowShifters = new BoolSetting(false);
        public BoolSetting Horses = new BoolSetting(false);
        public BoolSetting GunsAirReload = new BoolSetting(true);
        public BoolSetting AllowStock = new BoolSetting(true);
        public BoolSetting ClearKDROnRestart = new BoolSetting(false);
        public BoolSetting GlobalMinimapDisable = new BoolSetting(false);
        public BoolSetting RealismMode = new BoolSetting(false);
        public BoolSetting CustomStats = new BoolSetting(true);
        public BoolSetting CustomPerks = new BoolSetting(true);
        public StringSetting Motd = new StringSetting(string.Empty, maxLength: 1000);
    }

    public enum PVPMode
    {
        Off,
        FFA,
        Team
    }
}