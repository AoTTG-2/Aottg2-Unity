using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsPanel: SettingsCategoryPanel
    {
        protected override bool CategoryPanel => true;
        protected override string DefaultCategoryPanel => "Human";
        protected Dictionary<string, ICustomSkinSettings> _settings = new Dictionary<string, ICustomSkinSettings>();

        public void CreateCommonSettings(Transform panelLeft, Transform panelRight)
        {
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            ElementStyle toggleStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ICustomSkinSettings settings = GetCurrentSettings();
            string category = _currentCategoryPanelName.Value;
            string[] categories = new string[] { "Human", "Shifter", "Skybox" };
            ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, _currentCategoryPanelName, UIManager.GetLocaleCommon("Category"),
                categories, elementWidth: 260f, onDropdownOptionSelect: () => RebuildCategoryPanel());
            string sub = "Skins.Common";
            string cat = ((SettingsPopup)Parent).LocaleCategory;
            ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, settings.GetSelectedSetIndex(), UIManager.GetLocale(cat, sub, "Set"), 
                settings.GetSetNames(), elementWidth: 260f, 
                onDropdownOptionSelect: () => RebuildCategoryPanel());
            GameObject group = ElementFactory.CreateHorizontalGroup(panelLeft, 10f, TextAnchor.UpperRight);
            foreach (string button in new string[] { "Create", "Delete", "Rename", "Copy" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, dropdownStyle, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnSkinsPanelButtonClick(button));
            }
            ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsEnabled(), 
                category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsEnabled"));
            ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsLocal(), 
                category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsLocal"), tooltip: UIManager.GetLocale(cat, "Skins.Common", "SkinsLocalTooltip"));
        }

        private void OnSkinsPanelButtonClick(string name)
        {
            SetNamePopup skinSetNamePopup = UIManager.CurrentMenu.SetNamePopup;
            ICustomSkinSettings settings = _settings[_currentCategoryPanelName.Value];
            switch (name)
            {
                case "Create":
                    skinSetNamePopup.Show("New set", () => OnSkinsSetOperationFinish(name), UIManager.GetLocaleCommon("Create"));
                    break;
                case "Delete":
                    if (settings.CanDeleteSelectedSet())
                        UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnSkinsSetOperationFinish(name),
                            UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Rename":
                    string currentSetName = settings.GetSelectedSet().Name.Value;
                    skinSetNamePopup.Show(currentSetName, () => OnSkinsSetOperationFinish(name), UIManager.GetLocaleCommon("Rename"));
                    break;
                case "Copy":
                    skinSetNamePopup.Show("New set", () => OnSkinsSetOperationFinish(name), UIManager.GetLocaleCommon("Copy"));
                    break;
            }
        }

        private void OnSkinsSetOperationFinish(string name)
        {
            SetNamePopup skinSetNamePopup = UIManager.CurrentMenu.SetNamePopup;
            ICustomSkinSettings settings = GetCurrentSettings();
            switch (name)
            {
                case "Create":
                    settings.CreateSet(skinSetNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Delete":
                    settings.DeleteSelectedSet();
                    settings.GetSelectedSetIndex().Value = 0;
                    break;
                case "Rename":
                    settings.GetSelectedSet().Name.Value = skinSetNamePopup.NameSetting.Value;
                    break;
                case "Copy":
                    settings.CopySelectedSet(skinSetNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
            }
            RebuildCategoryPanel();
        }

        public void CreateSkinListStringSettings(ListSetting<StringSetting> list, Transform panel, string title)
        {
            ElementStyle style = new ElementStyle(titleWidth: 0f, themePanel: ThemePanel);
            ElementFactory.CreateDefaultLabel(panel, style, title);
            foreach (StringSetting setting in list.Value)
            {
                ElementFactory.CreateInputSetting(panel, style, setting, string.Empty, elementWidth: 420f);
            }
        }

        public void CreateSkinStringSettings(Transform panelLeft, Transform panelRight, float titleWidth = 140f, float elementWidth = 260f, 
            int leftCount = 0)
        {
            ElementStyle style = new ElementStyle(titleWidth: titleWidth, themePanel: ThemePanel);
            BaseSettingsContainer container = GetCurrentSettings().GetSelectedSet();
            string cat = ((SettingsPopup)Parent).LocaleCategory;
            string sub = "Skins." + _currentCategoryPanelName.Value;
            int count = 1;
            foreach (DictionaryEntry entry in container.Settings)
            {
                BaseSetting setting = (BaseSetting)entry.Value;
                string name = (string)entry.Key;
                Transform side = count <= leftCount ? panelLeft : panelRight;
                if ((setting.GetType() == typeof(StringSetting) || setting.GetType() == typeof(FloatSetting)) && name != "Name")
                {
                    string currentSub = sub;
                    if (name == "Ground")
                        currentSub = "Skins.Common";
                    ElementFactory.CreateInputSetting(side, style, setting, UIManager.GetLocale(cat, currentSub, name), elementWidth: elementWidth);
                    count += 1;
                }
            }
        }

        public ICustomSkinSettings GetCurrentSettings()
        {
            return _settings[_currentCategoryPanelName.Value];
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Human", typeof(SettingsSkinsHumanPanel));
            // _categoryPanelTypes.Add("Titan", typeof(SettingsSkinsTitanPanel));
            _categoryPanelTypes.Add("Shifter", typeof(SettingsSkinsDefaultPanel));
            _categoryPanelTypes.Add("Skybox", typeof(SettingsSkinsDefaultPanel));
            //_categoryPanelTypes.Add("Forest", typeof(SettingsSkinsForestPanel));
            //_categoryPanelTypes.Add("City", typeof(SettingsSkinsCityPanel));
            _settings.Add("Human", SettingsManager.CustomSkinSettings.Human);
            // _settings.Add("Titan", SettingsManager.CustomSkinSettings.Titan);
            _settings.Add("Shifter", SettingsManager.CustomSkinSettings.Shifter);
            _settings.Add("Skybox", SettingsManager.CustomSkinSettings.Skybox);
            //_settings.Add("Forest", SettingsManager.CustomSkinSettings.Forest);
            //_settings.Add("City", SettingsManager.CustomSkinSettings.City);
        }
    }
}
