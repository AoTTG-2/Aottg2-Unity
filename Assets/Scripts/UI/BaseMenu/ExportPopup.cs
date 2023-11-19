using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ExportPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Export");
        protected override float Width => 500f;
        protected override float Height => 590f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
        private InputSettingElement _element;
        public StringSetting ExportSetting = new StringSetting(string.Empty);

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Done"), onClick: () => OnButtonClick("Done"));
            _element = ElementFactory.CreateInputSetting(SinglePanel, style, ExportSetting, string.Empty, elementWidth: 460f, elementHeight: 440f, multiLine: true).
                GetComponent<InputSettingElement>();
        }

        public void Show(string value)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            ExportSetting.Value = value;
            _element.SyncElement();
        }

        private void OnButtonClick(string name)
        {
            if (name == "Done")
            {
                Hide();
            }
        }
    }
}
