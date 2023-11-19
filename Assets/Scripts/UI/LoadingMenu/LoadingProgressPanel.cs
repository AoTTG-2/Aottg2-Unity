using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class LoadingProgressPanel: BasePopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Loading");
        protected override string ThemePanel => "LoadingPanel";
        protected override float Width => 340f;
        protected override float Height => 180f;
        protected override float TopBarHeight => 55f;
        protected override float BottomBarHeight => 55f;
        protected override int TitleFontSize => 26;
        protected override int ButtonFontSize => 22;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 0;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float AnimationTime => 0.2f;
        protected float SliderWidth = 200f;
        protected float SliderHeight = 30f;
        private Text _label;
        private Slider _slider;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle defaultStyle = new ElementStyle(themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Quit"), onClick: () => OnButtonClick("Quit"));
            GameObject slider = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/InGame/LoadingSlider");
            _label = slider.transform.Find("Value").GetComponent<Text>();
            _slider = slider.transform.Find("Slider").GetComponent<Slider>();
            _slider.GetComponent<LayoutElement>().preferredWidth = SliderWidth;
            _slider.GetComponent<LayoutElement>().preferredHeight = SliderHeight;
            _slider.transform.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "ProgressBar", "ProgressBarBackgroundColor");
            _slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor(ThemePanel, "ProgressBar", "ProgressBarFillColor");
            _slider.interactable = false;
            _label.color = UIManager.GetThemeColor(ThemePanel, "DefaultLabel", "TextColor");
        }

        public void Show(float progress)
        {
            _label.text = Util.FormatFloat(progress * 100, 0).ToString() + "%";
            _slider.value = progress;
            base.Show();
        }

        private void OnButtonClick(string name)
        {
            InGameManager.LeaveRoom();
        }
    }
}
