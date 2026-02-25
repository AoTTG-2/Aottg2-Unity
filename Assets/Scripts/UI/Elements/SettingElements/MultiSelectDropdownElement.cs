using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utility;

namespace UI
{
    class MultiSelectDropdownElement : BaseSettingElement
    {
        protected GameObject _optionsPanel;
        protected GameObject _selectedButton;
        protected GameObject _selectedButtonLabel;
        protected string[] _options;
        protected float _currentScrollValue = 1f;
        protected Scrollbar _scrollBar;
        private Vector3 _optionsOffset;
        protected UnityAction _onSelectionChanged;
        protected HashSetSetting<int> _hashSetSetting;
        protected Dictionary<int, Toggle> _toggles = new Dictionary<int, Toggle>();
        Vector3 _lastKnownPosition = Vector3.zero;
        private string _themePanel;
        private float _checkMarkSizeMultiplier = 0.67f;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>();

        public void Setup(HashSetSetting<int> setting, ElementStyle style, string title, string[] options, string tooltip, 
            float elementWidth, float elementHeight, float optionsWidth, float maxScrollHeight, UnityAction onSelectionChanged)
        {
            if (options.Length == 0)
                throw new ArgumentException("MultiSelectDropdown cannot have 0 options.");
            _hashSetSetting = setting;
            _onSelectionChanged = onSelectionChanged;
            _options = options;
            _themePanel = style.ThemePanel;
            _optionsPanel = transform.Find("Dropdown/Mask").gameObject;
            _selectedButton = transform.Find("Dropdown/SelectedButton").gameObject;
            _selectedButtonLabel = _selectedButton.transform.Find("Label").gameObject;
            SetupLabel(_selectedButtonLabel, title, style.FontSize);
            _selectedButton.GetComponent<Button>().onClick.AddListener(() => OnDropdownSelectedButtonClick());
            _selectedButton.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            _selectedButton.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            
            for (int i = 0; i < options.Length; i++)
            {
                CreateOptionToggle(options[i], i, optionsWidth, elementHeight, style.FontSize, style.ThemePanel);
            }
            
            _selectedButton.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Dropdown");
            _selectedButtonLabel.GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
            _selectedButton.transform.Find("Image").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
            _optionsPanel.transform.Find("Options").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownBorderColor");
            Canvas.ForceUpdateCanvases();
            float optionsHeight = _optionsPanel.transform.Find("Options").GetComponent<RectTransform>().sizeDelta.y;
            if (optionsHeight > maxScrollHeight)
                optionsHeight = maxScrollHeight;
            else
            {
                _optionsPanel.GetComponent<ScrollRect>().verticalScrollbar = null;
                _optionsPanel.transform.Find("Scrollbar").gameObject.SetActive(false);
            }
            _scrollBar = _optionsPanel.transform.Find("Scrollbar").GetComponent<Scrollbar>();
            _scrollBar.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "DropdownScrollbar");
            _scrollBar.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownScrollbarBackgroundColor");
            _optionsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(optionsWidth, optionsHeight);
            transform.Find("Label").GetComponent<LayoutElement>().preferredHeight = elementHeight;
            transform.Find("Label").gameObject.SetActive(false);
            SetupTooltip(tooltip, style);
            
            float optionsOffsetX = (optionsWidth - elementWidth) * 0.5f;
            float optionsOffsetY = -(elementHeight + optionsHeight) * 0.5f + 2f;
            _optionsOffset = new Vector3(optionsOffsetX, optionsOffsetY, 0f);
            _optionsPanel.transform.SetParent(transform.root, true);
            _optionsPanel.SetActive(false);
            
            SyncElement();
        }

        public void FixScale()
        {
            _optionsPanel.transform.localScale = Vector3.one;
        }

        public bool IsOpen()
        {
            return _optionsPanel != null && _optionsPanel.activeSelf;
        }

        protected void SetOptionsPosition()
        {
            Vector3 position = _selectedButton.transform.position + _optionsOffset * UIManager.CurrentCanvasScale;
            _optionsPanel.transform.GetComponent<RectTransform>().position = position;
        }

        void OnDisable()
        {
            if (_optionsPanel != null)
                _optionsPanel.SetActive(false);
        }

        void OnDestroy()
        {
            if (_optionsPanel != null)
                Destroy(_optionsPanel);
        }

        void Update()
        {
            if (_optionsPanel != null && _optionsPanel.activeSelf)
            {
                if (transform.position != _lastKnownPosition)
                    CloseOptions();
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    RectTransform panelRect = _optionsPanel.GetComponent<RectTransform>();
                    RectTransform buttonRect = _selectedButton.GetComponent<RectTransform>();
                    if (!RectTransformUtility.RectangleContainsScreenPoint(panelRect, Input.mousePosition) &&
                        !RectTransformUtility.RectangleContainsScreenPoint(buttonRect, Input.mousePosition))
                        CloseOptions();
                }
            }
        }

        protected void CreateOptionToggle(string option, int index, float width, float height, int fontSize, string themePanel)
        {
            // Use the DropdownOption prefab as base (same as DropdownSettingElement)
            GameObject optionObj = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Elements/DropdownOption");
            optionObj.transform.SetParent(_optionsPanel.transform.Find("Options"), false);
            optionObj.GetComponent<LayoutElement>().preferredWidth = width;
            optionObj.GetComponent<LayoutElement>().preferredHeight = height;
            
            // Setup label with padding for checkbox
            Transform labelTransform = optionObj.transform.Find("Label");
            Text labelText = labelTransform.GetComponent<Text>();
            float checkboxSize = height * 0.6f;
            float checkboxPadding = checkboxSize + 10f;
            labelText.text = "      " + option; // Add spacing for checkbox
            labelText.fontSize = fontSize;
            labelText.alignment = TextAnchor.MiddleLeft;
            labelText.color = UIManager.GetThemeColor(themePanel, "DefaultSetting", "DropdownTextColor");
            
            // Create toggle (checkbox) positioned on the left
            GameObject toggleObj = new GameObject("Toggle");
            toggleObj.transform.SetParent(optionObj.transform, false);
            
            RectTransform toggleRect = toggleObj.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0f, 0.5f);
            toggleRect.anchorMax = new Vector2(0f, 0.5f);
            toggleRect.pivot = new Vector2(0f, 0.5f);
            toggleRect.anchoredPosition = new Vector2(5f, 0f);
            toggleRect.sizeDelta = new Vector2(checkboxSize, checkboxSize);
            
            // Background for toggle
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(toggleObj.transform, false);
            RectTransform bgRect = bgObj.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;
            Image bgToggleImage = bgObj.AddComponent<Image>();
            bgToggleImage.color = UIManager.GetThemeColorBlock(themePanel, "DefaultSetting", "Toggle").normalColor;
            
            // Checkmark
            GameObject checkmarkObj = new GameObject("Checkmark");
            checkmarkObj.transform.SetParent(bgObj.transform, false);
            RectTransform checkmarkRect = checkmarkObj.AddComponent<RectTransform>();
            checkmarkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkmarkRect.pivot = new Vector2(0.5f, 0.5f);
            checkmarkRect.anchoredPosition = Vector2.zero;
            float checkmarkSize = checkboxSize * _checkMarkSizeMultiplier;
            checkmarkRect.sizeDelta = new Vector2(checkmarkSize, checkmarkSize);
            Image checkmarkImage = checkmarkObj.AddComponent<Image>();
            checkmarkImage.color = UIManager.GetThemeColor(themePanel, "DefaultSetting", "ToggleFilledColor");
            
            // Setup toggle component
            Toggle toggle = toggleObj.AddComponent<Toggle>();
            toggle.targetGraphic = bgToggleImage;
            toggle.graphic = checkmarkImage;
            toggle.isOn = _hashSetSetting.Contains(index);
            toggle.onValueChanged.AddListener((bool value) => OnToggleValueChanged(index, value));
            
            // Setup button (reuse existing from prefab)
            Button rowButton = optionObj.GetComponent<Button>();
            rowButton.onClick.RemoveAllListeners();
            rowButton.colors = UIManager.GetThemeColorBlock(themePanel, "DefaultSetting", "Dropdown");
            rowButton.onClick.AddListener(() => toggle.isOn = !toggle.isOn);
            
            _toggles[index] = toggle;
        }

        protected void OnToggleValueChanged(int index, bool value)
        {
            if (value)
                _hashSetSetting.Add(index);
            else
                _hashSetSetting.Remove(index);
            _onSelectionChanged?.Invoke();
        }

        protected void OnDropdownSelectedButtonClick()
        {
            if (!_optionsPanel.activeSelf)
                StartCoroutine(WaitAndEnableOptions());
            else
                CloseOptions();
        }

        IEnumerator WaitAndEnableOptions()
        {
            yield return new WaitForEndOfFrame();
            SetOptionsPosition();
            _optionsPanel.transform.SetAsLastSibling();
            _lastKnownPosition = transform.position;
            _optionsPanel.SetActive(true);
            UIManager.NeedResizeText = true;
            yield return new WaitForEndOfFrame();
            if (_scrollBar != null)
                _scrollBar.value = _currentScrollValue;
        }

        protected void CloseOptions()
        {
            if (_scrollBar != null)
                _currentScrollValue = _scrollBar.value;
            _optionsPanel.SetActive(false);
        }

        public override void SyncElement()
        {
            foreach (var kvp in _toggles)
            {
                kvp.Value.isOn = _hashSetSetting.Contains(kvp.Key);
            }
        }
    }
}
