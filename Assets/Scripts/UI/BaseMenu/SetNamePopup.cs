using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SetNamePopup: PromptPopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 320f;
        protected override float Height => 230f;
        protected override int VerticalPadding => 40;
        private UnityAction _onSave;
        private InputSettingElement _element;
        public StringSetting NameSetting = new StringSetting(string.Empty);
        private string _initialValue;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _element = ElementFactory.CreateInputSetting(SinglePanel, style, NameSetting, UIManager.GetLocaleCommon("SetName"), elementWidth: 140f).
                GetComponent<InputSettingElement>();
        }

        public void Show(string initialValue, UnityAction onSave, string title)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _initialValue = initialValue;
            _onSave = onSave;
            SetTitle(title);
            NameSetting.Value = initialValue;
            _element.SyncElement();
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                if (NameSetting.Value == string.Empty)
                    NameSetting.Value = _initialValue;
                _onSave.Invoke();
                Hide();
            }
        }
    }
}
