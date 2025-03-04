using ApplicationManagers;
using CustomLogic;
using ExitGames.Client.Photon;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class KDRPanel : MonoBehaviour, IInRoomCallbacks, IMatchmakingCallbacks
    {
        private ElementStyle _style;
        private Dictionary<int, PlayerKDRRow> _players = new Dictionary<int, PlayerKDRRow>();
        private Dictionary<string, TeamKDRRow> _teamHeaders = new Dictionary<string, TeamKDRRow>();
        // Syncing
        private const float MaxSyncDelay = 0.2f;
        private float _currentSyncDelay = 1f;
        private KDRMode _kdrMode = KDRMode.Off;
        private PVPMode _pvpMode = PVPMode.Off;
        private string _defaultTeam = "Individuals";
        public bool _showScoreboardLoadout = false;
        public bool _showScoreboardStatus = false;
        public void Setup(ElementStyle style)
        {
            _style = style;
            DestroyAndRecreate();
            Sync();
        }

        private void Update()
        {
            _currentSyncDelay -= Time.deltaTime;
            if (_currentSyncDelay <= 0f)
                Sync();
        }

        /// <summary>
        /// Sync the rows with the player data by doing a diff of PlayerRow's values
        /// </summary>
        private void Sync()
        {
            // Check if kdr mode changed
            if ((KDRMode)SettingsManager.UISettings.KDR.Value != _kdrMode)
            {
                _kdrMode = (KDRMode)SettingsManager.UISettings.KDR.Value;
                DestroyAndRecreate();
            }

            if ((PVPMode)SettingsManager.InGameCurrent.Misc.PVP.Value != _pvpMode)
            {
                _pvpMode = (PVPMode)SettingsManager.InGameCurrent.Misc.PVP.Value;
                DestroyAndRecreate();
            }
            var evaluator = CustomLogicManager.Evaluator;
            if (evaluator != null)
            {
                if (_showScoreboardStatus != evaluator.ShowScoreboardStatus || _showScoreboardLoadout != evaluator.ShowScoreboardLoadout)
                {
                    _showScoreboardLoadout = evaluator.ShowScoreboardLoadout;
                    _showScoreboardStatus = evaluator.ShowScoreboardStatus;
                    DestroyAndRecreate();
                }
            }
            _currentSyncDelay = MaxSyncDelay;
        }

        private string GetPlayerTeam(Player player)
        {
            if (player == null)
                return string.Empty;

            // If teams are not enabled, return empty string
            if (SettingsManager.InGameCurrent.Misc.PVP.Value != (int)PVPMode.Team)
                return _defaultTeam;

            return player.GetStringProperty(PlayerProperty.Team, _defaultTeam);
        }

        private void ReorganizeLayout()
        {
            var sortedHeaders = _teamHeaders.OrderBy(x => x.Key).ToList();
            var sortedPlayers = _players.OrderBy(x => GetPlayerTeam(x.Value.player)).ThenBy(x => x.Key).ToList();

            int siblingIndex = 0;

            foreach (var header in sortedHeaders)
            {
                header.Value.transform.SetSiblingIndex(siblingIndex++);
                foreach (var kvp in sortedPlayers)
                {
                    if (GetPlayerTeam(kvp.Value.player) == header.Key)
                    {
                        kvp.Value.transform.SetSiblingIndex(siblingIndex++);
                    }
                }
            }
        }

        private void AddPlayer(Player player, bool redoLayout=false, bool isVisible=true)
        {
            if (player == null)
                return;
            if (_players.ContainsKey(player.ActorNumber))
                return;

            var playerRow = ElementFactory.CreatePlayerKDRRow(transform, _style, player);
            var playerKDRRow = playerRow.GetComponent<PlayerKDRRow>();
            if (!isVisible)
                playerKDRRow.gameObject.SetActive(false);
            _players.Add(player.ActorNumber, playerKDRRow);

            if (_pvpMode == PVPMode.Team)
            {
                // Create header for team if it doesn't exist and set element position to the last team header
                string team = GetPlayerTeam(player);
                if (!_teamHeaders.ContainsKey(team))
                {
                    GameObject header = ElementFactory.CreateTeamKDRRow(transform, _style, team);
                    TeamKDRRow teamKDRRow = header.GetComponent<TeamKDRRow>();
                    header.SetActive(SettingsManager.UISettings.KDR.Value != (int)KDRMode.Off);
                    _teamHeaders.Add(team, teamKDRRow);
                }

                var headerUI = _teamHeaders[team];
                headerUI.AddPlayerStats(playerKDRRow);
                headerUI.UpdateRow();
                if (redoLayout)
                    ReorganizeLayout();
            }
        }

        private void RemovePlayer(Player player, bool redoLayout=false)
        {
            if (player == null)
                return;
            if (!_players.ContainsKey(player.ActorNumber))
                return;

            // Remove player from team header
            if (_pvpMode == PVPMode.Team)
            {
                string team = _players[player.ActorNumber].team;
                if (!_teamHeaders.ContainsKey(team))
                {
                    team = _defaultTeam;
                }
                if (_teamHeaders.ContainsKey(team))
                {
                    var header = _teamHeaders[team];
                    header.RemovePlayerStats(_players[player.ActorNumber]);

                    if (_teamHeaders[team].playerCount <= 0)
                    {
                        Destroy(_teamHeaders[team].gameObject);
                        _teamHeaders.Remove(team);
                    }
                    else
                    {
                        _teamHeaders[team].UpdateRow();
                    }
                }

                if (redoLayout)
                    ReorganizeLayout();

            }

            // Remove the player row
            Destroy(_players[player.ActorNumber].gameObject);
            _players.Remove(player.ActorNumber);
        }

        public void DestroyAndRecreate()
        {
            foreach (var player in _players)
            {
                Destroy(player.Value.gameObject);
            }
            _players.Clear();

            foreach (var team in _teamHeaders)
            {
                Destroy(team.Value.gameObject);
            }
            _teamHeaders.Clear();

            bool isVisible = SettingsManager.UISettings.KDR.Value != (int)KDRMode.Off;

            AddPlayer(PhotonNetwork.LocalPlayer, isVisible: isVisible);

            isVisible = isVisible && SettingsManager.UISettings.KDR.Value != (int)KDRMode.Mine;
            // Create rows for all other players and set them
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                AddPlayer(player, isVisible: isVisible);
            }

            ReorganizeLayout();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            bool isVisible = !(SettingsManager.UISettings.KDR.Value != (int)KDRMode.All || newPlayer == null);
            AddPlayer(newPlayer, redoLayout: true, isVisible);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All || otherPlayer == null)
                return;
            RemovePlayer(otherPlayer, redoLayout: true);
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            bool isVisible = SettingsManager.UISettings.KDR.Value != (int)KDRMode.Off && (SettingsManager.UISettings.KDR.Value != (int)KDRMode.Mine || targetPlayer == PhotonNetwork.LocalPlayer);

            // if hashtable contains team prop, swap team
            changedProps.TryGetValue(PlayerProperty.Team, out object team);

            if (team != null)
            {
                RemovePlayer(targetPlayer);
                AddPlayer(targetPlayer, redoLayout: true, isVisible);
            }
            else
            {
                if (!targetPlayer.IsLocal && SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                {
                    return;
                }

                // Update player row
                if (_players.ContainsKey(targetPlayer.ActorNumber))
                {
                    // Update team header
                    if (_pvpMode == PVPMode.Team)
                    {
                        string playerTeam = GetPlayerTeam(targetPlayer);
                        if (_teamHeaders.ContainsKey(playerTeam))
                        {
                            _teamHeaders[playerTeam].RemovePlayerStats(_players[targetPlayer.ActorNumber]);
                            _players[targetPlayer.ActorNumber].UpdateRow(targetPlayer);
                            _teamHeaders[playerTeam].AddPlayerStats(_players[targetPlayer.ActorNumber]);
                            _teamHeaders[playerTeam].UpdateRow();
                        }
                    }
                    else
                    {
                        _players[targetPlayer.ActorNumber].UpdateRow(targetPlayer);
                    }
                }
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
        public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
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
