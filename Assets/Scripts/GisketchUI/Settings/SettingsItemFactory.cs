using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GisketchUI
{
    public static class SettingItemFactory
    {
        private static GameObject GetPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load<GameObject>($"GisketchUI/Prefabs/Settings/{prefabName}");
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab: {prefabName}. Make sure it exists in Resources/GisketchUI/Prefabs/");
            }
            return prefab;
        }

        private static GameObject InstantiateSettingItem(Transform parent, string label)
        {
            GameObject prefab = GetPrefab("SettingItem");
            if (prefab == null)
            {
                Debug.LogError("SettingItem prefab is null. Cannot instantiate.");
                return null;
            }
            GameObject settingItem = Object.Instantiate(prefab, parent);
            Text labelComponent = settingItem.GetComponentInChildren<Text>();
            if (labelComponent != null)
            {
                labelComponent.text = label;
            }
            return settingItem;
        }

        private static T InstantiateSettingComponent<T>(GameObject prefab, Transform parent) where T : Component
        {
            GameObject settingComponent = Object.Instantiate(prefab, parent);
            return settingComponent.GetComponent<T>();
        }

        public static SettingToggle CreateToggleSetting(Transform parent, string label, bool defaultValue, System.Action<bool> onValueChanged = null)
        {
            GameObject settingItem = InstantiateSettingItem(parent, label);
            SettingToggle toggle = InstantiateSettingComponent<SettingToggle>(GetPrefab("SettingToggle"), settingItem.transform);

            toggle.Initialize(defaultValue);
            if (onValueChanged != null)
            {
                toggle.toggleButton.onClick.AddListener(() => onValueChanged(toggle.GetValue()));
            }

            return toggle;
        }

        public static SettingSlider CreateSliderSetting(Transform parent, string label, float minValue, float maxValue, float defaultValue, bool isInteger = false, int decimalPlaces = 2, System.Action<float> onValueChanged = null)
        {
            GameObject settingItem = InstantiateSettingItem(parent, label);
            SettingSlider slider = InstantiateSettingComponent<SettingSlider>(GetPrefab("SettingSlider"), settingItem.transform);

            slider.Initialize(minValue, maxValue, defaultValue, isInteger, decimalPlaces);
            if (onValueChanged != null)
            {
                slider.slider.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            return slider;
        }

        public static SettingInputField CreateInputSetting(Transform parent, string label, string placeholder, string defaultValue, bool numberOnly = false, System.Action<string> onValueChanged = null)
        {
            GameObject settingItem = InstantiateSettingItem(parent, label);
            SettingInputField inputField = InstantiateSettingComponent<SettingInputField>(GetPrefab("SettingInputField"), settingItem.transform);

            inputField.Initialize(placeholder, defaultValue, numberOnly);
            if (onValueChanged != null)
            {
                inputField.inputField.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            return inputField;
        }

        public static SettingOptions CreateOptionSetting(Transform parent, string label, List<string> options, int defaultIndex = 0, System.Action<string> onValueChanged = null)
        {
            GameObject settingItem = InstantiateSettingItem(parent, label);
            SettingOptions optionSetting = InstantiateSettingComponent<SettingOptions>(GetPrefab("SettingOptions"), settingItem.transform);

            optionSetting.SetOptions(options);
            optionSetting.SetOption(options[defaultIndex]);
            if (onValueChanged != null)
            {
                // Since there's no direct event for option change, we'll need to add listeners to the arrows and item button
                optionSetting.leftArrow.onClick.AddListener(() => onValueChanged(optionSetting.GetCurrentOption()));
                optionSetting.rightArrow.onClick.AddListener(() => onValueChanged(optionSetting.GetCurrentOption()));
                optionSetting.itemButton.onClick.AddListener(() => onValueChanged(optionSetting.GetCurrentOption()));
            }

            return optionSetting;
        }
    }
}