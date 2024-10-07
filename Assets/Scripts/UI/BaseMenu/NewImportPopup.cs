using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI
{
    class NewImportPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("New");
        protected override float Width => 500f;
        protected override float Height => 615f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
        protected override float VerticalSpacing => 10f;
        protected float DefaultInputHeight => 380f;
        private UnityAction _onSave;
        private InputSettingElement _nameElement;
        private InputSettingElement _importElement;
        private Text _errorText;
        public StringSetting ImportSetting = new StringSetting(string.Empty);
        public StringSetting FileName = new StringSetting(string.Empty);
        private bool _manualHide;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel, titleWidth: 100F);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _nameElement = ElementFactory.CreateInputSetting(SinglePanel, style, FileName, UIManager.GetLocaleCommon("Name"), elementWidth: 460f)
                .GetComponent<InputSettingElement>();
            _importElement = ElementFactory.CreateInputSetting(SinglePanel, style, ImportSetting, string.Empty, elementWidth: 460f, 
                elementHeight: DefaultInputHeight, multiLine: true).GetComponent<InputSettingElement>();
            _errorText = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
            _errorText.color = Color.red;
        }

        public void Show(UnityAction onSave, bool manualHide = true)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _onSave = onSave;
            _manualHide = manualHide;
            ImportSetting.Value = string.Empty;
            FileName.Value = string.Empty;
            _errorText.gameObject.SetActive(false);
            SyncSettingElements();
        }

        public void ShowError(string error)
        {
            _errorText.text = error;
            _errorText.gameObject.SetActive(true);
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
