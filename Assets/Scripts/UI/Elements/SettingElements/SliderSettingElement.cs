using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;

namespace UI
{
    class SliderSettingElement : BaseSettingElement
    {
        protected Slider _slider;
        protected Text _valueLabel;
        protected NumberFormatInfo _formatInfo;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Float,
            SettingType.Int
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight, int decimalPlaces)
        {
            _formatInfo = new NumberFormatInfo();
            _formatInfo.NumberDecimalDigits = decimalPlaces;
            _slider = transform.Find("Slider").GetComponent<Slider>();
            _valueLabel = transform.Find("Value").GetComponent<Text>();
            _settingType = GetSettingType(setting);
            if (_settingType == SettingType.Int)
            {
                _slider.wholeNumbers = true;
                _slider.minValue = ((IntSetting)setting).MinValue;
                _slider.maxValue = ((IntSetting)setting).MaxValue;
            }
            else if (_settingType == SettingType.Float)
            {
                _slider.wholeNumbers = false;
                _slider.minValue = ((FloatSetting)setting).MinValue;
                _slider.maxValue = ((FloatSetting)setting).MaxValue;
            }
            _slider.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            _slider.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            _slider.onValueChanged.AddListener((float value) => OnValueChanged(value));
            _valueLabel.fontSize = style.FontSize;
            _slider.transform.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderBackgroundColor");
            _slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderFillColor");
            _slider.transform.Find("Handle Slide Area/Handle").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderHandleColor");
            _valueLabel.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
            base.Setup(setting, style, title, tooltip);
        }

        protected void OnValueChanged(float value)
        {
            if (_settingType == SettingType.Float)
            {
                if (value >= 0.99f && value <= 1.01f)
                {
                    value = 1f;
                    _slider.value = value;
                }
                ((FloatSetting)_setting).Value = value;
            }
            else if (_settingType == SettingType.Int)
                ((IntSetting)_setting).Value = (int)value;
            UpdateValueLabel();
        }

        protected void UpdateValueLabel()
        {
            if (_settingType == SettingType.Float)
                _valueLabel.text = string.Format(_formatInfo, "{0:N}", _slider.value);
            else if (_settingType == SettingType.Int)
                _valueLabel.text = ((int)_slider.value).ToString();
        }

        public override void SyncElement()
        {
            if (_settingType == SettingType.Float)
                _slider.value = ((FloatSetting)_setting).Value;
            else if (_settingType == SettingType.Int)
                _slider.value = ((IntSetting)_setting).Value;
            UpdateValueLabel();
        }
    }
}
