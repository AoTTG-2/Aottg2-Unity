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
            if (description != "")
                ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, description, alignment: TextAnchor.MiddleLeft);
            int count = 0;
            CreateHorizontalDivider(DoublePanelLeft);
            var tooltips = new Dictionary<string, string>();
            foreach (string key in settings.Keys)
            {
                BaseSetting setting = settings[key];
                if (key.EndsWith("Tooltip") && setting is StringSetting)
                {
                    tooltips[key.Substring(0, key.Length - 7)] = ((StringSetting)setting).Value;
                }
            }
            foreach (string key in settings.Keys)
            {
                Transform panel = count < settings.Keys.Count / 2 ? DoublePanelLeft : DoublePanelRight;
                BaseSetting setting = settings[key];
                if (key == "Description")
                    continue;
                if (key.EndsWith("Tooltip") && setting is StringSetting)
                    continue;
                string title = Util.PascalToSentence(key);
                string translated = UIManager.GetLocale(cat, sub, key, defaultValue: title);
                string tooltip = "";
                if (tooltips.ContainsKey(key))
                    tooltip = tooltips[key];
                if (setting is BoolSetting)
                    ElementFactory.CreateToggleSetting(panel, style, setting, translated, tooltip);
                else if (setting is StringSetting || setting is FloatSetting || setting is IntSetting)
                    ElementFactory.CreateInputSetting(panel, style, setting, translated, tooltip, elementWidth: 180f);
                count += 1;
            }
        }
    }
}
