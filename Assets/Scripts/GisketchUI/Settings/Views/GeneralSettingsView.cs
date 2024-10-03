using UnityEngine;
using Settings;
using System.Collections.Generic;
using UI;
using System;

namespace GisketchUI
{
    public class GeneralSettingsView : MonoBehaviour
    {
        private Transform contentTransform;

        public void Initialize(Transform content)
        {
            contentTransform = content;
            SetupSettings();
        }

        private void SetupSettings()
        {
            GeneralSettings settings = SettingsManager.GeneralSettings;
            string cat = "SettingsPopup";
            string sub = "General";

            // Language
            var languages = UIManager.GetLanguages();
            SettingItemFactory.CreateOptionSetting(contentTransform, "Language",
                new List<string>(languages), Array.IndexOf(languages, settings.Language.Value),
                (value) => { settings.Language.Value = value; });

            // Camera Mode
            string[] cameraModes = { "TPS", "Original", "FPS" };
            SettingItemFactory.CreateOptionSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraMode"),
                new List<string>(cameraModes), settings.CameraMode.Value,
                (value) => { settings.CameraMode.Value = Array.IndexOf(cameraModes, value); });


            // Camera Distance
            SettingItemFactory.CreateSliderSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraDistance"),
                settings.CameraDistance.MinValue, settings.CameraDistance.MaxValue, settings.CameraDistance.Value,
                false, 1, (value) => { settings.CameraDistance.Value = value; });

            // Camera Height
            SettingItemFactory.CreateSliderSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraHeight"),
                settings.CameraHeight.MinValue, settings.CameraHeight.MaxValue, settings.CameraHeight.Value,
                false, 1, (value) => { settings.CameraHeight.Value = value; });

            // Camera Side
            SettingItemFactory.CreateSliderSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraSide"),
                settings.CameraSide.MinValue, settings.CameraSide.MaxValue, settings.CameraSide.Value,
                false, 1, (value) => { settings.CameraSide.Value = value; });

            // Camera Tilt
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraTilt"),
                settings.CameraTilt.Value, (value) => { settings.CameraTilt.Value = value; });

            // Camera Clipping
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "CameraClipping"),
                settings.CameraClipping.Value, (value) => { settings.CameraClipping.Value = value; });

            // FOV Min
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "FOVMin"),
                "Enter FOV Min", settings.FOVMin.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.FOVMin.Value = result; });

            // FOV Max
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "FOVMax"),
                "Enter FOV Max", settings.FOVMax.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.FOVMax.Value = result; });

            // FPS FOV Min
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "FPSFOVMin"),
                "Enter FPS FOV Min", settings.FPSFOVMin.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.FPSFOVMin.Value = result; });

            // FPS FOV Max
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "FPSFOVMax"),
                "Enter FPS FOV Max", settings.FPSFOVMax.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.FPSFOVMax.Value = result; });

            // Mouse Speed
            SettingItemFactory.CreateSliderSetting(contentTransform, UIManager.GetLocale(cat, sub, "MouseSpeed"),
                settings.MouseSpeed.MinValue, settings.MouseSpeed.MaxValue, settings.MouseSpeed.Value,
                false, 2, (value) => { settings.MouseSpeed.Value = value; });

            // Invert Mouse
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "InvertMouse"),
                settings.InvertMouse.Value, (value) => { settings.InvertMouse.Value = value; });

            // Minimap Enabled
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "MinimapEnabled"),
                settings.MinimapEnabled.Value, (value) => { settings.MinimapEnabled.Value = value; });

            // Minimap Height
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "MinimapHeight"),
                "Enter Minimap Height", settings.MinimapHeight.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.MinimapHeight.Value = result; });

            // Snapshots Enabled
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "SnapshotsEnabled"),
                settings.SnapshotsEnabled.Value, (value) => { settings.SnapshotsEnabled.Value = value; });

            // Snapshots Show In Game
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "SnapshotsShowInGame"),
                settings.SnapshotsShowInGame.Value, (value) => { settings.SnapshotsShowInGame.Value = value; });

            // Snapshots Minimum Damage
            SettingItemFactory.CreateInputSetting(contentTransform, UIManager.GetLocale(cat, sub, "SnapshotsMinimumDamage"),
                "Enter Minimum Damage", settings.SnapshotsMinimumDamage.Value.ToString(), true,
                (value) => { if (int.TryParse(value, out int result)) settings.SnapshotsMinimumDamage.Value = result; });

            // Skip Cutscenes
            SettingItemFactory.CreateToggleSetting(contentTransform, UIManager.GetLocale(cat, sub, "SkipCutscenes"),
                settings.SkipCutscenes.Value, (value) => { settings.SkipCutscenes.Value = value; });
        }
    }
}