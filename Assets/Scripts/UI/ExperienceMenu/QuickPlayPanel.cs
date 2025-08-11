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

        private GameObject CarouselLayout;
        private CarouselSelector categorySelector;
        private CarouselSelector subCategorySelector;
        private CarouselSelector modeSelector;
        private CarouselSelector mapSelector;
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

            CarouselLayout = ElementFactory.CreateVerticalGroup(SinglePanel, 10f);
            VerticalLayoutGroup vert = CarouselLayout.GetComponent<VerticalLayoutGroup>();
            vert.childForceExpandWidth = true;
            vert.childForceExpandHeight = false;
            LayoutElement le = vert.AddComponent<LayoutElement>();
            le.flexibleWidth = 1f;
            ContentSizeFitter cnt = CarouselLayout.AddComponent<ContentSizeFitter>();
            cnt.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            HorizontalLayoutGroup horiz = ElementFactory.CreateHorizontalGroup(vert.transform, 25f, TextAnchor.MiddleLeft).GetComponent<HorizontalLayoutGroup>();
            horiz.childForceExpandWidth = true;
            horiz.childForceExpandHeight = false;
            LayoutElement leHoriz = horiz.AddComponent<LayoutElement>();
            leHoriz.flexibleWidth = 1f;

            string[] categories = new string[] { "General", "Ranking", "PVP", "Sandbox" };
            foreach (string category in categories)
            {
                // Create panels for each category
                var btn = ElementFactory.InstantiateAndBind(horiz.transform, "Prefabs/Misc/MapSelectObjectButton");
                var rect = btn.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(256, 120);
                btn.GetComponent<Button>().onClick.AddListener(() => OnCategorySelected(category));
                btn.transform.Find("Icon").GetComponent<RawImage>().color = new Color(0.32f, 0.32f, 0.32f);
                btn.transform.Find("Text").GetComponent<Text>().text = category;
                btn.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }

            // Setup and bind CarouselSelectors
            subCategorySelector = ElementFactory.CreateCarouselSelector(CarouselLayout.transform).GetComponent<CarouselSelector>();
            modeSelector = ElementFactory.CreateCarouselSelector(CarouselLayout.transform).GetComponent<CarouselSelector>();
            mapSelector = ElementFactory.CreateCarouselSelector(CarouselLayout.transform).GetComponent<CarouselSelector>();

            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);


            allPresets = BuiltinLevels.GetExperiences();
            PopulateCategories();
        }

        private void PopulateCategories()
        {
            var categories = allPresets.Select(p => p.Category).Distinct().ToList();

            // Clear deeper selectors
            subCategorySelector.ClearButtons();
            modeSelector.ClearButtons();
            mapSelector.ClearButtons();

            // Will need to hide Headed panel's button based on this thing's logic.
        }

        private void OnCategorySelected(string category)
        {
            selectedCategory = category;

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

            Debug.Log(subCategories.Count);

            subCategorySelector.Populate(subCategories.Select(sc => new YourOptionData { ID = sc, Name = sc }).ToList(), OnSubCategorySelected);

            if (subCategories.Count == 1)
            {
                selectedSubCategory = subCategories[0];
                OnSubCategorySelected(new YourOptionData { ID = selectedSubCategory, Name = selectedSubCategory });
            }

            // Clear lower selectors
            modeSelector.ClearButtons();
            mapSelector.ClearButtons();

            //playButton.interactable = false;
            //settingsButton.gameObject.SetActive(false);
        }

        private void OnSubCategorySelected(YourOptionData subCategory)
        {
            selectedSubCategory = subCategory.ID;
            var experience = allPresets.FirstOrDefault(p => p.Category == selectedCategory && p.SubCategory == selectedSubCategory);

            selectedPreset = BuiltinLevels.LoadExperience(experience.Name);

            if (selectedPreset == null)
            {
                Debug.LogError($"Preset not found for {selectedCategory} / {selectedSubCategory}");
                return;
            }

            // Populate modes (or auto-select if only one)
            var maps = BuiltinLevels.QueryMapsNames(selectedPreset.MapRule);
            var modes = BuiltinLevels.QueryModeNames(selectedPreset.ModeRule);

            modeSelector.Populate(modes.Select(m => new YourOptionData { ID = m, Name = m }).ToList(), OnModeSelected);

            if (modes.Length == 1)
            {
                selectedMode = modes[0];
            }

            // This will be changed to a Map Selector/Mode Selector/Weather/Difficulty
            if (maps.Length == 1)
            {
                selectedMap = maps[0];
                mapSelector.ClearButtons();
            }
            else if (maps.Length > 1)
            {
                mapSelector.Populate(maps.Select(m => new YourOptionData { ID = m, Name = m }).ToList(), OnMapSelected);
            }
            else
            {
                selectedMap = null;
                mapSelector.ClearButtons();
            }

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
