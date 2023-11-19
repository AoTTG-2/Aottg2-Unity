using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    class StylebarPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 0f;
        protected override float Height => 0f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float AnimationTime => 0.2f;
        private Text _letterLabel;
        private Text _sentenceLabel;
        private Text _scoreLabel;
        private Text _bottomLabel;
        private Image _bladeFill;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var styleBar = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/StylebarLabel").transform;
            _letterLabel = styleBar.Find("LetterLabel").GetComponent<Text>();
            _sentenceLabel = styleBar.Find("SentenceLabel").GetComponent<Text>();
            _scoreLabel = styleBar.Find("ScoreLabel").GetComponent<Text>();
            _bottomLabel = styleBar.Find("BottomLabel").GetComponent<Text>();
            _bladeFill = styleBar.Find("BladeFill").GetComponent<Image>();
        }

        public void SetText(string letter, string sentence)
        {
            _letterLabel.text = letter;
            _sentenceLabel.text = sentence;
        }

        public void SetScore(string score, string bottom)
        {
            _scoreLabel.text = score;
            _bottomLabel.text = bottom;
        }

        public void SetFill(float fill)
        {
            _bladeFill.fillAmount = fill;
        }
    }
}
