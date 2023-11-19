using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsKeybindsDefaultPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsKeybindsPanel keybindsPanel = (SettingsKeybindsPanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)keybindsPanel.Parent;
            keybindsPanel.CreateGategoryDropdown(DoublePanelLeft);
            string cat = settingsPopup.LocaleCategory;
            ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            string sub = keybindsPanel.GetCurrentCategoryName().Replace(" ", "");
            BaseSettingsContainer settings = (BaseSettingsContainer)SettingsManager.InputSettings.Settings[sub];
            CreateKeybindSettings(settings, UIManager.CurrentMenu.KeybindPopup, cat, "Keybinds." + sub, style);
            if (sub == "Human")
            {
                ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.InputSettings.Human.DashDoubleTap,
                UIManager.GetLocale(cat, "Keybinds.Human", "DashDoubleTap"));
                ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.InputSettings.Human.SwapTSAttackSpecial,
                UIManager.GetLocale(cat, "Keybinds.Human", "SwapTSAttackSpecial"), tooltip: UIManager.GetLocale(cat, "Keybinds.Human", "SwapTSAttackSpecialTooltip"));
                ElementFactory.CreateSliderSetting(DoublePanelRight, style, SettingsManager.InputSettings.Human.ReelOutScrollSmoothing,
                    UIManager.GetLocale(cat, "Keybinds.Human", "ReelOutScrollSmoothing"), elementWidth: 130f,
                    tooltip: UIManager.GetLocale(cat, "Keybinds.Human", "ReelOutScrollSmoothingTooltip"));
            }
            else if (sub == "General")
            {
                ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.InputSettings.General.TapScoreboard,
                    UIManager.GetLocale(cat, "Keybinds.General", "TapScoreboard"), tooltip: UIManager.GetLocale(cat, "Keybinds.General", "TapScoreboardTooltip"));
            }
        }

        private void CreateKeybindSettings(BaseSettingsContainer container, KeybindPopup popup, string cat, string sub, ElementStyle style)
        {
            int count = 0;
            foreach (DictionaryEntry entry in container.Settings)
            {
                BaseSetting setting = (BaseSetting)entry.Value;
                string name = (string)entry.Key;
                if (setting.GetType() == typeof(KeybindSetting))
                {
                    Transform side = count < (container.Settings.Count / 2) ? DoublePanelLeft : DoublePanelRight;
                    GameObject obj = ElementFactory.CreateKeybindSetting(side, style, setting, UIManager.GetLocale(cat, sub, name),
                        popup);
                    count += 1;
                }
            }
        }
    }
}
