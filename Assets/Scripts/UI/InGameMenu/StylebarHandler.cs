using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;

namespace UI
{
    class StylebarHandler : MonoBehaviour
    {

        private StylebarPopup _styleBarPopup;
        private string[] Letters = new string[] { "D", "C", "B", "A", "S", "SS", "SSS", "X" };
        private string[] ColorTags = new string[] { "FFFFFF", "ACFA58", "F4FA58", "FAAC58", "FA8258", "BE81F7", "FF0000", "000000" };
        private string[] Sentences = new string[] { "eja Vu", "asual", "oppin!", "mazing!", "ensational!", "pectacular!!", "tylish!!!", "TREEME!!!" };
        private float[] Multipliers = new float[] { 1f, 1.1f, 1.2f, 1.3f, 1.5f, 1.7f, 2f, 2.3f, 2.5f };
        private int[] PointThresholds = new int[] { 350, 950, 0x992, 0x11c6, 0x1b58, 0x3a98, 0x186a0 };
        private int[] PointDecays = new int[] { 1, 2, 5, 10, 15, 20, 0x19, 0x19 };

        private int _hits;
        private float _points;
        private int _rank;
        private bool _lostRank;
        private float _chainTime;
        private int _chainKillRank;

        public void Awake()
        {
            _styleBarPopup = ElementFactory.CreateDefaultPopup<StylebarPopup>(transform);
            ElementFactory.SetAnchor(_styleBarPopup.gameObject, TextAnchor.MiddleRight, TextAnchor.MiddleRight, new Vector2(-20f, 0f));
            _styleBarPopup.gameObject.AddComponent<StylebarScaler>();
            _styleBarPopup.SetFill(1f);
            _styleBarPopup.SetScore("", "");
            _styleBarPopup.SetText("", "");
        }

        public void OnHit(int damage)
        {
            if (!SettingsManager.UISettings.ShowStylebar.Value)
                return;
            if (damage >= 0)
            {
                _points += (int)((damage + 200) * Multipliers[_chainKillRank]);
                _chainKillRank = (_chainKillRank >= (Multipliers.Length - 1)) ? _chainKillRank : (_chainKillRank + 1);
                _chainTime = 5f;
                _hits++;
                UpdateRank();
            }
            else if (_points == 0f)
            {
                _points++;
                UpdateRank();
            }
            string total = ((int)_points).ToString();
            string hits = _hits.ToString() + ((_hits <= 1) ? "Hit" : "Hits") + "\n";
            if (_chainKillRank > 0)
                hits += "x" + Multipliers[_chainKillRank].ToString() + "!";
            _styleBarPopup.SetScore(total, hits);
            UpdateLabels();
            _styleBarPopup.Show();
        }

        private void UpdateRank()
        {
            int currentRank = _rank;
            int index = 0;
            while (index < PointThresholds.Length && _points > PointThresholds[index])
                index++;
            if (index < PointThresholds.Length)
                _rank = index;
            else
                _rank = PointThresholds.Length;
            if (_rank < currentRank)
            {
                if (_lostRank)
                {
                    _points = 0f;
                    _hits = 0;
                    _rank = 0;
                }
                else
                    _lostRank = true;
            }
            else if (_rank > currentRank)
                _lostRank = false;
        }

        private void UpdateLabels()
        {
            string letter = "<color=#" + ColorTags[_rank] + ">" + Letters[_rank] + "</color>";
            string sentence = Sentences[_rank];
            _styleBarPopup.SetText(letter, sentence);
            _styleBarPopup.SetFill(GetRankPercent() * 0.01f);
        }

        private int GetRankPercent()
        {
            if (_rank > 0 && _rank < PointThresholds.Length)
                return (int)(((_points - PointThresholds[_rank - 1]) * 100f) / ((float)(PointThresholds[_rank] - PointThresholds[_rank - 1])));
            if (_rank == 0)
                return (((int)(_points * 100f)) / PointThresholds[_rank]);
            return 100;
        }

        private void Update()
        {
            if (!SettingsManager.UISettings.ShowStylebar.Value)
            {
                _styleBarPopup.Hide();
                _points = 0f;
                return;
            }
            if (_points > 0f)
            {
                _points -= PointDecays[_rank] * Time.deltaTime * 10f;
                UpdateRank();
                UpdateLabels();
            }
            else
                _styleBarPopup.Hide();
            if (_chainTime > 0f)
                _chainTime -= Time.deltaTime;
            else
            {
                _chainTime = 0f;
                _chainKillRank = 0;
            }
        }
    }
}
