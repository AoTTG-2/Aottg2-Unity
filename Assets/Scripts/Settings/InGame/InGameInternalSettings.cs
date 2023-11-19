namespace Settings
{
    class InGameInternalSettings : BaseSettingsContainer
    {
        public StringSetting ScoreboardFormat = new StringSetting("Kills/Deaths/Avg Dmg/Total Dmg");
    }
}