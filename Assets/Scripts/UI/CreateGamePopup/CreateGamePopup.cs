using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using CustomLogic;
using Map;
using Photon.Pun;
using Photon.Realtime;

namespace UI
{
    class CreateGamePopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1010f;
        protected override float Height => 630f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "CreateGamePopup";
        public bool IsMultiplayer = false;
        protected override bool UseSound => true;


        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        public Dictionary<string, BaseSetting> SyncModeSettings(MapScript script)
        {
            string logic = BuiltinLevels.LoadLogic(SettingsManager.InGameUI.General.GameMode.Value);
            if (logic == BuiltinLevels.UseMapLogic)
                logic = script.Logic;
            Dictionary<string, BaseSetting> settings = CustomLogicManager.GetModeSettings(logic);
            Dictionary<string, BaseSetting> current = SettingsManager.InGameUI.Mode.Current;
            foreach (string key in new List<string>(settings.Keys))
            {
                if (current.ContainsKey(key))
                    settings[key] = current[key];
            }
            SettingsManager.InGameUI.Mode.Current = settings;
            return settings;
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

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "General", "Mode", "Titans", "Weather", "Misc", "Custom" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button"),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("General", typeof(CreateGameGeneralPanel));
            _categoryPanelTypes.Add("Mode", typeof(CreateGameModePanel));
            _categoryPanelTypes.Add("Titans", typeof(CreateGameTitansPanel));
            _categoryPanelTypes.Add("Weather", typeof(CreateGameWeatherPanel));
            _categoryPanelTypes.Add("Misc", typeof(CreateGameMiscPanel));
            _categoryPanelTypes.Add("Custom", typeof(CreateGameCustomPanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            string start = SceneLoader.SceneName == SceneName.InGame ? "Restart" : "Start";
            foreach (string buttonName in new string[] { "LoadPreset", "SavePreset", start, "Back" })
            {
                string locale = UIManager.GetLocaleCommon(buttonName);
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, locale,
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        public override void Hide()
        {
            if (gameObject.activeSelf && SceneLoader.SceneName == SceneName.MainMenu)
            {
                PhotonNetwork.Disconnect();
            }
            base.Hide();
        }

        public void HideNoDisconnect()
        {
            base.Hide();
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Restart":
                    InGameManager.RestartGame();
                    break;
                case "Start":
                    MusicManager.PlayEffect();
                    MusicManager.PlayTransition();
                    SettingsManager.InGameCurrent.Copy(SettingsManager.InGameUI);
                    if (!IsMultiplayer)
                        SettingsManager.MultiplayerSettings.ConnectOffline();
                    StartRoom();
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
                case "LoadPreset":
                    UIManager.CurrentMenu.SelectListPopup.ShowLoad(SettingsManager.InGameSettings.InGameSets.GetSetNames().ToList(), onLoad: () => OnLoadPreset(),
                        disallowedDelete: GetPresetDisallowedDelete(), onDelete: () => OnDeletePreset());
                    break;
                case "SavePreset":
                    List<string> disallowedDelete = GetPresetDisallowedDelete();
                    UIManager.CurrentMenu.SelectListPopup.ShowSave(SettingsManager.InGameSettings.InGameSets.GetSetNames().ToList(), onSave: () => OnSavePreset(),
                        disallowedSave: disallowedDelete, disallowedDelete: disallowedDelete, onDelete: () => OnDeletePreset());
                    break;
            }
            SettingsManager.WeatherSettings.Save();
            SettingsManager.WeatherSettings.Load();
        }

        private void StartRoom()
        {
            InGameSet settings = SettingsManager.InGameCurrent;
            string roomName = settings.General.RoomName.Value;
            string mapName = settings.General.MapName.Value;
            string gameMode = settings.General.GameMode.Value;
            int maxPlayers = settings.General.MaxPlayers.Value;
            string password = settings.General.Password.Value;
            if (password.Length > 0)
                password = new SimpleAES().Encrypt(password);
            string roomId = UnityEngine.Random.Range(0, 100000).ToString();
            var properties = new ExitGames.Client.Photon.Hashtable
            {
                { RoomProperty.Name, roomName },
                { RoomProperty.Map, mapName },
                { RoomProperty.GameMode, gameMode },
                { RoomProperty.Password, password }
            };
            string[] lobbyProperties = new string[] { RoomProperty.Name, RoomProperty.Map, RoomProperty.GameMode, RoomProperty.Password };
            var roomOptions = new RoomOptions();
            roomOptions.CustomRoomProperties = properties;
            roomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom(roomId, roomOptions);
        }

        private void OnDeletePreset()
        {
            string name = UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value;
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
            {
                if (set.Name.Value == name && !set.Preset.Value)
                {
                    SettingsManager.InGameSettings.InGameSets.Sets.Value.Remove(set);
                    SettingsManager.InGameSettings.Save();
                    return;
                }
            }
        }

        private List<string> GetPresetDisallowedDelete()
        {
            List<string> disallowedDelete = new List<string>();
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.Value)
            {
                if (set.Preset.Value)
                    disallowedDelete.Add(set.Name.Value);
            }
            return disallowedDelete;
        }

        private void OnLoadPreset()
        {
            string name = UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value;
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
            {
                if (set.Name.Value == name)
                {
                    SettingsManager.InGameUI.Copy(set);
                    RebuildCategoryPanel();
                    return;
                }
            }
        }

        private void OnSavePreset()
        {
            string name = UIManager.CurrentMenu.SelectListPopup.FinishSetting.Value;
            if (name == string.Empty)
                return;
            foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
            {
                if (set.Name.Value == name)
                {
                    if (set.Preset.Value)
                        Debug.Log("Attempting to overwrite preset.");
                    else
                    {
                        set.Copy(SettingsManager.InGameUI);
                        SettingsManager.InGameSettings.Save();
                    }
                    return;
                }
            }
            InGameSet newSet = new InGameSet();
            newSet.Copy(SettingsManager.InGameUI);
            newSet.Name.Value = name;
            SettingsManager.InGameSettings.InGameSets.Sets.AddItem(newSet);
            SettingsManager.InGameSettings.Save();
        }
    }
}
