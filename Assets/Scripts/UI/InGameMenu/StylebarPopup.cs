using UnityEngine.UI;
using UnityEngine;
using ApplicationManagers;
using Utility;
using NUnit.Framework;
using System.Collections.Generic;

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
        private Image _bladeBackground;
        private Sprite[] _fillSprites;
        private Sprite[] _backgroundSprites;
        private int _rank = 0;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var styleBar = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/StylebarLabel").transform;
            _letterLabel = styleBar.Find("LetterLabel").GetComponent<Text>();
            _sentenceLabel = styleBar.Find("SentenceLabel").GetComponent<Text>();
            _scoreLabel = styleBar.Find("ScoreLabel").GetComponent<Text>();
            _bottomLabel = styleBar.Find("BottomLabel").GetComponent<Text>();
            _bladeFill = styleBar.Find("BladeFill").GetComponent<Image>();
            _bladeBackground = _bladeFill.transform.Find("BladeBackground").GetComponent<Image>();
            _fillSprites = Resources.LoadAll<Sprite>("UI/Sprites/HUD/StyleMeterBarSpriteSheet");
            _backgroundSprites = Resources.LoadAll<Sprite>("UI/Sprites/HUD/StyleMeterSpriteSheet");
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

        public void SetRank(int rank)
        {
            if (rank < 0)
                rank = 0;
            if (rank > 9)
                rank = 9;
            if (rank != _rank)
            {
                _rank = rank;
                _bladeFill.sprite = _fillSprites[_rank];
                _bladeBackground.sprite = _backgroundSprites[_rank];
            }
        }
    }
}
