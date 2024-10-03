using UnityEngine;
using System.Collections.Generic;
using GisketchUI;

public class TestSettings : MonoBehaviour
{
    private void Start()
    {
        // Create a toggle setting
        SettingItemFactory.CreateToggleSetting(transform, "Enable Feature", true, OnFeatureToggled);

        // Create a slider setting
        SettingItemFactory.CreateSliderSetting(transform, "Volume", 0f, 1f, 0.5f, false, 2, OnVolumeChanged);

        // Create an input setting
        SettingItemFactory.CreateInputSetting(transform, "Username", "Enter username", "", false, OnUsernameChanged);

        // Create an option setting
        SettingItemFactory.CreateOptionSetting(transform, "Graphics Quality", new List<string> { "Low", "Medium", "High" }, 1, OnQualityChanged);

        // Create another toggle setting
        SettingItemFactory.CreateToggleSetting(transform, "Enable Notifications", false, OnNotificationsToggled);

        // Create another slider setting
        SettingItemFactory.CreateSliderSetting(transform, "Music Volume", 0f, 100f, 75f, true, 0, OnMusicVolumeChanged);

        // Create another input setting
        SettingItemFactory.CreateInputSetting(transform, "Email", "Enter email address", "", false, OnEmailChanged);

        // Create another option setting
        SettingItemFactory.CreateOptionSetting(transform, "Language", new List<string> { "English", "Spanish", "French", "German" }, 0, OnLanguageChanged);
    }

    private void OnFeatureToggled(bool value)
    {
        Debug.Log($"Feature toggled: {value}");
    }

    private void OnVolumeChanged(float value)
    {
        Debug.Log($"Volume changed: {value}");
    }

    private void OnUsernameChanged(string value)
    {
        Debug.Log($"Username changed: {value}");
    }

    private void OnQualityChanged(string value)
    {
        Debug.Log($"Graphics Quality changed: {value}");
    }

    private void OnNotificationsToggled(bool value)
    {
        Debug.Log($"Notifications toggled: {value}");
    }

    private void OnMusicVolumeChanged(float value)
    {
        Debug.Log($"Music Volume changed: {value}");
    }

    private void OnEmailChanged(string value)
    {
        Debug.Log($"Email changed: {value}");
    }

    private void OnLanguageChanged(string value)
    {
        Debug.Log($"Language changed: {value}");
    }
}