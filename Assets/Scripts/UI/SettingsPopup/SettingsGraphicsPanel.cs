using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsGraphicsPanel: SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "Graphics";
            GraphicsSettings settings = SettingsManager.GraphicsSettings;
            settings.ScreenResolution.Value = FullscreenHandler.SanitizeResolutionSetting(settings.ScreenResolution.Value);
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.PresetQuality, UIManager.GetLocale(cat, sub, "PresetQuality"),
                UIManager.GetLocaleArray(cat, sub, "PresetQualityOptions"), elementWidth: 200f, onDropdownOptionSelect: () => OnSelectPreset());
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.FullScreenMode, UIManager.GetLocale(cat, sub, "Fullscreen"),
                UIManager.GetLocaleArray(cat, sub, "FullscreenOptions"), tooltip: UIManager.GetLocale(cat, sub, "FullscreenTooltip"), elementWidth: 160f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.ScreenResolution, UIManager.GetLocale(cat, sub, "Resolution"),
                FullscreenHandler.GetResolutionOptions(), elementWidth: 200f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.RenderDistance, UIManager.GetLocale(cat, sub, "RenderDistance"), elementWidth: 100f, tooltip: UIManager.GetLocale(cat, sub, "RenderDistanceTooltip"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSCap, UIManager.GetLocale(cat, sub, "FPSCap"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.MenuFPSCap, UIManager.GetLocale(cat, sub, "MenuFPSCap"), elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.VSync, UIManager.GetLocale(cat, sub, "VSync"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.InterpolationEnabled, UIManager.GetLocale(cat, sub, "InterpolationEnabled"), tooltip: UIManager.GetLocale(cat, sub, "InterpolationEnabledTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ShowFPS, UIManager.GetLocale(cat, sub, "ShowFPS"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.MipmapEnabled, UIManager.GetLocale(cat, sub, "MipmapEnabled"), tooltip: UIManager.GetLocale(cat, sub, "MipmapEnabledTooltip"));
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.TextureQuality, UIManager.GetLocale(cat, sub, "TextureQuality"),
                UIManager.GetLocaleArray(cat, sub, "TextureQualityOptions"), elementWidth: 200f);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.ShadowQuality, UIManager.GetLocale(cat, sub, "ShadowQuality"),
                UIManager.GetLocaleArray(cat, sub, "ShadowQualityOptions"), elementWidth: 200f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.ShadowDistance, UIManager.GetLocale(cat, sub, "ShadowDistance"),
               elementWidth: 130f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.LightDistance, UIManager.GetLocale(cat, sub, "LightDistance"),
               elementWidth: 130f);
            // Added by Snake for Terrain Detail Slider 28 may 24   
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.DetailDistance, UIManager.GetLocale(cat, sub, "DetailDistance"),
               elementWidth: 130f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.DetailDensity, UIManager.GetLocale(cat, sub, "DetailDensity"),
               elementWidth: 130f);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, settings.TreeDistance, UIManager.GetLocale(cat, sub, "TreeDistance"),
               elementWidth: 130f);    
            //           
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.AntiAliasing, UIManager.GetLocale(cat, sub, "AntiAliasing"),
               UIManager.GetLocaleArray(cat, sub, "AntiAliasingOptions"), elementWidth: 200f);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.AnisotropicFiltering, UIManager.GetLocale(cat, sub, "Anisotropic"),
               UIManager.GetLocaleArray(cat, sub, "AnisotropicOptions"), elementWidth: 200f);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.WeatherEffects, UIManager.GetLocale(cat, sub, "WeatherEffects"),
               UIManager.GetLocaleArray(cat, sub, "WeatherEffectsOptions"), elementWidth: 200f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.NapeBloodEnabled, UIManager.GetLocale(cat, sub, "NapeBloodEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.WeaponTrailEnabled, UIManager.GetLocale(cat, sub, "WeaponTrailEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.WindEffectEnabled, UIManager.GetLocale(cat, sub, "WindEffectEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.BloodSplatterEnabled, UIManager.GetLocale(cat, sub, "BloodSplatterEnabled"));
        }

        protected void OnSelectPreset()
        {
            SettingsManager.GraphicsSettings.OnSelectPreset();
            SyncSettingElements();
        }
    }
}
