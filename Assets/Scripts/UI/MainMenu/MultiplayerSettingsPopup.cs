using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MultiplayerSettingsPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "MultiplayerSettingsPopup", "Title");
        protected override float Width => 480f;
        protected override float Height => 550f;
        protected override bool DoublePanel => false;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "MultiplayerSettingsPopup";
            MultiplayerSettings settings = SettingsManager.MultiplayerSettings;
            float inputWidth = 180f;
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle style = new ElementStyle(titleWidth: 160f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"), onClick: () => OnSaveButtonClick());
            ElementFactory.CreateToggleGroupSetting(SinglePanel, style, settings.LobbyMode, UIManager.GetLocale(cat, sub, "Lobby"),
               UIManager.GetLocaleArray(cat, sub, "LobbyOptions"), tooltip: UIManager.GetLocale(cat, sub, "LobbyTooltip"));
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.CustomLobby, UIManager.GetLocale(cat, sub, "LobbyCustom"), elementWidth: inputWidth);
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateToggleGroupSetting(SinglePanel, style, settings.AppIdMode, UIManager.GetLocale(cat, sub, "AppId"),
                UIManager.GetLocaleArray(cat, sub, "AppIdOptions"), tooltip: UIManager.GetLocale(cat, sub, "AppIdTooltip"));
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.CustomAppId, UIManager.GetLocale(cat, sub, "AppIdCustom"), elementWidth: inputWidth);
        }

        protected void OnSaveButtonClick()
        {
            SettingsManager.MultiplayerSettings.Save();
            Hide();
        }
    }
}
