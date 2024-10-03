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

namespace UI
{
    struct PlayerRow
    {
        public Transform row;
        public GameObject gameObject;
        public Player Player;
        public Text id;
        public Text name;
        public RawImage weapon;
        public Text score;
        public bool isSet;
        public bool isMasterClient;
        public int actorNumber;
        public string status;
        public string character;
    }

    class KDRPanel : BasePanel, IInRoomCallbacks, IMatchmakingCallbacks
    {
        protected override string ThemePanel => "KDRPanel";
        private GameObject _panel;
        private PlayerRow _myRow = new PlayerRow();
        private List<PlayerRow> _rows = new List<PlayerRow>();
        private Player[] _lastPlayers;
        private const float MaxSyncDelay = 1f;
        private float _currentSyncDelay = 1f;
        private ElementStyle _style;
        private string[] trackedProperties = new string[] { "Kills", "Deaths", "HighestDamage", "TotalDamage" };
        private Dictionary<string, string> PlayerStatuses;
        private Dictionary<string, string> CharacterTypes;
        private Dictionary<string, Texture> WeaponIcons;


        public override void Setup(BasePanel parent = null)
        {
            // _panel = transform.Find("Content/Panel").gameObject;
            _panel = transform.Find("Panel").gameObject;
            _style = new ElementStyle(themePanel: ThemePanel);

            PlayerStatuses = new Dictionary<string, string>
            {
                { PlayerStatus.Spectating, " <color=red>*dead*</color> " },
                { PlayerStatus.Dead, " <color=white>*spec*</color> " },
                { PlayerStatus.Alive, string.Empty }
            };
            CharacterTypes = new Dictionary<string, string>
            {
                { TeamInfo.Human, Util.ColorText(" H ", TeamInfo.GetTeamColor(TeamInfo.Human)) },
                { TeamInfo.Titan, Util.ColorText(" T ", TeamInfo.GetTeamColor(TeamInfo.Titan)) },
                { TeamInfo.Blue, Util.ColorText(" B ", TeamInfo.GetTeamColor(TeamInfo.Blue)) },
                { TeamInfo.Red, Util.ColorText(" R ", TeamInfo.GetTeamColor(TeamInfo.Red)) },

            };
            WeaponIcons = new Dictionary<string, Texture>
            {
                { HumanLoadout.Blades, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/BladeIcon", true) },
                { HumanLoadout.AHSS, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/AHSSIcon", true) },
                { HumanLoadout.APG, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/APGIcon", true) },
                { HumanLoadout.Thunderspears, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ThunderspearIcon", true) },
                { PlayerCharacter.Titan, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/TitanIcon", true) },
                { PlayerCharacter.Shifter, (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ShifterIcon", true) }
            };

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
            // Sync my row
            if (_myRow.Player != null)
            {
                SetRow(_myRow, PhotonNetwork.LocalPlayer);
            }

            // Iterate over all rows and sync
            foreach (var row in _rows)
            {
                var player = row.Player;
                if (player == null)
                    continue;
                SetRow(row, player);
            }
            _currentSyncDelay = MaxSyncDelay;
        }

        private PlayerRow CreateRow(Player player, ElementStyle style)
        {
            // NOTE: Added spacing for horizontal group
            GameObject rowGameObject = ElementFactory.CreateHorizontalGroup(_panel.transform, 12f, TextAnchor.MiddleCenter);
            Transform rowGroup = rowGameObject.transform;
            // player
            var id = ElementFactory.CreateDefaultLabel(rowGroup, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft);   // host, id, status (0)
            var weapon = ElementFactory.CreateRawImage(rowGroup, style, "Icons/Game/BladeIcon");  // loadout/character type   (1)
            var name = ElementFactory.CreateDefaultLabel(rowGroup, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft); // name   (2)
            var score = ElementFactory.CreateDefaultLabel(rowGroup, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter); // score    (3)
            PlayerRow row = new PlayerRow
            {
                row = rowGroup,
                gameObject = rowGameObject,
                id = id.GetComponent<Text>(),
                name = name.GetComponent<Text>(),
                weapon = weapon.GetComponent<RawImage>(),
                score = score.GetComponent<Text>(),
                isSet = false,
                isMasterClient = false,
                actorNumber = -1,
                status = string.Empty,
                character = string.Empty
            };

            SetRow(row, player);

            return row;
        }

        private PlayerRow SetRow(PlayerRow row, Player player)
        {
            string playerName = player.GetStringProperty(PlayerProperty.Name);
            string status = player.GetStringProperty(PlayerProperty.Status);
            string character = player.GetStringProperty(PlayerProperty.Character);
            string loadout = player.GetStringProperty(PlayerProperty.Loadout);
            if (!row.isSet)
            {
                row.isSet = true;
                row.actorNumber = player.ActorNumber;
                row.id.text = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient, player.IsLocal) + PlayerStatuses[status];
                row.name.text = playerName;
                row.status = status;
                row.Player = player;
            }

            // If host, id, status, or type change, change the row transform label
            if (row.isMasterClient != player.IsMasterClient || row.status != status)
            {
                row.isMasterClient = player.IsMasterClient;
                row.status = status;
                row.id.text = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient, player.IsLocal) + PlayerStatuses[status];
            }

            // If human and loadout change, change the weapon icon, otherwise if titan or shifter, change the icon
            if (character == PlayerCharacter.Human && row.character != character || loadout != row.character)
            {
                row.character = character;
                row.weapon.texture = WeaponIcons[loadout];
            }
            else if (character != row.character)
            {
                row.character = character;
                row.weapon.texture = WeaponIcons[character];
            }

            // If name changes, change the name label
            if (playerName != row.name.text)
                row.name.text = playerName + ": ";

            // If score changes, change the score label
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

            if (score != row.score.text)
                row.score.text = score;

            return row;
        }

        public void DestroyAndRecreate()
        {
            // Destroy all rows and add all players via PhotonNetwork.PlayerListOthers
            foreach (var row in _rows)
                Destroy(row.gameObject);
            _rows.Clear();

            // Create my row and set it
            _myRow = CreateRow(PhotonNetwork.LocalPlayer, _style);

            // Create rows for all other players and set them
            foreach (var player in PhotonNetwork.PlayerListOthers)
            {
                if (player != null)
                {
                    var row = CreateRow(player, _style);
                    _rows.Add(row);
                }
            }
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            // Create a row for the new player and set it
            var row = CreateRow(newPlayer, _style);
            _rows.Add(row);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (SettingsManager.UISettings.KDR.Value != (int)KDRMode.All)
                return;
            // Destroy the row for the player that left
            for (int i = 0; i < _rows.Count; i++)
            {
                if (_rows[i].actorNumber == otherPlayer.ActorNumber)
                {
                    Destroy(_rows[i].gameObject);
                    _rows.RemoveAt(i);
                    break;
                }
            }
        }

        public void OnJoinedRoom()
        {
            DestroyAndRecreate();
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            //throw new NotImplementedException();
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
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
    }
}
