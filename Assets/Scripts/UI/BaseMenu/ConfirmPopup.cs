using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ConfirmPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Confirm");
        protected override float Width => 300f;
        protected override float Height => 240f;
        protected override int VerticalPadding => 30;
        protected override int HorizontalPadding => 30;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected float LabelHeight = 60f;
        private Text _label;
        private UnityAction _onConfirm;

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

        public void Show(string message, UnityAction onConfirm, string title = null)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _label.text = message;
            _onConfirm = onConfirm;
            if (title != null)
                SetTitle(title);
            else
                SetTitle(Title);
        }

        private void OnButtonClick(string name)
        {
            if (name == "Confirm")
                _onConfirm.Invoke();
            Hide();
        }
    }
}
