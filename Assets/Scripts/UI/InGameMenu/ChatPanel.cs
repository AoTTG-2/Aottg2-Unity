using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Settings;
using GameManagers;
using TMPro;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System.Collections;
using System;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private const int POOL_SIZE = 20;
        private InputField _inputField;
        private GameObject _panel;
        private readonly List<TMP_InputField> _linesPool = new List<TMP_InputField>();
        private int _oldestIndex = 0;
        private List<string> _allMessages = new List<string>();
        private ChatScrollRect _scrollRect;
        protected override string ThemePanel => "ChatPanel";
        protected Transform _caret;
        public bool IgnoreNextActivation;
        private bool _caretInitialized = false;
        private GameObject _currentSelectedObject;
        
        // Cache these regex patterns as static readonly fields at class level
        private static readonly Regex _richTextPattern = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.Compiled);

        // Cache RectTransforms used in IsPointerOverChatUI
        private RectTransform _chatPanelRect;
        private RectTransform _inputFieldRect;
        private RectTransform _contentRect;
        private RectTransform _scrollbarRect;

        // Add this field at class level
        public Dictionary<string, DateTime> _hashHistory = new Dictionary<string, DateTime>();

        // Add just this one field
        private GameObject _suggestionsObject;

        public override void Setup(BasePanel parent = null)
        {
            _inputField = transform.Find("InputField").GetComponent<InputField>();
            _panel = transform.Find("Content/Panel").gameObject;
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            var style = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            _inputField.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "InputField", "Input");
            _inputField.transform.Find("Text").GetComponent<Text>().color =
                UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputTextColor");
            _inputField.selectionColor =
                UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputSelectionColor");
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 0f);
            _inputField.onEndEdit.AddListener((string text) => OnEndEdit(text));
            _inputField.text = "";
            if (SettingsManager.UISettings.ChatWidth.Value == 0f)
            {
                _inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            }

            // Load the font asset once
            var fontAsset = Resources.Load<TMP_FontAsset>("UI/Fonts/Vegur-Regular-SDF");

            // Pre-create our fixed-size pool of line objects
            for (int i = 0; i < POOL_SIZE; i++)
            {
                TMP_InputField lineObj = CreateLine(string.Empty);
                // Apply the font to each pooled line
                var textComponent = lineObj.textComponent as TextMeshProUGUI;
                textComponent.font = fontAsset;
                lineObj.fontAsset = fontAsset;
                
                lineObj.gameObject.SetActive(false);
                _linesPool.Add(lineObj);
            }
            Sync();

            var content = transform.Find("Content");
            var contentRect = content.GetComponent<RectTransform>();
            var layoutElement = content.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = SettingsManager.UISettings.ChatHeight.Value;

            // Create Scrollbar GameObject with RectTransform
            var scrollbarGo = new GameObject("Scrollbar", typeof(RectTransform));
            scrollbarGo.transform.SetParent(content, false);
            scrollbarGo.SetActive(true);
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
            
            // Add ChatScrollRect to Content
            var scrollRect = content.gameObject.AddComponent<ChatScrollRect>();
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            scrollRect.viewport = contentRect;
            scrollRect.content = _panel.GetComponent<RectTransform>();
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarSpacing = -3;
            
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

            // Adjust panel rect for scrollbar
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.offsetMax = new Vector2(-12, panelRect.offsetMax.y);

            // Cache RectTransforms
            _chatPanelRect = _panel.GetComponent<RectTransform>();
            _inputFieldRect = _inputField.GetComponent<RectTransform>();
            _contentRect = transform.Find("Content").GetComponent<RectTransform>();
            _scrollbarRect = transform.Find("Content/Scrollbar")?.GetComponent<RectTransform>();

            // Create minimal suggestions object (invisible)
            _suggestionsObject = new GameObject("SuggestionsObject");
            _suggestionsObject.transform.SetParent(transform, false);
        }

    
        public void Sync()
        {
            _allMessages.Clear();
            
            foreach (var line in ChatManager.Lines)
            {
                _allMessages.Add(line.GetFormattedMessage());
            }
            
            UpdateVisibleMessages();
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
            ChatManager.ResetTabCompletion(); // Reset tab completion state
            ChatManager.ClearLastSuggestions(); // Add this to clear suggestions
            IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
            ChatManager.HandleInput(input);
        }

        // Also clear suggestions when input is deactivated
        public void OnDeactivate()
        {
            ChatManager.ClearLastSuggestions();
        }

        public void AddLine(string line)
        {
            _allMessages.Add(line);
            
            UpdateVisibleMessages();
        }

        public void ReplaceLastLine(string line)
        {
            int lastIndex = _oldestIndex - 1;
            if (lastIndex < 0)
                lastIndex = POOL_SIZE - 1;

            TMP_InputField lineObj = _linesPool[lastIndex];
            if (!lineObj.gameObject.activeSelf)
            {
                AddLine(line);
            }
            else
            {
                lineObj.text = line;
                Canvas.ForceUpdateCanvases();
            }
        }

        public void AddLines(List<string> lines)
        {
            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        private void InitializeCaret()
        {
            if (_inputField != null)
            {
                _caret = _inputField.transform.Find(_inputField.transform.name + " Input Caret");
                if (_caret)
                {
                    var graphic = _caret.GetComponent<Graphic>();
                    if (!graphic)
                        _caret.gameObject.AddComponent<Image>();
                    _caretInitialized = true;
                }
            }
        }

        private void Update()
        {
            // Only initialize caret once
            if (!_caretInitialized && _inputField != null)
            {
                InitializeCaret();
            }

            // Add mouse click handling
            if (Input.GetMouseButtonDown(0) && !IsPointerOverChatUI() && IsInputActive())
            {
                _inputField.DeactivateInputField();
            }

            // Cache and check selected object only when it changes
            GameObject newSelectedObject = EventSystem.current.currentSelectedGameObject;
            if (newSelectedObject != _currentSelectedObject)
            {
                _currentSelectedObject = newSelectedObject;
            }

            // Only clean clipboard if we're copying from a chat line
            if (_currentSelectedObject != null && 
                _currentSelectedObject.GetComponent<TMP_InputField>() != null && 
                _currentSelectedObject.transform.IsChildOf(_panel.transform))
            {
                CleanClipboardIfNeeded();
            }
            // Add this back to get dynamic suggestions
            if (IsInputActive() )
            {
                ChatManager.GetAutocompleteSuggestion(_inputField.text);
            }
            // Handle tab completion
            if (IsInputActive() && Input.GetKeyDown(KeyCode.Tab))
            {
                string completion = ChatManager.GetNextTabCompletion(_inputField.text);
                if (completion != null)
                {
                    _inputField.text = completion;
                    _inputField.caretPosition = _inputField.text.Length;
                }
            }
        }

        private void CleanClipboardIfNeeded()
        {
            string clipboardText = GUIUtility.systemCopyBuffer;
            if (!string.IsNullOrEmpty(clipboardText))
            {
                // Only clean if there are actually rich text tags to remove
                if (clipboardText.Contains("<") && clipboardText.Contains(">"))
                {
                    string cleanText = _richTextPattern.Replace(clipboardText, string.Empty);
                    if (cleanText != clipboardText)
                    {
                        GUIUtility.systemCopyBuffer = cleanText;
                    }
                }
            }
        }

        public bool IsPointerOverChatUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePosition = Input.mousePosition;

                // Use cached RectTransforms
                if (RectTransformUtility.RectangleContainsScreenPoint(_chatPanelRect, mousePosition))
                {
                    return true;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(_inputFieldRect, mousePosition) ||
                        RectTransformUtility.RectangleContainsScreenPoint(_contentRect, mousePosition) ||
                        (_scrollbarRect != null && RectTransformUtility.RectangleContainsScreenPoint(_scrollbarRect, mousePosition)))
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

        protected TMP_InputField CreateLine(string text)
        {
            var style = new ElementStyle(fontSize: SettingsManager.UISettings.ChatFontSize.Value, themePanel: ThemePanel);
            
            // Create the main GameObject for the line
            var lineGO = new GameObject("ChatLine", typeof(RectTransform));
            lineGO.transform.SetParent(_panel.transform, false);
            var inputField = lineGO.AddComponent<TMP_InputField>();

            // Create text area
            var textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(lineGO.transform, false);
            var textComponent = textArea.AddComponent<TextMeshProUGUI>();

            // Configure the chatlines (TMP_InputField)
            inputField.textComponent = textComponent;
            inputField.textViewport = textArea.GetComponent<RectTransform>();
            inputField.readOnly = true;
            inputField.richText = true;
            inputField.onFocusSelectAll = false;
            inputField.resetOnDeActivation = false;
            inputField.restoreOriginalTextOnEscape = false;
            
            textComponent.fontSize = SettingsManager.UISettings.ChatFontSize.Value;
            textComponent.color = UIManager.GetThemeColor(style.ThemePanel, "TextColor", "Default");
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.enableWordWrapping = true;
            
            var rectTransform = lineGO.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.sizeDelta = new Vector2(0, 30);
            
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.sizeDelta = Vector2.zero;
            textAreaRect.anchoredPosition = Vector2.zero;
            inputField.text = text;
            
            return inputField;
        }

        private void UpdateVisibleMessages()
        {
            if (_allMessages.Count == 0)
            {
                foreach (var lineObj in _linesPool)
                {
                    lineObj.gameObject.SetActive(false);
                }
                if (_scrollRect && _scrollRect.verticalScrollbar)
                {
                    _scrollRect.verticalScrollbar.size = 1;
                }
                return;
            }

            float scrollPos = _scrollRect?.verticalNormalizedPosition ?? 0f;
            int totalMessages = _allMessages.Count;
            
            // Cache pool size calculations
            float size = Mathf.Min(1f, (float)POOL_SIZE / totalMessages);
            if (_scrollRect && _scrollRect.verticalScrollbar)
            {
                _scrollRect.verticalScrollbar.size = size;
            }
            
            // Calculate start index with bounds checking
            int startIndex = 0;
            if (totalMessages > POOL_SIZE)
            {
                int maxStartIndex = totalMessages - POOL_SIZE;
                startIndex = scrollPos > 0f 
                    ? Mathf.Clamp(Mathf.FloorToInt((1f - scrollPos) * maxStartIndex), 0, maxStartIndex)
                    : maxStartIndex;
            }

            // Update pool objects
            for (int i = 0; i < POOL_SIZE; i++)
            {
                var lineObj = _linesPool[i];
                int messageIndex = startIndex + i;
                
                if (messageIndex < totalMessages)
                {
                    if (!lineObj.gameObject.activeSelf)
                    {
                        lineObj.gameObject.SetActive(true);
                    }
                    lineObj.text = _allMessages[messageIndex];
                    lineObj.transform.SetSiblingIndex(i);
                }
                else if (lineObj.gameObject.activeSelf)
                {
                    lineObj.gameObject.SetActive(false);
                }
            }

            Canvas.ForceUpdateCanvases();
        }

        private void OnScroll(Vector2 scrollPosition)
        {
            UpdateVisibleMessages();
        }
    }
}
