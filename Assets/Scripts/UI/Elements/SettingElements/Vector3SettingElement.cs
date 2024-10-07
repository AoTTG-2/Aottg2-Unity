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
    class Vector3SettingElement : BaseSettingElement
    {
        private Text _text;
        private Vector3Popup _vector3Popup;
        private UnityAction _onChangeVector;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Vector3
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, Vector3Popup vector3Popup, string tooltip, 
            float elementWidth, float elementHeight, UnityAction onChangeVector)
        {
            _vector3Popup = vector3Popup;
            GameObject button = transform.Find("Vector3Button").gameObject;
            button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked());
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            _text = button.transform.Find("Text").GetComponent<Text>();
            _text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            _text.fontSize = style.FontSize;
            _onChangeVector = onChangeVector;
            base.Setup(setting, style, title, tooltip);
        }

        protected void OnButtonClicked()
        {
            _vector3Popup.Show(((Vector3Setting)_setting), _text, _onChangeVector);
        }

        public override void SyncElement()
        {
            _text.text = ((Vector3Setting)_setting).Value.ToDisplayString();
        }
    }
}
