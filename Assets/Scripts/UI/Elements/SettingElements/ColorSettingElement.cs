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
    class ColorSettingElement : BaseSettingElement
    {
        private Image _image;
        private ColorPickPopup _colorPickPopup;
        private UnityAction _onChangeColor;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Color
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, ColorPickPopup colorPickPopup, string tooltip, 
            float elementWidth, float elementHeight, UnityAction onChangeColor)
        {
            _colorPickPopup = colorPickPopup;
            GameObject button = transform.Find("ColorButton").gameObject;
            button.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            button.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked());
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
            _image = button.transform.Find("Border/Image").GetComponent<Image>();
            _onChangeColor = onChangeColor;
            base.Setup(setting, style, title, tooltip);
        }

        protected void OnButtonClicked()
        {
            _colorPickPopup.Show(((ColorSetting)_setting), _image, _onChangeColor);
        }

        public override void SyncElement()
        {
            _image.color = ((ColorSetting)_setting).Value.ToColor();
        }
    }
}
