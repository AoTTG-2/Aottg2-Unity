using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UI
{
    class IncrementSettingElement : BaseSettingElement
    {
        protected Text _valueLabel;
        protected string[] _options;
        protected UnityAction _onValueChanged;
        protected Func<bool> _validation;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Int
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, 
            float elementWidth, float elementHeight, string[] options, UnityAction onValueChanged, Func<bool> validation = null)
        {
            _valueLabel = transform.Find("Increment/ValueLabel").GetComponent<Text>();
            _valueLabel.fontSize = style.FontSize;
            _options = options;
            _onValueChanged = onValueChanged;
            _validation = validation;
            Button leftButton = transform.Find("Increment/LeftButton").GetComponent<Button>();
            Button rightButton = transform.Find("Increment/RightButton").GetComponent<Button>();
            LayoutElement leftLayout = leftButton.GetComponent<LayoutElement>();
            LayoutElement rightLayout = rightButton.GetComponent<LayoutElement>();

            var trigger = leftButton.gameObject.AddComponent<HoldableButton>();
            trigger.OnClick += () => OnButtonPressed(increment: false);

            trigger = rightButton.gameObject.AddComponent<HoldableButton>();
            trigger.OnClick += () => OnButtonPressed(increment: true);

        leftLayout.preferredWidth = rightLayout.preferredWidth = elementWidth;
        leftLayout.preferredHeight = rightLayout.preferredHeight = elementHeight;
        // Prevent value label from stretching/scaling to fit buttons
        var valueLabelLayout = _valueLabel.GetComponent<LayoutElement>();
        if (valueLabelLayout != null)
        {
            valueLabelLayout.flexibleWidth = 0;
            valueLabelLayout.minWidth = -1;
            valueLabelLayout.preferredWidth = -1; // Let label use its preferred width
        }
        base.Setup(setting, style, title, tooltip);
        leftButton.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
        rightButton.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
        _valueLabel.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
        }

        protected void OnButtonPressed(bool increment)
        {
            if (_settingType == SettingType.Int)
            {
                var intSetting = (IntSetting)_setting;
                int previousValue = intSetting.Value;
                
                if (increment)
                    intSetting.Value++;
                else
                    intSetting.Value--;
                
                // If validation exists and fails, revert to previous value
                if (_validation != null && !_validation())
                {
                    intSetting.Value = previousValue;
                }
            }
            UpdateValueLabel();
            _onValueChanged?.Invoke();
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
