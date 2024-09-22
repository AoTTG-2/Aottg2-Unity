using GameManagers;

namespace Settings
{
    class InGameMiscSettings : BaseSettingsContainer
    {
        public IntSetting PVP = new IntSetting(0);
        public BoolSetting EndlessRespawnEnabled = new BoolSetting(false);
        public FloatSetting EndlessRespawnTime = new FloatSetting(5f, minValue: 1f);
        public FloatSetting AllowSpawnTime = new FloatSetting(60f, minValue: 0f);
        public FloatSetting InvincibilityTime = new FloatSetting(3f, minValue: 0f);
        public BoolSetting ThunderspearPVP = new BoolSetting(false);
        public BoolSetting APGPVP = new BoolSetting(false);
        public BoolSetting AllowBlades = new BoolSetting(true);
        public BoolSetting AllowAHSS = new BoolSetting(true);
        public BoolSetting AllowAPG = new BoolSetting(true);
        public BoolSetting AllowThunderspears = new BoolSetting(true);
        public BoolSetting AllowPlayerTitans = new BoolSetting(true);
        public BoolSetting AllowShifterSpecials = new BoolSetting(true);
        public BoolSetting AllowShifters = new BoolSetting(false);
        public BoolSetting AllowVoteKicking = new BoolSetting(false);
        public BoolSetting Horses = new BoolSetting(false);
        public BoolSetting GunsAirReload = new BoolSetting(true);
        public BoolSetting AllowStock = new BoolSetting(true);
        public BoolSetting ClearKDROnRestart = new BoolSetting(true);
        public BoolSetting GlobalMinimapDisable = new BoolSetting(false);
        public BoolSetting RealismMode = new BoolSetting(false);
        public BoolSetting CustomStats = new BoolSetting(true);
        public BoolSetting CustomPerks = new BoolSetting(true);
        public StringSetting Motd = new StringSetting(string.Empty, maxLength: 1000);
        public IntSetting VoiceChat = new IntSetting(0);
        public FloatSetting ProximityMinDistance = new FloatSetting(30f, minValue: 1f);
        public FloatSetting ProximityMaxDistance = new FloatSetting(50f, minValue: 1f);
        public IntSetting HumanHealth = new IntSetting(1, minValue: 1);
        public IntSetting ShifterHealth = new IntSetting(1000, minValue: 1);
    }

    public enum PVPMode
    {
        Off,
        FFA,
        Team
    }

    public enum VoiceChatMode
    {
        Global,
        Proximity,
        Off
    }
}