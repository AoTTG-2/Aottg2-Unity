using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    class DropdownSelectElement : DropdownSettingElement
    {
        public override void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string tooltip, float elementWidth, float elementHeight,
            float optionsWidth, float maxScrollHeight, UnityAction onDropdownOptionSelect)
        {
            base.Setup(setting, style, string.Empty, options, tooltip, elementWidth, elementHeight, optionsWidth, maxScrollHeight, onDropdownOptionSelect);
            SetupLabel(_selectedButtonLabel, title, style.FontSize);
        }
        protected override void OnDropdownOptionClick(string option, int index)
        {
            CloseOptions();
            if (_settingType == SettingType.String)
                ((StringSetting)_setting).Value = option;
            else if (_settingType == SettingType.Int)
                ((IntSetting)_setting).Value = index;
            _onDropdownOptionSelect?.Invoke();
        }

        public override void SyncElement()
        {
        }
    }
}
