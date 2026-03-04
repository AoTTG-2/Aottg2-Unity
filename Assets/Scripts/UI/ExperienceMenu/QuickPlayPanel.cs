using ApplicationManagers;
using Map;
using Settings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class QuickPlayPanel : CategoryPanel
    {
        protected override float Width => 1500;
        protected override float Height => 750f;
        protected override bool ScrollBar => true;

        // Main layout wrapper
        private GameObject _mainLayout;

        // Category tabs
        private HorizontalLayoutGroup _categoryBar;
        private Dictionary<string, Button> _categoryButtons = new Dictionary<string, Button>();

        // Carousel selectors
        private CarouselSelector _subCategorySelector;
        private CarouselSelector _presetSelector;

        // Bottom details section
        private GameObject _detailsSection;

        // Data
        private List<Experience> _allPresets;
        private string _selectedCategory;
        private string _selectedSubCategory;
        private PresetDefinition _selectedPreset;
        private string _selectedMode;
        private string _selectedMap;

        // Category tab colors
        private static readonly Color NormalColor = new Color(0.25f, 0.25f, 0.25f, 1f);
        private static readonly Color SelectedColor = new Color(0.35f, 0.35f, 0.55f, 1f);
        private static readonly Color SandboxColor = new Color(0.2f, 0.55f, 0.2f, 1f);
        private static readonly Color SandboxSelectedColor = new Color(0.3f, 0.65f, 0.3f, 1f);

        protected override float GetWidth()
        {
            return Parent.GetPanelWidth();
        }

        protected override float GetHeight()
        {
            return Parent.GetPanelHeight();
        }

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _allPresets = BuiltinLevels.GetExperiences();

            // Wrapper layout — childForceExpandWidth ensures carousels and details stretch properly
            _mainLayout = ElementFactory.CreateVerticalGroup(SinglePanel, 10f);
            VerticalLayoutGroup vert = _mainLayout.GetComponent<VerticalLayoutGroup>();
            vert.childForceExpandWidth = true;
            vert.childForceExpandHeight = false;
            LayoutElement le = _mainLayout.GetComponent<LayoutElement>();
            le.flexibleWidth = 1f;

            // Category tabs + search button row
            CreateCategoryTabs();

            // Carousel selectors
            _subCategorySelector = ElementFactory.CreateCarouselSelector(_mainLayout.transform, 70).GetComponent<CarouselSelector>();
            _presetSelector = ElementFactory.CreateCarouselSelector(_mainLayout.transform, 70).GetComponent<CarouselSelector>();
            _subCategorySelector.ClearButtons();
            _presetSelector.ClearButtons();
        }

        private void CreateCategoryTabs()
        {
            var topRow = ElementFactory.CreateHorizontalGroup(_mainLayout.transform, 0f, TextAnchor.MiddleCenter);
            LayoutElement topLE = topRow.GetComponent<LayoutElement>();
            topLE.flexibleWidth = 1f;
            topLE.preferredHeight = 55f;

            // Category buttons
            _categoryBar = ElementFactory.CreateHorizontalGroup(topRow.transform, 10f, TextAnchor.MiddleLeft)
                .GetComponent<HorizontalLayoutGroup>();
            _categoryBar.childForceExpandWidth = false;
            _categoryBar.childForceExpandHeight = false;
            LayoutElement barLE = _categoryBar.GetComponent<LayoutElement>();
            barLE.flexibleWidth = 1f;

            string[] categories = { "General", "Ranking", "PVP", "Sandbox" };
            ElementStyle tabStyle = new ElementStyle(fontSize: 22, themePanel: ThemePanel);

            foreach (string category in categories)
            {
                bool isSandbox = category == "Sandbox";
                string label = isSandbox ? "\u25B6 " + category : category;

                var btnObj = ElementFactory.CreateDefaultButton(
                    _categoryBar.transform, tabStyle, label,
                    elementWidth: 160f, elementHeight: 45f,
                    onClick: () => OnCategorySelected(category));

                Button btn = btnObj.GetComponent<Button>();
                Color normal = isSandbox ? SandboxColor : NormalColor;
                SetButtonColor(btn, normal);
                _categoryButtons[category] = btn;
            }

            // Search / Load Preset button
            ElementStyle searchStyle = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            var searchBtn = ElementFactory.CreateDefaultButton(topRow.transform, searchStyle, "Search",
                elementWidth: 90f, elementHeight: 45f,
                onClick: () => OnSearchPresetClicked());
            searchBtn.GetComponent<LayoutElement>().flexibleWidth = 0f;
        }

        private void SetButtonColor(Button btn, Color color)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = color;
            colors.highlightedColor = color + new Color(0.08f, 0.08f, 0.08f, 0f);
            colors.pressedColor = color - new Color(0.05f, 0.05f, 0.05f, 0f);
            colors.selectedColor = color;
            btn.colors = colors;
        }

        private void HighlightCategory(string selected)
        {
            foreach (var kvp in _categoryButtons)
            {
                bool isActive = kvp.Key == selected;
                bool isSandbox = kvp.Key == "Sandbox";
                Color color;
                if (isActive)
                    color = isSandbox ? SandboxSelectedColor : SelectedColor;
                else
                    color = isSandbox ? SandboxColor : NormalColor;
                SetButtonColor(kvp.Value, color);
            }
        }

        private void OnCategorySelected(string category)
        {
            _selectedCategory = category;
            _selectedSubCategory = null;
            _selectedPreset = null;
            _selectedMode = null;
            _selectedMap = null;

            _subCategorySelector.ClearButtons();
            _presetSelector.ClearButtons();
            DestroyDetailsSection();
            HighlightCategory(category);

            ExperienceMenu expMenu = Parent as ExperienceMenu;
            if (expMenu != null)
                expMenu.SelectedPreset = null;

            if (category == "Sandbox")
            {
                OnSandboxClicked();
                return;
            }

            var subCategories = _allPresets
                .Where(p => p.Category == category)
                .Select(p => p.SubCategory)
                .Distinct()
                .ToList();

            if (subCategories.Count == 1)
            {
                // Auto-skip when only one subcategory
                _selectedSubCategory = subCategories[0];
                PopulatePresets();
            }
            else if (subCategories.Count > 1)
            {
                _subCategorySelector.Populate(
                    subCategories.Select(sc => new YourOptionData { ID = sc, Name = sc }).ToList(),
                    OnSubCategorySelected);
            }
        }

        private void OnSubCategorySelected(YourOptionData subCategory)
        {
            _selectedSubCategory = subCategory.ID;
            _selectedPreset = null;
            _selectedMode = null;
            _selectedMap = null;

            _presetSelector.ClearButtons();
            DestroyDetailsSection();
            PopulatePresets();
        }

        private void PopulatePresets()
        {
            var presetNames = _allPresets
                .Where(p => p.Category == _selectedCategory && p.SubCategory == _selectedSubCategory)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            if (presetNames.Count == 1)
            {
                // Auto-select single preset
                OnPresetSelected(new YourOptionData { ID = presetNames[0], Name = presetNames[0] });
            }
            else if (presetNames.Count > 1)
            {
                _presetSelector.Populate(
                    presetNames.Select(n => new YourOptionData { ID = n, Name = n }).ToList(),
                    OnPresetSelected);
            }
        }

        private void OnPresetSelected(YourOptionData preset)
        {
            _selectedPreset = BuiltinLevels.LoadExperience(preset.Name);
            if (_selectedPreset == null)
            {
                Debug.LogError($"Preset not found: {preset.Name}");
                return;
            }

            ApplyPresetDefaults(_selectedPreset);
            DestroyDetailsSection();
            ShowDetailsSection(_selectedPreset);
            UpdateButtonsState();

            ExperienceMenu expMenu = Parent as ExperienceMenu;
            if (expMenu != null)
                expMenu.SelectedPreset = _selectedPreset;
        }

        private void ApplyPresetDefaults(PresetDefinition definition)
        {
            if (definition.UIRulesObject == null) return;
            InGameSet settings = SettingsManager.InGameUI;
            CreateGameCategoryPanel.ApplyPresetDefaults(definition, settings);
        }

        private void ShowDetailsSection(PresetDefinition definition)
        {
            // Two-column details section
            _detailsSection = ElementFactory.CreateHorizontalGroup(_mainLayout.transform, 20f, TextAnchor.UpperLeft);
            HorizontalLayoutGroup hlg = _detailsSection.GetComponent<HorizontalLayoutGroup>();
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;
            LayoutElement sectionLE = _detailsSection.GetComponent<LayoutElement>();
            sectionLE.flexibleWidth = 1f;

            // Fade-in animation
            CanvasGroup cg = _detailsSection.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            StartCoroutine(FadeIn(cg, 0.2f));

            float panelWidth = GetWidth();
            string cat = "CreateGamePopup";
            string sub = "General";
            InGameSet settings = SettingsManager.InGameUI;
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);

            // ─── Left column: Preset info ───
            var leftCol = ElementFactory.CreateVerticalGroup(_detailsSection.transform, 8f);
            LayoutElement leftLE = leftCol.GetComponent<LayoutElement>();
            leftLE.preferredWidth = panelWidth * 0.45f;
            leftLE.flexibleWidth = 0f;

            ElementStyle headerStyle = new ElementStyle(fontSize: 24, themePanel: ThemePanel);
            ElementStyle bodyStyle = new ElementStyle(fontSize: 18, themePanel: ThemePanel);

            // Preset name
            ElementFactory.CreateDefaultLabel(leftCol.transform, headerStyle, definition.Name, FontStyle.Bold, TextAnchor.MiddleLeft);

            // Description
            string description = !string.IsNullOrEmpty(definition.Description) ? definition.Description : "No description available.";
            ElementFactory.CreateDefaultLabel(leftCol.transform, bodyStyle, description, alignment: TextAnchor.UpperLeft);

            // Map selection — filtered by MapRule
            string[] allowedMaps = BuiltinLevels.QueryMapsNames(definition.MapRule);
            if (allowedMaps.Length > 0 && !allowedMaps.Contains(settings.General.MapName.Value))
            {
                settings.General.MapName.Value = allowedMaps[0];
                settings.General.MapCategory.Value = BuiltinLevels.FindMapCategory(allowedMaps[0]);
            }
            bool mapLocked = definition.MapRule != null && definition.MapRule.DisableEditing;
            bool mapChangeable = !mapLocked && allowedMaps.Length > 1;

            // Map preview image — clickable when the map can be changed
            string mapName = settings.General.MapName.Value;
            object previewTexture = ResourceManager.LoadAsset(ResourcePaths.BuiltinMaps, "Previews/" + mapName + "Preview");
            if (previewTexture != null)
            {
                var previewImg = ElementFactory.CreateRawImage(leftCol.transform, bodyStyle, "", 200f, 200f);
                previewImg.GetComponent<RawImage>().texture = (Texture2D)previewTexture;

                if (mapChangeable)
                {
                    // Make the image clickable
                    var btn = previewImg.AddComponent<Button>();
                    ColorBlock colors = btn.colors;
                    colors.highlightedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                    colors.pressedColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                    btn.colors = colors;
                    btn.targetGraphic = previewImg.GetComponent<RawImage>();
                    btn.onClick.AddListener(() => OnMapPreviewClicked(definition));

                    // "Click to change" overlay label
                    var overlay = new GameObject("ChangeMapLabel", typeof(RectTransform), typeof(Text));
                    overlay.transform.SetParent(previewImg.transform, false);
                    var overlayRT = overlay.GetComponent<RectTransform>();
                    overlayRT.anchorMin = new Vector2(0f, 0f);
                    overlayRT.anchorMax = new Vector2(1f, 0.25f);
                    overlayRT.offsetMin = Vector2.zero;
                    overlayRT.offsetMax = Vector2.zero;
                    var overlayText = overlay.GetComponent<Text>();
                    overlayText.text = "\u270E Change Map";
                    overlayText.fontSize = 14;
                    overlayText.alignment = TextAnchor.MiddleCenter;
                    overlayText.color = new Color(1f, 1f, 1f, 0.85f);
                    overlayText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                    overlayText.raycastTarget = false;

                    // Semi-transparent background behind the label
                    var overlayBg = new GameObject("ChangeMapBg", typeof(RectTransform), typeof(Image));
                    overlayBg.transform.SetParent(previewImg.transform, false);
                    overlayBg.transform.SetSiblingIndex(overlay.transform.GetSiblingIndex());
                    var bgRT = overlayBg.GetComponent<RectTransform>();
                    bgRT.anchorMin = new Vector2(0f, 0f);
                    bgRT.anchorMax = new Vector2(1f, 0.25f);
                    bgRT.offsetMin = Vector2.zero;
                    bgRT.offsetMax = Vector2.zero;
                    overlayBg.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.55f);
                    overlayBg.GetComponent<Image>().raycastTarget = false;
                }
            }

            // Map name label
            ElementFactory.CreateDefaultLabel(leftCol.transform, bodyStyle,
                settings.General.MapName.Value,
                alignment: TextAnchor.MiddleLeft);

            ElementFactory.CreateHorizontalLine(leftCol.transform, new ElementStyle(themePanel: ThemePanel), panelWidth * 0.4f);

            // Mode selector — filtered by ModeRule
            string[] modes = BuiltinLevels.QueryModeNames(definition.ModeRule);
            bool modeLocked = definition.ModeRule != null && definition.ModeRule.DisableEditing;
            if (modeLocked || modes.Length <= 1)
            {
                ElementFactory.CreateDefaultLabel(leftCol.transform, dropdownStyle,
                    UIManager.GetLocale(cat, sub, "GameMode") + ": " + settings.General.GameMode.Value,
                    alignment: TextAnchor.MiddleLeft);
            }
            else
            {
                if (!modes.Contains(settings.General.GameMode.Value) && modes.Length > 0)
                    settings.General.GameMode.Value = modes[0];
                ElementFactory.CreateDropdownSetting(leftCol.transform, dropdownStyle, settings.General.GameMode,
                    UIManager.GetLocale(cat, sub, "GameMode"), modes,
                    elementWidth: 180f, optionsWidth: 240f,
                    onDropdownOptionSelect: () => Parent.RebuildCategoryPanel());
            }

            // ─── Right column: Configurable settings ───
            var rightCol = ElementFactory.CreateVerticalGroup(_detailsSection.transform, 8f);
            LayoutElement rightLE = rightCol.GetComponent<LayoutElement>();
            rightLE.preferredWidth = panelWidth * 0.45f;
            rightLE.flexibleWidth = 0f;

            ElementFactory.CreateDefaultLabel(rightCol.transform, headerStyle, "Settings", FontStyle.Bold, TextAnchor.MiddleLeft);

            // Difficulty — filtered by allowed values
            UIRule difficultyRule = GetRule(definition, "general.Difficulty");
            if (IsSettingVisible(definition, "general.Difficulty"))
            {
                string[] allDiffOptions = UIManager.GetLocaleArray(cat, sub, "DifficultyOptions");
                if (difficultyRule != null && difficultyRule.AllowedValues != null && difficultyRule.AllowedValues.Count > 0)
                {
                    // Filter difficulty options based on allowed values
                    string[] diffNames = { "training", "easy", "normal", "hard", "abnormal" };
                    List<string> filteredOptions = new List<string>();
                    List<int> filteredIndices = new List<int>();
                    for (int i = 0; i < diffNames.Length && i < allDiffOptions.Length; i++)
                    {
                        if (difficultyRule.AllowedValues.Any(a => a.Equals(diffNames[i], System.StringComparison.OrdinalIgnoreCase)))
                        {
                            filteredOptions.Add(allDiffOptions[i]);
                            filteredIndices.Add(i);
                        }
                    }
                    // Use a StringSetting-backed dropdown for filtered difficulty
                    int currentIdx = settings.General.Difficulty.Value;
                    string currentLabel = (currentIdx >= 0 && currentIdx < allDiffOptions.Length) ? allDiffOptions[currentIdx] : allDiffOptions[0];
                    if (!filteredOptions.Contains(currentLabel))
                        currentLabel = filteredOptions[0];
                    StringSetting diffProxy = new StringSetting(currentLabel);
                    ElementFactory.CreateDropdownSetting(rightCol.transform, dropdownStyle, diffProxy,
                        UIManager.GetLocale(cat, sub, "Difficulty"), filteredOptions.ToArray(),
                        elementWidth: 180f, optionsWidth: 200f,
                        onDropdownOptionSelect: () =>
                        {
                            int idx = filteredOptions.IndexOf(diffProxy.Value);
                            if (idx >= 0 && idx < filteredIndices.Count)
                                settings.General.Difficulty.Value = filteredIndices[idx];
                        });
                }
                else
                {
                    ElementFactory.CreateDropdownSetting(rightCol.transform, dropdownStyle, settings.General.Difficulty,
                        UIManager.GetLocale(cat, sub, "Difficulty"), allDiffOptions,
                        elementWidth: 180f, optionsWidth: 200f);
                }
            }

            // Weather
            if (IsSettingVisible(definition, "general.WeatherIndex"))
            {
                ElementFactory.CreateDropdownSetting(rightCol.transform, dropdownStyle, settings.WeatherIndex,
                    UIManager.GetLocale(cat, sub, "Weather"),
                    SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);
            }

            // Multiplayer settings
            if (((ExperienceMenu)Parent).IsMultiplayer && SceneLoader.SceneName == SceneName.MainMenu)
            {
                ElementFactory.CreateHorizontalLine(rightCol.transform, new ElementStyle(themePanel: ThemePanel), panelWidth * 0.4f);
                ElementFactory.CreateDefaultLabel(rightCol.transform, headerStyle, "Multiplayer", FontStyle.Bold, TextAnchor.MiddleLeft);
                ElementFactory.CreateInputSetting(rightCol.transform, dropdownStyle, settings.General.RoomName,
                    UIManager.GetLocale(cat, sub, "RoomName"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(rightCol.transform, dropdownStyle, settings.General.MaxPlayers,
                    UIManager.GetLocale(cat, sub, "MaxPlayers"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(rightCol.transform, dropdownStyle, settings.General.Password,
                    UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
            }
        }

        private UIRule GetRule(PresetDefinition definition, string settingId)
        {
            if (definition.UIRulesObject == null) return null;
            foreach (var rule in definition.UIRulesObject)
            {
                if (rule.SettingId == settingId)
                    return rule;
            }
            return null;
        }

        private bool IsSettingVisible(PresetDefinition definition, string settingId)
        {
            if (definition.UIRulesObject == null || definition.UIRulesObject.Count == 0)
                return true;

            foreach (var rule in definition.UIRulesObject)
            {
                if (rule.SettingId == settingId && rule.Type == UIRuleType.Hide)
                    return false;
            }
            return true;
        }

        private IEnumerator FadeIn(CanvasGroup cg, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (cg == null) yield break;
                elapsed += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            if (cg != null) cg.alpha = 1f;
        }

        private void OnMapPreviewClicked(PresetDefinition definition)
        {
            CreateGameSelectMapPopup selectPopup;
            if (SceneLoader.SceneName == SceneName.InGame)
                selectPopup = (CreateGameSelectMapPopup)((InGameMenu)UIManager.CurrentMenu)._selectMapPopup;
            else
                selectPopup = (CreateGameSelectMapPopup)((MainMenu)UIManager.CurrentMenu)._selectMapPopup;
            selectPopup.ActiveMapRule = definition.MapRule;
            selectPopup.Show();
            StartCoroutine(WaitForMapPopupClosed(selectPopup, definition));
        }

        private IEnumerator WaitForMapPopupClosed(BasePopup popup, PresetDefinition definition)
        {
            // Wait one frame so the popup is fully active
            yield return null;
            while (popup != null && popup.gameObject.activeSelf)
                yield return null;
            // Rebuild the details section with the newly selected map
            if (_selectedPreset != null)
            {
                DestroyDetailsSection();
                ShowDetailsSection(definition);
            }
        }

        private void DestroyDetailsSection()
        {
            if (_detailsSection != null)
                Destroy(_detailsSection);
        }

        private void UpdateButtonsState()
        {
            if (_selectedPreset == null)
                return;

            ExperienceMenu menu = Parent as ExperienceMenu;
            if (menu == null)
                return;

            bool canPlay = true;

            if (_selectedPreset.MapRule != null)
            {
                var maps = BuiltinLevels.QueryMapsNames(_selectedPreset.MapRule);
                if (maps != null && maps.Length > 1)
                    canPlay &= !string.IsNullOrEmpty(_selectedMap);
            }

            if (_selectedPreset.ModeRule != null)
            {
                var modes = BuiltinLevels.QueryModeNames(_selectedPreset.ModeRule);
                if (modes != null && modes.Length > 1)
                    canPlay &= !string.IsNullOrEmpty(_selectedMode);
            }

            if (menu.playButton != null)
                menu.playButton.interactable = canPlay;
            if (menu.settingsButton != null)
                menu.settingsButton.gameObject.SetActive(
                    _selectedPreset.UIRulesObject != null && _selectedPreset.UIRulesObject.Count > 0);
        }

        private void OnSearchPresetClicked()
        {
            UIManager.CurrentMenu.SelectListPopup.ShowLoad(
                SettingsManager.InGameSettings.InGameSets.GetSetNames().ToList(),
                onLoad: () => OnLoadPreset());
        }

        private void OnLoadPreset()
        {
            string name = UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value;
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
            {
                if (set.Name.Value == name)
                {
                    SettingsManager.InGameUI.Copy(set);
                    Parent.RebuildCategoryPanel();
                    return;
                }
            }
        }

        private void OnSandboxClicked()
        {
            if (SceneLoader.SceneName == SceneName.InGame)
            {
                ((ExperienceMenu)((InGameMenu)UIManager.CurrentMenu)._experienceGamePopup).OpenSettings();
            }
            else
            {
                Hide();
                ((ExperienceMenu)((MainMenu)UIManager.CurrentMenu)._experienceMenuPopup).OpenSettings();
            }
        }
    }
}
