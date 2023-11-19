using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class GlobalPauseGamePopup: BasePopup
    {
        protected override string Title => "Game Paused";
        protected override string ThemePanel => "LoadingPanel";
        protected override float Width => 320f;
        protected override float Height => 220f;
        protected override float TopBarHeight => 55f;
        protected override float BottomBarHeight => 55f;
        protected override int TitleFontSize => 26;
        protected override int ButtonFontSize => 22;
        protected override int VerticalPadding => 30;
        protected override int HorizontalPadding => 30;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float AnimationTime => 0.2f;
        private Text _label;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle defaultStyle = new ElementStyle(themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Quit"), onClick: () => OnButtonClick("Quit"));
            _label = ElementFactory.CreateDefaultLabel(SinglePanel, defaultStyle, "").GetComponent<Text>();
            _label.color = UIManager.GetThemeColor(ThemePanel, "DefaultLabel", "TextColor");
            _label.GetComponent<LayoutElement>().preferredWidth = GetWidth() - (HorizontalPadding * 2);
            _label.GetComponent<LayoutElement>().preferredHeight = 40f;
        }

        public void SetLabel(string label)
        {
            _label.text = label;
        }

        private void OnButtonClick(string name)
        {
            InGameManager.LeaveRoom();
        }
    }
}
