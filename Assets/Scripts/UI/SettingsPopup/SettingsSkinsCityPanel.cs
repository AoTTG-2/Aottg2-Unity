using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsCityPanel : SettingsCategoryPanel
    {
        protected override float VerticalSpacing => 20f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            /*
            base.Setup(parent);
            SettingsSkinsPanel skinsPanel = (SettingsSkinsPanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)skinsPanel.Parent;
            BaseCustomSkinSettings<CityCustomSkinSet> settings = SettingsManager.CustomSkinSettings.City;
            CityCustomSkinSet selectedSet = (CityCustomSkinSet)settings.GetSelectedSet();
            string cat = settingsPopup.LocaleCategory;
            string sub = "Skins.City";
            ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            skinsPanel.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
            CreateHorizontalDivider(DoublePanelLeft);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, selectedSet.Ground, UIManager.GetLocale(cat, "Skins.Common", "Ground"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, selectedSet.Wall, UIManager.GetLocale(cat, sub, "Wall"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, selectedSet.Gate, UIManager.GetLocale(cat, sub, "Gate"), elementWidth: 260f);
            skinsPanel.CreateSkinListStringSettings(selectedSet.Houses, DoublePanelRight, UIManager.GetLocale(cat, sub, "Houses"));
            */
        }
    }
}
