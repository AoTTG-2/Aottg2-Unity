using UnityEngine;
using UnityEngine.UI;
using GameManagers;
using GameProgress;
using ApplicationManagers;
using Utility;

namespace UI
{
    class KillFeedBigPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 0f;
        protected override float Height => 0f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Tween;
        protected override float AnimationTime => 0.2f;
        private Text _leftLabel;
        private Text _rightLabel;
        private Text _scoreLabel;
        private Text _backgroundLabel;
        private RawImage _image;
        public float TimeLeft;
        public string Killer;
        public string Victim;
        public int Score;
        public string Weapon;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var go = ElementFactory.InstantiateAndBind(transform, "Prefabs/InGame/KillFeedLabelBig");
            _leftLabel = go.transform.Find("LeftLabel").GetComponent<Text>();
            _rightLabel = go.transform.Find("RightLabel").GetComponent<Text>();
            _scoreLabel = go.transform.Find("ScoreLabel").GetComponent<Text>();
            _backgroundLabel = go.transform.Find("ScoreLabel/BackgroundLabel").GetComponent<Text>();
            _image = go.GetComponent<RawImage>();
            go.transform.parent.Find("Border").gameObject.SetActive(false);
        }

        public void Show(string killer, string victim, int score, string weapon)
        {
            Killer = killer;
            Victim = victim;
            Score = score;
            Weapon = weapon;
            _image.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, GetWeaponIcon(weapon), true);
            _leftLabel.text = killer;
            _rightLabel.text = victim;
            _scoreLabel.text = score.ToString();
            _backgroundLabel.text = score.ToString();
            if (score >= 1000)
                _backgroundLabel.color = Color.red;
            else
                _backgroundLabel.color = Color.white;
            IsActive = false;
            TimeLeft = 8f + AnimationTime;
            base.Show();
        }

        private string GetWeaponIcon(string weapon)
        {
            if (weapon == "AHSS")
                return "Icons/Game/AHSSIcon";
            else if (weapon == "APG")
                return "Icons/Game/APGIcon";
            else if (weapon == "Thunderspear")
                return "Icons/Game/ThunderspearIcon";
            else if (weapon.StartsWith("Titan"))
                return "Icons/Game/TitanIcon";
            else if (weapon.StartsWith("Shifter"))
                return "Icons/Game/ShifterIcon";
            return "Icons/Game/KillFeedIcon";
        }
    }
}
