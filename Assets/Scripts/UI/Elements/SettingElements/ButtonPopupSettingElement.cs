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
    class ButtonPopupSettingElement : BaseSettingElement
    {
        private Text _label;
        protected BasePopup _popup;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.String
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, 
            BasePopup popup, string tooltip, float elementWidth, float elementHeight)
        {
            GameObject button = transform.Find("Button").gameObject;
            GameObject text = transform.Find("Button/Text").gameObject;
            if (elementWidth > 0f)
            {
                button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
                text.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            }
            if (elementHeight > 0f)
            {
                button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
                text.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            }
            _popup = popup;
            button.GetComponent<Button>().onClick.AddListener(() => OnClick());
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            _label = button.transform.Find("Text").GetComponent<Text>();
            _label.fontSize = style.FontSize;
            _label.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            base.Setup(setting, style, title, tooltip);
        }

        private void OnClick()
        {
            _popup.Show();
        }

        public override void SyncElement()
        {
            _label.text = ((StringSetting)_setting).Value;
        }
    }
}
