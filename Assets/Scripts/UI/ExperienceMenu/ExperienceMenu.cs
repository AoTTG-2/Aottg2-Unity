using ApplicationManagers;
using GameManagers;
using Map;
using Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class ExperienceMenu : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1500;
        protected override float Height => 800f;
        protected override bool CategoryPanel => false;
        protected override bool CategoryButtons => false;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "ExperiencePopup";
        public bool IsMultiplayer = false;
        protected override bool UseSound => true;

        private CarouselSelector categorySelector;
        private CarouselSelector subCategorySelector;
        private CarouselSelector modeSelector;
        private CarouselSelector mapSelector;

        private Button playButton;
        private Button settingsButton;
        private Button sandboxButton;

        private List<Experience> allPresets;

        private string selectedCategory;
        private string selectedSubCategory;
        private PresetDefinition selectedPreset;
        private string selectedMode;
        private string selectedMap;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);

            // Setup and bind CarouselSelectors
            categorySelector = ElementFactory.CreateCarouselSelector(SinglePanel).GetComponent<CarouselSelector>();
            subCategorySelector = ElementFactory.CreateCarouselSelector(SinglePanel).GetComponent<CarouselSelector>();
            modeSelector = ElementFactory.CreateCarouselSelector(SinglePanel).GetComponent<CarouselSelector>();
            mapSelector = ElementFactory.CreateCarouselSelector(SinglePanel).GetComponent<CarouselSelector>();

            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);

            sandboxButton = ElementFactory.CreateTextButton(BottomBar, style, "Sandbox", onClick: () => OnSandboxClicked()).GetComponent<Button>();
            settingsButton = ElementFactory.CreateTextButton(BottomBar, style, "Settings", onClick: () => OnSettingsClicked()).GetComponent<Button>();
            playButton = ElementFactory.CreateTextButton(BottomBar, style, "Play", onClick: () => OnPlayClicked()).GetComponent<Button>();



            allPresets = BuiltinLevels.GetExperiences();

            PopulateCategories();

            SetupBottomButtons();

        }

        private void PopulateCategories()
        {
            var categories = allPresets.Select(p => p.Category).Distinct().ToList();
            categorySelector.Populate(categories.Select(c => new YourOptionData { ID = c, Name = c }).ToList(), OnCategorySelected);    // need to resolve locale and icon eventually.

            // Clear deeper selectors
            subCategorySelector.ClearButtons();
            modeSelector.ClearButtons();
            mapSelector.ClearButtons();

            playButton.interactable = false;
            settingsButton.gameObject.SetActive(false);
        }

        private void OnCategorySelected(YourOptionData category)
        {
            selectedCategory = category.ID;

            var subCategories = allPresets
                .Where(p => p.Category == selectedCategory)
                .Select(p => p.SubCategory)
                .Distinct()
                .ToList();

            if (subCategories.Count == 1)
            {
                selectedSubCategory = subCategories[0];
                subCategorySelector.ClearButtons();
                OnSubCategorySelected(new YourOptionData { ID = selectedSubCategory, Name = selectedSubCategory });
            }
            else
            {
                subCategorySelector.Populate(subCategories.Select(sc => new YourOptionData { ID = sc, Name = sc }).ToList(), OnSubCategorySelected);
            }

            // Clear lower selectors
            modeSelector.ClearButtons();
            mapSelector.ClearButtons();

            playButton.interactable = false;
            settingsButton.gameObject.SetActive(false);
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
            var modes = selectedPreset.ModesRaw ?? new List<string>();
            if (modes.Count == 1)
            {
                selectedMode = modes[0];
                modeSelector.ClearButtons();
            }
            else if (modes.Count > 1)
            {
                modeSelector.Populate(modes.Select(m => new YourOptionData { ID = m, Name = m }).ToList(), OnModeSelected);
            }
            else
            {
                selectedMode = null;
                modeSelector.ClearButtons();
            }

            // Populate maps (or auto-select if only one)
            var maps = selectedPreset.MapsRaw ?? new List<string>();
            if (maps.Count == 1)
            {
                selectedMap = maps[0];
                mapSelector.ClearButtons();
            }
            else if (maps.Count > 1)
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

            if (selectedPreset.ModesRaw != null && selectedPreset.ModesRaw.Count > 1)
                canPlay &= !string.IsNullOrEmpty(selectedMode);
            if (selectedPreset.MapsRaw != null && selectedPreset.MapsRaw.Count > 1)
                canPlay &= !string.IsNullOrEmpty(selectedMap);

            playButton.interactable = canPlay;
            settingsButton.gameObject.SetActive(selectedPreset != null && selectedPreset.UIRulesRaw?.Count > 0);
        }

        private void OnPlayClicked()
        {
            // Start game with selectedPreset + selectedMode + selectedMap
            Debug.Log($"Playing {selectedCategory}/{selectedSubCategory} Mode:{selectedMode} Map:{selectedMap}");
        }

        private void OnSettingsClicked()
        {
            // Open settings panel, passing selectedPreset
            Debug.Log("Open settings panel");
        }

        private void OnSandboxClicked()
        {
            // Open sandbox panel with no preset
            Debug.Log("Open sandbox panel");
        }

        private void SetupBottomButtons()
        {
            //ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            //string start = SceneLoader.SceneName == SceneName.InGame ? "Restart" : "Start";
            //foreach (string buttonName in new string[] { "Import", "Export", "LoadPreset", "SavePreset", start, "Back" })
            //{
            //    string locale = UIManager.GetLocaleCommon(buttonName);
            //    GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, locale,
            //        onClick: () => OnBottomBarButtonClick(buttonName));
            //}
        }

        public void Show(bool isMultiplayer)
        {
            base.Show();
            if (IsMultiplayer != isMultiplayer)
            {
                IsMultiplayer = isMultiplayer;
                StartCoroutine(WaitAndRebuildCategoryPanel(0.2f));
            }
        }

        public void HideNoDisconnect()
        {
            base.Hide();
        }

        public override void Hide()
        {
            if (gameObject.activeSelf && SceneLoader.SceneName == SceneName.MainMenu)
            {
                SettingsManager.MultiplayerSettings.Disconnect();
            }
            base.Hide();
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Start":
                    MusicManager.PlayEffect();
                    MusicManager.PlayTransition();
                    SettingsManager.InGameCurrent.Copy(SettingsManager.InGameUI);
                    if (!IsMultiplayer)
                        SettingsManager.MultiplayerSettings.ConnectOffline();
                    SettingsManager.MultiplayerSettings.StartRoom();
                    break;

                case "Restart":
                    InGameManager.RestartGame();
                    ((InGameMenu)UIManager.CurrentMenu).SkipAHSSInput = true; // Prevents AHSS players from shooting when restarting the map/level
                    break;

                case "Settings":
                    break;

                case "Back":
                    if (SceneLoader.SceneName == SceneName.InGame)
                    {
                        ((InGameMenu)UIManager.CurrentMenu).SkipAHSSInput = true;
                        HideNoDisconnect();
                    }
                    else if (IsMultiplayer)
                    {
                        HideNoDisconnect();
                        ((MultiplayerRoomListPopup)((MainMenu)UIManager.CurrentMenu)._multiplayerRoomListPopup).Show();
                    }
                    else
                        Hide();
                    break;
            }
            SettingsManager.WeatherSettings.Save();
            SettingsManager.WeatherSettings.Load();
        }
    }
}
