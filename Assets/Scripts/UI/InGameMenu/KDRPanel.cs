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
        private GameObject _kdrGroup;
        private ElementStyle _style;
        private Dictionary<string, PlayerListPanel> _teams = new Dictionary<string, PlayerListPanel>();

        // Syncing
        private const float MaxSyncDelay = 0.2f;
        private float _currentSyncDelay = 1f;
        private KDRMode _kdrMode = KDRMode.Off;
        private PVPMode _pvpMode = PVPMode.Off;
        private string _defaultTeam = "Individuals";
        public void Setup(ElementStyle style)
        {
            _kdrGroup = ElementFactory.CreateVerticalGroup(transform, 10f, TextAnchor.UpperLeft);
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

        private void AddPlayer(Player player)
        {
            if (player == null)
                return;
            string team = GetPlayerTeam(player);
            if (!_teams.ContainsKey(team))
            {
                // Create PlayerListPanel for team and add to teams
                var playerlist = ElementFactory.CreateVerticalGroup(_kdrGroup.transform, 0f, TextAnchor.UpperLeft);
                var playerListComponent = playerlist.AddComponent<PlayerListPanel>();
                playerListComponent.Setup(_style, team);
                _teams.Add(team, playerListComponent);
            }
            _teams[team].AddRow(player);

        }

        private void RemovePlayer(Player player)
        {
            if (player == null)
                return;
            string team = FindPlayerTeam(player);
            if (_teams.ContainsKey(team))
            {
                _teams[team].RemoveRow(player);

                // if the team is empty, remove the team from the dictionary
                if (!_teams[team].HasPlayers())
                {
                    _teams[team].Cleanup();
                    Destroy(_teams[team].gameObject);
                    _teams.Remove(team);
                }
            }
            
        }

        public void DestroyAndRecreate()
        {
            // Destroy all rows and add all players via PhotonNetwork.PlayerListOthers
            Debug.Log("Resetting KDRPanel UI");
            foreach (var playerList in _teams)
            {
                if (playerList.Value != null)
                {
                    playerList.Value.Cleanup();
                    Destroy(playerList.Value.gameObject);
                }
            }
            _teams.Clear();

            if (SettingsManager.UISettings.KDR.Value == (int)KDRMode.Off)
                return;

            AddPlayer(PhotonNetwork.LocalPlayer);

            if (SettingsManager.UISettings.KDR.Value == (int)KDRMode.Mine)
                return;
            // Create rows for all other players and set them
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                AddPlayer(player);
            }
        }

        public string FindPlayerTeam(Player player)
        {
            foreach (var team in _teams)
            {
                if (team.Value.ContainsPlayer(player))
                {
                    return team.Key;
                }
            }
            return string.Empty;
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            AddPlayer(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            RemovePlayer(otherPlayer);
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (SettingsManager.UISettings.KDR.Value == (int)KDRMode.Off)
                return;

            // if hashtable contains team prop, swap team
            changedProps.TryGetValue(PlayerProperty.Team, out object team);

            if (team != null)
            {
                RemovePlayer(targetPlayer);
                AddPlayer(targetPlayer);
            }
            else
            {
                if (targetPlayer.IsLocal || SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                {
                    return;
                }

                // Sync the player row with the target player
                if (_teams.ContainsKey(targetPlayer.GetStringProperty(PlayerProperty.Team)))
                {
                    _teams[targetPlayer.GetStringProperty(PlayerProperty.Team)].UpdatePlayer(targetPlayer);
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
