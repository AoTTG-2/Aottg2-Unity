﻿using System.Collections.Generic;
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
using Photon.Realtime;
using System.Linq;

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
        public VoiceChatPanel VoiceChatPanel;
        public GameObject TopLeftHud;
        public GameObject KDRReference;
        public KDRPanel KDRPanel;
        public Telemetry TelemetryPanel;
        public BasePopup _settingsPopup;
        public BasePopup _createGamePopup;
        public BasePopup _pausePopup;
        public BasePopup _characterPopup;
        public BasePopup _characterChangePopup;
        public BasePopup _scoreboardPopup;
        public BasePopup _mapPopup;
        public BasePopup _selectMapPopup;
        public SkillTooltipPopup SkillTooltipPopup;
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
        private Text _middleLeftLabel;
        private Text _middleRightLabel;
        private Text _bottomLeftLabel;
        private Text _bottomRightLabel;
        private Text _bottomCenterLabel;
        private bool _showingBlood;
        public GameObject _minimapPanel;
        private List<BasePopup> _allPausePopups = new List<BasePopup>();
        private Dictionary<string, float> _labelTimeLeft = new Dictionary<string, float>();
        private Dictionary<string, bool> _labelHasTimeLeft = new Dictionary<string, bool>();
        List<string> labelsToDeactivate = new List<string>();
        private float _killScoreTimeLeft;
        private float _snapshotTimeLeft;
        private string _middleCenterText;
        private string _bottomRightText;
        private string _bottomCenterText;
        private string _topLeftText;
        public float _spectateUpdateTimeLeft;
        public int _spectateCount;
        private InGameManager _gameManager;
        private Dictionary<string, BasePopup> _customPopups = new Dictionary<string, BasePopup>();
        private string[] trackedProperties = new string[] { "Kills", "Deaths", "HighestDamage", "TotalDamage" };

        public override void Setup()
        {
            base.Setup();
            SetupLoading();
            SetupTopLeftHud();
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

        public void ApplyUISettings()
        {
            TopLeftHud.GetComponent<TopLeftHUD>().ApplySettings();
        }

        public void SetupTopLeftHud()
        {
            // Create the top left HUD layout group, add the telemetry, kdr, and topleftlabel will be created after this and added to the group
            var panel = ElementFactory.InstantiateAndSetupPanel<TopLeftHUD>(transform, "Prefabs/InGame/TopLeftHUD");
            ElementFactory.SetAnchor(panel, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            TopLeftHud = panel;
            KDRReference = panel.GetComponent<TopLeftHUD>().kdrAndLabel;
            panel.SetActive(true); // ????
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

        public bool IsCustomPopupActive(string name)
        {
            if (!_customPopups.ContainsKey(name))
                return false;
            return _customPopups[name].IsActive;
        }

        public List<string> GetAllCustomPopups()
        {
            return _customPopups.Keys.ToList();
        }

        public void SetupMinimap()
        {
            _minimapPanel = ElementFactory.InstantiateAndBind(transform, "Minimap/Prefabs/MinimapPanel");
            ElementFactory.SetAnchor(_minimapPanel, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-10f, -10f));
            _minimapPanel.AddComponent<MinimapScaler>();
            _minimapPanel.SetActive(false);
            gameObject.AddComponent<MinimapHandler>();
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
            if (SettingsManager.SoundSettings.VoiceChatInput.Value != (int)VoiceChatInputMode.Off)
            {
                VoiceChatPanel = ElementFactory.InstantiateAndSetupPanel<VoiceChatPanel>(transform, "Prefabs/InGame/VoiceChatPanel", true).GetComponent<VoiceChatPanel>();
                ElementFactory.SetAnchor(VoiceChatPanel.gameObject, TextAnchor.MiddleLeft, TextAnchor.MiddleLeft, new Vector2(10, 10f));
            }

            ChatPanel = ElementFactory.InstantiateAndSetupPanel<ChatPanel>(transform, "Prefabs/InGame/ChatPanel", true).GetComponent<ChatPanel>();
            ElementFactory.SetAnchor(ChatPanel.gameObject, TextAnchor.LowerLeft, TextAnchor.LowerLeft, new Vector2(10f, 10f));
        }

        private void SetupLabels()
        {
            ElementStyle style = new ElementStyle(fontSize: 22);
            _topCenterLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_topCenterLabel.gameObject, TextAnchor.UpperCenter, TextAnchor.UpperCenter, new Vector2(0f, -10f));
            _topLeftLabel = ElementFactory.CreateHUDLabel(KDRReference.transform, style, "", FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            ElementFactory.SetAnchor(_topLeftLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(10f, -10f));
            _topRightLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleRight).GetComponent<Text>();
            ElementFactory.SetAnchor(_topRightLabel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-10f, -10f));
            _middleCenterLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_middleCenterLabel.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, new Vector2(0f, 100f));
            _middleRightLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_middleRightLabel.gameObject, TextAnchor.MiddleRight, TextAnchor.MiddleRight, new Vector2(-10f, 0f));
            _middleLeftLabel = ElementFactory.CreateHUDLabel(transform, style, "", FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>();
            ElementFactory.SetAnchor(_middleLeftLabel.gameObject, TextAnchor.MiddleLeft, TextAnchor.MiddleLeft, new Vector2(10f, 0f));
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
            int feedCount = SettingsManager.UISettings.KillFeedCount.Value - 1;
            for (int i = 0; i < feedCount; i++)
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
            _mapPopup = ElementFactory.CreateDefaultPopup<MapPopup>(transform);
            _cutsceneDialoguePanel = ElementFactory.CreateDefaultPopup<CutsceneDialoguePanel>(transform);
            ElementFactory.SetAnchor(_cutsceneDialoguePanel.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 100f));
            _popups.Add(_characterPopup);
            _popups.Add(_scoreboardPopup);
            _popups.Add(_mapPopup);
            _gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (_minimapPanel != null && SettingsManager.GeneralSettings.MinimapEnabled.Value && AllowMap())
                _minimapPanel.SetActive(true);
        }

        public bool AllowMap()
        {
            return (!SettingsManager.InGameCurrent.Misc.GlobalMinimapDisable.Value && !SettingsManager.InGameCurrent.Misc.RealismMode.Value);
        }

        public static bool InMenu()
        {
            var menu = (InGameMenu)UIManager.CurrentMenu;
            foreach (BasePopup popup in menu._popups)
            {
                if (popup.IsActive)
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
            SetScoreboardMenu(!_scoreboardPopup.gameObject.activeSelf, false);
            ToggleUI(true);
        }

        public void SetScoreboardMenu(bool enabled, bool fromClick)
        {
            if (enabled && !InMenu())
            {
                HideAllMenus();
                _scoreboardPopup.Show();
            }
            else if (!enabled)
            {
                _scoreboardPopup.Hide();
                if (fromClick)
                    SkipAHSSInput = true;
            }
        }

        public void ToggleMapMenu()
        {
            SetMapMenu(!_mapPopup.gameObject.activeSelf, false);
            ToggleUI(true);
        }

        public void SetMapMenu(bool enabled, bool fromClick)
        {
            if (enabled && !InMenu() && AllowMap())
            {
                HideAllMenus();
                _mapPopup.Show();
            }
            else if (!enabled)
            {
                _mapPopup.Hide();
                if (fromClick)
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
            {
                _characterPopup.Hide();
                if (_characterChangePopup != null)
                    _characterChangePopup.Hide();
            }
            ToggleUI(true);
        }

        public void ShowCharacterChangeMenu()
        {
            if (!InMenu())
            {
                HideAllMenus();
                if (_characterChangePopup == null)
                {
                    _characterChangePopup = ElementFactory.CreateDefaultPopup<CharacterChangePopup>(transform);
                    _popups.Add(_characterChangePopup);
                }
                _characterChangePopup.Show();
            }
        }

        public void ShowCutsceneMenu(string icon, string title, string content, bool full)
        {
            _cutsceneDialoguePanel.Show(icon, title, content, full);
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
            int feedCount = SettingsManager.UISettings.KillFeedCount.Value;
            if (feedCount <= 0)
                return;
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

        public void ShowKillScore(int score, bool force = false)
        {
            if (!force && !CustomLogicManager.Evaluator.DefaultShowKillScore)
                return;
            _killScorePopup.Show(score);
            _killScoreTimeLeft = 3f;
            StylebarHandler.OnHit(score);
        }

        public int GetStylebarRank()
        {
            if (StylebarHandler != null)
                return StylebarHandler.GetRank();
            return 0;
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
            else if (label == "MiddleLeft")
                _middleLeftLabel.text = message;
            else if (label == "MiddleRight")
                _middleRightLabel.text = message;
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
            labelsToDeactivate.Clear();
            foreach (var kvp in _labelHasTimeLeft)
            {
                if (kvp.Value)
                {
                    _labelTimeLeft[kvp.Key] -= Time.deltaTime;
                    if (_labelTimeLeft[kvp.Key] <= 0f)
                    {
                        labelsToDeactivate.Add(kvp.Key);
                        SetLabelText(kvp.Key, "");
                    }
                }
            }
            foreach (var label in labelsToDeactivate)
            {
                _labelHasTimeLeft[label] = false;
            }
            if (_gameManager.IsEnding)
                _middleCenterLabel.text = _middleCenterText + "\n" + "Restarting in " + ((int)_gameManager.EndTimeLeft).ToString();
            else
                _middleCenterLabel.text = _middleCenterText;
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            _spectateUpdateTimeLeft -= Time.deltaTime;
            if (_spectateUpdateTimeLeft <= 0f)
            {
                _spectateUpdateTimeLeft = 1f;
                int spectateCount = 0;
                if (camera._follow != null)
                {
                    int actorId = camera._follow.Cache.PhotonView.Owner.ActorNumber;
                    foreach (var player in PhotonNetwork.PlayerList)
                    {
                        if (player.GetIntProperty(PlayerProperty.SpectateID, -1) == actorId)
                            spectateCount++;
                    }
                }
                _spectateCount = spectateCount;
            }
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
                    spectating += $"{camera.SpecMode.Current()}: " + ChatManager.GetColorString(input.ChangeCamera.ToString(), ChatTextColor.System);
                }
                if (camera._follow != null && camera._follow != _gameManager.CurrentCharacter)
                {
                    spectating = "Spectating " + camera._follow.Name + " (" + _spectateCount.ToString() + "). " + spectating;
                }
                else
                    spectating = "Spectating. " + spectating;
                _bottomCenterLabel.text = _bottomCenterText + "\n" + spectating;
            }
            else
                _bottomCenterLabel.text = _bottomCenterText;
            _bottomRightLabel.text = (_bottomRightText + "\n" + GetKeybindStrings());
            _topLeftLabel.text = _topLeftText;
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
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
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
            if (SettingsManager.UISettings.ShowKeybindTip.Value)
            {
                var settings = SettingsManager.InputSettings;
                if (str != "")
                    str += ", ";
                str += "Pause: " + ChatManager.GetColorString(settings.General.Pause.ToString(), ChatTextColor.System);
                str += ", " + "Scoreboard: " + ChatManager.GetColorString(settings.General.ToggleScoreboard.ToString(), ChatTextColor.System);
                str += ", " + "Change Char: " + ChatManager.GetColorString(settings.General.ChangeCharacter.ToString(), ChatTextColor.System);
            }
            if (SettingsManager.UISettings.Coordinates.Value == (int)CoordinateMode.BottomRight)
            {
                var position = camera.Cache.Transform.position;
                if (camera._follow != null)
                    position = camera._follow.Cache.Transform.position;
                string pos = "Position: " + position.x.ToString("F0") + ", " + position.y.ToString("F0") + ", " + position.z.ToString("F0");
                if (str != "")
                    pos += "\n";
                str = pos + str;
            }
            if (camera._follow != null && _spectateCount > 0 && camera._follow.IsMainCharacter())
            {
                string spectate = "Spectating: " + _spectateCount.ToString();
                if (str != "")
                    spectate += "\n";
                str = spectate + str;
            }
            return str;
        }

        private string GetPlayerListEntry(Player player)
        {
            string row = string.Empty;
            // read player props
            string status = player.GetStringProperty(PlayerProperty.Status);
            if (status != PlayerStatus.Alive)
                status = " <color=red>*dead*</color> ";
            else
                status = string.Empty;

            string team = player.GetStringProperty(PlayerProperty.Team);
            string teamColor = TeamInfo.GetTeamColor(team);
            bool teamsEnabled = SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team;
            string character = player.GetStringProperty(PlayerProperty.Character);
            if (team == TeamInfo.None)
                team = string.Empty;
            if (teamsEnabled)
            {
                if (team == TeamInfo.Blue)
                    team = " B ";
                else if (team == TeamInfo.Red)
                    team = " R ";
                else if (team == TeamInfo.Titan)
                    team = " T ";
                else if (team == TeamInfo.Human)
                    team = " H ";
                else
                    team = string.Empty;
            }
            else
            {
                if (team == TeamInfo.Blue || team == TeamInfo.Red)
                {
                    if (character == PlayerCharacter.Human || character == PlayerCharacter.Shifter)
                    {
                        team = " H ";
                        teamColor = TeamInfo.GetTeamColor(TeamInfo.Human);
                    }
                    else if (character == PlayerCharacter.Titan)
                    {
                        team = " T ";
                        teamColor = TeamInfo.GetTeamColor(TeamInfo.Titan);
                    }
                }
                else if (team == TeamInfo.Titan)
                    team = " T ";
                else if (team == TeamInfo.Human)
                    team = " H ";
                else
                    team = string.Empty;
            }

            team = Util.ColorText(team, teamColor);


            string loadout = player.GetStringProperty(PlayerProperty.Loadout);
            if (loadout == HumanLoadout.APG)
                loadout = " APG ";
            else if (loadout == HumanLoadout.AHSS)
                loadout = " AHSS ";
            else if (loadout == HumanLoadout.Thunderspear)
                loadout = " TS ";
            else
                loadout = string.Empty;


            string name = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient, player.IsLocal) + status + team + loadout + player.GetStringProperty(PlayerProperty.Name);

            string score = string.Empty;
            if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.ScoreboardProperty != string.Empty)
            {
                var property = player.GetCustomProperty(CustomLogicManager.Evaluator.ScoreboardProperty);
                if (property == null)
                    score = string.Empty;
                else
                    score = property.ToString();
            }
            else
            {
                for (int i = 0; i < trackedProperties.Length; i++)
                {
                    object value = player.GetCustomProperty(trackedProperties[i]);
                    score += value != null ? value.ToString() : string.Empty;
                    if (i < trackedProperties.Length - 1)
                    {
                        score += " / ";
                    }
                }
            }
            
            row = Util.SizeText($"{name}: {score}", 19);

            return row;
        }

        private string GetAggregateStats(IGrouping<string, Player> group)
        {
            int kills = 0;
            int deaths = 0;
            int highestDmg = 0;
            int totalDmg = 0;

            foreach (var player in group)
            {
                kills += player.GetIntProperty(PlayerProperty.Kills);
                deaths += player.GetIntProperty(PlayerProperty.Deaths);
                highestDmg += player.GetIntProperty(PlayerProperty.HighestDamage);
                totalDmg += player.GetIntProperty(PlayerProperty.TotalDamage);
            }

            return $"{Util.ColorText(group.Key, TeamInfo.GetTeamColor(group.Key))}: {kills}/{deaths}/{highestDmg}/{totalDmg}\n";
        }

        private string GetPlayerList()
        {
            string list = string.Empty;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                list += GetPlayerListEntry(player) + "\n";
            }
            return list;
        }

        private string GetPlayerListTeams()
        {
            string list = string.Empty;
            var individuals = PhotonNetwork.PlayerList.Where(e => e.GetStringProperty(PlayerProperty.Team) == TeamInfo.None);
            var grouped = PhotonNetwork.PlayerList.Where(e => e.GetStringProperty(PlayerProperty.Team) != TeamInfo.None).GroupBy(e => e.GetStringProperty(PlayerProperty.Team));
            foreach (var group in grouped)
            {
                list += GetAggregateStats(group);
                foreach (Player player in group)
                {
                    list += "\t" + GetPlayerListEntry(player) + "\n";
                }
            }
            foreach (Player player in individuals)
            {
                list += individuals + ":\n";
                list += "\t" + GetPlayerListEntry(player) + "\n";
            }
            return list;
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
            _pausePopup = ElementFactory.CreateHeadedPanel<PausePopup>(transform).GetComponent<PausePopup>();
            _selectMapPopup = ElementFactory.CreateHeadedPanel<CreateGameSelectMapPopup>(transform).GetComponent<CreateGameSelectMapPopup>();
            _createGamePopup = ElementFactory.CreateHeadedPanel<CreateGamePopup>(transform).GetComponent<CreateGamePopup>();
            _customAssetUrlPopup = ElementFactory.CreateDefaultPopup<CustomAssetUrlPopup>(transform).GetComponent<CustomAssetUrlPopup>();
            SkillTooltipPopup = ElementFactory.CreateTooltipPopup<SkillTooltipPopup>(IconPickPopup.transform).GetComponent<SkillTooltipPopup>();
            _popups.Add(_settingsPopup);
            _popups.Add(_pausePopup);
            _popups.Add(_createGamePopup);
            _popups.Add(_selectMapPopup);
            _popups.Add(_customAssetUrlPopup);
            _popups.Add(SkillTooltipPopup);
            _allPausePopups.Add(_settingsPopup);
            _allPausePopups.Add(_pausePopup);
            _allPausePopups.Add(_createGamePopup);
            _allPausePopups.Add(_customAssetUrlPopup);
            _allPausePopups.Add(_selectMapPopup);
        }
    }
}
