using ApplicationManagers;
using GameManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class ExperienceMenu : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1200f;
        protected override float Height => 800f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "ExperiencePopup";

        protected override bool UseSound => true;

        protected override bool ScrollBar => true;
        public Button playButton;
        public Button settingsButton;
        public Button backButton;
        public Button sandboxButton;
        public bool IsMultiplayer = false;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();

        }

        private void SetupBottomButtons()
        {

            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);

            // Create containers
            GameObject leftContainer = new GameObject("LeftContainer", typeof(RectTransform));
            GameObject rightContainer = new GameObject("RightContainer", typeof(RectTransform));

            leftContainer.transform.SetParent(BottomBar.transform, false);
            rightContainer.transform.SetParent(BottomBar.transform, false);

            // Add layout groups
            leftContainer.AddComponent<HorizontalLayoutGroup>();
            rightContainer.AddComponent<HorizontalLayoutGroup>();

            // Optional: Add LayoutElement to control sizing
            LayoutElement leftLayout = leftContainer.AddComponent<LayoutElement>();
            LayoutElement rightLayout = rightContainer.AddComponent<LayoutElement>();
            leftLayout.flexibleWidth = 1;
            rightLayout.flexibleWidth = 1;

            // Create buttons
            string start = SceneLoader.SceneName == SceneName.InGame ? "Restart" : "Start";

            backButton = ElementFactory.CreateTextButton(leftContainer.transform, style, "Back",
                onClick: () => OnBottomBarButtonClick("Back")).GetComponent<Button>();

            sandboxButton = ElementFactory.CreateTextButton(rightContainer.transform, style, "Sandbox",
                onClick: () => OnBottomBarButtonClick("Sandbox")).GetComponent<Button>();

            settingsButton = ElementFactory.CreateTextButton(rightContainer.transform, style, "Settings",
                onClick: () => OnBottomBarButtonClick("Settings")).GetComponent<Button>();

            playButton = ElementFactory.CreateTextButton(rightContainer.transform, style, start,
                onClick: () => OnBottomBarButtonClick(start)).GetComponent<Button>();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "General", "Add Content" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, buttonName, // UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button") do this later
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("General", typeof(QuickPlayPanel));
            _categoryPanelTypes.Add("Add Content", typeof(CreateGameCustomPanel));
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


        public void OpenSettings(PresetDefinition preset = null)
        {
            if (SceneLoader.SceneName == SceneName.InGame)
            {
                ((InGameMenu)UIManager.CurrentMenu).SkipAHSSInput = true;
                ((CreateGamePopup)((InGameMenu)UIManager.CurrentMenu)._createGamePopup).Show();
                HideNoDisconnect();
            }
            else if (IsMultiplayer)
            {
                HideNoDisconnect();
                ((CreateGamePopup)((MainMenu)UIManager.CurrentMenu)._createGamePopup).Show();
            }
            else
            {
                Hide();
                ((CreateGamePopup)((MainMenu)UIManager.CurrentMenu)._createGamePopup).Show();
            }
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
                    OpenSettings();
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
