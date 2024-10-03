using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSONFixed;
using DentedPixel;

namespace UI
{
    class TipPanel : MonoBehaviour
    {
        private Text _label;
        private int currentTipIndex = -1;
        private RectTransform _labelRectTransform;
        private const float AnimationDuration = 0.5f;

        public void Setup()
        {
            _label = transform.Find("Label").GetComponent<Text>();
            _labelRectTransform = _label.GetComponent<RectTransform>();
        }

        public void SetRandomTip()
        {
            LeanTween.scale(_labelRectTransform, Vector3.zero, AnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    ChangeTextContent();
                    LeanTween.scale(_labelRectTransform, Vector3.one, AnimationDuration)
                        .setEase(LeanTweenType.easeInOutQuad);
                });
        }

        private void ChangeTextContent()
        {
            var tips = MainMenu.MainBackgroundInfo["Tips"];
            int tipIndex = currentTipIndex;
            while (tipIndex == currentTipIndex)
                tipIndex = Random.Range(0, tips.Count);
            currentTipIndex = tipIndex;
            _label.text = "Tip: " + tips[tipIndex].Value;
        }

        public void SetPressAnyKey()
        {
            LeanTween.scale(_labelRectTransform, Vector3.zero, AnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    _label.text = UIManager.GetLocaleCommon("PressAnyKey");
                    LeanTween.scale(_labelRectTransform, Vector3.one, AnimationDuration)
                        .setEase(LeanTweenType.easeInOutQuad);
                });
        }
    }
}