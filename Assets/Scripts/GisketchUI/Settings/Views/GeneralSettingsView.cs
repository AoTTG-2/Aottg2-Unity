using UnityEngine;
using Settings;
using System.Collections.Generic;
using UI;
using System;
using System.Linq;

namespace GisketchUI
{
    public class GeneralSettingsView : MonoBehaviour
    {
        private Transform contentTransform;
        private GeneralSettings settings;
        private const string CATEGORY = "SettingsPopup";
        private const string SUBCATEGORY = "General";

        public void Initialize(Transform content)
        {
            contentTransform = content;
            settings = SettingsManager.GeneralSettings;
            SetupSettings();
        }

        private void SetupSettings()
        {
            var settingSetups = new List<Action>
            {
                () => CreateLanguageSetting(),
                () => CreateCameraModeSetting(),
                () => CreateSliderSetting("CameraDistance", settings.CameraDistance),
                () => CreateSliderSetting("CameraHeight", settings.CameraHeight),
                () => CreateSliderSetting("CameraSide", settings.CameraSide),
                () => CreateToggleSetting("CameraTilt", settings.CameraTilt),
                () => CreateToggleSetting("CameraClipping", settings.CameraClipping),
                () => CreateInputSetting("FOVMin", settings.FOVMin),
                () => CreateInputSetting("FOVMax", settings.FOVMax),
                () => CreateInputSetting("FPSFOVMin", settings.FPSFOVMin),
                () => CreateInputSetting("FPSFOVMax", settings.FPSFOVMax),
                () => CreateSliderSetting("MouseSpeed", settings.MouseSpeed, 2),
                () => CreateToggleSetting("InvertMouse", settings.InvertMouse),
                () => CreateToggleSetting("MinimapEnabled", settings.MinimapEnabled),
                () => CreateInputSetting("MinimapHeight", settings.MinimapHeight),
                () => CreateToggleSetting("SnapshotsEnabled", settings.SnapshotsEnabled),
                () => CreateToggleSetting("SnapshotsShowInGame", settings.SnapshotsShowInGame),
                () => CreateInputSetting("SnapshotsMinimumDamage", settings.SnapshotsMinimumDamage),
                () => CreateToggleSetting("SkipCutscenes", settings.SkipCutscenes)
            };

            foreach (var setup in settingSetups)
            {
                setup();
            }
        }

        private void CreateLanguageSetting()
        {
            var languages = UIManager.GetLanguages();
            SettingItemFactory.CreateOptionSetting(contentTransform, "Language",
                new List<string>(languages), Array.IndexOf(languages, settings.Language.Value),
                (value) => { settings.Language.Value = value; });
        }

        private void CreateCameraModeSetting()
        {
            string[] cameraModes = { "TPS", "Original", "FPS" };
            CreateOptionSetting("CameraMode", cameraModes, settings.CameraMode.Value,
                value => settings.CameraMode.Value = Array.IndexOf(cameraModes, value));
        }

        private void CreateOptionSetting<T>(string key, IList<T> options, int defaultIndex, Action<T> onValueChanged)
        {
            SettingItemFactory.CreateOptionSetting(contentTransform, GetLocale(key),
                new List<string>(options.Select(o => o.ToString())), defaultIndex,
                value => onValueChanged(options[int.Parse(value)]));
        }

        private void CreateSliderSetting(string key, FloatSetting setting, int decimalPlaces = 1)
        {
            SettingItemFactory.CreateSliderSetting(contentTransform, GetLocale(key),
                setting.MinValue, setting.MaxValue, setting.Value,
                false, decimalPlaces, value => setting.Value = value);
        }

        private void CreateToggleSetting(string key, BoolSetting setting)
        {
            SettingItemFactory.CreateToggleSetting(contentTransform, GetLocale(key),
                setting.Value, value => setting.Value = value);
        }

        private void CreateInputSetting(string key, FloatSetting setting)
        {
            SettingItemFactory.CreateInputSetting(contentTransform, GetLocale(key),
                $"Enter {key}", setting.Value.ToString(), true,
                value => { if (float.TryParse(value, out float result)) setting.Value = result; });
        }

        private void CreateInputSetting(string key, IntSetting setting)
        {
            SettingItemFactory.CreateInputSetting(contentTransform, GetLocale(key),
                $"Enter {key}", setting.Value.ToString(), true,
                value => { if (int.TryParse(value, out int result)) setting.Value = result; });
        }

        private string GetLocale(string key) => UIManager.GetLocale(CATEGORY, SUBCATEGORY, key);
    }
}