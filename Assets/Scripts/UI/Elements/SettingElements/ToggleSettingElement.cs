using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UI
{
    class ToggleSettingElement : BaseSettingElement
    {
        protected Toggle _toggle;
        private float _checkMarkSizeMultiplier = 0.66f;
        private UnityAction _onValueChanged;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Bool
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight,
            UnityAction onValueChanged)
        {
            _onValueChanged = onValueChanged;
            _toggle = transform.Find("Toggle").GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener((bool value) => OnValueChanged(value));
            LayoutElement background = _toggle.transform.Find("Background").GetComponent<LayoutElement>();
            RectTransform checkmark = background.transform.Find("Checkmark").GetComponent<RectTransform>();
            background.preferredHeight = elementHeight;
            background.preferredWidth = elementWidth;
            checkmark.sizeDelta = new Vector2(elementWidth * _checkMarkSizeMultiplier, elementHeight * _checkMarkSizeMultiplier);
            base.Setup(setting, style, title, tooltip);
            checkmark.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
            _toggle.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
        }

        protected void OnValueChanged(bool value)
        {
            ((BoolSetting)_setting).Value = value;
            if (_onValueChanged != null)
                _onValueChanged.Invoke();
        }

        public override void SyncElement()
        {
            _toggle.isOn = ((BoolSetting)_setting).Value;
        }
    }
}
