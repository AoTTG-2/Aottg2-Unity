using GameManagers;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace UI
{
    class TeamKDRRow : MonoBehaviour
    {
        // UI
        public Text teamText;
        public Text scoreText;

        // stats
        public string team;
        public int kills;
        public int deaths;
        public int maxDamage;
        public int totalDamage;
        public int playerCount;

        // private
        private StringBuilder _scoreBuilder = new StringBuilder();

        public void Setup(ElementStyle style, string team)
        {
            // HUD
            teamText = ElementFactory.CreateDefaultLabel(this.transform, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();      // host, id (0)
            scoreText = ElementFactory.CreateDefaultLabel(this.transform, style, string.Empty, FontStyle.Normal, TextAnchor.MiddleCenter).GetComponent<Text>(); // score    (1)

            // Set text
            teamText.text = team;
            teamText.color = TeamInfo.GetTeamColorUnity(team);
            scoreText.text = string.Empty;

            // Init stats
            ResetStats();
        }

        public void ResetStats()
        {
            kills = 0;
            deaths = 0;
            maxDamage = 0;
            totalDamage = 0;
            playerCount = 0;
        }

        public void RemovePlayerStats(PlayerKDRRow player)
        {
            kills -= player.kills;
            deaths -= player.deaths;
            maxDamage = Math.Max(maxDamage, player.maxDamage);
            totalDamage -= player.totalDamage;
            playerCount--;
        }

        public void AddPlayerStats(PlayerKDRRow player)
        {
            kills += player.kills;
            deaths += player.deaths;
            maxDamage = Math.Max(maxDamage, player.maxDamage);
            totalDamage += player.totalDamage;
            playerCount++;
        }

        public void UpdateRow()
        {
            // Update score
            _scoreBuilder.Clear();
            _scoreBuilder.Append(kills);
            _scoreBuilder.Append("/");
            _scoreBuilder.Append(deaths);
            _scoreBuilder.Append("/");
            _scoreBuilder.Append(maxDamage);
            _scoreBuilder.Append("/");
            _scoreBuilder.Append(totalDamage);
            _scoreBuilder.Append("\t(");
            _scoreBuilder.Append(playerCount);
            _scoreBuilder.Append(")");
            scoreText.text = _scoreBuilder.ToString();
        }
    }
}
