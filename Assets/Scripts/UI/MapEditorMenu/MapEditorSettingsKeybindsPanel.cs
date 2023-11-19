using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorSettingsKeybindsPanel: CategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var settingsPopup = (MapEditorSettingsPopup)Parent;
            string cat = settingsPopup.LocaleCategory;
            ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            string sub = "Keybinds";
            var settings = SettingsManager.InputSettings.MapEditor;
            CreateKeybindSettings(settings, UIManager.CurrentMenu.KeybindPopup, cat, sub, style);
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
