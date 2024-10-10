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
    class PlayerKDRRow
    {
        #region Fields
        public Transform leftRow;
        public Transform rightRow;
        public GameObject leftGameObject;
        public GameObject rightGameObject;
        public Player player;
        public Text id;
        public Text name;
        public Text score;
        public RawImage weapon;

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
        private string _deadStatus = " <color=red>*dead*</color> ";
        private StringBuilder _scoreBuilder = new StringBuilder();
        #endregion

        public PlayerKDRRow(Transform leftParent, Transform rightParent, ElementStyle style, Player player)
        {
            // NOTE: Added spacing for horizontal group
            leftGameObject = ElementFactory.CreateHorizontalGroup(leftParent, 10f, TextAnchor.MiddleLeft);
            rightGameObject = ElementFactory.CreateHorizontalGroup(rightParent, 10f, TextAnchor.MiddleLeft);
            leftRow = leftGameObject.transform;
            rightRow = rightGameObject.transform;

            // HUD
            id = ElementFactory.CreateDefaultLabel(leftRow, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();      // host, id (0)
            weapon = ElementFactory.CreateRawImage(rightRow, style, "Icons/Game/BladeIcon", 24f, 24f).GetComponent<RawImage>();                                 // loadout/character type   (1)
            name = ElementFactory.CreateDefaultLabel(rightRow, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();    // status, name   (2)
            score = ElementFactory.CreateDefaultLabel(rightRow, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>(); // score    (3)

            // Save player information
            isMasterClient = player.IsMasterClient;
            actorNumber = player.ActorNumber;
            status = string.Empty;
            character = string.Empty;
            loadout = string.Empty;
            team = string.Empty;
            isSet = false;
            this.player = player;

            UpdateRow();
        }

        public void Destroy()
        {
            if (leftGameObject != null)
                GameObject.Destroy(leftGameObject);
            if (rightGameObject != null)
                GameObject.Destroy(rightGameObject);
        }

        public int GetKillDiff()
        {
            return player.GetCustomProperty("Kills") != null ? player.GetIntProperty("Kills") - kills : 0;
        }

        public int GetDeathDiff()
        {
            return player.GetCustomProperty("Deaths") != null ? player.GetIntProperty("Deaths") - deaths : 0;
        }

        public int GetMaxDamageDiff()
        {
            return player.GetCustomProperty("HighestDamage") != null ? player.GetIntProperty("HighestDamage") - maxDamage : 0;
        }

        public int GetTotalDamageDiff()
        {
            return player.GetCustomProperty("TotalDamage") != null ? player.GetIntProperty("TotalDamage") - totalDamage : 0;
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
            if (playerName != name.text || status != this.status)
            {
                this.status = status;
                name.text = GetPlayerStatus(status) + playerName + ": ";
            }

            // Update team
            if (team != player.GetStringProperty(PlayerProperty.Team))
            {
                team = player.GetStringProperty(PlayerProperty.Team);
            }

            // Update if score changes 
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
                _scoreBuilder.Clear();
                for (int i = 0; i < trackedProperties.Length; i++)
                {
                    object value = player.GetCustomProperty(trackedProperties[i]);
                    _scoreBuilder.Append(value != null ? value.ToString() : string.Empty);
                    if (i < trackedProperties.Length - 1)
                    {
                        _scoreBuilder.Append("/");
                    }
                }
                score = _scoreBuilder.ToString();
            }

            if (score != this.score.text)
            {
                this.score.text = score;
            }
        }

        public string GetPlayerStatus(string status)
        {
            if (status == PlayerStatus.Dead || status == PlayerStatus.Spectating)
            {
                return _deadStatus;
            }
            return string.Empty;
        }

        public Texture GetPlayerIcon(string character, string loadout)
        {
            if (character == PlayerCharacter.Human)
            {
                // return based off loadout
                if (loadout == HumanLoadout.Blades)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/BladeIcon", true);
                else if (loadout == HumanLoadout.AHSS)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/AHSSIcon", true);
                else if (loadout == HumanLoadout.APG)
                    return (Texture)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Game/APGIcon", true);
                else if (loadout == HumanLoadout.Thunderspears)
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
