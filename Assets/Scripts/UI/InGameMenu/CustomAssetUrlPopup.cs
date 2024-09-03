using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class CustomAssetUrlPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Confirm");
        protected override float Width => 450f;
        protected override float Height => 250f;
        protected override int VerticalPadding => 30;
        protected override int HorizontalPadding => 30;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected float LabelHeight = 60f;
        private Text _label;
        public bool Done = false;
        public bool Confirmed = false;

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
        }

        public void Show(string url)
        {
            base.Show();
            Done = false;
            Confirmed = false;
            _label.supportRichText = false;
            _label.text = "You are about to download a file: " + url;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Confirm")
            {
                Confirmed = true;
            }
            Done = true;
            Hide();
        }
    }
}
