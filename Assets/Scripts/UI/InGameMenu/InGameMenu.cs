using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Settings;
using GameManagers;
using Utility;
using SimpleJSONFixed;
using ApplicationManagers;
using Characters;
using System.Collections;
using Cameras;
using CustomLogic;
using Photon.Pun;

namespace UI
{
    class InGameMenu: BaseMenu
    {
        public EmoteHandler EmoteHandler;
        public ItemHandler ItemHandler;
        public CharacterInfoHandler CharacterInfoHandler;
        public HUDBottomHandler HUDBottomHandler;
        public StylebarHandler StylebarHandler;
        public GameObject NapeLock;
        public ChatPanel ChatPanel;
        public FeedPanel FeedPanel;
        public BasePopup _settingsPopup;
        public BasePopup _ExpeditionPopup;
        public BasePopup _createGamePopup;
        public BasePopup _pausePopup;
        public BasePopup _characterPopup;
        public BasePopup _scoreboardPopup;
        public BasePopup _selectMapPopup;
        public CustomAssetUrlPopup _customAssetUrlPopup;
        public SnapshotPopup _snapshotPopup;
        public GlobalPauseGamePopup _globalPauseGamePopup;
        public CutsceneDialoguePanel _cutsceneDialoguePanel;
        public bool SkipAHSSInput;
        private InGameBackgroundMenu _backgroundMenu;
        private KillFeedBigPopup _killFeedBigPopup;
        private List<KillFeedSmallPopup> _killFeedSmallPopups = new List<KillFeedSmallPopup>();
        private KillScorePopup _killScorePopup;
        private Text _topCenterLabel;
        private Text _topLeftLabel;
        private Text _topRightLabel;
        private Text _middleCenterLabel;
        private Text _bottomLeftLabel;
        private Text _bottomRightLabel;
        private Text _bottomCenterLabel;
        private bool _showingBlood;
        private GameObject _minimapPanel;
        private List<BasePopup> _allPausePopups = new List<BasePopup>();
        private Dictionary<string, float> _labelTimeLeft = new Dictionary<string, float>();
        private Dictionary<string, bool> _labelHasTimeLeft = new Dictionary<string, bool>();
        private float _killScoreTimeLeft;
        private float _snapshotTimeLeft;
        private string _middleCenterText;
        private string _bottomRightText;
        private string _bottomCenterText;
        private string _topLeftText;
        private InGameManager _gameManager;
        private Dictionary<string, BasePopup> _customPopups = new Dictionary<string, BasePopup>();
        
        public override void Setup()
        {
            base.Setup();
            SetupLoading();
            SetupLabels();
            EmoteHandler = gameObject.AddComponent<EmoteHandler>();
            ItemHandler = gameObject.AddComponent<ItemHandler>();
            HUDBottomHandler = gameObject.AddComponent<HUDBottomHandler>();
            CharacterInfoHandler = gameObject.AddComponent<CharacterInfoHandler>();
            StylebarHandler = gameObject.AddComponent<StylebarHandler>();
            gameObject.AddComponent<CrosshairHandler>();
            NapeLock = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/NapeLockImage");
            NapeLock.SetActive(false);
            SetupChat();
            SetupMinimap();
            SetupSnapshot();
            HideAllMenus();
        }

        public void ToggleUI(bool toggle)
        {
            GetComponent<Canvas>().enabled = toggle;
        }

        public bool IsActive()
        {
            return GetComponent<Canvas>().enabled;
        }

        public void CreateCustomPopup(string name, string title, float width, float height)
        {
            var popup = ElementFactory.InstantiateAndSetupCustomPopup(transform, title, width, height).GetComponent<CustomPopup>();
            _popups.Add(popup);
            _customPopups[name] = popup;
        }

        public CustomPopup GetCustomPopup(string name)
        {
            return (CustomPopup)_customPopups[name];
        }

        public void SetupMinimap()
        {
            gameObject.AddComponent<MinimapHandler>();
            if (SettingsManager.GeneralSettings.MinimapEnabled.Value && !SettingsManager.InGameCurrent.Misc.GlobalMinimapDisable.Value && !SettingsManager.InGameCurrent.Misc.RealismMode.Value)
            {
                _minimapPanel = ElementFactory.InstantiateAndBind(transform, "Minimap/Prefabs/MinimapPanel");
                ElementFactory.SetAnchor(_minimapPanel, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-10f, -10f));
                _minimapPanel.AddComponent<MinimapScaler>();
                _minimapPanel.SetActive(false);
            }
            else
                GetComponent<MinimapHandler>().Disable();
        }

        public void SetupSnapshot()
        {
            var go = ElementFactory.InstantiateAndSetupPanel<SnapshotPopup>(transform, "Prefabs/Snapshot/SnapshotPopup", false);
            _snapshotPopup = go.GetComponent<SnapshotPopup>();
            go.transform.localScale = new Vector2(0.8f, 0.8f);
            ElementFactory.SetAnchor(go, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -130f));
        }

        private void SetupChat()
        {
            if (SettingsManager.UISettings.FeedConsole.Value && SettingsManager.UISettings.GameFeed.Value)
            {
                FeedPanel = ElementFactory.InstantiateAndSetupPanel<FeedPanel>(_bottomRightLabel.transform, "Prefabs/InGame/FeedPanel", true).GetComponent<FeedPanel>();
                ElementFactory.SetAnchor(FeedPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(0f, -50f));
            }
            ChatPanel = ElementFactory.InstantiateAndSetupPanel<ChatPanel>(transform, "Prefabs/InGame/ChatPanel", true).GetComponent<ChatPanel>();
            ElementFactory.SetAnchor(ChatPanel.gameObject, TextAnchor.LowerLeft, TextAnchor.LowerLeft, new Vector2(10f, 10f));
        }

        private void SetupLabels()
        {
            ElementStyle style = new ElementStyle(fontSize: 22);
            _topCenterLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_topCenterLabel.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, -10f));
            _topLeftLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            ElementFactory.SetAnchor(_topLeftLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(10f, -10f));
            _topRightLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleRight).GetComponent<Text>();
            ElementFactory.SetAnchor(_topRightLabel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-10f, -10f));
            _middleCenterLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_middleCenterLabel.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, 100f));
            _bottomCenterLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_bottomCenterLabel.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 10f));
            _bottomLeftLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            ElementFactory.SetAnchor(_bottomLeftLabel.gameObject, TextAnchor.LowerLeft, TextAnchor.LowerLeft, new Vector2(10f, 10f));
            _bottomRightLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleRight).GetComponent<Text>();
            ElementFactory.SetAnchor(_bottomRightLabel.gameObject, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(-10f, 10f));
            _killScorePopup = ElementFactory.CreateDefaultPopup<KillScorePopup>(transform);
            _killScorePopup.gameObject.AddComponent<IgnoreScaler>();
            ElementFactory.SetAnchor(_killScorePopup.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, 100f));
            _killFeedBigPopup = ElementFactory.CreateDefaultPopup<KillFeedBigPopup>(transform);
            _killFeedBigPopup.gameObject.AddComponent<KillFeedScaler>();
            ElementFactory.SetAnchor(_killFeedBigPopup.gameObject, TextAnchor.UpperCenter, TextAnchor.MiddleCenter, new Vector2(0f, -120f));
            for (int i = 0; i < 4; i++)
            {
                float y = -162f - i * 35f;
                var popup = ElementFactory.CreateDefaultPopup<KillFeedSmallPopup>(transform);
                popup.gameObject.AddComponent<KillFeedScaler>();
                ElementFactory.SetAnchor(popup.gameObject, TextAnchor.UpperCenter, TextAnchor.MiddleCenter, new Vector2(0f, y));
                _killFeedSmallPopups.Add(popup);
            }
        }

        private void SetupLoading()
        {
            _backgroundMenu = ElementFactory.CreateMenu<InGameBackgroundMenu>("Prefabs/Panels/BackgroundMenu");
            _backgroundMenu.Setup();
            _backgroundMenu.transform.SetAsFirstSibling();
            _globalPauseGamePopup = ElementFactory.CreateDefaultPopup<GlobalPauseGamePopup>(transform);
        }

        public void OnFinishLoading()
        {
            _characterPopup = ElementFactory.CreateDefaultPopup<CharacterPopup>(transform);
            _scoreboardPopup = ElementFactory.CreateDefaultPopup<ScoreboardPopup>(transform);
            _cutsceneDialoguePanel = ElementFactory.CreateDefaultPopup<CutsceneDialoguePanel>(transform);
            ElementFactory.SetAnchor(_cutsceneDialoguePanel.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 100f));
            _popups.Add(_characterPopup);
            _popups.Add(_scoreboardPopup);
            _gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (_minimapPanel != null)
            {
                _minimapPanel.SetActive(true);
            }

        }

        public static bool InMenu()
        {
            var menu = (InGameMenu)UIManager.CurrentMenu;
            foreach (BasePopup popup in menu._popups)
            {
                if (popup.IsActive || EmVariables.LogisticianOpen || EmVariables.AbilityWheelOpen)
                    return true;
            }
            return menu.EmoteHandler.IsActive || menu.ItemHandler.IsActive;
        }

        public void SetPauseMenu(bool enabled)
        {
            if (enabled && !IsPauseMenuActive())
            {
                HideAllMenus();
                _pausePopup.Show();
            }            
            else if (!enabled)
            {
                HideAllMenus();
                SkipAHSSInput = true;
            }
            ToggleUI(true);
        }

        public void ToggleScoreboardMenu()
        {
            SetScoreboardMenu(!_scoreboardPopup.gameObject.activeSelf);
            ToggleUI(true);
        }

        public void SetScoreboardMenu(bool enabled)
        {
            if (enabled && !InMenu())
            {
                HideAllMenus();
                _scoreboardPopup.Show();
            }
            else if (!enabled)
            {
                _scoreboardPopup.Hide();
                SkipAHSSInput = true;
            }
        }

        public void SetCharacterMenu(bool enabled)
        {
            if (enabled && !InMenu())
            {
                HideAllMenus();
                _characterPopup.Show();
                InGameManager.UpdateRoundPlayerProperties();
            }
            else if (!enabled)
                _characterPopup.Hide();
            ToggleUI(true);
        }

        public void ShowCutsceneMenu(string icon, string title, string content)
        {
            _cutsceneDialoguePanel.Show(icon, title, content);
        }

        public void HideCutsceneMenu()
        {
            _cutsceneDialoguePanel.Hide();
        }

        public bool IsPauseMenuActive()
        {
            foreach (BasePopup popup in _allPausePopups)
            {
                if (popup.gameObject.activeSelf)
                    return true;
            }
            return false;
        }

        public void ShowBlood()
        {
            if (!_showingBlood)
            {
                _showingBlood = true;
                StartCoroutine(WaitAndShowBlood());
            }
        }

        public void ShowSnapshot(Texture2D texture)
        {
            _snapshotPopup.Load(texture);
            _snapshotPopup.Show();
            _snapshotTimeLeft = 2f;
        }

        public void ShowKillFeed(string killer, string victim, int score, string weapon)
        {
            if (_killFeedBigPopup.TimeLeft > 0f)
            {
                ShowKillFeedPushSmall(_killFeedBigPopup.Killer, _killFeedBigPopup.Victim, _killFeedBigPopup.Score, 
                    _killFeedBigPopup.Weapon, _killFeedBigPopup.TimeLeft, 0);
            }
            _killFeedBigPopup.Show(killer, victim, score, weapon);
        }

        private void ShowKillFeedPushSmall(string killer, string victim, int score, string weapon, float timeLeft, int index)
        {
            if (index >= _killFeedSmallPopups.Count)
                return;
            var popup = _killFeedSmallPopups[index];
            if (popup.TimeLeft > 0f)
                ShowKillFeedPushSmall(popup.Killer, popup.Victim, popup.Score, popup.Weapon, popup.TimeLeft, index + 1);
            popup.ShowImmediate(killer, victim, score, weapon, timeLeft);
        }

        public void ShowKillScore(int score)
        {
            _killScorePopup.Show(score);
            _killScoreTimeLeft = 3f;
            StylebarHandler.OnHit(score);
        }

        public void SetLabel(string label, string message, float time)
        {
            SetLabelText(label, message);
            if (time == 0f)
                _labelHasTimeLeft[label] = false;
            else
                _labelHasTimeLeft[label] = true;
            _labelTimeLeft[label] = time;
        }

        private void SetLabelText(string label, string message)
        {
            if (label == "TopCenter")
                _topCenterLabel.text = message;
            else if (label == "TopLeft")
                _topLeftText = message;
            else if (label == "TopRight")
                _topRightLabel.text = message;
            else if (label == "MiddleCenter")
                _middleCenterText = message;
            else if (label == "BottomLeft")
                _bottomLeftLabel.text = message;
            else if (label == "BottomRight")
                _bottomRightText = message;
            else if (label == "BottomCenter")
                _bottomCenterText = message;
        }

        IEnumerator WaitAndShowBlood()
        {
            _backgroundMenu.ShowBlood();
            yield return new WaitForSeconds(5f);
            _backgroundMenu.HideBlood();
            _showingBlood = false;
        }

        void Update()
        {
            if (_gameManager == null)
                return;
            if (_gameManager.GlobalPause)
            {
                _globalPauseGamePopup.Show();
                if (_gameManager.PauseTimeLeft >= 0f)
                    _globalPauseGamePopup.SetLabel("Unpausing in: " + Util.FormatFloat(_gameManager.PauseTimeLeft, 1));
                else
                    _globalPauseGamePopup.SetLabel("Paused by master client.");
            }
            else
                _globalPauseGamePopup.Hide();
            foreach (string label in new List<string>(_labelHasTimeLeft.Keys))
            {
                if (_labelHasTimeLeft[label])
                {
                    _labelTimeLeft[label] -= Time.deltaTime;
                    if (_labelTimeLeft[label] <= 0f)
                    {
                        _labelHasTimeLeft[label] = false;
                        SetLabelText(label, "");
                    }
                }
            }
            if (_gameManager.IsEnding)
                _middleCenterLabel.text = _middleCenterText + "\n" + "Restarting in " + ((int)_gameManager.EndTimeLeft).ToString();
            else
                _middleCenterLabel.text = _middleCenterText;
            var inGame = SettingsManager.InGameCharacterSettings;
            if (inGame.ChooseStatus.Value == (int)ChooseCharacterStatus.Spectating || (_gameManager.CurrentCharacter == null || _gameManager.CurrentCharacter.Dead))
            {
                var input = SettingsManager.InputSettings.General;
                string spectating = "";
                if (inGame.ChooseStatus.Value != (int)ChooseCharacterStatus.Choosing)
                {
                    spectating = "Prev: " + ChatManager.GetColorString(input.SpectatePreviousPlayer.ToString(), ChatTextColor.System) + ", ";
                    spectating += "Next: " + ChatManager.GetColorString(input.SpectateNextPlayer.ToString(), ChatTextColor.System) + ", ";
                    spectating += "Join: " + ChatManager.GetColorString(input.ChangeCharacter.ToString(), ChatTextColor.System) + ", ";
                    spectating += "Free Cam: " + ChatManager.GetColorString(input.ChangeCamera.ToString(), ChatTextColor.System);
                }
                var camera = (InGameCamera)SceneLoader.CurrentCamera;
                if (camera._follow != null && camera._follow != _gameManager.CurrentCharacter)
                    spectating = "Spectating " + camera._follow.Name + ". " + spectating;
                else
                    spectating = "Spectating. " + spectating;
                _bottomCenterLabel.text = _bottomCenterText + "\n" + spectating;
            }
            else
                _bottomCenterLabel.text = _bottomCenterText;
            _bottomRightLabel.text = (_bottomRightText + "\n" + GetKeybindStrings()).Trim();
            _topLeftLabel.text = (GetTelemetricStrings().Trim() + "\n" + _topLeftText).Trim();
            _killFeedBigPopup.TimeLeft -= Time.deltaTime;
            if (_killFeedBigPopup.IsActive && _killFeedBigPopup.TimeLeft <= 0f)
                _killFeedBigPopup.Hide();
            _killScoreTimeLeft -= Time.deltaTime;
            if (_killScoreTimeLeft <= 0f)
                _killScorePopup.Hide();
            foreach (var popup in _killFeedSmallPopups)
            {
                popup.TimeLeft -= Time.deltaTime;
                if (popup.IsActive && popup.TimeLeft <= 0f)
                    popup.Hide();
            }
            _snapshotTimeLeft -= Time.deltaTime;
            if (_snapshotPopup.IsActive && _snapshotTimeLeft <= 0f)
                _snapshotPopup.Hide();
        }

        private string GetKeybindStrings()
        {
            string str = "";
            if (SettingsManager.UISettings.ShowInterpolation.Value)
            {
                var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
                if (gameManager.CurrentCharacter != null && gameManager.CurrentCharacter is Human)
                {
                    if (((Human)gameManager.CurrentCharacter).Cache.Rigidbody.interpolation == RigidbodyInterpolation.Interpolate)
                        str = "Interpolation: " + ChatManager.GetColorString("ON", ChatTextColor.System);
                    else
                        str = "Interpolation: " + ChatManager.GetColorString("OFF", ChatTextColor.System);
                }
            }
            if (!SettingsManager.UISettings.ShowKeybindTip.Value)
                return str;
            var settings = SettingsManager.InputSettings;
            if (str != "")
                str += ", ";
            str += "Pause: " + ChatManager.GetColorString(settings.General.Pause.ToString(), ChatTextColor.System);
            str += ", " + "Scoreboard: " + ChatManager.GetColorString(settings.General.ToggleScoreboard.ToString(), ChatTextColor.System);
            str += ", " + "Change Char: " + ChatManager.GetColorString(settings.General.ChangeCharacter.ToString(), ChatTextColor.System);
            return str;
        }

        private string GetTelemetricStrings()
        {
            string timeLine = "";
            string fpsLine = "";
            string kdrLine = "";
            if (SettingsManager.UISettings.ShowGameTime.Value)
            {
                if (CustomLogicManager.Evaluator != null)
                    timeLine += "Game Time: " + ChatManager.GetColorString(Util.FormatFloat(CustomLogicManager.Evaluator.CurrentTime, 0), ChatTextColor.System);
                else
                    timeLine += "Game Time: " + ChatManager.GetColorString("0", ChatTextColor.System);
                var dt = System.DateTime.Now;
                timeLine += ", System: " + ChatManager.GetColorString(GetTimeString(dt.Hour) + ":" + GetTimeString(dt.Minute) + ":" + GetTimeString(dt.Second), ChatTextColor.System);
            }
            if (SettingsManager.GraphicsSettings.ShowFPS.Value)
                fpsLine += "FPS:" + UIManager.GetFPS().ToString();
            if (!PhotonNetwork.OfflineMode && SettingsManager.UISettings.ShowPing.Value)
            {
                if (fpsLine != "")
                    fpsLine += ", ";
                fpsLine += "Ping:" + PhotonNetwork.GetPing().ToString();
            }
            if (SettingsManager.UISettings.ShowKDR.Value)
            {
                string kills = PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.Kills).ToString();
                string deaths = PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.Deaths).ToString();
                string max = PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.HighestDamage).ToString();
                string total = PhotonNetwork.LocalPlayer.GetIntProperty(PlayerProperty.TotalDamage).ToString();
                kdrLine += "KDR: " + string.Join(" / ", new string[] { kills, deaths, max, total }) + "\n";
            }
            string final = timeLine;
            if (timeLine != "")
                final += "\n";
            final += fpsLine;
            if (fpsLine != "")
                final += "\n";
            final += kdrLine;
            return final;
        }

        private string GetTimeString(int time)
        {
            string str = time.ToString();
            if (str.Length == 1)
                str = "0" + str;
            return str;
        }

        private void HideAllMenus()
        {
            HideAllPopups();
            EmoteHandler.SetEmoteWheel(false);
            ItemHandler.SetItemWheel(false);
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(transform).GetComponent<BasePopup>();
            _ExpeditionPopup = ElementFactory.CreateHeadedPanel<ExpeditionPopup>(transform).GetComponent<BasePopup>();
            _pausePopup = ElementFactory.CreateHeadedPanel<PausePopup>(transform).GetComponent<PausePopup>();
            _selectMapPopup = ElementFactory.CreateHeadedPanel<CreateGameSelectMapPopup>(transform).GetComponent<CreateGameSelectMapPopup>();
            _createGamePopup = ElementFactory.CreateHeadedPanel<CreateGamePopup>(transform).GetComponent<CreateGamePopup>();
            _customAssetUrlPopup = ElementFactory.CreateDefaultPopup<CustomAssetUrlPopup>(transform).GetComponent<CustomAssetUrlPopup>();
            _popups.Add(_settingsPopup);
            _popups.Add(_ExpeditionPopup);
            _popups.Add(_pausePopup);
            _popups.Add(_createGamePopup);
            _popups.Add(_selectMapPopup);
            _popups.Add(_customAssetUrlPopup);
            _allPausePopups.Add(_settingsPopup);
            _allPausePopups.Add(_ExpeditionPopup);
            _allPausePopups.Add(_pausePopup);
            _allPausePopups.Add(_createGamePopup);
            _allPausePopups.Add(_customAssetUrlPopup);
            _allPausePopups.Add(_selectMapPopup);
        }
    }
}
