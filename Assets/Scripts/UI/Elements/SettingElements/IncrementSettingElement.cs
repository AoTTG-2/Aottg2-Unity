using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UI
{
    class IncrementSettingElement : BaseSettingElement
    {
        protected Text _valueLabel;
        protected string[] _options;
        protected UnityAction _onValueChanged;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Int
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, 
            float elementWidth, float elementHeight, string[] options, UnityAction onValueChanged)
        {
            _valueLabel = transform.Find("Increment/ValueLabel").GetComponent<Text>();
            _valueLabel.fontSize = style.FontSize;
            _options = options;
            _onValueChanged = onValueChanged;
            Button leftButton = transform.Find("Increment/LeftButton").GetComponent<Button>();
            Button rightButton = transform.Find("Increment/RightButton").GetComponent<Button>();
            LayoutElement leftLayout = leftButton.GetComponent<LayoutElement>();
            LayoutElement rightLayout = rightButton.GetComponent<LayoutElement>();
            leftButton.onClick.AddListener(() => OnButtonPressed(increment: false));
            rightButton.onClick.AddListener(() => OnButtonPressed(increment: true));
            leftLayout.preferredWidth = rightLayout.preferredWidth = elementWidth;
            leftLayout.preferredHeight = rightLayout.preferredHeight = elementHeight;
            base.Setup(setting, style, title, tooltip);
            leftButton.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            rightButton.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            _valueLabel.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
        }

        protected void OnButtonPressed(bool increment)
        {
            if (_settingType == SettingType.Int)
            {
                if (increment)
                    ((IntSetting)_setting).Value += 1;
                else
                    ((IntSetting)_setting).Value -= 1;
            }
            UpdateValueLabel();
            if (_onValueChanged != null)
                _onValueChanged.Invoke();
        }

        protected void UpdateValueLabel()
        {
            if (_settingType == SettingType.Int)
            {
                if (_options == null)
                    _valueLabel.text = ((IntSetting)_setting).Value.ToString();
                else
                    _valueLabel.text = _options[((IntSetting)_setting).Value];

            }
        }

        public override void SyncElement()
        {
            UpdateValueLabel();
        }
    }
}
