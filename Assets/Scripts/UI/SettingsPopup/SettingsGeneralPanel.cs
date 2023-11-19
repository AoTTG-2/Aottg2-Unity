using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGeneralPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "General";
            GeneralSettings settings = SettingsManager.GeneralSettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.Language, "Language", UIManager.GetLanguages(),
                elementWidth: 160f, onDropdownOptionSelect: () => settingsPopup.RebuildCategoryPanel(), tooltip: UIManager.GetLocaleCommon("RequireRestart"));
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.CameraMode, UIManager.GetLocale(cat, sub, "CameraMode"),
                 new string[] { "TPS", "Original" }, elementWidth: 200f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraDistance, UIManager.GetLocale(cat, sub, "CameraDistance"),
               elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraHeight, UIManager.GetLocale(cat, sub, "CameraHeight"),
               elementWidth: 135f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CameraTilt, UIManager.GetLocale(cat, sub, "CameraTilt"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FOVMin, UIManager.GetLocale(cat, sub, "FOVMin"), tooltip:
                UIManager.GetLocale(cat, sub, "FOVMinTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FOVMax, UIManager.GetLocale(cat, sub, "FOVMax"), tooltip:
                UIManager.GetLocale(cat, sub, "FOVMaxTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSFOVMin, UIManager.GetLocale(cat, sub, "FPSFOVMin"), tooltip:
                UIManager.GetLocale(cat, sub, "FPSFOVMinTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSFOVMax, UIManager.GetLocale(cat, sub, "FPSFOVMax"), tooltip:
                UIManager.GetLocale(cat, sub, "FPSFOVMaxTooltip"), elementWidth: 100f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, new ElementStyle(titleWidth: 145f, themePanel: ThemePanel), settings.MouseSpeed, UIManager.GetLocale(cat, sub, "MouseSpeed"),
              elementWidth: 180f, decimalPlaces: 3);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.InvertMouse, UIManager.GetLocale(cat, sub, "InvertMouse"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.MinimapEnabled, UIManager.GetLocale(cat, sub, "MinimapEnabled"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.MinimapHeight, UIManager.GetLocale(cat, sub, "MinimapHeight"),
                elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsEnabled, UIManager.GetLocale(cat, sub, "SnapshotsEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsShowInGame, UIManager.GetLocale(cat, sub, "SnapshotsShowInGame"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapshotsMinimumDamage, UIManager.GetLocale(cat, sub, "SnapshotsMinimumDamage"),
                elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SkipCutscenes, UIManager.GetLocale(cat, sub, "SkipCutscenes"));
        }
    }
}
