using CustomLogic;
using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class CreateGameModePanel: CreateGameCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "CreateGamePopup";
            string sub = "Mode";
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            string mapCategory = SettingsManager.InGameUI.General.MapCategory.Value;
            string mapName = SettingsManager.InGameUI.General.MapName.Value;
            MapScript script = new MapScript();
            script.Deserialize(BuiltinLevels.LoadMap(mapCategory, mapName));
            var inGameUI = SettingsManager.InGameUI;
            if (inGameUI.General.GameMode.Value != inGameUI.General.PrevGameMode.Value)
            {
                CreateGameGeneralPanel.SetDefaultMisc();
                BuiltinLevels.LoadMiscSettings(inGameUI.General.MapCategory.Value, inGameUI.General.MapName.Value,
                    inGameUI.General.GameMode.Value, inGameUI.Misc);
                inGameUI.Mode.Current = new Dictionary<string, BaseSetting>();
            }
            inGameUI.General.PrevGameMode.Value = inGameUI.General.GameMode.Value;
            var settings = ((CreateGamePopup)Parent).SyncModeSettings(script);
            var description = CustomLogicManager.GetModeDescription(settings);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, SettingsManager.InGameUI.General.GameMode, UIManager.GetLocale(cat, "General", "GameMode"),
                BuiltinLevels.GetGameModes(SettingsManager.InGameUI.General.MapCategory.Value, SettingsManager.InGameUI.General.MapName.Value, script.Logic.Trim() != string.Empty), 
                elementWidth: 180f, optionsWidth: 240f, onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
            int count = 1;
            int totalCount = settings.Keys.Count;
            if (description != "")
            {
                ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, description, alignment: TextAnchor.MiddleLeft);
                count += 1;
            }
            CreateHorizontalDivider(DoublePanelLeft);
            var tooltips = new Dictionary<string, string>();
            var dropboxes = new Dictionary<string, string[]>();
            foreach (string key in settings.Keys)
            {
                BaseSetting setting = settings[key];
                if (key == "Description")
                {
                    totalCount -= 1;
                }
                if (key.EndsWith("Tooltip") && setting is StringSetting)
                {
                    totalCount -= 1;
                    tooltips[key.Substring(0, key.Length - 7)] = ((StringSetting)setting).Value;
                }
                else if (key.EndsWith("Dropbox") && setting is StringSetting)
                {
                    totalCount -= 1;
                    List<string> options = new List<string>();
                    foreach (string option in ((StringSetting)setting).Value.Split(','))
                        options.Add(option.Trim());
                    if (options.Count == 0)
                        options.Add("None");
                    dropboxes[key.Substring(0, key.Length - 7)] = options.ToArray();
                }
            }
            int splitIdx = totalCount > 5 ? totalCount / 2 : int.MaxValue;
            foreach (string key in settings.Keys)
            {
                Transform panel = count < splitIdx ? DoublePanelLeft : DoublePanelRight;
                BaseSetting setting = settings[key];
                if (key == "Description")
                    continue;
                if (key.EndsWith("Tooltip") && setting is StringSetting)
                    continue;
                if (key.EndsWith("Dropbox") && setting is StringSetting)
                    continue;
                string title = Util.PascalToSentence(key);
                string translated = UIManager.GetLocale(cat, sub, key, defaultValue: title);
                string tooltip = "";
                if (tooltips.ContainsKey(key))
                    tooltip = tooltips[key];
                if (dropboxes.ContainsKey(key) && setting is StringSetting)
                    ElementFactory.CreateDropdownSetting(panel, style, setting, translated, dropboxes[key], tooltip, elementWidth: 180f);
                else if (setting is BoolSetting)
                    ElementFactory.CreateToggleSetting(panel, style, setting, translated, tooltip);
                else if (setting is StringSetting || setting is FloatSetting || setting is IntSetting)
                    ElementFactory.CreateInputSetting(panel, style, setting, translated, tooltip, elementWidth: 180f);
                count += 1;
            }
        }
    }
}
