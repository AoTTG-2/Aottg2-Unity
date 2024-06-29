using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections;
using Photon.Pun;
using Utility;

namespace UI
{
    class MainMenu: BaseMenu
    {
        public BasePopup _createGamePopup;
        public BasePopup _selectMapPopup;
        public BasePopup _multiplayerMapPopup;
        public BasePopup _settingsPopup;
        public BasePopup _toolsPopup;
        public BasePopup _multiplayerRoomListPopup;
        public BasePopup _editProfilePopup;
        public BasePopup _leaderboardPopup;
        public BasePopup _socialPopup;
        public BasePopup _aboutPopup;
        public BasePopup _questPopup;
        public BasePopup _tutorialPopup;
        public OutdatedPopup _outdatedPopup;
        public MainBackgroundMenu _backgroundMenu;
        public TipPanel _tipPanel;
        protected Text _multiplayerStatusLabel;
        protected string _lastButtonClicked;
        protected Image _introPanelBackground;
        public static JSONNode MainBackgroundInfo = null;
        protected const float ChangeBackgroundTime = 20f;
        private static bool ShowedOutdated = false;

        public override void Setup()
        {
            base.Setup();
            if (MainBackgroundInfo == null)
                MainBackgroundInfo = JSON.Parse(ResourceManager.LoadText(ResourcePaths.Info, "MainBackgroundInfo"));
            SetupMainBackground();
            SetupIntroPanel();
            SetupLabels();
        }

        private void SetupMainBackground()
        {
            _backgroundMenu = ElementFactory.CreateMenu<MainBackgroundMenu>("Prefabs/Panels/BackgroundMenu");
            _backgroundMenu.Setup();
            _tipPanel = ElementFactory.CreateTipPanel(transform, enabled: true);
            _tipPanel.SetRandomTip();
            ElementFactory.SetAnchor(_tipPanel.gameObject, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(10f, -10f));
            StartCoroutine(WaitAndChangeBackground());
        }

        public void ShowMultiplayerRoomListPopup()
        {
            HideAllPopups();
            _multiplayerRoomListPopup.Show();
        }

        public void ShowMultiplayerMapPopup()
        {
            HideAllPopups();
            _multiplayerMapPopup.Show();
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _selectMapPopup = ElementFactory.CreateHeadedPanel<CreateGameSelectMapPopup>(transform).GetComponent<CreateGameSelectMapPopup>();
            _createGamePopup = ElementFactory.CreateHeadedPanel<CreateGamePopup>(transform).GetComponent<CreateGamePopup>();
            _multiplayerMapPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerMapPopup>(transform, "Prefabs/MainMenu/MultiplayerMapPopup").
                GetComponent<BasePopup>();
            _editProfilePopup = ElementFactory.CreateHeadedPanel<EditProfilePopup>(transform).GetComponent<BasePopup>();
            _settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(transform).GetComponent<BasePopup>();
            _toolsPopup = ElementFactory.CreateHeadedPanel<ToolsPopup>(transform).GetComponent<BasePopup>();
            _multiplayerRoomListPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerRoomListPopup>(transform, "Prefabs/MainMenu/MultiplayerRoomListPopup").
                GetComponent<BasePopup>();
            _leaderboardPopup = ElementFactory.CreateHeadedPanel<LeaderboardPopup>(transform).GetComponent<BasePopup>();
            _socialPopup = ElementFactory.CreateHeadedPanel<SocialPopup>(transform).GetComponent<BasePopup>();
            _aboutPopup = ElementFactory.CreateHeadedPanel<AboutPopup>(transform).GetComponent<BasePopup>();
            _questPopup = ElementFactory.CreateHeadedPanel<QuestPopup>(transform).GetComponent<BasePopup>();
            _tutorialPopup = ElementFactory.CreateHeadedPanel<TutorialPopup>(transform).GetComponent<BasePopup>();
            _outdatedPopup = ElementFactory.CreateDefaultPopup<OutdatedPopup>(transform).GetComponent<OutdatedPopup>();
            _popups.Add(_createGamePopup);
            _popups.Add(_multiplayerMapPopup);
            _popups.Add(_editProfilePopup);
            _popups.Add(_settingsPopup);
            _popups.Add(_toolsPopup);
            _popups.Add(_multiplayerRoomListPopup);
            _popups.Add(_leaderboardPopup);
            _popups.Add(_socialPopup);
            _popups.Add(_aboutPopup);
            _popups.Add(_questPopup);
            _popups.Add(_tutorialPopup);
            _popups.Add(_selectMapPopup);
            _popups.Add(_outdatedPopup);
        }

        private void SetupIntroPanel()
        {
            GameObject introPanel = ElementFactory.InstantiateAndBind(transform, "Prefabs/MainMenu/IntroPanel");
            introPanel.AddComponent<IgnoreScaler>();
            ElementFactory.SetAnchor(introPanel, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            foreach (Transform transform in introPanel.transform.Find("Buttons"))
            {
                IntroButton introButton = transform.gameObject.AddComponent<IntroButton>();
                introButton.onClick.AddListener(() => OnIntroButtonClick(introButton.name));
            }
            foreach (Transform transform in introPanel.transform.Find("Icons"))
            {
                Button button = transform.gameObject.GetComponent<Button>();
                button.onClick.AddListener(() => OnIntroButtonClick(transform.name));
                ColorBlock block = new ColorBlock
                {
                    colorMultiplier = 1f,
                    fadeDuration = 0.1f,
                    normalColor = new Color(0.9f, 0.9f, 0.9f),
                    highlightedColor = new Color(0.75f, 0.75f, 0.75f),
                    pressedColor = new Color(0.5f, 0.5f, 0.5f),
                    disabledColor = new Color(0.5f, 0.5f, 0.5f)
                };
                button.colors = block;
            }
            _introPanelBackground = introPanel.transform.Find("Background").GetComponent<Image>();
        }

        private void SetupLabels()
        {
            _multiplayerStatusLabel = ElementFactory.CreateDefaultLabel(transform, ElementStyle.Default, string.Empty, alignment: TextAnchor.MiddleLeft).GetComponent<Text>();
            ElementFactory.SetAnchor(_multiplayerStatusLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            _multiplayerStatusLabel.color = Color.black;
            Text versionText = ElementFactory.CreateDefaultLabel(transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
            ElementFactory.SetAnchor(versionText.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
            versionText.color = Color.white;
            if (ApplicationConfig.DevelopmentMode)
                versionText.text = "AOTTG2 DEVELOPMENT VERSION";
            else
                versionText.text = "AOTTG2 Version " + ApplicationConfig.GameVersion + ".";
            versionText.text = "";
        }

        private void ChangeMainBackground()
        {
            _backgroundMenu.ChangeMainBackground();
            _tipPanel.SetRandomTip();
        }

        private IEnumerator WaitAndChangeBackground()
        {
            while (true)
            {
                yield return new WaitForSeconds(ChangeBackgroundTime);
                ChangeMainBackground();
            }
        }

        private void Update()
        {
            if (_multiplayerStatusLabel != null)
            {
                string label = "";
                if (SettingsManager.GraphicsSettings.ShowFPS.Value)
                    label = "FPS:" + UIManager.GetFPS().ToString() + "\n";
                if (_multiplayerMapPopup.IsActive || _multiplayerRoomListPopup.IsActive || (_createGamePopup.IsActive && PhotonNetwork.IsConnected))
                {
                    label += PhotonNetwork.NetworkClientState.ToString();
                    if (PhotonNetwork.IsConnected)
                        label += " Ping:" + PhotonNetwork.GetPing().ToString();
                }
                _multiplayerStatusLabel.text = label;
            }
            if (SettingsManager.UISettings.FadeMainMenu.Value)
            {
                if (HoverIntroPanel())
                {
                    float alpha = _introPanelBackground.color.a;
                    if (alpha < 1f)
                        _introPanelBackground.color = new Color(1f, 1f, 1f, Mathf.Min(1f, alpha + Time.deltaTime * 2f));
                }
                else
                {
                    float alpha = _introPanelBackground.color.a;
                    if (alpha > 0f)
                        _introPanelBackground.color = new Color(1f, 1f, 1f, Mathf.Max(0f, alpha - Time.deltaTime * 1f));
                }
            }
            else
                _introPanelBackground.color = Color.white;
            if (!ShowedOutdated)
            {
                if (PastebinLoader.Status == PastebinStatus.Loaded)
                {
                    ShowedOutdated = true;
                    if (PastebinLoader.Version["Version"].Value != ApplicationConfig.GameVersion)
                        _outdatedPopup.Show("Your game version is outdated. \nIf using the launcher, try restarting and repairing." +
                            "\nFor standalone, download the latest version from https://aottg2.itch.io/aottg2.");
                }
            }
        }

        private bool IsPopupActive()
        {
            foreach (BasePopup popup in _popups)
            {
                if (popup.IsActive)
                    return true;
            }
            return false;
        }

        private bool HoverIntroPanel()
        {
            float x = _introPanelBackground.GetComponent<RectTransform>().sizeDelta.x * UIManager.CurrentMenu.GetComponent<Canvas>().scaleFactor * 0.25f;
            return Input.mousePosition.x < x;
        }

        private void OnIntroButtonClick(string name)
        {
            bool isPopupAactive = IsPopupActive();
            HideAllPopups();
            if (isPopupAactive && _lastButtonClicked == name)
                return;
            _lastButtonClicked = name;
            switch (name)
            {
                case "TutorialButton":
                    _tutorialPopup.Show();
                    break;
                case "CreditsButton":
                    SceneLoader.LoadScene(SceneName.Credits);
                    break;
                case "SingleplayerButton":
                    ((CreateGamePopup)_createGamePopup).Show(false);
                    break;
                case "MultiplayerButton":
                    _multiplayerMapPopup.Show();
                    break;
                case "ProfileButton":
                    _editProfilePopup.Show();
                    break;
                case "SettingsButton":
                    _settingsPopup.Show();
                    break;
                case "ToolsButton":
                    _toolsPopup.Show();
                    break;
                case "QuitButton":
                    Application.Quit();
                    break;
                case "QuestButton":
                    _questPopup.Show();
                    break;
                case "LeaderboardButton":
                    _leaderboardPopup.Show();
                    break;
                case "SocialButton":
                    _socialPopup.Show();
                    break;
                case "HelpButton":
                    _aboutPopup.Show();
                    break;
                case "PatreonButton":
                    ExternalLinkPopup.Show("https://www.patreon.com/aottg2");
                    break;
            }
        }
    }
}
