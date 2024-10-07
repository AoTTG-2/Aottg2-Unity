using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsTitanPanel: SettingsCategoryPanel
    {
        protected override float VerticalSpacing => 20f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
             /*
            base.Setup(parent);
            SettingsSkinsPanel skinsPanel = (SettingsSkinsPanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)skinsPanel.Parent;
            BaseCustomSkinSettings<TitanCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Titan;
            TitanCustomSkinSet selectedSet = (TitanCustomSkinSet)settings.GetSelectedSet();
            string cat = settingsPopup.LocaleCategory;
            string sub = "Skins.Titan";
            skinsPanel.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, selectedSet.RandomizedPairs,
                           UIManager.GetLocale(cat, "Skins.Common", "RandomizedPairs"),
                           tooltip: UIManager.GetLocale(cat, "Skins.Common", "RandomizedPairsTooltip"));
            CreateHorizontalDivider(DoublePanelLeft);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, UIManager.GetLocale(cat, sub, "Hairs"));
            List<string> hairModelOptionsList = new List<string>() { "Random" };
            for (int i = 0; i < 10; i++)
                hairModelOptionsList.Add("Hair " + i.ToString());
            string[] hairModelOptions = hairModelOptionsList.ToArray();
            style.TitleWidth = 0f;
            for (int i = 0; i < selectedSet.Hairs.GetCount(); i++)
            {
                GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 20f);
                ElementFactory.CreateInputSetting(group.transform, style, selectedSet.Hairs.GetItemAt(i), string.Empty, elementWidth: 260f);
                ElementFactory.CreateDropdownSetting(group.transform, style, selectedSet.HairModels.GetItemAt(i), string.Empty, hairModelOptions, elementWidth: 140f);
            }
            skinsPanel.CreateSkinListStringSettings(selectedSet.Bodies, DoublePanelRight, UIManager.GetLocale(cat, sub, "Bodies"));
            CreateHorizontalDivider(DoublePanelRight);
            skinsPanel.CreateSkinListStringSettings(selectedSet.Eyes, DoublePanelRight, UIManager.GetLocale(cat, sub, "Eyes"));
             */
        }
    }
}
