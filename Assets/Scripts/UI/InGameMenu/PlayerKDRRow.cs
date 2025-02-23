using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using GameManagers;
using Utility;
using ApplicationManagers;
using CustomLogic;
using Discord;

namespace UI
{
    class PlayerKDRRow : MonoBehaviour
    {
        #region Fields
        public Player player;
        public Text id;
        public RawImage weapon;
        public Text playerName;
        public Text score;

        // state
        public bool isSet;
        public bool isMasterClient;
        public int actorNumber;
        public string status;
        public string character;
        public string loadout;
        public string team;

        // stats
        public int kills;
        public int deaths;
        public int maxDamage;
        public int totalDamage;

        // private
        private string[] trackedProperties = new string[] { "Kills", "Deaths", "HighestDamage", "TotalDamage" };
        private const string _deadStatus = " <color=red>*dead*</color> ";
        private StringBuilder _scoreBuilder = new StringBuilder();
        private KDRPanel _kdrPanel;

        #endregion

        public void Setup(ElementStyle style, Player player, KDRPanel panel)
        {
            // HUD
            id = ElementFactory.CreateDefaultLabel(this.transform, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();      // host, id (0)
            weapon = ElementFactory.CreateRawImage(this.transform, style, "Icons/Game/BladeIcon", 24f, 24f).GetComponent<RawImage>();                                 // loadout/character type   (1)
            playerName = ElementFactory.CreateDefaultLabel(this.transform, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();    // status, name   (2)
            score = ElementFactory.CreateDefaultLabel(this.transform, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>(); // score    (3)

            // Save player information
            isMasterClient = player.IsMasterClient;
            actorNumber = player.ActorNumber;
            status = string.Empty;
            character = string.Empty;
            loadout = string.Empty;
            team = string.Empty;
            isSet = false;
            this.player = player;
            _kdrPanel = panel;
            UpdateRow();
        }

        public bool StatsChanged()
        {
            int kills = player.GetIntProperty("Kills", 0);
            int deaths = player.GetIntProperty("Deaths", 0);
            int maxDamage = player.GetIntProperty("HighestDamage", 0);
            int totalDamage = player.GetIntProperty("TotalDamage", 0);

            return StatsChanged(kills, deaths, maxDamage, totalDamage);
        }

        public bool StatsChanged(int kills, int deaths, int maxDamage, int totalDamage)
        {
            return kills != this.kills || deaths != this.deaths || maxDamage != this.maxDamage || totalDamage != this.totalDamage;
        }

        public int GetKillDiff()
        {
            return player.GetIntProperty("Kills", 0) - kills;
        }

        public int GetDeathDiff()
        {
            return player.GetIntProperty("Deaths", 0) - deaths;
        }

        public int GetMaxDamageDiff()
        {
            return player.GetIntProperty("HighestDamage", 0) - maxDamage;
        }

        public int GetTotalDamageDiff()
        {
            return player.GetIntProperty("TotalDamage", 0) - totalDamage;
        }

        public void UpdateRow(Player player)
        {
            this.player = player;
            UpdateRow();
        }

        /// <summary>
        /// Diff each element in the horizontal group and update if the values changed.
        /// </summary>
        /// <param name="player"></param>
        public void UpdateRow()
        {
            if (player == null)
                return;
            string playerName = player.GetStringProperty(PlayerProperty.Name);
            string status = player.GetStringProperty(PlayerProperty.Status);
            string character = player.GetStringProperty(PlayerProperty.Character);
            string loadout = player.GetStringProperty(PlayerProperty.Loadout);

            // Update if masterclient or id changed.
            if (isMasterClient != player.IsMasterClient || actorNumber != player.ActorNumber || isSet == false)
            {
                isMasterClient = player.IsMasterClient;
                actorNumber = player.ActorNumber;
                isSet = true;
                id.text = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient, player.IsLocal);
            }

            // Update if character changed or loadout changed.
            if (character != this.character || loadout != this.loadout)
            {
                this.character = character;
                this.loadout = loadout;
                weapon.texture = GetPlayerIcon(character, loadout);
            }

            // Update if name or status changed.
            if (playerName != this.playerName.text || status != this.status)
            {
                this.status = status;
                this.playerName.text = GetPlayerStatus(status) + playerName + ": ";
            }

            // Update team
            if (team != player.GetStringProperty(PlayerProperty.Team))
            {
                team = player.GetStringProperty(PlayerProperty.Team);
            }

            // Get Stats
            int kills = player.GetIntProperty("Kills", 0);
            int deaths = player.GetIntProperty("Deaths", 0);
            int maxDamage = player.GetIntProperty("HighestDamage", 0);
            int totalDamage = player.GetIntProperty("TotalDamage", 0);

            // Update if score changes 
            if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.ScoreboardProperty != string.Empty)
            {
                string score = string.Empty;
                var property = player.GetCustomProperty(CustomLogicManager.Evaluator.ScoreboardProperty);
                if (property != null)
                    score = property.ToString();                    
                if (score != this.score.text)
                {
                    this.score.text = score;
                }
            }
            else
            {
                _scoreBuilder.Clear();
                for (int i = 0; i < trackedProperties.Length; i++)
                {
                    int value = player.GetIntProperty(trackedProperties[i], 0);
                    _scoreBuilder.Append(value.ToString());
                    if (i < trackedProperties.Length - 1)
                    {
                        _scoreBuilder.Append("/");
                    }
                }
                this.score.text = _scoreBuilder.ToString();

                // Update stats
                this.kills = kills;
                this.deaths = deaths;
                this.maxDamage = maxDamage;
                this.totalDamage = totalDamage;
            }
        }

        public string GetPlayerStatus(string status)
        {
            if (_kdrPanel._showScoreboardStatus && (status == PlayerStatus.Dead || status == PlayerStatus.Spectating))
                return _deadStatus;
            return string.Empty;
        }

        public Texture GetPlayerIcon(string character, string loadout)
        {
            if (!_kdrPanel._showScoreboardLoadout)
                return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Specials/NoneSpecialIcon", true);
            if (character == PlayerCharacter.Human)
            {
                // return based off loadout
                if (loadout == HumanLoadout.Blade)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/BladeIcon", true);
                else if (loadout == HumanLoadout.AHSS)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/AHSSIcon", true);
                else if (loadout == HumanLoadout.APG)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/APGIcon", true);
                else if (loadout == HumanLoadout.Thunderspear)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ThunderSpearIcon", true);
            }
            else
            {
                // return based off character
                if (character == PlayerCharacter.Titan)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/TitanIcon", true);
                else if (character == PlayerCharacter.Shifter)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/ShifterIcon", true);
            }

            return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/BladeIcon", true);
        }

        public Texture GetPlayerIconFull(string character, string loadout, string status)
        {
            if (status == PlayerStatus.Dead)
            {
                return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Quests/Skull1Icon", true);
            }
            else if (status == PlayerStatus.Spectating)
            {
                return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/SpectatingIcon", true);
            }
            return GetPlayerIcon(character, loadout);
        }
    }
}
