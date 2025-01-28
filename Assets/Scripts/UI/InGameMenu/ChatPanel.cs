using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Settings;
using GameManagers;
using System;
using System.IO;
using System.Linq;
using Utility;
using ApplicationManagers;
using TMPro;
using System.Text.RegularExpressions;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private InputField _inputField;
        private GameObject _panel;
        private TMP_InputField _chatDisplay;
        private List<string> _allLines = new List<string>();
        protected override string ThemePanel => "ChatPanel";
        protected Transform _caret;
        public bool IgnoreNextActivation;
        private bool isAutocompleting = false;
        private Button _downloadButton;
        public string LastDownloadedFile { get; private set; }
        public string LastFileHash { get; private set; }
        public readonly Dictionary<string, DateTime> _hashHistory = new Dictionary<string, DateTime>();

        public override void Setup(BasePanel parent = null)
        {
            _inputField = transform.Find("InputField").GetComponent<InputField>();
            _panel = transform.Find("Content/Panel").gameObject;
            
            // Setup Content size and ScrollRect
            var content = transform.Find("Content");
            var contentRect = content.GetComponent<RectTransform>();
            var layoutElement = content.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            
            
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.offsetMax = new Vector2(-12, panelRect.offsetMax.y);  // 15 pixels from right edge for scrollbar
            
            // The Content object already has a Mask component
            
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

            // Adjust input field rect to make room for button
            var inputRect = _inputField.GetComponent<RectTransform>();
            inputRect.offsetMax = new Vector2(-35, inputRect.offsetMax.y);  // 35 pixels from right edge
            inputRect.offsetMin = new Vector2(inputRect.offsetMin.x, 10f);  // Add 10 pixels padding at the top
            
            var chatGO = new GameObject("ChatDisplay", typeof(RectTransform));
            chatGO.transform.SetParent(_panel.transform, false);
            _chatDisplay = chatGO.AddComponent<TMP_InputField>();

            // Set up the visual components required by TMP_InputField
            var textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(chatGO.transform, false);
            var textComponent = textArea.AddComponent<TextMeshProUGUI>();

            _chatDisplay.textComponent = textComponent;
            _chatDisplay.textViewport = textArea.GetComponent<RectTransform>();

            // Configure the input field
            _chatDisplay.readOnly = true;
            _chatDisplay.richText = true;
            _chatDisplay.onFocusSelectAll = false;
            _chatDisplay.resetOnDeActivation = false;
            _chatDisplay.restoreOriginalTextOnEscape = false;
            _chatDisplay.textComponent.fontSize = SettingsManager.UISettings.ChatFontSize.Value;
            _chatDisplay.textComponent.color = Color.white;
            _chatDisplay.textComponent.alignment = TextAlignmentOptions.BottomLeft;
            _chatDisplay.textComponent.enableWordWrapping = true;
            _chatDisplay.interactable = true;  // Make sure the input field is interactable

            // Make sure the RectTransform fills the panels
            var rectTransform = chatGO.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.sizeDelta = Vector2.zero;
            textAreaRect.anchoredPosition = Vector2.zero;

            // Create Scrollbar GameObject with RectTransform
            var scrollbarGo = new GameObject("Scrollbar", typeof(RectTransform));
            scrollbarGo.transform.SetParent(content, false);
            scrollbarGo.SetActive(true);  // Explicitly enable the scrollbar
            var scrollbarRect = scrollbarGo.GetComponent<RectTransform>();
            scrollbarRect.anchorMin = new Vector2(1, 0);
            scrollbarRect.anchorMax = new Vector2(1, 1);
            scrollbarRect.pivot = new Vector2(1, 0.5f);
            scrollbarRect.sizeDelta = new Vector2(10, 0);
            
            // Add Scrollbar component
            var scrollbar = scrollbarGo.AddComponent<Scrollbar>();
            
            // Create Sliding Area
            var slidingArea = new GameObject("Sliding Area", typeof(RectTransform));
            slidingArea.transform.SetParent(scrollbarGo.transform, false);
            var slidingAreaRect = slidingArea.GetComponent<RectTransform>();
            slidingAreaRect.anchorMin = Vector2.zero;
            slidingAreaRect.anchorMax = Vector2.one;
            slidingAreaRect.sizeDelta = Vector2.zero;
            
            // Create Handle
            var handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            handle.transform.SetParent(slidingArea.transform, false);
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = Vector2.zero;
            handleRect.anchorMax = Vector2.one;
            handleRect.sizeDelta = Vector2.zero;
            
            // Setup handle image
            var handleImage = handle.GetComponent<Image>();
            handleImage.color = new Color(0.8f, 0.8f, 0.8f, 0.4f);
            
            // Configure scrollbar
            scrollbar.handleRect = handleRect;
            scrollbar.targetGraphic = handleImage;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            
            // Add custom ScrollRect to Content
            var scrollRect = content.gameObject.AddComponent<ChatScrollRect>();
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            scrollRect.viewport = contentRect;
            scrollRect.content = _panel.GetComponent<RectTransform>();
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarSpacing = -3;  // Negative value to expand viewport
            
            // Adjust panel width to account for scrollbar
            
            // Create download button
            var buttonGo = new GameObject("DownloadButton", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonGo.transform.SetParent(transform, false);
            _downloadButton = buttonGo.GetComponent<Button>();
            
            // Position the button next to input field
            var buttonRect = buttonGo.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(1, 0);
            buttonRect.anchorMax = new Vector2(1, 0);
            buttonRect.pivot = new Vector2(1, 0);
            buttonRect.sizeDelta = new Vector2(25, 25);
            buttonRect.anchoredPosition = new Vector2(-5, 5);
            
            // Set button image
            var buttonImage = buttonGo.GetComponent<Image>();
            buttonImage.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);  // RGBA: semi-transparent grey
            
            // Add click listener
            _downloadButton.onClick.AddListener(DownloadChatHistory);
            
            // Add event triggers for mouse enter/exit
            var eventTrigger = content.gameObject.AddComponent<EventTrigger>();
            
            var enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { scrollRect.OnMouseEnter(); });
            eventTrigger.triggers.Add(enterEntry);
            
            var exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { scrollRect.OnMouseExit(); });
            eventTrigger.triggers.Add(exitEntry);

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

            // Strip any rich text tags from input
            string cleanText = Regex.Replace(text, @"<[^>]+>|</[^>]+>", string.Empty);
            if (cleanText != text)
            {
                isAutocompleting = true;
                int caretPos = _inputField.caretPosition;
                _inputField.text = cleanText;
                _inputField.caretPosition = Mathf.Min(caretPos, cleanText.Length);
                isAutocompleting = false;
                text = cleanText;
            }

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

                // Get all relevant RectTransforms
                RectTransform chatPanelRect = _panel.GetComponent<RectTransform>();
                RectTransform inputFieldRect = _inputField.GetComponent<RectTransform>();
                RectTransform contentRect = transform.Find("Content").GetComponent<RectTransform>();
                RectTransform scrollbarRect = transform.Find("Content/Scrollbar")?.GetComponent<RectTransform>();

                // Check if mouse is over any of the chat UI elements
                if (RectTransformUtility.RectangleContainsScreenPoint(chatPanelRect, mousePosition))
                {
                    // If over chat display area, make it the selected object for copy to work
                    EventSystem.current.SetSelectedGameObject(_chatDisplay.gameObject);
                    return true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(inputFieldRect, mousePosition) ||
                        RectTransformUtility.RectangleContainsScreenPoint(contentRect, mousePosition) ||
                        (scrollbarRect != null && RectTransformUtility.RectangleContainsScreenPoint(scrollbarRect, mousePosition)))
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

        private void DownloadChatHistory()
        {
            try
            {
                DateTime timestamp = DateTime.UtcNow;
                string filename = $"chat_history_{timestamp:yyyy-MM-dd_HH-mm-ss}.txt";
                
                string downloadsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );
                string filePath = Path.Combine(downloadsPath, filename);
                
                // Collect chat content
                string chatContent = string.Join("\n", _allLines.Select(line => 
                    System.Text.RegularExpressions.Regex.Replace(line, "<.*?>", string.Empty)
                ));
                
                // Calculate hash of content
                string hash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(chatContent);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
                
                // Store hash and timestamp
                _hashHistory[hash] = timestamp;
                
                // Create file content with hash header
                string fileContent = $"[HASH:{hash}]\n[TIME:{timestamp:yyyy-MM-dd HH:mm:ss UTC}]\n\n{chatContent}";
                
                // Write to file
                File.WriteAllText(filePath, fileContent);
                File.SetAttributes(filePath, FileAttributes.ReadOnly);
                
                // Notify user
                ChatManager.AddLine($"<color=#FFC800>Chat history saved to Downloads/{filename}</color>");
            }
            catch (Exception e)
            {
                ChatManager.AddLine("<color=#FF0000>Failed to save chat history.</color>");
            }
        }
    }
}