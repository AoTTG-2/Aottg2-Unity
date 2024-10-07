using Settings;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class KeybindPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Title");
        protected override float Width => 300f;
        protected override float Height => 250f;
        protected override float VerticalSpacing => 15f;
        protected override int VerticalPadding => 30;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        private InputKey _setting;
        private Text _settingLabel;
        private Text _displayLabel;
        private InputKey _buffer;
        private bool _isDone;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Unbind"), onClick: () => OnButtonClick("Unbind"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            ElementFactory.CreateDefaultLabel(SinglePanel, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "CurrentKey") + ":").GetComponent<Text>();
            _displayLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, string.Empty).GetComponent<Text>();
            _buffer = new InputKey();
        }

        private void Update()
        {
            if (_setting != null && !_isDone && _buffer.ReadNextInput())
            {
                _isDone = true;

                // Avoid conflict with button presses
                if (_buffer.ToString() == "Mouse0")
                    StartCoroutine(WaitAndUpdateSetting());
                else
                    UpdateSetting();
            }
        }

        IEnumerator WaitAndUpdateSetting()
        {
            yield return new WaitForEndOfFrame();
            UpdateSetting();
        }

        private void UpdateSetting()
        {
            _setting.LoadFromString(_buffer.ToString());
            _settingLabel.text = _setting.ToString();
            gameObject.SetActive(false);
        }

        public void Show(InputKey setting, Text label)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _setting = setting;
            _settingLabel = label;
            _displayLabel.text = _setting.ToString();
            _isDone = false;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Unbind")
            {
                _setting.LoadFromString(SpecialKey.None.ToString());
                _settingLabel.text = SpecialKey.None.ToString();
            }
            _isDone = true;
            Hide();
        }
    }
}
