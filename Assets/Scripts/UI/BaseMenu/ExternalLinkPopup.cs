using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ExternalLinkPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Confirm");
        protected override float Width => 450f;
        protected override float Height => 250f;
        protected override int VerticalPadding => 30;
        protected override int HorizontalPadding => 30;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected float LabelHeight = 60f;
        private Text _label;
        private string _url;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle defaultStyle = new ElementStyle(themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Confirm"),
                onClick: () => OnButtonClick("Confirm"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), 
                onClick: () => OnButtonClick("Cancel"));
            _label = ElementFactory.CreateDefaultLabel(SinglePanel, defaultStyle, string.Empty).GetComponent<Text>();
            _label.GetComponent<LayoutElement>().preferredWidth = GetWidth() - (HorizontalPadding * 2);
            _label.GetComponent<LayoutElement>().preferredHeight = LabelHeight;
            _url = "";
        }

        public void Show(string url)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _label.text = UIManager.GetLocaleCommon("ExternalLinkConfirm") + ": " + url;
            _url = url;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Confirm")
                Application.OpenURL(_url);
            Hide();
        }
    }
}
