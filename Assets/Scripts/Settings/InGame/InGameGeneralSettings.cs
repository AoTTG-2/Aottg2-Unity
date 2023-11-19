using GameManagers;

namespace Settings
{
    class InGameGeneralSettings : BaseSettingsContainer
    {
        public StringSetting MapCategory = new StringSetting("General");
        public StringSetting MapName = new StringSetting("Forest");
        public StringSetting GameMode = new StringSetting("Survive");
        public StringSetting PrevGameMode = new StringSetting("");
        public IntSetting Difficulty = new IntSetting((int)GameDifficulty.Normal);

        // multiplayer
        public StringSetting RoomName = new StringSetting("FoodForTitan", maxLength: 100);
        public StringSetting Password = new StringSetting(string.Empty, maxLength: 100);
        public IntSetting MaxPlayers = new IntSetting(10, minValue: 0, maxValue: 255);
    }

    public enum GameDifficulty
    {
        Training,
        Easy,
        Normal,
        Hard
    }
}