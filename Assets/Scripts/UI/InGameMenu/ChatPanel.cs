using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Settings;
using GameManagers;
using System;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private InputField _inputField;
        private GameObject _panel;
        private Text _chatDisplay;
        private List<string> _allLines = new List<string>();
        protected override string ThemePanel => "ChatPanel";
        protected Transform _caret;
        public bool IgnoreNextActivation;
        private bool isAutocompleting = false;

        public override void Setup(BasePanel parent = null)
        {
            _inputField = transform.Find("InputField").GetComponent<InputField>();
            _panel = transform.Find("Content/Panel").gameObject;
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            var style = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            _inputField.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "InputField", "Input");
            _inputField.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputTextColor");
            _inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputSelectionColor");
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 0f);
            _inputField.onEndEdit.AddListener((string text) => OnEndEdit(text));
            _inputField.onValueChanged.AddListener((string text) => OnInputValueChanged(text));
            _inputField.text = "";
            if (SettingsManager.UISettings.ChatWidth.Value == 0f)
            {
                _inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            }

            var textStyle = new ElementStyle(fontSize: SettingsManager.UISettings.ChatFontSize.Value, themePanel: ThemePanel);
            _chatDisplay = ElementFactory.CreateDefaultLabel(_panel.transform, textStyle, "", alignment: TextAnchor.LowerLeft).GetComponent<Text>();
            _chatDisplay.supportRichText = true;
            
            Sync();
        }

        public void AddLine(string line)
        {
            _allLines.Add(line);
            UpdateText();
        }

        public void AddLines(List<string> lines)
        {
            _allLines.AddRange(lines);
            UpdateText();
        }

        public void ReplaceLastLine(string line)
        {
            if (_allLines.Count > 0)
            {
                _allLines[_allLines.Count - 1] = line;
            }
            else
            {
                _allLines.Add(line);
            }
            UpdateText();
        }

        private void UpdateText()
        {
            _chatDisplay.text = string.Join("\n", _allLines);
            Canvas.ForceUpdateCanvases();
        }

        public void Sync()
        {
            _allLines.Clear();
            foreach (var message in ChatManager.Lines)
            {
                _allLines.Add(message.GetFormattedMessage());
            }
            UpdateText();
        }

        public void Activate()
        {
            _inputField.Select();
            _inputField.ActivateInputField();
        }

        public bool IsInputActive()
        {
            return _inputField.isFocused;
        }

        public void OnEndEdit(string text)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
                return;
            string input = _inputField.text;
            _inputField.text = "";
            IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
            ChatManager.HandleInput(input);
        }

        private void OnInputValueChanged(string text)
        {
            if (isAutocompleting)
                return;

            string suggestion = ChatManager.GetAutocompleteSuggestion(text);
            if (suggestion != null)
            {
                isAutocompleting = true;
                int caretPos = _inputField.caretPosition;
                _inputField.text = suggestion;
                _inputField.caretPosition = caretPos;
                isAutocompleting = false;
            }
        }

        private void Update()
        {
            if (!_caret && _inputField != null)
            {
                _caret = _inputField.transform.Find(_inputField.transform.name + " Input Caret");
                if (_caret)
                {
                    var graphic = _caret.GetComponent<Graphic>();
                    if (!graphic)
                        _caret.gameObject.AddComponent<Image>();
                }
            }

            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                if (!IsPointerOverChatUI())
                {
                    // Deactivate chat input if clicked outside
                    if (IsInputActive())
                    {
                        _inputField.DeactivateInputField();
                    }
                }
            }
        }

        public bool IsPointerOverChatUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePosition = Input.mousePosition;

                RectTransform chatPanelRect = _panel.GetComponent<RectTransform>();
                RectTransform inputFieldRect = _inputField.GetComponent<RectTransform>();

                if (RectTransformUtility.RectangleContainsScreenPoint(chatPanelRect, mousePosition) || 
                    RectTransformUtility.RectangleContainsScreenPoint(inputFieldRect, mousePosition))
                {
                    if (IsInputActive())
                    {
                        return true;
                    }
                    else
                    {
                        Activate();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}