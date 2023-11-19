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
    class ButtonSettingElement : BaseSettingElement
    {
        private Text _label;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.String
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip,
            float elementWidth, float elementHeight, UnityAction onClick)
        {
            GameObject button = transform.Find("Button").gameObject;
            if (elementWidth > 0f)
                button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            if (elementHeight > 0f)
                button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            _label = button.transform.Find("Text").GetComponent<Text>();
            _label.fontSize = style.FontSize;
            _label.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            base.Setup(setting, style, title, tooltip);
        }

        public override void SyncElement()
        {
            _label.text = ((StringSetting)_setting).Value;
        }
    }
}
