using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

namespace UI
{
    class SetNamePopup: PromptPopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 350f;
        protected override float Height => 230f;
        protected override int VerticalPadding => 8;
        private UnityAction _onSave;
        private InputSettingElement _element;
        private Text _errorText;
        private Func<string, string> _validateName;
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
            ElementStyle errorStyle = new ElementStyle(titleWidth: 0f, fontSize: 18, themePanel: ThemePanel);
            _errorText = ElementFactory.CreateDefaultLabel(SinglePanel, errorStyle, "").GetComponent<Text>();
            _errorText.color = Color.red;
            _errorText.gameObject.SetActive(false);
        }

        public void Show(string initialValue, UnityAction onSave, string title, Func<string, string> validateName = null)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _initialValue = initialValue;
            _onSave = onSave;
            _validateName = validateName;
            SetTitle(title);
            NameSetting.Value = initialValue;
            _element.SyncElement();
            _errorText.gameObject.SetActive(false);
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                string enteredName = NameSetting.Value.Trim();
                if (_validateName != null)
                {
                    string errorMessage = _validateName(enteredName);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        _errorText.text = errorMessage;
                        _errorText.gameObject.SetActive(true);
                        return;
                    }
                }
                string finalName = string.IsNullOrEmpty(enteredName) ? _initialValue : enteredName;
                NameSetting.Value = finalName;
                _onSave.Invoke();
                Hide();
            }
        }
    }
}
