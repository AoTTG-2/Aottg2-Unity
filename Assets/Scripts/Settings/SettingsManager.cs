using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settings
{
    class SettingsManager
    {
        public static MultiplayerSettings MultiplayerSettings;
        public static ProfileSettings ProfileSettings;
        public static CustomSkinSettings CustomSkinSettings;
        public static GraphicsSettings GraphicsSettings;
        public static GeneralSettings GeneralSettings;
        public static UISettings UISettings;
        public static AbilitySettings AbilitySettings;
        public static InputSettings InputSettings;
        public static InGameSettings InGameSettings;
        public static WeatherSettings WeatherSettings;
        public static InGameSet InGameCurrent;
        public static InGameSet InGameUI;
        public static HumanCustomSettings HumanCustomSettings;
        public static InGameCharacterSettings InGameCharacterSettings;
        public static MapEditorSettings MapEditorSettings;
        public static SoundSettings SoundSettings;

        public static void Init()
        {
            MultiplayerSettings = new MultiplayerSettings();
            ProfileSettings = new ProfileSettings();
            CustomSkinSettings = new CustomSkinSettings();
            GraphicsSettings = new GraphicsSettings();
            GeneralSettings = new GeneralSettings();
            UISettings = new UISettings();
            AbilitySettings = new AbilitySettings();
            InputSettings = new InputSettings();
            InGameSettings = new InGameSettings();
            WeatherSettings = new WeatherSettings();
            InGameCurrent = new InGameSet();
            InGameUI = new InGameSet();
            HumanCustomSettings = new HumanCustomSettings();
            InGameCharacterSettings = new InGameCharacterSettings();
            MapEditorSettings = new MapEditorSettings();
            SoundSettings = new SoundSettings();
        }
    }
}
