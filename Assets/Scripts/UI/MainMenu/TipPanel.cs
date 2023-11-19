using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSONFixed;

namespace UI
{
    class TipPanel: MonoBehaviour
    {
        private Text _label;
        private int currentTipIndex = -1;

        public void Setup()
        {
            _label = transform.Find("Label").GetComponent<Text>();
        }

        public void SetRandomTip()
        {
            var tips = MainMenu.MainBackgroundInfo["Tips"];
            int tipIndex = currentTipIndex;
            while (tipIndex == currentTipIndex)
                tipIndex = Random.Range(0, tips.Count);
            currentTipIndex = tipIndex;
            _label.text = "Tip: " + tips[tipIndex].Value;
        }
    }
}
