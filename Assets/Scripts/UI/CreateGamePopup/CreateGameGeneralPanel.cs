using ApplicationManagers;
using Map;
using Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    class CreateGameGeneralPanel : CreateGameCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            InGameSet settings = SettingsManager.InGameUI;
            string cat = "CreateGamePopup";
            string sub = "General";
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            var preset = GetActivePreset();
            bool hasMapFilter = preset?.MapRule != null;
            bool hasModeFilter = preset?.ModeRule != null;

            // Determine available maps
            string[] mapNames;
            if (hasMapFilter)
            {
                mapNames = BuiltinLevels.QueryMapsNames(preset.MapRule);
                if (mapNames.Length > 0 && !mapNames.Contains(settings.General.MapName.Value))
                {
                    settings.General.MapName.Value = mapNames[0];
                    settings.General.MapCategory.Value = BuiltinLevels.FindMapCategory(mapNames[0]);
                }
                else if (mapNames.Contains(settings.General.MapName.Value))
                {
                    settings.General.MapCategory.Value = BuiltinLevels.FindMapCategory(settings.General.MapName.Value);
                }
            }
            else
            {
                mapNames = BuiltinLevels.GetMapNames(settings.General.MapCategory.Value);
            }
            if (!mapNames.Contains(settings.General.MapName.Value))
            {
                if (mapNames.Length > 0)
                    settings.General.MapName.Value = mapNames[0];
                else
                    settings.General.MapName.Value = "";
            }

            string mapCategory = settings.General.MapCategory.Value;
            string mapName = settings.General.MapName.Value;
            MapScript script = new MapScript();
            try
            {
                script.Deserialize(BuiltinLevels.LoadMap(mapCategory, mapName));
            }
            catch
            {
                // Need a non-persisting banner to show up here as popups break when used in setup.
            }
            bool hasMapLogic = script.Logic.Trim() != string.Empty;

            // Determine available game modes
            string[] gameModes;
            if (hasModeFilter)
            {
                gameModes = BuiltinLevels.QueryModeNames(preset.ModeRule);
            }
            else
            {
                gameModes = BuiltinLevels.GetGameModes(settings.General.MapCategory.Value, settings.General.MapName.Value, hasMapLogic);
            }
            if (!gameModes.Contains(settings.General.GameMode.Value))
                settings.General.GameMode.Value = gameModes[0];

            if (settings.General.GameMode.Value != settings.General.PrevGameMode.Value || (settings.General.GameMode.Value == BuiltinLevels.UseMapLogic))
            {
                SetDefaultMisc();
                BuiltinLevels.LoadMiscSettings(settings.General.MapCategory.Value, settings.General.MapName.Value,
                    settings.General.GameMode.Value, settings.Misc);
                // Re-apply preset forced defaults after misc reset
                if (preset != null)
                    ApplyPresetDefaults(preset, settings);
                SettingsManager.InGameUI.Mode.Current = new Dictionary<string, BaseSetting>();
            }
            settings.General.PrevGameMode.Value = settings.General.GameMode.Value;
            //((CreateGamePopup)Parent).SyncModeSettings(script);

            // Map selection
            if (!settings.General.MapName.IsHidden)
            {
                bool mapLocked = hasMapFilter && preset.MapRule.DisableEditing;
                if (mapLocked || mapNames.Length <= 1)
                {
                    ElementFactory.CreateDefaultLabel(DoublePanelLeft, dropdownStyle,
                        UIManager.GetLocale(cat, sub, "MapName") + ": " + settings.General.MapName.Value,
                        alignment: TextAnchor.MiddleLeft);
                }
                else if (hasMapFilter)
                {
                    ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, settings.General.MapName,
                        UIManager.GetLocale(cat, sub, "MapName"), mapNames,
                        elementWidth: 180f, optionsWidth: 240f,
                        onDropdownOptionSelect: () =>
                        {
                            settings.General.MapCategory.Value = BuiltinLevels.FindMapCategory(settings.General.MapName.Value);
                            parent.RebuildCategoryPanel();
                        });
                }
                else
                {
                    BasePopup selectPopup;
                    if (SceneLoader.SceneName == SceneName.InGame)
                        selectPopup = ((InGameMenu)UIManager.CurrentMenu)._selectMapPopup;
                    else
                        selectPopup = ((MainMenu)UIManager.CurrentMenu)._selectMapPopup;
                    ElementFactory.CreateButtonPopupSetting(DoublePanelLeft, dropdownStyle, settings.General.MapName,
                        UIManager.GetLocale(cat, sub, "MapName"), selectPopup, elementWidth: 180f);
                }
            }
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, dropdownStyle,
                UIManager.GetLocale(cat, sub, "MapCategory") + ": " + settings.General.MapCategory.Value,
                alignment: TextAnchor.MiddleLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, dropdownStyle, script.Options.Description,
                alignment: TextAnchor.MiddleLeft);

            // Game mode
            if (!settings.General.GameMode.IsHidden)
            {
                bool modeLocked = hasModeFilter && preset.ModeRule.DisableEditing;
                if (modeLocked || gameModes.Length <= 1)
                {
                    ElementFactory.CreateDefaultLabel(DoublePanelRight, dropdownStyle,
                        UIManager.GetLocale(cat, sub, "GameMode") + ": " + settings.General.GameMode.Value,
                        alignment: TextAnchor.MiddleLeft);
                }
                else
                {
                    ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.General.GameMode,
                        UIManager.GetLocale(cat, sub, "GameMode"), gameModes,
                        elementWidth: 180f, optionsWidth: 240f,
                        onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
                }
            }

            // Weather
            if (!settings.WeatherIndex.IsHidden)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.WeatherIndex,
                    UIManager.GetLocale(cat, sub, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(),
                    elementWidth: 180f);
            }

            // Difficulty
            if (!settings.General.Difficulty.IsHidden)
            {
                UIRule difficultyRule = GetRule("general.Difficulty");
                string[] allDiffOptions = UIManager.GetLocaleArray(cat, sub, "DifficultyOptions");

                if (difficultyRule != null && difficultyRule.AllowedValues != null && difficultyRule.AllowedValues.Count > 0)
                {
                    string[] diffNames = { "training", "easy", "normal", "hard", "abnormal" };
                    List<string> filteredOptions = new List<string>();
                    List<int> filteredIndices = new List<int>();
                    for (int i = 0; i < diffNames.Length && i < allDiffOptions.Length; i++)
                    {
                        if (difficultyRule.AllowedValues.Any(a => a.Equals(diffNames[i], System.StringComparison.OrdinalIgnoreCase)))
                        {
                            filteredOptions.Add(allDiffOptions[i]);
                            filteredIndices.Add(i);
                        }
                    }

                    int currentIdx = settings.General.Difficulty.Value;
                    string currentLabel = (currentIdx >= 0 && currentIdx < allDiffOptions.Length)
                        ? allDiffOptions[currentIdx] : allDiffOptions[0];
                    if (!filteredOptions.Contains(currentLabel))
                    {
                        currentLabel = filteredOptions.Count > 0 ? filteredOptions[0] : allDiffOptions[0];
                        if (filteredIndices.Count > 0)
                            settings.General.Difficulty.Value = filteredIndices[0];
                    }

                    StringSetting diffProxy = new StringSetting(currentLabel);
                    ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, diffProxy,
                        UIManager.GetLocale(cat, sub, "Difficulty"), filteredOptions.ToArray(),
                        elementWidth: 180f, optionsWidth: 200f,
                        onDropdownOptionSelect: () =>
                        {
                            int idx = filteredOptions.IndexOf(diffProxy.Value);
                            if (idx >= 0 && idx < filteredIndices.Count)
                                settings.General.Difficulty.Value = filteredIndices[idx];
                        });
                }
                else
                {
                    ElementFactory.CreateToggleGroupSetting(DoublePanelRight, dropdownStyle, settings.General.Difficulty,
                        UIManager.GetLocale(cat, sub, "Difficulty"), allDiffOptions);
                }
            }

            if (((CreateGamePopup)parent).IsMultiplayer && SceneLoader.SceneName == SceneName.MainMenu)
            {
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.RoomName,
                    UIManager.GetLocale(cat, sub, "RoomName"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.MaxPlayers,
                    UIManager.GetLocale(cat, sub, "MaxPlayers"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(DoublePanelRight, dropdownStyle, settings.General.Password,
                    UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
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
            misc.AllowVoteKicking.Value = false;
            misc.Horses.Value = false;
            misc.APGPVP.Value = false;
            misc.RealismMode.Value = false;
            misc.GlobalMinimapDisable.Value = false;
        }
    }
}
