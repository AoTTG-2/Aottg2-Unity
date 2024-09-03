using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class TooltipPopup : BasePopup
    {
        protected override float AnimationTime => 0.15f;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        private Text _label;
        private RectTransform _panel;
        public Button Caller;
        private float _offset;

        public override void Setup(BasePanel parent = null)
        {
            _label = transform.Find("Panel/Label").GetComponent<Text>();
            _label.text = string.Empty;
            _panel = transform.Find("Panel").GetComponent<RectTransform>();
            _label.color = UIManager.GetThemeColor(ThemePanel, "DefaultSetting", "TooltipTextColor");
            _panel.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "DefaultSetting", "TooltipBackgroundColor");
        }

        public void Show(string message, Button caller, float offset)
        {
            if (gameObject.activeSelf)
            {
                StopAllCoroutines();
                SetTransformAlpha(MaxFadeAlpha);
            }
            _label.text = message;
            Caller = caller;
            _offset = offset;
            Canvas.ForceUpdateCanvases();
            SetTooltipPosition();
            base.Show();
        }

        private void SetTooltipPosition()
        {
            float width = GetComponent<RectTransform>().sizeDelta.x;
            float offset = (width * 0.5f + _offset) * UIManager.CurrentCanvasScale;
            Vector3 position = Caller.transform.position;
            if (position.x + offset > Screen.width)
                position.x -= offset;
            else
                position.x += offset;
            transform.position = position;
        }

        private void Update()
        {
            if (Caller != null)
                SetTooltipPosition();
        }
    }
}
