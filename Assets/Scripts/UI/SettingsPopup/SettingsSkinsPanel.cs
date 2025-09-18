using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    class SettingsSkinsPanel : SettingsCategoryPanel
    {
        protected override bool CategoryPanel => true;
        protected override string DefaultCategoryPanel => "Human";
        protected Dictionary<string, ICustomSkinSettings> _settings = new Dictionary<string, ICustomSkinSettings>();
        private IntSetting _lastFilteredSetIndex = null;
        private List<int> _lastFilteredOriginalIndices = null;

        public void CreateCommonSettings(Transform panelLeft, Transform panelRight)
        {
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            ElementStyle toggleStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementStyle labelStyle = new ElementStyle(titleWidth: 0f, themePanel: ThemePanel);
            ICustomSkinSettings settings = GetCurrentSettings();
            string category = _currentCategoryPanelName.Value;
            string[] categories = new string[] { "Human", "Shifter", "Skybox" };
            ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, _currentCategoryPanelName, UIManager.GetLocaleCommon("Category"),
                categories, elementWidth: 260f, onDropdownOptionSelect: () => RebuildCategoryPanel());
            string sub = "Skins.Common";
            string cat = ((SettingsPopup)Parent).LocaleCategory;
            if (category == "Human")
            {
                var humanSettings = (HumanCustomSkinSettings)settings;
                string[] modes = new string[] { "Global", "Character" };
                ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, humanSettings.SkinMode, UIManager.GetLocale(cat, "Skins.Human", "SkinMode"),
                    modes, elementWidth: 260f, onDropdownOptionSelect: () => RebuildCategoryPanel());
                if (humanSettings.SkinMode.Value == 0) // Global mode
                {
                    string[] originalSetNames = settings.GetSetNames();
                    List<string> filteredSetNames = new List<string>();
                    List<int> originalIndices = new List<int>();
                    for (int i = 0; i < originalSetNames.Length; i++)
                    {
                        if (!originalSetNames[i].StartsWith("Custom Set:"))
                        {
                            filteredSetNames.Add(originalSetNames[i]);
                            originalIndices.Add(i);
                        }
                    }
                    var filteredSetIndex = new IntSetting(0);
                    if (originalIndices.Count > 0)
                    {
                        int rememberedIndex = humanSettings.LastGlobalPresetIndex.Value;
                        int filteredIndex = originalIndices.IndexOf(rememberedIndex);
                        if (filteredIndex >= 0)
                            filteredSetIndex.Value = filteredIndex;
                        else
                            filteredSetIndex.Value = 0;
                    }
                    _lastFilteredSetIndex = filteredSetIndex;
                    _lastFilteredOriginalIndices = originalIndices;
                    ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, filteredSetIndex, UIManager.GetLocale(cat, "Skins.Common", "Set"),
                        filteredSetNames.ToArray(), elementWidth: 260f,
                        onDropdownOptionSelect: () => OnGlobalPresetSelected(filteredSetIndex.Value, originalIndices, settings, humanSettings));
                    GameObject group = ElementFactory.CreateHorizontalGroup(panelLeft, 10f, TextAnchor.UpperRight);
                    foreach (string button in new string[] { "Create", "Delete", "Rename", "Copy" })
                    {
                        GameObject obj = ElementFactory.CreateDefaultButton(group.transform, dropdownStyle, UIManager.GetLocaleCommon(button),
                            onClick: () => OnSkinsPanelButtonClick(button));
                    }
                }
                else
                {
                    CreateCharacterSelectorDropdown(panelLeft, dropdownStyle, humanSettings);
                }
            }
            else
            {
                ElementFactory.CreateDropdownSetting(panelLeft, dropdownStyle, settings.GetSelectedSetIndex(), "Set",
                    settings.GetSetNames(), elementWidth: 260f,
                    onDropdownOptionSelect: () => RebuildCategoryPanel());
                GameObject group = ElementFactory.CreateHorizontalGroup(panelLeft, 10f, TextAnchor.UpperRight);
                foreach (string button in new string[] { "Create", "Delete", "Rename", "Copy" })
                {
                    GameObject obj = ElementFactory.CreateDefaultButton(group.transform, dropdownStyle, UIManager.GetLocaleCommon(button),
                        onClick: () => OnSkinsPanelButtonClick(button));
                }
            }
            if (category == "Human")
            {
                var humanSettings = (HumanCustomSkinSettings)settings;
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsEnabled(),
                    category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsEnabled"));
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsLocal(),
                    category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsLocal"), tooltip: UIManager.GetLocale(cat, "Skins.Common", "SkinsLocalTooltip"));
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, humanSettings.GlobalSkinOverridesEnabled,
                    UIManager.GetLocale(cat, "Skins.Human", "GlobalSkinOverridesEnabled"), tooltip: UIManager.GetLocale(cat, "Skins.Human", "GlobalSkinOverridesEnabledTooltip"));
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, humanSettings.SetSpecificSkinsEnabled,
                    UIManager.GetLocale(cat, "Skins.Human", "SetSpecificSkinsEnabled"), tooltip: UIManager.GetLocale(cat, "Skins.Human", "SetSpecificSkinsEnabledTooltip"));
            }
            else
            {
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsEnabled(),
                    category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsEnabled"));
                ElementFactory.CreateToggleSetting(panelRight, toggleStyle, settings.GetSkinsLocal(),
                    category + " " + UIManager.GetLocale(cat, "Skins.Common", "SkinsLocal"), tooltip: UIManager.GetLocale(cat, "Skins.Common", "SkinsLocalTooltip"));
            }
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
            var humanSettings = settings as HumanCustomSkinSettings;
            switch (name)
            {
                case "Create":
                    settings.CreateSet(skinSetNamePopup.NameSetting.Value);
                    if (_currentCategoryPanelName.Value == "Human" && humanSettings != null && humanSettings.SkinMode.Value == 0)
                    {
                        SelectNewlyCreatedOrCopiedSetInDropdown(settings, humanSettings);
                    }
                    else if (_currentCategoryPanelName.Value == "Human" && humanSettings != null && humanSettings.SkinMode.Value == 1)
                    {
                        var customSets = SettingsManager.HumanCustomSettings.CustomSets.GetSets().GetItems();
                        humanSettings.SelectedCharacterIndex.Value = customSets.Count - 1;
                    }
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    RebuildCategoryPanel();
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
                    if (_currentCategoryPanelName.Value == "Human" && humanSettings != null && humanSettings.SkinMode.Value == 0)
                    {
                        SelectNewlyCreatedOrCopiedSetInDropdown(settings, humanSettings);
                    }
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
            BaseSettingsContainer container;
            if (_currentCategoryPanelName.Value == "Human")
            {
                var humanSettings = (HumanCustomSkinSettings)GetCurrentSettings();
                if (humanSettings.SkinMode.Value == 1) // Character mode
                {
                    var customSets = SettingsManager.HumanCustomSettings.CustomSets.GetSets().GetItems();
                    int selectedIndex = humanSettings.SelectedCharacterIndex.Value;
                    if (selectedIndex >= 0 && selectedIndex < customSets.Count)
                    {
                        container = (BaseSettingsContainer)customSets[selectedIndex];
                    }
                    else
                    {
                        container = GetCurrentSettings().GetSelectedSet();
                    }
                }
                else // Global mode
                {
                    container = GetCurrentSettings().GetSelectedSet();
                }
            }
            else
            {
                container = GetCurrentSettings().GetSelectedSet();
            }

            string cat = ((SettingsPopup)Parent).LocaleCategory;
            string sub = "Skins." + _currentCategoryPanelName.Value;
            int count = 1;
            foreach (DictionaryEntry entry in container.Settings)
            {
                BaseSetting setting = (BaseSetting)entry.Value;
                string name = (string)entry.Key;
                Transform side = count <= leftCount ? panelLeft : panelRight;
                bool isSkinSetting = name.StartsWith("Skin") && (setting.GetType() == typeof(StringSetting) || setting.GetType() == typeof(FloatSetting));
                bool isNameSetting = name == "Name";
                if ((_currentCategoryPanelName.Value == "Human" && ((HumanCustomSkinSettings)GetCurrentSettings()).SkinMode.Value == 1))
                {
                    if (isSkinSetting)
                    {
                        string displayName = UIManager.GetLocale(cat, sub, name.Substring(4));
                        ElementFactory.CreateInputSetting(side, style, setting, displayName, elementWidth: elementWidth);
                        count += 1;
                    }
                }
                else
                {
                    bool isUniqueIdSetting = name == "UniqueId";
                    if ((setting.GetType() == typeof(StringSetting) || setting.GetType() == typeof(FloatSetting)) && !isNameSetting && !isUniqueIdSetting)
                    {
                        string currentSub = sub;
                        if (name == "Ground")
                            currentSub = "Skins.Common";
                        ElementFactory.CreateInputSetting(side, style, setting, UIManager.GetLocale(cat, currentSub, name), elementWidth: elementWidth);
                        count += 1;
                    }
                }
            }
        }

        public ICustomSkinSettings GetCurrentSettings()
        {
            return _settings[_currentCategoryPanelName.Value];
        }

        private void CreateCharacterSelectorDropdown(Transform panel, ElementStyle style, HumanCustomSkinSettings humanSettings)
        {
            var customSets = SettingsManager.HumanCustomSettings.CustomSets.GetSets().GetItems();
            string[] characterNames = new string[customSets.Count];
            for (int i = 0; i < customSets.Count; i++)
            {
                var customSet = (HumanCustomSet)customSets[i];
                characterNames[i] = customSet.Name.Value;
            }
            ElementFactory.CreateDropdownSetting(panel, style, humanSettings.SelectedCharacterIndex, "Character",
                characterNames, elementWidth: 260f, onDropdownOptionSelect: () => OnCharacterSelected(humanSettings));
        }

        private void OnCharacterSelected(HumanCustomSkinSettings humanSettings)
        {
            SettingsManager.CustomSkinSettings.Save();
            RebuildCategoryPanel();
        }

        private void OnGlobalPresetSelected(int filteredIndex, List<int> originalIndices, ICustomSkinSettings settings, HumanCustomSkinSettings humanSettings)
        {
            if (filteredIndex >= 0 && filteredIndex < originalIndices.Count)
            {
                int originalIndex = originalIndices[filteredIndex];
                settings.GetSelectedSetIndex().Value = originalIndex;
                humanSettings.LastGlobalPresetIndex.Value = originalIndex;
                RebuildCategoryPanel();
            }
        }

        private void SelectNewlyCreatedOrCopiedSetInDropdown(ICustomSkinSettings allSets, HumanCustomSkinSettings humanSettings)
        {
            if (_lastFilteredOriginalIndices != null && _lastFilteredSetIndex != null)
            {
                int newOriginalIndex = allSets.GetSets().GetCount() - 1;
                string newSetName = allSets.GetSetNames()[newOriginalIndex];
                if (!newSetName.StartsWith("Custom Set:"))
                {
                    int filteredIndex = _lastFilteredOriginalIndices.IndexOf(newOriginalIndex);
                    _lastFilteredSetIndex.Value = filteredIndex >= 0 ? filteredIndex : 0;
                    humanSettings.LastGlobalPresetIndex.Value = newOriginalIndex;
                }
            }
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
