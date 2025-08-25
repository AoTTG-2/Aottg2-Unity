using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ModLoginPopup: PromptPopup
    {
        protected override string Title => "Mod Login";
        protected override float Width => 480f;
        protected override float Height => 200f;
        protected override bool DoublePanel => false;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            MultiplayerSettings settings = SettingsManager.MultiplayerSettings;
            float inputWidth = 180f;
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle style = new ElementStyle(titleWidth: 160f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"), onClick: () => OnSaveButtonClick());
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.ModPassword, "Password", elementWidth: inputWidth);
        }

        protected void OnSaveButtonClick()
        {
            SettingsManager.MultiplayerSettings.Save();
            Hide();
        }
    }
}
