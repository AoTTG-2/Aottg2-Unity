using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using Utility;

namespace UI
{
    class ToggleGroupSettingElement : BaseSettingElement
    {
        protected ToggleGroup _toggleGroup;
        protected GameObject _optionsPanel;
        protected string[] _options;
        protected List<Toggle> _toggles = new List<Toggle>();
        private float _checkMarkSizeMultiplier = 0.67f;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.String,
            SettingType.Int
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string tooltip,
            float elementWidth, float elementHeight)
        {
            if (options.Length == 0)
                throw new ArgumentException("ToggleGroup cannot have 0 options.");
            _options = options;
            _optionsPanel = transform.Find("Options").gameObject;
            _toggleGroup = _optionsPanel.GetComponent<ToggleGroup>();
            for (int i = 0; i < options.Length; i++)
            {
                _toggles.Add(CreateOptionToggle(options[i], i, style, elementWidth, elementHeight));
            }
            gameObject.transform.Find("Label").GetComponent<LayoutElement>().preferredHeight = elementHeight;
            base.Setup(setting, style, title, tooltip);
        }

        protected Toggle CreateOptionToggle(string option, int index, ElementStyle style, float width, float height)
        {
            GameObject optionToggle = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Elements/ToggleGroupOption");
            optionToggle.transform.SetParent(_optionsPanel.transform, false);
            optionToggle.transform.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
            SetupLabel(optionToggle.transform.Find("Label").gameObject, option, style.FontSize);
            LayoutElement background = optionToggle.transform.Find("Background").GetComponent<LayoutElement>();
            RectTransform checkmark = background.transform.Find("Checkmark").GetComponent<RectTransform>();
            background.preferredWidth = width;
            background.preferredHeight = height;
            checkmark.sizeDelta = new Vector2(width * _checkMarkSizeMultiplier, height * _checkMarkSizeMultiplier);
            Toggle toggle = optionToggle.GetComponent<Toggle>();
            toggle.group = _toggleGroup;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((bool value) => OnValueChanged(option, index, value));
            toggle.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
            checkmark.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
            return toggle;
        }

        protected void OnValueChanged(string option, int index, bool value)
        {
            if (value)
            {
                if (_settingType == SettingType.String)
                    ((StringSetting)_setting).Value = option;
                else if (_settingType == SettingType.Int)
                    ((IntSetting)_setting).Value = index;
            }
        }

        public override void SyncElement()
        {
            _toggleGroup.SetAllTogglesOff();
            if (_settingType == SettingType.String)
            {
                int index = FindOptionIndex(((StringSetting)_setting).Value);
                _toggles[index].isOn = true;
            }
            else if (_settingType == SettingType.Int)
                _toggles[((IntSetting)_setting).Value].isOn = true;
        }

        private int FindOptionIndex(string option)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                if (_options[i] == option)
                    return i;
            }
            throw new ArgumentOutOfRangeException("Option not found");
        }
    }
}
