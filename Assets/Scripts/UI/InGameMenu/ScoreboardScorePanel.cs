using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using ApplicationManagers;
using GameManagers;
using Photon.Realtime;
using Photon.Pun;
using Utility;

namespace UI
{
    class ScoreboardScorePanel: ScoreboardCategoryPanel
    {
        private List<Transform> _rows = new List<Transform>();
        private Transform _header;
        private Player[] _lastPlayers;
        private const float MaxSyncDelay = 1f;
        private float _currentSyncDelay = 1f;
        protected override float VerticalSpacing => 10f;
        protected override int VerticalPadding => 15;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            Sync();
        }

        private void Update()
        {
            _currentSyncDelay -= Time.deltaTime;
            if (_currentSyncDelay <= 0f)
                Sync();
        }

        public void Sync()
        {
            _lastPlayers = (Player[])PhotonNetwork.PlayerList.Clone();
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            SetHeader(style);
            SetRows(style);
            _currentSyncDelay = MaxSyncDelay;
        }

        private void SetRows(ElementStyle style)
        {
            int rowCount = _rows.Count;
            if (rowCount > _lastPlayers.Length)
            {
                for (int i = 0; i < rowCount - _lastPlayers.Length; i++)
                {
                    Destroy(_rows[_rows.Count - 1].gameObject);
                    _rows.RemoveAt(_rows.Count - 1);
                }
            }
            else if (rowCount < _lastPlayers.Length)
            {
                for (int i = 0; i < _lastPlayers.Length - rowCount; i++)
                    _rows.Add(CreateRow(style, _rows.Count));
            }
            for (int i = 0; i < _rows.Count; i++)
                SetRow(_rows[i], _lastPlayers[i]);
        }

        private void SetHeader(ElementStyle style)
        {
            if (_header == null)
            {
                _header = ElementFactory.CreateHorizontalGroup(SinglePanel, 0f, TextAnchor.MiddleCenter).transform;
                ElementFactory.CreateDefaultLabel(_header, style, UIManager.GetLocale("ScoreboardPopup", "Scoreboard", "Player"), FontStyle.Bold, TextAnchor.MiddleCenter);
                ElementFactory.CreateDefaultLabel(_header, style, string.Empty, FontStyle.Bold, TextAnchor.MiddleCenter);
                ElementFactory.CreateDefaultLabel(_header, style, UIManager.GetLocale("ScoreboardPopup", "Scoreboard", "Action"), FontStyle.Bold, TextAnchor.MiddleCenter);
                foreach (Transform t in _header)
                    t.GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() / 3f;
                CreateHorizontalDivider(SinglePanel);
            }
            _header.GetChild(1).GetComponent<Text>().text = "Kills / Deaths / Max / Total";
        }

        private Transform CreateRow(ElementStyle style, int index)
        {
            Transform row = ElementFactory.CreateHorizontalGroup(SinglePanel, 0f, TextAnchor.MiddleCenter).transform;
            // player
            Transform playerRow = ElementFactory.CreateHorizontalGroup(row, 5f, TextAnchor.MiddleCenter).transform;
            ElementFactory.CreateRawImage(playerRow, style, "Icons/Quests/Skull1Icon");
            ElementFactory.CreateRawImage(playerRow, style, "Icons/Game/BladeIcon");
            ElementFactory.CreateDefaultLabel(playerRow, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter);
            // score
            ElementFactory.CreateDefaultLabel(row, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter);
            // action
            Transform actionRow = ElementFactory.CreateHorizontalGroup(row, 10f, TextAnchor.MiddleCenter).transform;
            ElementFactory.CreateIconButton(actionRow, style, "Icons/Intro/UserIcon", elementWidth: 26f, elementHeight: 26f, onClick: () => OnClickProfile(index));
            ElementFactory.CreateIconButton(actionRow, style, "Icons/Game/VolumeOffIcon", elementWidth: 30f, elementHeight: 30f, onClick: () => OnClickMute(index));
            if (PhotonNetwork.IsMasterClient)
                ElementFactory.CreateIconButton(actionRow, style, "Icons/Navigation/CloseIcon", elementWidth: 24f, elementHeight: 24f, onClick: () => OnClickKick(index));
            foreach (Transform t in row)
                t.GetComponent<LayoutElement>().preferredWidth = GetPanelWidth() / 3f;
            return row;
        }

        private void SetRow(Transform row, Player player)
        {
            // read player props
            string name = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient) + player.GetStringProperty(PlayerProperty.Name);

            if (player.CustomProperties.ContainsKey("Cannoneer")) { name += " [<color=#74B831>CAN</color>]"; }
            if (player.CustomProperties.ContainsKey("Carpenter")) { name += " [<color=#2C84DC>CAR</color>]"; }
            if (player.CustomProperties.ContainsKey("Veteran")) { name += " [<color=#7B31B8>VET</color>]"; }
            if (player.CustomProperties.ContainsKey("Logistician")) { name += " [<color=#DC2C2C>LOG</color>]"; }
            if (player.CustomProperties.ContainsKey("Wagon")) { name += " [<color=#DC2C2C>WAG</color>]"; }

            string status = player.GetStringProperty(PlayerProperty.Status);
            string character = player.GetStringProperty(PlayerProperty.Character);
            string loadout = player.GetStringProperty(PlayerProperty.Loadout);
            List<string> scoreList = new List<string>();
            foreach (string property in new string[] {"Kills", "Deaths", "HighestDamage", "TotalDamage"})
            {
                object value = player.GetCustomProperty(property);
                string str = value != null ? value.ToString() : string.Empty;
                scoreList.Add(str);
            }
            string score = string.Join(" / ", scoreList.ToArray());
            // update status icon
            Transform playerRow = row.GetChild(0);
            RawImage statusImage = playerRow.GetChild(0).GetComponent<RawImage>();
            if (status == PlayerStatus.Spectating)
            {
                statusImage.gameObject.SetActive(true);
                statusImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/SpectateIcon", true);
                statusImage.color = UIManager.GetThemeColor(ThemePanel, "Icon", "SpectateColor");
            }
            else if (status == PlayerStatus.Dead)
            {
                statusImage.gameObject.SetActive(true);
                statusImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Quests/Skull1Icon", true);
                statusImage.color = UIManager.GetThemeColor(ThemePanel, "Icon", "DeadColor");
            }
            else
                statusImage.gameObject.SetActive(false);
            // update loadout icon
            RawImage loadoutImage = playerRow.GetChild(1).GetComponent<RawImage>();
            if (character == PlayerCharacter.Human)
            {
                loadoutImage.color = UIManager.GetThemeColor(ThemePanel, "Icon", "LoadoutHuman");
                if (loadout == HumanLoadout.Blades)
                    loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/BladeIcon", true);
                else if (loadout == HumanLoadout.AHSS)
                    loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/AHSSIcon", true);
                else if (loadout == HumanLoadout.APG)
                    loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/APGIcon", true);
                else if (loadout == HumanLoadout.Thunderspears)
                    loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ThunderspearIcon", true);
            }
            else if (character == PlayerCharacter.Titan)
            {
                loadoutImage.color = UIManager.GetThemeColor(ThemePanel, "Icon", "LoadoutTitan");
                loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/TitanIcon", true);
            }
            else if (character == PlayerCharacter.Shifter)
            {
                loadoutImage.color = UIManager.GetThemeColor(ThemePanel, "Icon", "LoadoutShifter");
                loadoutImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ShifterIcon", true);
            }
            if (status == PlayerStatus.Spectating)
                loadoutImage.gameObject.SetActive(false);
            else
                loadoutImage.gameObject.SetActive(true);

            // update name and score
            playerRow.GetChild(2).GetComponent<Text>().text = name;
            row.GetChild(1).GetComponent<Text>().text = score;

            // update action icons
            Transform actionRow = row.GetChild(2);
            bool isMine = PhotonNetwork.LocalPlayer == player;
            actionRow.GetChild(1).gameObject.SetActive(!isMine);
            if (actionRow.childCount > 2)
                actionRow.GetChild(2).gameObject.SetActive(!isMine);
        }

        private void OnClickProfile(int index)
        {
            var player = _lastPlayers[index];
            ((ScoreboardPopup)Parent)._profilePopup.Show(player);
        }

        private void OnClickKick(int index)
        {
            var player = _lastPlayers[index];
            ((ScoreboardPopup)Parent)._kickPopup.Show("Kick this player?", onConfirm: () => FinishKickPlayer(player));
        }
        
        private void FinishKickPlayer(Player player)
        {
            ChatManager.KickPlayer(player);
        }

        private void OnClickMute(int index)
        {
            var player = _lastPlayers[index];
            ((ScoreboardPopup)Parent)._mutePopup.Show(player);
        }
    }
}
