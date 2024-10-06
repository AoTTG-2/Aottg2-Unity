using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    class PlayerListPanel : MonoBehaviour
    {
        public string Team { get; set; }
        private MultiTextLabel _teamHeader;
        private GameObject _mainPanel;
        private GameObject _idPanel;
        private GameObject _namePanel;
        private ElementStyle _style;
        private Dictionary<int, PlayerKDRRow> _rows = new Dictionary<int, PlayerKDRRow>();
        private const float MaxSyncDelay = 0.2f;
        private float _currentSyncDelay = 1f;

        // State
        private int kills = 0;
        private int deaths = 0;
        private int maxDamage = 0;
        private int totalDamage = 0;
        private KDRMode _kdrMode = KDRMode.Off;

        public void Setup(ElementStyle style, string team)
        {
            _teamHeader = ElementFactory.CreateMultiTextLabel(
                gameObject.transform, style, FontStyle.Normal, TextAnchor.MiddleLeft, 24f, 2
                ).GetComponent<MultiTextLabel>();

            // Set team to team color.
            if (ColorUtility.TryParseHtmlString(TeamInfo.GetTeamColor(team), out Color c))
                _teamHeader.ChangeTextColor(0, c);
            else
                _teamHeader.ChangeTextColor(0, Color.white);

            _teamHeader.SetValue(0, $"{team}: ");
            _teamHeader.SetValue(1, "0/0/0/0");
            _mainPanel = ElementFactory.CreateHorizontalGroup(gameObject.transform, 5, TextAnchor.UpperLeft);
            _idPanel = ElementFactory.CreateVerticalGroup(_mainPanel.transform, 0, TextAnchor.UpperLeft);
            _namePanel = ElementFactory.CreateVerticalGroup(_mainPanel.transform, 0, TextAnchor.UpperLeft);
            _style = style;
            Team = team;
        }

        public void Update()
        {
            // Only sync if this player list contains the local player (all other players update when their props change)
            if (PhotonNetwork.LocalPlayer != null && _rows.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                _currentSyncDelay -= Time.deltaTime;
                if (_currentSyncDelay <= 0f)
                    Sync();
            }            
        }

        public void Sync()
        {
            _currentSyncDelay = MaxSyncDelay;
            bool teamsEnabled = SettingsManager.InGameCurrent.Misc.PVP.Value == (int)PVPMode.Team
                                && (KDRMode)SettingsManager.UISettings.KDR.Value == KDRMode.All;

            UpdatePlayer(PhotonNetwork.LocalPlayer);
            if (_teamHeader.isActiveAndEnabled != teamsEnabled)
                _teamHeader.gameObject.SetActive(teamsEnabled);

            if (!teamsEnabled)
                return;

            // temp. fix later
            ResetState();
            foreach (var row in _rows)
                AggregatePlayerStats(row.Value);

            _teamHeader.SetValue(2, $"{kills}/{deaths}/{maxDamage}/{totalDamage}");
        }

        public void UpdatePlayer(Player player)
        {
            if (player == null)
                return;
            if (_rows.ContainsKey(player.ActorNumber))
                _rows[player.ActorNumber].UpdateRow();
        }

        public bool ContainsPlayer(Player player)
        {
            if (player == null)
                return false;
            return _rows.ContainsKey(player.ActorNumber);
        }

        public bool HasPlayers()
        {
            return _rows.Count > 0;
        }

        public void AddRow(Player player)
        {
            if (player == null)
                return;
            if (_rows.ContainsKey(player.ActorNumber))
                return;
            _rows.Add(player.ActorNumber, new PlayerKDRRow(_idPanel.transform, _namePanel.transform, _style, player));
        }

        public void RemoveRow(Player player)
        {
            if (player == null)
                return;
            if (_rows.ContainsKey(player.ActorNumber))
            {
                _rows[player.ActorNumber].Destroy();
                _rows.Remove(player.ActorNumber);
            }
        }

        public void Cleanup()
        {
            Debug.Log($"Destroying {Team}'s panel");
            foreach (var row in _rows)
                if (row.Value != null)
                    row.Value.Destroy();
            _rows.Clear();
        }

        private void ResetState()
        {
            kills = 0;
            deaths = 0;
            maxDamage = 0;
            totalDamage = 0;
        }
        
        private void AggregatePlayerStats(PlayerKDRRow row)
        {
            kills += row.kills;
            deaths += row.deaths;
            maxDamage = Math.Max(maxDamage, row.maxDamage);
            totalDamage += row.totalDamage;
        }


    }
}
