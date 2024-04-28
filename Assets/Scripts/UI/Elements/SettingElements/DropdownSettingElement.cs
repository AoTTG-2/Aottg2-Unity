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
    class DropdownSettingElement : BaseSettingElement
    {
        protected GameObject _optionsPanel;
        protected GameObject _selectedButton;
        protected GameObject _selectedButtonLabel;
        protected string[] _options;
        protected float _currentScrollValue = 1f;
        protected Scrollbar _scrollBar;
        private Vector3 _optionsOffset;
        protected UnityAction _onDropdownOptionSelect;
        Vector3 _lastKnownPosition = Vector3.zero;

        protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>()
        {
            SettingType.String,
            SettingType.Int
        };

        public virtual void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string tooltip, float elementWidth, float elementHeight, 
            float optionsWidth, float maxScrollHeight, UnityAction onDropdownOptionSelect)
        {
            if (options.Length == 0)
                throw new ArgumentException("Dropdown cannot have 0 options.");
            _onDropdownOptionSelect = onDropdownOptionSelect;
            _options = options;
            _optionsPanel = transform.Find("Dropdown/Mask").gameObject;
            _selectedButton = transform.Find("Dropdown/SelectedButton").gameObject;
            _selectedButtonLabel = _selectedButton.transform.Find("Label").gameObject;
            SetupLabel(_selectedButtonLabel, options[0], style.FontSize);
            _selectedButton.GetComponent<Button>().onClick.AddListener(() => OnDropdownSelectedButtonClick());
            _selectedButton.GetComponent<LayoutElement>().preferredWidth = elementWidth;
            _selectedButton.GetComponent<LayoutElement>().preferredHeight = elementHeight;
            for (int i = 0; i < options.Length; i++)
            {
                CreateOptionButton(options[i], i, optionsWidth, elementHeight, style.FontSize, style.ThemePanel);
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
            float optionsOffsetX = (optionsWidth - elementWidth) * 0.5f;
            float optionsOffsetY = -(elementHeight + optionsHeight) * 0.5f + 2f;
            _optionsOffset = new Vector3(optionsOffsetX, optionsOffsetY, 0f);
            _optionsPanel.transform.SetParent(transform.root, true);
            _optionsPanel.SetActive(false);
            base.Setup(setting, style, title, tooltip);
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
                if ((Input.GetKeyUp(KeyCode.Mouse0) && EventSystem.current.currentSelectedGameObject != _scrollBar.gameObject) || 
                    transform.position != _lastKnownPosition)
                    StartCoroutine(WaitAndCloseOptions());
            }
        }

        protected void CreateOptionButton(string option, int index, float width, float height, int fontSize, string themePanel)
        {
            GameObject optionButton = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.UI, "Prefabs/Elements/DropdownOption");
            optionButton.transform.SetParent(_optionsPanel.transform.Find("Options"), false);
            SetupLabel(optionButton.transform.Find("Label").gameObject, option, fontSize);
            optionButton.GetComponent<Button>().onClick.AddListener(() => OnDropdownOptionClick(option, index));
            optionButton.GetComponent<LayoutElement>().preferredWidth = width;
            optionButton.GetComponent<LayoutElement>().preferredHeight = height;
            optionButton.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(themePanel, "DefaultSetting", "Dropdown");
            optionButton.transform.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor(themePanel, "DefaultSetting", "DropdownTextColor");
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
            _scrollBar.value = _currentScrollValue;
        }

        IEnumerator WaitAndCloseOptions()
        {
            yield return new WaitForEndOfFrame();
            CloseOptions();
        }

        protected virtual void OnDropdownOptionClick(string option, int index)
        {
            SetupLabel(_selectedButtonLabel, option);
            CloseOptions();
            if (_settingType == SettingType.String)
                ((StringSetting)_setting).Value = option;
            else if (_settingType == SettingType.Int)
                ((IntSetting)_setting).Value = index;
            _onDropdownOptionSelect?.Invoke();
        }

        protected void CloseOptions()
        {
            _currentScrollValue = _scrollBar.value;
            _optionsPanel.SetActive(false);
        }

        public override void SyncElement()
        {
            if (_settingType == SettingType.String)
                SetupLabel(_selectedButtonLabel, ((StringSetting)_setting).Value);
            else if (_settingType == SettingType.Int)
            {
                IntSetting setting = (IntSetting)_setting;
                if (setting.Value >= _options.Length)
                    setting.Value = 0;
                SetupLabel(_selectedButtonLabel, _options[setting.Value]);
            }
        }
    }
}
