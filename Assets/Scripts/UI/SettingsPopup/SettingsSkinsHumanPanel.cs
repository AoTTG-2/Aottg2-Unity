using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsSkinsHumanPanel: SettingsCategoryPanel
    {
        protected override float VerticalSpacing => 20f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsSkinsPanel skinsPanel = (SettingsSkinsPanel)parent;
            SettingsPopup settingsPopup = (SettingsPopup)skinsPanel.Parent;
            HumanCustomSkinSettings settings = SettingsManager.CustomSkinSettings.Human;
            skinsPanel.CreateCommonSettings(DoublePanelLeft, DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: ThemePanel), settings.GasEnabled,
                           UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "GasEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, new ElementStyle(titleWidth: 200f, themePanel: ThemePanel), settings.HookEnabled,
                          UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "HookEnabled"));
            CreateHorizontalDivider(DoublePanelLeft);
            CreateHorizontalDivider(DoublePanelRight);
            skinsPanel.CreateSkinStringSettings(DoublePanelLeft, DoublePanelRight, titleWidth: 200f, elementWidth: 200f, leftCount: 9);
        }
    }
}
