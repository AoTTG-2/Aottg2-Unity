using ApplicationManagers;
using CustomLogic;
using ExitGames.Client.Photon;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using static UnityEngine.Rendering.DebugUI.Table;

namespace UI
{
    class KDRPanel : BasePanel, IInRoomCallbacks, IMatchmakingCallbacks
    {
        protected override string ThemePanel => "KDRPanel";
        private GameObject _panel;
        private PlayerKDRRow _myRow;
        private Dictionary<int, PlayerKDRRow> _rows = new Dictionary<int, PlayerKDRRow>();
        private Text _gameTimeText;
        private Text _systemTimeText;
        private Text _fpsText;
        private Text _pingText;
        private const float MaxSyncDelay = 0.2f;
        private float _currentSyncDelay = 1f;

        private const float MaxTelemetryDelay = 0.01f;
        private float _currentTelemetryDelay = 1f;

        private ElementStyle _style;
        private float _maxWidth = 0f;
        private int _ownerMaxWidth = -10;

        public override void Setup(BasePanel parent = null)
        {
            _panel = transform.Find("Panel").gameObject;
            _style = new ElementStyle(themePanel: ThemePanel);
            CreateTelemetry();
            DestroyAndRecreate();
            Sync();
        }

        private void Update()
        {
            _currentSyncDelay -= Time.deltaTime;
            if (_currentSyncDelay <= 0f)
                Sync();

            _currentTelemetryDelay -= Time.deltaTime;
            if (_currentTelemetryDelay <= 0f)
            {
                SyncTelemetry();
            }
        }

        private void CreateTelemetry()
        {
            if (_gameTimeText != null || _systemTimeText != null || _fpsText != null || _pingText != null)
                return;
            var timeRow = ElementFactory.CreateHorizontalGroup(_panel.transform, 25f, TextAnchor.MiddleLeft);
            _gameTimeText = ElementFactory.CreateDefaultLabel(timeRow.transform, _style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            _systemTimeText = ElementFactory.CreateDefaultLabel(timeRow.transform, _style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();

            var performanceRow = ElementFactory.CreateHorizontalGroup(_panel.transform, 25f, TextAnchor.MiddleLeft);
            _fpsText = ElementFactory.CreateDefaultLabel(performanceRow.transform, _style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
            _pingText = ElementFactory.CreateDefaultLabel(performanceRow.transform, _style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
        }

        private void SyncTelemetry()
        {
            if (SettingsManager.UISettings.ShowGameTime.Value)
            {
                if (!_gameTimeText.gameObject.activeSelf)
                    _gameTimeText.gameObject.SetActive(true);
                if (!_systemTimeText.gameObject.activeSelf)
                    _systemTimeText.gameObject.SetActive(true);
                _gameTimeText.text = "Game Time: " + ChatManager.GetColorString(Util.FormatFloat(CustomLogicManager.Evaluator?.CurrentTime ?? 0, 2), ChatTextColor.System) + ",";
                var dt = System.DateTime.Now;
                _systemTimeText.text = "System: " + ChatManager.GetColorString(ChatManager.GetTimeString(dt.Hour) + ":" + ChatManager.GetTimeString(dt.Minute) + ":" + ChatManager.GetTimeString(dt.Second), ChatTextColor.System);
            }
            else
            {
                _gameTimeText.text = string.Empty;
                _systemTimeText.text = string.Empty;

                // disable
                _gameTimeText.gameObject.SetActive(false);
                _systemTimeText.gameObject.SetActive(false);

            }
            if (SettingsManager.GraphicsSettings.ShowFPS.Value)
            {
                if (!_fpsText.gameObject.activeSelf)
                    _fpsText.gameObject.SetActive(true);
                _fpsText.text = "FPS:" + UIManager.GetFPS().ToString();
                if (!PhotonNetwork.OfflineMode && SettingsManager.UISettings.ShowPing.Value)
                    _fpsText.text += ",";
            }
            else
                _fpsText.gameObject.SetActive(false);
            if (!PhotonNetwork.OfflineMode && SettingsManager.UISettings.ShowPing.Value)
            {
                if (!_pingText.gameObject.activeSelf)
                    _pingText.gameObject.SetActive(true);
                _pingText.text = "Ping:" + PhotonNetwork.GetPing().ToString();
            }
            else
                _pingText.gameObject.SetActive(false);

            _currentTelemetryDelay = MaxTelemetryDelay;
        }

        /// <summary>
        /// Sync the rows with the player data by doing a diff of PlayerRow's values
        /// </summary>
        private void Sync()
        {
            // Sync my row
            if (PhotonNetwork.LocalPlayer != null)
                _myRow.UpdateRow();

            _currentSyncDelay = MaxSyncDelay;
        }

        public void DestroyAndRecreate()
        {
            // Destroy all rows and add all players via PhotonNetwork.PlayerListOthers
            Debug.Log("Destroying and recreating");
            foreach (var row in _rows)
                if (row.Value?.gameObject != null)
                    Destroy(row.Value.gameObject);
            _rows.Clear();

            // Destroy my row and recreate it
            if (_myRow?.gameObject != null)
                Destroy(_myRow.gameObject);
            _myRow = new PlayerKDRRow(_panel.transform, _style, PhotonNetwork.LocalPlayer);

            // Create rows for all other players and set them
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                if (player != null)
                {
                    _rows.Add(player.ActorNumber, new PlayerKDRRow(_panel.transform, _style, player));

                    // Check if this new element has a larger width
                    if (_rows[player.ActorNumber].GetFirstElementWidth() > _maxWidth)
                    {
                        _maxWidth = _rows[player.ActorNumber].GetFirstElementWidth();
                        _ownerMaxWidth = player.ActorNumber;
                    }
                }
            }
            // Update all rows with the new width
            foreach (var row in _rows)
            {
                row.Value.UpdateFirstElementWidth(_maxWidth);
            }
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            _rows.Add(newPlayer.ActorNumber, new PlayerKDRRow(_panel.transform, _style, newPlayer));

            // Check if this new element has a larger width
            if (_rows[newPlayer.ActorNumber].GetFirstElementWidth() > _maxWidth)
            {
                _maxWidth = _rows[newPlayer.ActorNumber].GetFirstElementWidth();
                _ownerMaxWidth = newPlayer.ActorNumber;

                // Update all rows with the new width
                foreach (var row in _rows)
                {
                    if (row.Key != newPlayer.ActorNumber)
                        row.Value.UpdateFirstElementWidth(_maxWidth);
                }

            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            
            // try to get the row of the player and destroy it if it exists, then remove it from the dictionary
            if (_rows.ContainsKey(otherPlayer.ActorNumber))
            {
                Destroy(_rows[otherPlayer.ActorNumber].gameObject);
                _rows.Remove(otherPlayer.ActorNumber);

                // Check if the player with the largest width left
                if (_ownerMaxWidth == otherPlayer.ActorNumber)
                {
                    _maxWidth = 0f;
                    _ownerMaxWidth = -10;

                    // Find the new player with the largest width
                    foreach (var row in _rows)
                    {
                        if (row.Value.GetFirstElementWidth() > _maxWidth)
                        {
                            _maxWidth = row.Value.GetFirstElementWidth();
                            _ownerMaxWidth = row.Key;
                        }
                    }

                    // Update all rows with the new width
                    foreach (var row in _rows)
                    {
                        if (row.Key != otherPlayer.ActorNumber)
                            row.Value.UpdateFirstElementWidth(_maxWidth);
                    }
                }
            }
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.IsLocal)
            {
                // Sync local player on timer (faster)
                return;
            }

            // Sync the player row with the target player
            if (_rows.ContainsKey(targetPlayer.ActorNumber))
            {
                _rows[targetPlayer.ActorNumber].UpdateRow();
            }
        }

        public void OnJoinedRoom()
        {
            DestroyAndRecreate();
        }

        public virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #region Unused
        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            //throw new NotImplementedException();
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            //throw new NotImplementedException();
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            //throw new NotImplementedException();
        }

        public void OnCreatedRoom()
        {
            //throw new NotImplementedException();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            //throw new NotImplementedException();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            //throw new NotImplementedException();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            //throw new NotImplementedException();
        }

        public void OnLeftRoom()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
