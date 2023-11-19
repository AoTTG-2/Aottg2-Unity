using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using Utility;

namespace UI
{
    class KeybindSettingElement : BaseSettingElement
    {
        private List<Text> _buttonLabels = new List<Text>();
        private KeybindPopup _keybindPopup;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.Keybind
        };

        public void Setup(BaseSetting setting, ElementStyle style, string title, KeybindPopup keybindPopup, string tooltip, float elementWidth, 
            float elementHeight, int bindCount)
        {
            _keybindPopup = keybindPopup;
            for (int i = 0; i < bindCount; i++)
                CreateKeybindButton(i, style, elementWidth, elementHeight);
            base.Setup(setting, style, title, tooltip);
        }

        private void CreateKeybindButton(int index, ElementStyle style, float width, float height)
        {
            GameObject keybindButton = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/elements/KeybindButton");
            Text text = keybindButton.transform.Find("Text").GetComponent<Text>();
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "KeybindTextColor");
            text.fontSize = style.FontSize;
            keybindButton.GetComponent<LayoutElement>().preferredWidth = width;
            keybindButton.GetComponent<LayoutElement>().preferredHeight = height;
            keybindButton.transform.SetParent(transform, false);
            keybindButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(index));
            keybindButton.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Keybind");
            _buttonLabels.Add(text);
        }

        protected void OnButtonClicked(int index)
        {
            _keybindPopup.Show(((KeybindSetting)_setting).InputKeys[index], _buttonLabels[index]);
        }

        public override void SyncElement()
        {
            for (int i = 0; i < _buttonLabels.Count; i++)
                _buttonLabels[i].text = ((KeybindSetting)_setting).InputKeys[i].ToString();
        }
    }
}
