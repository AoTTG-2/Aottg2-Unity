using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsForestPanel : SettingsCategoryPanel
    {
        protected override float VerticalSpacing => 20f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            /*
            base.Setup(parent);
            SettingsSkinsPanel skinsPanel = (SettingsSkinsPanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)skinsPanel.Parent;
            BaseCustomSkinSettings<ForestCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Forest;
            ForestCustomSkinSet selectedSet = (ForestCustomSkinSet)settings.GetSelectedSet();
            string cat = settingsPopup.LocaleCategory;
            string sub = "Skins.Forest";
            skinsPanel.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: ThemePanel), selectedSet.RandomizedPairs,
                                               UIManager.GetLocale(cat, "Skins.Common", "RandomizedPairs"),
                                               tooltip: UIManager.GetLocale(cat, "Skins.Common", "RandomizedPairsTooltip"));
            CreateHorizontalDivider(DoublePanelLeft);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateInputSetting(DoublePanelRight, new ElementStyle(titleWidth: 140f, themePanel: ThemePanel), selectedSet.Ground,
                UIManager.GetLocale(cat, "Skins.Common", "Ground"), elementWidth: 260f);
            skinsPanel.CreateSkinListStringSettings(selectedSet.TreeTrunks, DoublePanelLeft,
                UIManager.GetLocale(cat, sub, "TreeTrunks"));
            skinsPanel.CreateSkinListStringSettings(selectedSet.TreeLeafs, DoublePanelRight,
                UIManager.GetLocale(cat, sub, "TreeLeafs"));
            */
        }
    }
}
