using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI
{
    class ImportPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Import");
        protected override float Width => 500f;
        protected override float Height => 585f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
        protected override float VerticalSpacing => 10f;
        protected float DefaultInputHeight => 430f;
        protected float TextHeight => 32f;
        private UnityAction _onSave;
        private InputSettingElement _element;
        private Text _errorText;
        private Text _topText;
        public StringSetting ImportSetting = new StringSetting(string.Empty);
        private bool _manualHide;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _topText = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
            _element = ElementFactory.CreateInputSetting(SinglePanel, style, ImportSetting, string.Empty, elementWidth: 460f, 
                elementHeight: DefaultInputHeight, multiLine: true).
                GetComponent<InputSettingElement>();
            _errorText = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
            _errorText.color = Color.red;
        }

        public void Show(UnityAction onSave, bool manualHide = true, string topText = "")
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _onSave = onSave;
            _manualHide = manualHide;
            ImportSetting.Value = string.Empty;
            _errorText.gameObject.SetActive(false);
            if (topText == "")
            {
                _topText.gameObject.SetActive(false);
                _element.GetComponent<LayoutElement>().preferredHeight = DefaultInputHeight;
            }
            else
            {
                _topText.gameObject.SetActive(true);
                _topText.text = topText;
                _element.GetComponent<LayoutElement>().preferredHeight = DefaultInputHeight - TextHeight;
            }
            _element.SyncElement();
        }

        public void ShowError(string error)
        {
            _errorText.text = error;
            _errorText.gameObject.SetActive(true);
            if (_topText.gameObject.activeSelf)
                _element.GetComponent<LayoutElement>().preferredHeight = DefaultInputHeight - TextHeight * 2;
            else
                _element.GetComponent<LayoutElement>().preferredHeight = DefaultInputHeight - TextHeight;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                _onSave.Invoke();
                if (!_manualHide)
                    Hide();
            }
        }
    }
}
