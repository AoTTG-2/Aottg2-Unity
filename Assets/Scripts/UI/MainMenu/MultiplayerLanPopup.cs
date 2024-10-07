using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MultiplayerLanPopup: PromptPopup
    {
        protected override string Title => "LAN";
        protected override float Width => 400f;
        protected override float Height => 370f;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "MultiplayerLanPopup";
            MultiplayerSettings settings = SettingsManager.MultiplayerSettings;
            float elementWidth = 200f;
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle inputStyle = new ElementStyle(titleWidth: 120f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocale(cat, sub, "Connect"), onClick: () => OnButtonClick("Connect"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateInputSetting(SinglePanel, inputStyle, settings.LanIP, "IP", elementWidth: elementWidth);
            ElementFactory.CreateInputSetting(SinglePanel, inputStyle, settings.LanPort, "Port", elementWidth: elementWidth);
            ElementFactory.CreateInputSetting(SinglePanel, inputStyle, settings.LanPassword, "Password (optional)", elementWidth: elementWidth);
        }

        protected void OnButtonClick(string name)
        {
            SettingsManager.MultiplayerSettings.Save();
            if (name == "Connect")
                SettingsManager.MultiplayerSettings.ConnectLAN();
            else if (name == "Back")
                Hide();
        }
    }
}
