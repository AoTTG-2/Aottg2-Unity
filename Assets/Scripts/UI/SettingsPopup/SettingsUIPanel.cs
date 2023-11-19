using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsUIPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "UI";
            UISettings settings = SettingsManager.UISettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, SettingsManager.UISettings.UITheme, UIManager.GetLocale(cat, sub, "Theme"),
                UIManager.GetUIThemes(), elementWidth: 160f, tooltip: UIManager.GetLocaleCommon("RequireRestart"));
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.UIMasterScale, UIManager.GetLocale(cat, sub, "UIScale"), elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.HUDScale, UIManager.GetLocale(cat, sub, "HUDScale"), elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.MinimapScale, UIManager.GetLocale(cat, sub, "MinimapScale"), elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.StylebarScale, UIManager.GetLocale(cat, sub, "StylebarScale"), elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.KillScoreScale, UIManager.GetLocale(cat, sub, "KillScoreScale"), elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, SettingsManager.UISettings.KillFeedScale, UIManager.GetLocale(cat, sub, "KillFeedScale"), elementWidth: 135f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowStylebar, UIManager.GetLocale(cat, sub, "ShowStylebar"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.GameFeed, UIManager.GetLocale(cat, sub, "GameFeed"), tooltip: UIManager.GetLocale(cat, sub, "GameFeedTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.FeedConsole, UIManager.GetLocale(cat, sub, "FeedConsole"), tooltip: UIManager.GetLocale(cat, sub, "FeedConsoleTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowKDR, UIManager.GetLocale(cat, sub, "ShowKDR"), tooltip: UIManager.GetLocale(cat, sub, "ShowKDRTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowPing, UIManager.GetLocale(cat, sub, "ShowPing"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowGameTime, UIManager.GetLocale(cat, sub, "ShowGameTime"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowEmotes, UIManager.GetLocale(cat, sub, "ShowEmotes"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, SettingsManager.UISettings.HighVisibilityNames, UIManager.GetLocale(cat, sub, "HighVisibilityNames"), tooltip: UIManager.GetLocale(cat, sub, "HighVisibilityNamesTooltip"));
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowNames, UIManager.GetLocale(cat, sub, "ShowNames"),
                UIManager.GetLocaleArray(cat, sub, "ShowNamesOptions"), elementWidth: 160f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, SettingsManager.UISettings.ShowHealthbars, UIManager.GetLocale(cat, sub, "ShowHealthbars"),
                UIManager.GetLocaleArray(cat, sub, "ShowHealthbarsOptions"), elementWidth: 160f);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, SettingsManager.UISettings.CrosshairStyle, UIManager.GetLocale(cat, sub, "CrosshairStyle"),
                UIManager.GetLocaleArray(cat, sub, "CrosshairStyleOptions"), elementWidth: 200f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, new ElementStyle(titleWidth: 150f, themePanel: ThemePanel), 
                SettingsManager.UISettings.CrosshairScale, UIManager.GetLocale(cat, sub, "CrosshairScale"), elementWidth: 185f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, new ElementStyle(titleWidth: 150f, themePanel: ThemePanel),
                SettingsManager.UISettings.CrosshairTextScale, UIManager.GetLocale(cat, sub, "CrosshairTextScale"), elementWidth: 185f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.UISettings.ShowCrosshairDistance, UIManager.GetLocale(cat, sub, "ShowCrosshairDistance"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.UISettings.ShowCrosshairArrows, UIManager.GetLocale(cat, sub, "ShowCrosshairArrows"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.CrosshairSkin, UIManager.GetLocale(cat, sub, "CrosshairSkin"), elementWidth: 160f);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, style, SettingsManager.UISettings.Speedometer, UIManager.GetLocale(cat, sub, "Speedometer"),
                UIManager.GetLocaleArray(cat, sub, "SpeedometerOptions"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.UISettings.FadeMainMenu, UIManager.GetLocale(cat, sub, "FadeMainMenu"), tooltip: UIManager.GetLocale(cat, sub, "FadeMainMenuTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.UISettings.ShowInterpolation, UIManager.GetLocale(cat, sub, "ShowInterpolation"), tooltip: UIManager.GetLocale(cat, sub, "ShowInterpolationTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, SettingsManager.UISettings.ShowKeybindTip, UIManager.GetLocale(cat, sub, "ShowKeybindTip"), tooltip: UIManager.GetLocale(cat, sub, "ShowKeybindTooltip"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.ChatWidth, UIManager.GetLocale(cat, sub, "ChatWidth"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.ChatHeight, UIManager.GetLocale(cat, sub, "ChatHeight"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.ChatFontSize, UIManager.GetLocale(cat, sub, "ChatFontSize"), elementWidth: 100f);
        }
    }
}
