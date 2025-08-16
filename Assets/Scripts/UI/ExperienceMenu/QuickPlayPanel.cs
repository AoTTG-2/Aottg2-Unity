using ApplicationManagers;
using Map;
using Settings;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class QuickPlayPanel : CategoryPanel
    {
        protected override float Width => 1500;
        protected override float Height => 700f;
        protected override bool ScrollBar => true;

        // Top UI
        private GameObject CarouselLayout;
        private HorizontalLayoutGroup categorySelector;
        private CarouselSelector subCategorySelector;
        private CarouselSelector presetSelector;

        // Bottom UI
        private HorizontalLayoutGroup bottomUI;
        private GameObject mapSelector;
        private GameObject modeSelector;
        private GameObject weatherSelector;
        private GameObject difficultySelector;


        private List<Experience> allPresets;

        private string selectedCategory;
        private string selectedSubCategory;
        private PresetDefinition selectedPreset;
        private string selectedMode;
        private string selectedMap;

        public bool IsMultiplayer = false;

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

            // Query format: Category, SubCategory, PresetDefinition -> PresetDefinition (Modes, Maps)
            // Each category as n >= 1 subcategories
            // If there's only one subcategory we should skip its selector and instead show a selector for the PresetDefinitions under the Category/SubCategory

            CarouselLayout = ElementFactory.CreateVerticalGroup(SinglePanel, 10f);
            VerticalLayoutGroup vert = CarouselLayout.GetComponent<VerticalLayoutGroup>();
            vert.childForceExpandWidth = true;
            vert.childForceExpandHeight = false;
            LayoutElement le = vert.AddComponent<LayoutElement>();
            le.flexibleWidth = 1f;
            ContentSizeFitter cnt = CarouselLayout.AddComponent<ContentSizeFitter>();
            cnt.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            categorySelector = ElementFactory.CreateHorizontalGroup(vert.transform, 25f, TextAnchor.MiddleLeft).GetComponent<HorizontalLayoutGroup>();
            categorySelector.childForceExpandWidth = true;
            categorySelector.childForceExpandHeight = false;
            LayoutElement leHoriz = categorySelector.AddComponent<LayoutElement>();
            leHoriz.flexibleWidth = 1f;

            string[] categories = new string[] { "General", "Ranking", "PVP", "Sandbox" };
            foreach (string category in categories)
            {
                // Create panels for each category
                var btn = ElementFactory.InstantiateAndBind(categorySelector.transform, "Prefabs/Misc/MapSelectObjectButton");
                var rect = btn.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(256, 120);
                btn.GetComponent<Button>().onClick.AddListener(() => OnCategorySelected(category));
                btn.transform.Find("Icon").GetComponent<RawImage>().color = new Color(0.32f, 0.32f, 0.32f);
                btn.transform.Find("Icon").gameObject.SetActive(true);
                btn.transform.Find("Text").GetComponent<Text>().text = category;
                btn.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }

            // Setup and bind CarouselSelectors
            subCategorySelector = ElementFactory.CreateCarouselSelector(CarouselLayout.transform, 100).GetComponent<CarouselSelector>();
            presetSelector = ElementFactory.CreateCarouselSelector(CarouselLayout.transform, 100).GetComponent<CarouselSelector>();

            allPresets = BuiltinLevels.GetExperiences();

            // Hide all but the category
            subCategorySelector.ClearButtons();
            presetSelector.ClearButtons();
            DestroyPresetInformation();
        }

        private void ShowPresetInformation(PresetDefinition definition)
        {
            // Setup Map, Mode, Weather, and Difficulty Selector
            bottomUI = ElementFactory.CreateHorizontalGroup(CarouselLayout.transform, 25f, TextAnchor.MiddleLeft).GetComponent<HorizontalLayoutGroup>();
            var left = ElementFactory.CreateVerticalGroup(bottomUI.transform, 10f);
            string cat = "CreateGamePopup";
            string sub = "General";
            InGameSet settings = SettingsManager.InGameUI;
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            BasePopup selectPopup;
            if (SceneLoader.SceneName == SceneName.InGame)
                selectPopup = ((InGameMenu)UIManager.CurrentMenu)._selectMapPopup;
            else
                selectPopup = ((MainMenu)UIManager.CurrentMenu)._selectMapPopup;
            mapSelector = ElementFactory.CreateButtonPopupSetting(left.transform, dropdownStyle, settings.General.MapName, UIManager.GetLocale(cat, sub, "MapName"), selectPopup, elementWidth: 180f);

            // Create right side
            string[] maps = BuiltinLevels.QueryMapsNames(definition.MapRule);
            string[] modes = BuiltinLevels.QueryModeNames(definition.ModeRule);

            modeSelector = ElementFactory.CreateDropdownSetting(left.transform, dropdownStyle, settings.General.GameMode, UIManager.GetLocale(cat, sub, "GameMode"),
                modes, elementWidth: 180f, optionsWidth: 240f,
                onDropdownOptionSelect: () => Parent.RebuildCategoryPanel());

            ElementFactory.CreateDefaultLabel(left.transform, dropdownStyle, UIManager.GetLocale(cat, sub, "MapCategory") + ": " + settings.General.MapCategory.Value, alignment: TextAnchor.MiddleLeft);
            ElementFactory.CreateDefaultLabel(left.transform, dropdownStyle, "Description here once we get default selector working...", alignment: TextAnchor.MiddleLeft);


            ElementFactory.CreateDropdownSetting(left.transform, dropdownStyle, settings.WeatherIndex,
                UIManager.GetLocale(cat, sub, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), elementWidth: 180f);

            var right = ElementFactory.CreateVerticalGroup(bottomUI.transform, 10f);
            ElementFactory.CreateToggleGroupSetting(right.transform, dropdownStyle, settings.General.Difficulty, UIManager.GetLocale(cat, sub, "Difficulty"),
                UIManager.GetLocaleArray(cat, sub, "DifficultyOptions"));
            if (((ExperienceMenu)Parent).IsMultiplayer && SceneLoader.SceneName == SceneName.MainMenu)
            {
                ElementFactory.CreateInputSetting(right.transform, dropdownStyle, settings.General.RoomName, UIManager.GetLocale(cat, sub, "RoomName"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(right.transform, dropdownStyle, settings.General.MaxPlayers, UIManager.GetLocale(cat, sub, "MaxPlayers"), elementWidth: 200f);
                ElementFactory.CreateInputSetting(right.transform, dropdownStyle, settings.General.Password, UIManager.GetLocaleCommon("Password"), elementWidth: 200f);
            }

            // Load Settings...
        }

        private void DestroyPresetInformation()
        {
            if (bottomUI != null)
                Destroy(bottomUI.gameObject);
        }

        private void OnCategorySelected(string category)
        {
            selectedCategory = category;
            subCategorySelector.ClearButtons();
            presetSelector.ClearButtons();
            DestroyPresetInformation();

            // Collapse Category Selector Images
            foreach (Transform cat in categorySelector.transform)
            {
                cat.Find("Icon").gameObject.SetActive(false);
            }

            if (selectedCategory == "Sandbox")
            {
                OnSandboxClicked();
                return;
            }

            var subCategories = allPresets
                .Where(p => p.Category == selectedCategory)
                .Select(p => p.SubCategory)
                .Distinct()
                .ToList();

            Debug.Log($"There are {subCategories.Count} subcategory(s)");

            subCategorySelector.Populate(subCategories.Select(sc => new YourOptionData { ID = sc, Name = sc }).ToList(), OnSubCategorySelected);
        }

        private void OnSubCategorySelected(YourOptionData subCategory)
        {
            selectedSubCategory = subCategory.ID;
            presetSelector.ClearButtons();
            DestroyPresetInformation();

            var experiences = allPresets
                .Where(p => p.Category == selectedCategory && p.SubCategory == selectedSubCategory)
                .Select(p => p.Name)
                .Distinct()
                .ToList();

            Debug.Log($"There are {experiences.Count} preset(s)");

            presetSelector.Populate(experiences.Select(sc => new YourOptionData { ID = sc, Name = sc }).ToList(), OnPresetSelected);
        }

        private void OnPresetSelected(YourOptionData preset)
        {
            selectedPreset = BuiltinLevels.LoadExperience(preset.Name);


            if (selectedPreset == null)
            {
                Debug.LogError($"Preset not found for {selectedCategory} / {selectedSubCategory}");
                return;
            }

            ShowPresetInformation(selectedPreset);
            UpdateButtonsState();
        }

        private void OnModeSelected(YourOptionData mode)
        {
            selectedMode = mode.ID;
            UpdateButtonsState();
        }

        private void OnMapSelected(YourOptionData map)
        {
            selectedMap = map.ID;
            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            bool canPlay = selectedPreset != null;

            var maps = BuiltinLevels.QueryMapsNames(selectedPreset.MapRule);
            var modes = BuiltinLevels.QueryModeNames(selectedPreset.ModeRule);

            if (modes != null && modes.Length > 1)
                canPlay &= !string.IsNullOrEmpty(selectedMode);
            if (maps != null && maps.Length > 1)
                canPlay &= !string.IsNullOrEmpty(selectedMap);

            //playButton.interactable = canPlay;
            //settingsButton.gameObject.SetActive(selectedPreset != null && selectedPreset.UIRulesRaw?.Count > 0);
        }

        private void OnPlayClicked()
        {
            // Start game with selectedPreset + selectedMode + selectedMap
            Debug.Log($"Playing {selectedCategory}/{selectedSubCategory} Mode:{selectedMode} Map:{selectedMap}");
        }

        private void OnSettingsClicked()
        {
            if (SceneLoader.SceneName == SceneName.InGame)
            {
                ((ExperienceMenu)((InGameMenu)UIManager.CurrentMenu)._experienceGamePopup).OpenSettings(selectedPreset);
            }
            else
            {
                Hide();
                ((ExperienceMenu)((MainMenu)UIManager.CurrentMenu)._experienceMenuPopup).OpenSettings(selectedPreset);
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
