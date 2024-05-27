using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ApplicationManagers;
using UnityEngine.UI;
using UnityEngine.Events;
using Settings;
using Utility;

namespace UI
{
    class ElementFactory: MonoBehaviour
    {
        public static T CreateDefaultMenu<T>() where T : BaseMenu
        {
            return CreateMenu<T>("Prefabs/Panels/DefaultMenu");
        }

        public static T CreateMenu<T>(string asset) where T : BaseMenu
        {
            GameObject menu = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, asset, cached: true);
            menu.transform.position = Vector3.zero;
            BaseMenu component = menu.AddComponent<T>();
            return (T)component;
        }

        public static T CreateEmptyPanel<T>(Transform parent, bool enabled = false) where T : BasePanel
        {
            return InstantiateAndSetupPanel<T>(parent, "Prefabs/Panels/EmptyPanel", enabled).GetComponent<T>();
        }

        public static GameObject CreateEmptyPanel(Transform parent, Type t, bool enabled = false)
        {
            GameObject panel = InstantiateAndBind(parent, "Prefabs/Panels/EmptyPanel");
            ((BasePanel)panel.AddComponent(t)).Setup(parent.GetComponent<BasePanel>());
            panel.SetActive(enabled);
            return panel;
        }

        public static T CreateSimplePanel<T>(Transform parent, bool enabled = false) where T : SimplePanel
        {
            return InstantiateAndSetupPanel<T>(parent, "Prefabs/Panels/SimplePanel", enabled).GetComponent<T>();
        }

        public static T CreateDefaultPopup<T>(Transform parent, bool enabled = false) where T : BasePopup
        {
            return InstantiateAndSetupPanel<T>(parent, "Prefabs/Panels/HeadedPanel", enabled).GetComponent<T>();
        }

        public static T CreateHeadedPanel<T>(Transform parent, bool enabled = false) where T : HeadedPanel
        {
            return InstantiateAndSetupPanel<T>(parent, "Prefabs/Panels/HeadedPanel", enabled).GetComponent<T>();
        }

        public static GameObject CreateTooltipPopup(Transform parent, bool enabled = false)
        {
            return InstantiateAndSetupPanel<TooltipPopup>(parent, "Prefabs/Misc/TooltipPopup", enabled);
        }

        public static TipPanel CreateTipPanel(Transform parent, bool enabled = false)
        {
            GameObject tip = InstantiateAndBind(parent, "Prefabs/MainMenu/TipPanel");
            TipPanel tipPanel = tip.AddComponent<TipPanel>();
            tipPanel.Setup();
            tipPanel.gameObject.SetActive(enabled);
            return tipPanel;
        }

        public static GameObject CreateDefaultButton(Transform parent, ElementStyle style, string title, float elementWidth = 0f, float elementHeight = 0f,
            UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Elements/DefaultButton");
            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            LayoutElement layout = button.GetComponent<LayoutElement>();
            if (elementWidth > 0f)
                layout.preferredWidth = elementWidth;
            if (elementHeight > 0f)
                layout.preferredHeight = elementHeight;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            return button;
        }

        public static GameObject CreatePerkButton(Transform parent, ElementStyle style, string title, string tooltip, 
            float elementWidth = 0f, float elementHeight = 0f, float offset = 0f, UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Misc/PerkButton");
            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            LayoutElement layout = button.GetComponent<LayoutElement>();
            if (elementWidth > 0f)
                layout.preferredWidth = elementWidth;
            if (elementHeight > 0f)
                layout.preferredHeight = elementHeight;
            PerkButton perkButton = button.AddComponent<PerkButton>();
            perkButton.Setup(tooltip, style, offset);
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            return button;
        }

        public static GameObject CreateIconButton(Transform parent, ElementStyle style, string icon, float elementWidth = 32f, float elementHeight = 32f, 
            UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Elements/IconButton");
            LayoutElement layout = button.GetComponent<LayoutElement>();
            layout.preferredWidth = elementWidth;
            layout.preferredHeight = elementHeight;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            RawImage img = button.GetComponent<RawImage>();
            img.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, icon, true);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "IconButton", "");
            return button;
        }

        public static GameObject CreateRawImage(Transform parent, ElementStyle style, string image, float elementWidth = 32f,
            float elementHeight = 32f)
        {
            GameObject obj = InstantiateAndBind(parent, "Prefabs/Elements/RawImage");
            RawImage rawImage = obj.GetComponent<RawImage>();
            LayoutElement layout = obj.GetComponent<LayoutElement>();
            layout.preferredWidth = elementWidth;
            layout.preferredHeight = elementHeight;
            if (image != string.Empty)
                rawImage.texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, image, true);
            return obj;
        }

        public static GameObject CreateTooltipIcon(Transform parent, ElementStyle style, string tooltip, float elementWidth = 30f, float elementHeight = 30f)
        {
            GameObject obj = CreateRawImage(parent, style, "Icons/Navigation/TooltipIcon", elementWidth, elementHeight);
            TooltipButton button = obj.AddComponent<TooltipButton>();
            button.Setup(tooltip, style);
            return obj;
        }

        public static GameObject CreateTextButton(Transform parent, ElementStyle style, string title, float width = 0f, UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Elements/TextButton");
            Text text = button.GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            text.fontStyle = FontStyle.Bold;
            if (width != 0f)
                text.GetComponent<LayoutElement>().preferredWidth = width;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "TextButton", "");
            return button;
        }

        public static GameObject CreateLinkButton(Transform parent, ElementStyle style, string title, UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Elements/TextButton");
            Text text = button.GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "LinkButton", "");
            return button;
        }

        public static GameObject CreateCategoryButton(Transform parent, ElementStyle style, string title, UnityAction onClick = null)
        {
            GameObject button = InstantiateAndBind(parent, "Prefabs/Elements/CategoryButton");
            Text text = button.GetComponent<Text>();
            text.text = title;
            text.fontSize = style.FontSize;
            if (onClick != null)
                button.GetComponent<Button>().onClick.AddListener(onClick);
            button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "CategoryButton", "");
            return button;
        }

        public static GameObject CreateDropdownSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options,
            string tooltip = "", float elementWidth = 140f, float elementHeight = 40f, float maxScrollHeight = 300f, float? optionsWidth = null,
            UnityAction onDropdownOptionSelect = null)
        {
            GameObject dropdownSetting = InstantiateAndBind(parent, "Prefabs/Elements/DropdownSetting");
            DropdownSettingElement element = dropdownSetting.AddComponent<DropdownSettingElement>();
            if (optionsWidth == null)
                optionsWidth = elementWidth;
            element.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight, optionsWidth.Value,
                maxScrollHeight, onDropdownOptionSelect);
            return dropdownSetting;
        }

        public static GameObject CreateDropdownSelect(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options,
            string tooltip = "", float elementWidth = 140f, float elementHeight = 40f, float maxScrollHeight = 300f, float? optionsWidth = null,
            UnityAction onDropdownOptionSelect = null)
        {
            GameObject dropdownSetting = InstantiateAndBind(parent, "Prefabs/Elements/DropdownSetting");
            DropdownSelectElement element = dropdownSetting.AddComponent<DropdownSelectElement>();
            if (optionsWidth == null)
                optionsWidth = elementWidth;
            element.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight, optionsWidth.Value,
                maxScrollHeight, onDropdownOptionSelect);
            return dropdownSetting;
        }

        public static GameObject CreateIncrementSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 33f, float elementHeight = 30f, string[] options = null,
            UnityAction onValueChanged = null)
        {
            GameObject incrementSetting = InstantiateAndBind(parent, "Prefabs/Elements/IncrementSetting");
            IncrementSettingElement element = incrementSetting.AddComponent<IncrementSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, options, onValueChanged);
            return incrementSetting;
        }

        public static GameObject CreateToggleSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 30f, float elementHeight = 30f, UnityAction onValueChanged = null)
        {
            GameObject toggleSetting = InstantiateAndBind(parent, "Prefabs/Elements/ToggleSetting");
            ToggleSettingElement element = toggleSetting.AddComponent<ToggleSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, onValueChanged);
            return toggleSetting;
        }

        public static GameObject CreateToggleGroupSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options,
           string tooltip = "", float elementWidth = 30f, float elementHeight = 30f)
        {
            GameObject toggleGroupSetting = InstantiateAndBind(parent, "Prefabs/Elements/ToggleGroupSetting");
            ToggleGroupSettingElement element = toggleGroupSetting.AddComponent<ToggleGroupSettingElement>();
            element.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight);
            return toggleGroupSetting;
        }

        public static GameObject CreateSliderSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 150f, float elementHeight = 16f, int decimalPlaces = 2)
        {
            GameObject sliderSetting = InstantiateAndBind(parent, "Prefabs/Elements/SliderSetting");
            SliderSettingElement element = sliderSetting.AddComponent<SliderSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, decimalPlaces);
            return sliderSetting;
        }

        public static GameObject CreateInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float elementWidth = 140f, float elementHeight = 40f, bool multiLine = false,
            UnityAction onValueChanged = null, UnityAction onEndEdit = null)
        {
            GameObject inputSetting = InstantiateAndBind(parent, "Prefabs/Elements/InputSetting");
            InputSettingElement element = inputSetting.AddComponent<InputSettingElement>();
            element.Setup(setting, style, title, tooltip, elementWidth, elementHeight, multiLine, onValueChanged, onEndEdit);
            element.GetComponent<HorizontalLayoutGroup>().spacing = style.Spacing;
            return inputSetting;
        }

        public static GameObject CreateSliderInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "",
            float sliderWidth = 150f, float sliderHeight = 16f, float inputWidth = 70f,
            float inputHeight = 40f, int decimalPlaces = 2)
        {
            GameObject sliderInputSetting = InstantiateAndBind(parent, "Prefabs/Elements/SliderInputSetting");
            SliderInputSettingElement element = sliderInputSetting.AddComponent<SliderInputSettingElement>();
            element.Setup(setting, style, title, tooltip, sliderWidth, sliderHeight, inputWidth, inputHeight, decimalPlaces);
            return sliderInputSetting;
        }

        public static GameObject CreateDefaultLabel(Transform parent, ElementStyle style, string title, FontStyle fontStyle = FontStyle.Normal,
            TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GameObject label = InstantiateAndBind(parent, "Prefabs/Elements/DefaultLabel");
            Text text = label.GetComponent<Text>();
            text.fontSize = style.FontSize;
            text.text = title;
            text.fontStyle = fontStyle;
            text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
            text.alignment = alignment;
            if (parent.GetComponent<VerticalLayoutGroup>() != null)
                text.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            else
                text.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            if (parent.GetComponent<HorizontalLayoutGroup>() != null)
                text.GetComponent<LayoutElement>().flexibleWidth = 0f;
            return label;
        }

        public static GameObject CreateEmptySpace(Transform parent)
        {
            GameObject label = InstantiateAndBind(parent, "Prefabs/Elements/DefaultLabel");
            Text text = label.GetComponent<Text>();
            text.text = "";
            return label;
        }

        public static GameObject CreateHUDLabel(Transform parent, ElementStyle style, string title, FontStyle fontStyle = FontStyle.Normal,
            TextAnchor alignment = TextAnchor.MiddleCenter)
        {
            GameObject label = InstantiateAndBind(parent, "Prefabs/InGame/HUDLabel");
            Text text = label.GetComponent<Text>();
            text.fontSize = style.FontSize;
            text.text = title;
            text.fontStyle = fontStyle;
            // text.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
            text.alignment = alignment;
            if (parent.GetComponent<VerticalLayoutGroup>() != null)
                text.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            else
                text.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            if (parent.GetComponent<HorizontalLayoutGroup>() != null)
                text.GetComponent<LayoutElement>().flexibleWidth = 0f;
            return label;
        }

        public static GameObject CreateKeybindSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, KeybindPopup keybindPopup,
            string tooltip = "", float elementWidth = 120f, float elementHeight = 35f, int bindCount = 2)
        {
            GameObject keybindSetting = InstantiateAndBind(parent, "Prefabs/Elements/KeybindSetting");
            KeybindSettingElement element = keybindSetting.AddComponent<KeybindSettingElement>();
            element.Setup(setting, style, title, keybindPopup, tooltip, elementWidth, elementHeight, bindCount);
            return keybindSetting;
        }

        public static GameObject CreateColorSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, ColorPickPopup colorPickPopup,
            string tooltip = "", float elementWidth = 90f, float elementHeight = 30f, UnityAction onChangeColor = null)
        {
            GameObject colorSetting = InstantiateAndBind(parent, "Prefabs/Elements/ColorSetting");
            ColorSettingElement element = colorSetting.AddComponent<ColorSettingElement>();
            element.Setup(setting, style, title, colorPickPopup, tooltip, elementWidth, elementHeight, onChangeColor);
            return colorSetting;
        }

        public static GameObject CreateIconPickSetting(Transform parent, ElementStyle style, BaseSetting setting, string title,
            string[] options, string[] icons, IconPickPopup popup, string tooltip = "", float elementWidth = 0f, float elementHeight = 0f, 
            UnityAction onSelect = null)
        {
            GameObject buttonSetting = InstantiateAndBind(parent, "Prefabs/Elements/ButtonSetting");
            var element = buttonSetting.AddComponent<IconPickSettingElement>();
            element.Setup(setting, style, title, options, icons, popup, tooltip, elementWidth, elementHeight, onSelect);
            return buttonSetting;
        }

        public static GameObject CreateButtonPopupSetting(Transform parent, ElementStyle style, BaseSetting setting, string title,
            BasePopup popup, string tooltip = "", float elementWidth = 0f, float elementHeight = 0f)
        {
            GameObject buttonSetting = InstantiateAndBind(parent, "Prefabs/Elements/ButtonSetting");
            var element = buttonSetting.AddComponent<ButtonPopupSettingElement>();
            element.Setup(setting, style, title, popup, tooltip, elementWidth, elementHeight);
            return buttonSetting;
        }

        public static GameObject CreateVector3Setting(Transform parent, ElementStyle style, BaseSetting setting, string title, Vector3Popup vector3Popup,
            string tooltip = "", float elementWidth = 90f, float elementHeight = 30f, UnityAction onChangeVector = null)
        {
            GameObject vectorSetting = InstantiateAndBind(parent, "Prefabs/Elements/Vector3Setting");
            Vector3SettingElement element = vectorSetting.AddComponent<Vector3SettingElement>();
            element.Setup(setting, style, title, vector3Popup, tooltip, elementWidth, elementHeight, onChangeVector);
            return vectorSetting;
        }

        public static GameObject CreateHorizontalLine(Transform parent, ElementStyle style, float width, float height = 1f)
        {
            GameObject line = InstantiateAndBind(parent, "Prefabs/Elements/HorizontalLine");
            line.transform.Find("LineImage").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            line.transform.Find("LineImage").gameObject.AddComponent<HorizontalLineScaler>();
            line.transform.Find("LineImage").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "LineColor");
            return line;
        }

        public static GameObject CreateHorizontalGroup(Transform parent, float spacing, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            GameObject group = InstantiateAndBind(parent, "Prefabs/Elements/HorizontalGroup");
            group.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
            group.GetComponent<HorizontalLayoutGroup>().childAlignment = alignment;
            if (parent.GetComponent<HorizontalLayoutGroup>() != null)
                group.GetComponent<LayoutElement>().flexibleWidth = 0f;
            return group;
        }

        // Slow method. Use only in cases of extreme laziness.
        public static float GetTextWidth(Transform parent, ElementStyle style, string title, FontStyle fontStyle = FontStyle.Normal)
        {
            GameObject label = CreateDefaultLabel(parent, style, title, fontStyle, TextAnchor.MiddleCenter);
            Canvas.ForceUpdateCanvases();
            float width = label.GetComponent<RectTransform>().sizeDelta.x;
            Destroy(label);
            return width;
        }

        public static GameObject InstantiateAndSetupPanel<T>(Transform parent, string asset, bool enabled = false) where T : BasePanel
        {
            GameObject panel = InstantiateAndBind(parent, asset);
            panel.AddComponent<T>().Setup(parent.GetComponent<BasePanel>());
            panel.SetActive(false);
            panel.SetActive(enabled);
            return panel;
        }

        public static GameObject InstantiateAndSetupCustomPopup(Transform parent, string title, float width, float height, bool enabled = false)
        {
            GameObject panel = InstantiateAndBind(parent, "Prefabs/Panels/HeadedPanel");
            panel.AddComponent<CustomPopup>().Setup(parent.GetComponent<BasePanel>(), title, width, height);
            panel.SetActive(false);
            panel.SetActive(enabled);
            return panel;
        }

        public static GameObject InstantiateAndBind(Transform parent, string asset)
        {
            GameObject obj = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, asset, cached: true);
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

        public static void SetAnchor(GameObject obj, TextAnchor anchor, TextAnchor pivot, Vector2 offset)
        {
            RectTransform transform = obj.GetComponent<RectTransform>();
            transform.anchorMin = transform.anchorMax = GetAnchorVector(anchor);
            transform.pivot = GetAnchorVector(pivot);
            transform.anchoredPosition = offset;
        }

        public static Vector2 GetAnchorVector(TextAnchor anchor)
        {
            Vector2 anchorVector = new Vector2(0f, 0f);
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    anchorVector = new Vector2(0f, 1f);
                    break;
                case TextAnchor.MiddleLeft:
                    anchorVector = new Vector2(0f, 0.5f);
                    break;
                case TextAnchor.LowerLeft:
                    anchorVector = new Vector2(0f, 0f);
                    break;
                case TextAnchor.UpperCenter:
                    anchorVector = new Vector2(0.5f, 1f);
                    break;
                case TextAnchor.MiddleCenter:
                    anchorVector = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.LowerCenter:
                    anchorVector = new Vector2(0.5f, 0f);
                    break;
                case TextAnchor.UpperRight:
                    anchorVector = new Vector2(1f, 1f);
                    break;
                case TextAnchor.MiddleRight:
                    anchorVector = new Vector2(1f, 0.5f);
                    break;
                case TextAnchor.LowerRight:
                    anchorVector = new Vector2(1f, 0f);
                    break;
            }
            return anchorVector;
        }
    }
}
