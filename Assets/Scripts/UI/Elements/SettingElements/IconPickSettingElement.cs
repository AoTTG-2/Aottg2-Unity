using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Events;

namespace UI
{
    class IconPickSettingElement : BaseSettingElement
    {
        private Text _label;
        protected string[] _options;
        protected string[] _iconPaths;
        protected UnityAction _onSelect;
        protected IconPickPopup _popup;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.String,
            SettingType.Int
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string[] iconPaths, 
            IconPickPopup iconPickPopup, string tooltip, float elementWidth, float elementHeight, UnityAction onSelect)
        {
            GameObject button = transform.Find("Button").gameObject;
            if (elementWidth > 0f)
                button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            if (elementHeight > 0f)
                button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            _options = options;
            _iconPaths = iconPaths;
            _onSelect = onSelect;
            _popup = iconPickPopup;
            button.GetComponent<Button>().onClick.AddListener(() => OnClick());
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            _label = button.transform.Find("Text").GetComponent<Text>();
            _label.fontSize = style.FontSize;
            _label.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            base.Setup(setting, style, title, tooltip);
        }

        private void OnClick()
        {
            _popup.Show(_setting, _label, _options, _iconPaths, _onSelect);
        }

        public override void SyncElement()
        {
            if (_settingType == SettingType.String)
                _label.text = ((StringSetting)_setting).Value;
            else if (_settingType == SettingType.Int)
                _label.text = _options[((IntSetting)_setting).Value];
        }
    }
}
