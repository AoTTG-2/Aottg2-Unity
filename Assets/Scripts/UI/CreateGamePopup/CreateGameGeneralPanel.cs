using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using ApplicationManagers;

namespace UI
{
    class CreateGameGeneralPanel: CreateGameCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            InGameSet settings = SettingsManager.InGameUI;
            string cat = "CreateGamePopup";
            string sub = "General";
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            string[] mapNames = BuiltinLevels.GetMapNames(settings.General.MapCategory.Value);
            if (!mapNames.Contains(settings.General.MapName.Value))
            {
                if (mapNames.Length > 0)
                    settings.General.MapName.Value = mapNames[0];
                else
                    settings.General.MapName.Value = "";
            }
            string mapCategory = SettingsManager.InGameUI.General.MapCategory.Value;
            string mapName = SettingsManager.InGameUI.General.MapName.Value;
            MapScript script = new MapScript();
            script.Deserialize(BuiltinLevels.LoadMap(mapCategory, mapName));
            bool hasMapLogic = script.Logic.Trim() != string.Empty;
            string[] gameModes = BuiltinLevels.GetGameModes(settings.General.MapCategory.Value, settings.General.MapName.Value, hasMapLogic);
            if (!gameModes.Contains(settings.General.GameMode.Value))
                settings.General.GameMode.Value = gameModes[0];
            if (settings.General.GameMode.Value != settings.General.PrevGameMode.Value || (settings.General.GameMode.Value == BuiltinLevels.UseMapLogic))
            {
                SetDefaultMisc();
                BuiltinLevels.LoadMiscSettings(settings.General.MapCategory.Value, settings.General.MapName.Value, 
                    settings.General.GameMode.Value, settings.Misc);
                SettingsManager.InGameUI.Mode.Current = new Dictionary<string, BaseSetting>();
            }
            settings.General.PrevGameMode.Value = settings.General.GameMode.Value;
            ((CreateGamePopup)Parent).SyncModeSettings(script);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, settings.General.MapCategory, UIManager.GetLocale(cat, sub, "MapCategory"),
                BuiltinLevels.GetMapCategories(), elementWidth: 180f, optionsWidth: 180f, onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
            if (mapNames.Length > 0)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, settings.General.MapName, UIManager.GetLocale(cat, sub, "MapName"),
               mapNames, elementWidth: 180f, optionsWidth: 300f, onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
                ElementFactory.CreateDefaultLabel(DoublePanelLeft, dropdownStyle, script.Options.Description, alignment: TextAnchor.MiddleLeft);
            }
            ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.General.GameMode, UIManager.GetLocale(cat, sub, "GameMode"),
                BuiltinLevels.GetGameModes(settings.General.MapCategory.Value, settings.General.MapName.Value, hasMapLogic), elementWidth: 180f, optionsWidth: 240f,
                onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
            ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.WeatherIndex,
                UIManager.GetLocale(cat, sub, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, dropdownStyle, settings.General.Difficulty, UIManager.GetLocale(cat, sub, "Difficulty"),
                UIManager.GetLocaleArray(cat, sub, "DifficultyOptions"));
            if (((CreateGamePopup)parent).IsMultiplayer && SceneLoader.SceneName == SceneName.MainMenu)
            {
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.RoomName, UIManager.GetLocale(cat, sub, "RoomName"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.MaxPlayers, UIManager.GetLocale(cat, sub, "MaxPlayers"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.Password, UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
            }
        }

        public static void SetDefaultMisc()
        {
            var misc = SettingsManager.InGameUI.Misc;
            misc.EndlessRespawnEnabled.Value = false;
            misc.EndlessRespawnTime.Value = 5f;
            misc.ThunderspearPVP.Value = false;
            misc.PVP.Value = 0;
            misc.AllowBlades.Value = true;
            misc.AllowAHSS.Value = true;
            misc.AllowAPG.Value = true;
            misc.AllowThunderspears.Value = true;
            misc.AllowPlayerTitans.Value = true;
            misc.AllowShifterSpecials.Value = true;
            misc.AllowShifters.Value = false;
            misc.Horses.Value = false;
        }
    }
}
