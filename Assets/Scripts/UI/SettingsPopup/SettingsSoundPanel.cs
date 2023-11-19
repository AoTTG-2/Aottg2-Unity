using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSoundPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        private Text _currentSongLabel;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "Sound";
            SoundSettings settings = SettingsManager.SoundSettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.Volume, UIManager.GetLocale(cat, sub, "Volume"),
                elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.Music, UIManager.GetLocale(cat, sub, "Music"),
                elementWidth: 135f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanGrabMusic, UIManager.GetLocale(cat, sub, "TitanGrabMusic"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanVocalEffect, UIManager.GetLocale(cat, sub, "TitanVocalEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.GasEffect, UIManager.GetLocale(cat, sub, "GasEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ReelInEffect, UIManager.GetLocale(cat, sub, "ReelInEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ReelOutEffect, UIManager.GetLocale(cat, sub, "ReelOutEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.HookRetractEffect, UIManager.GetLocale(cat, sub, "HookRetractEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.HookImpactEffect, UIManager.GetLocale(cat, sub, "HookImpactEffect"));
            _currentSongLabel = ElementFactory.CreateDefaultLabel(DoublePanelRight, style, "", alignment: TextAnchor.MiddleLeft).GetComponent<Text>();
            ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, settings.ForcePlaylist, UIManager.GetLocale(cat, sub, "ForcePlaylist"),
                new string[] { "Default", "Custom", "Menu", "Peaceful", "Battle", "Boss", "Racing" }, elementWidth: 160f);
            string custom = settings.CustomPlaylist.Value;
            if (custom == "")
                custom = "None";
            ElementFactory.CreateDefaultLabel(DoublePanelRight, style, UIManager.GetLocale(cat, sub, "CustomPlaylist") + ": " +
                custom, alignment: TextAnchor.MiddleLeft);
            var group = ElementFactory.CreateHorizontalGroup(DoublePanelRight, 10f, TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocale(cat, sub, "AddSong"), onClick: () => OnButtonClick("Add"));
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Clear"), onClick: () => OnButtonClick("Clear"));
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.OldHookEffect, UIManager.GetLocale(cat, sub, "OldHookEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.OldBladeEffect, UIManager.GetLocale(cat, sub, "OldBladeEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.OldNapeEffect, UIManager.GetLocale(cat, sub, "OldNapeEffect"));
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Clear")
            {
                SettingsManager.SoundSettings.CustomPlaylist.Value = "";
                Parent.RebuildCategoryPanel();
            }
            else if (name == "Add")
            {
                UIManager.CurrentMenu.SelectListPopup.ShowLoad(MusicManager.GetAllSongs(), UIManager.GetLocale("SettingsPopup", "Sound", "AddSong"),
                    onLoad: () => OnButtonClick("AddFinish"));
            }
            else if (name == "AddFinish")
            {
                var currentSongs = new List<string>();
                foreach (string str in SettingsManager.SoundSettings.CustomPlaylist.Value.Split(','))
                {
                    if (str.Trim() != "")
                        currentSongs.Add(str);
                }
                string finish = UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value;
                if (finish != "")
                    currentSongs.Add(finish);
                if (currentSongs.Count == 0)
                    SettingsManager.SoundSettings.CustomPlaylist.Value = "";
                else
                    SettingsManager.SoundSettings.CustomPlaylist.Value = string.Join(",", currentSongs.ToArray());
                Parent.RebuildCategoryPanel();
            }
        }

        private void Update()
        {
            if (_currentSongLabel != null)
                _currentSongLabel.text = UIManager.GetLocale("SettingsPopup", "Sound", "CurrentSong") + ": " + MusicManager.GetCurrentSong();
        }
    }
}
