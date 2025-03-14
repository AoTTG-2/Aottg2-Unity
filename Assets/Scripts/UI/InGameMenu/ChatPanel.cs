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
using Photon.Pun;
using Photon.Realtime;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private const int POOL_SIZE = 20;
        private TMP_InputField _inputField;
        private GameObject _panel;
        private ChatScrollRect _scrollRect;
        private RectTransform _chatPanelRect;
        private RectTransform _inputFieldRect;
        private RectTransform _contentRect;
        private RectTransform _scrollbarRect;
        private Transform _caret;
        private readonly List<TMP_InputField> _linesPool = new List<TMP_InputField>();
        private readonly Dictionary<GameObject, TMP_InputField> _cachedInputFields = new Dictionary<GameObject, TMP_InputField>();
        private readonly List<string> _allMessages = new List<string>();
        private GameObject _currentSelectedObject;
        private bool _caretInitialized;
        private int _currentLineIndex = 0;
        protected override string ThemePanel => "ChatPanel";
        public bool IgnoreNextActivation;
        private static readonly Regex _richTextPattern = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.Compiled);
        private float _lastTypeTime = 0f;
        private const float TYPING_DEBOUNCE = 0.2f;
        private bool _requestCanvasUpdate;
        private Player _currentPMTarget;
        private string _pmPrefix;
        private bool _inPMMode;
        private List<Player> _pmPartners = new List<Player>();
        private int _currentPMIndex = -1;
        private bool _pmToggleActive = false;
        private Coroutine _pmToggleCoroutine;
        private GameObject _emojiPanel;
        private bool _emojiPanelActive = false;
        private Button _emojiButton;

        // First, expand UIAnchors with all needed combinations
        private static class UIAnchors
        {
            // Stretch anchors
            public static readonly Vector2 TopStretch = new Vector2(0, 1);      // Anchored to top, stretches horizontally
            public static readonly Vector2 TopStretchEnd = new Vector2(1, 1);   // End point for top stretch
            public static readonly Vector2 FullStretch = Vector2.one;           // Stretches in both directions
            public static readonly Vector2 FullStretchStart = Vector2.zero;     // Start point for full stretch
            public static readonly Vector2 RightStretch = new Vector2(1, 0);    // Anchored to right, stretches vertically
            public static readonly Vector2 RightStretchEnd = new Vector2(1, 1); // End point for right stretch
            
            // Center anchors
            public static readonly Vector2 CenterMiddle = new Vector2(0.5f, 0.5f); // Centered both ways
            public static readonly Vector2 TopCenter = new Vector2(0.5f, 1f);      // Centered horizontally, anchored to top
            public static readonly Vector2 RightCenter = new Vector2(1f, 0.5f);    // Centered vertically, anchored to right
            
            // Corner anchors
            public static readonly Vector2 BottomLeft = Vector2.zero;           // Anchored to bottom-left
            public static readonly Vector2 TopRight = Vector2.one;             // Anchored to top-right
        }

        public override void Setup(BasePanel parent = null)
        {
            // Find the existing InputField
            var oldInputField = transform.Find("InputField").GetComponent<InputField>();
            
            // Create a new TMP_InputField to replace it
            var inputFieldGO = new GameObject("TMPInputField", typeof(RectTransform), typeof(TMP_InputField));
            inputFieldGO.transform.SetParent(transform, false);
            
            // Copy the RectTransform properties
            var oldRect = oldInputField.GetComponent<RectTransform>();
            var newRect = inputFieldGO.GetComponent<RectTransform>();
            newRect.anchorMin = oldRect.anchorMin;
            newRect.anchorMax = oldRect.anchorMax;
            newRect.pivot = oldRect.pivot;
            newRect.sizeDelta = oldRect.sizeDelta;
            newRect.anchoredPosition = oldRect.anchoredPosition;
            
            // Setup the TMP_InputField
            _inputField = inputFieldGO.GetComponent<TMP_InputField>();
            
            // Create text area with RectMask2D
            var textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(inputFieldGO.transform, false);
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = UIAnchors.FullStretchStart;
            textAreaRect.anchorMax = UIAnchors.FullStretch;
            textAreaRect.sizeDelta = Vector2.zero;
            
            // Add RectMask2D to prevent text overflow
            textArea.AddComponent<RectMask2D>();
            
            // Create text component with proper settings
            var textComponent = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textComponent.transform.SetParent(textArea.transform, false);
            var textRect = textComponent.GetComponent<RectTransform>();
            textRect.anchorMin = UIAnchors.FullStretchStart;
            textRect.anchorMax = UIAnchors.FullStretch;
            textRect.sizeDelta = Vector2.zero;
            var tmpText = textComponent.GetComponent<TextMeshProUGUI>();
            
            // Configure TextMeshPro settings with UI settings font size
            tmpText.enableWordWrapping = false;
            tmpText.overflowMode = TextOverflowModes.ScrollRect;
            tmpText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmpText.fontSize = SettingsManager.UISettings.ChatFontSize.Value;
            
            // Setup TMP_InputField with proper settings
            _inputField.textViewport = textAreaRect;
            _inputField.textComponent = tmpText;
            _inputField.lineType = TMP_InputField.LineType.SingleLine;
            _inputField.inputType = TMP_InputField.InputType.Standard;
            _inputField.scrollSensitivity = 60f;
            
            // Modify the paste event handler and add escape key handling
            _inputField.onValidateInput += (text, charIndex, addedChar) =>
            {
                // Replace any line break characters or escape with nothing
                if (addedChar == '\n' || addedChar == '\r' || addedChar == '\u001B' || addedChar == (char)27)
                    return '\0';  // Return null character which will be ignored
                return addedChar;
            };
            
            // Add paste handler to strip rich text (except sprites) and line breaks
            _inputField.onFocusSelectAll = false;  // Prevent auto-selecting all text on focus
            _inputField.onSelect.AddListener((value) => {
                if (GUIUtility.systemCopyBuffer.Length > 0)
                {
                    string clipText = GUIUtility.systemCopyBuffer;
                    // Keep sprite tags but remove other rich text tags and escape characters
                    clipText = Regex.Replace(clipText, @"<(?!sprite=)[^>]+>|</[^>]+>", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\u001B", string.Empty); // Remove escape character
                    GUIUtility.systemCopyBuffer = clipText;
                }
            });
            
            // Disable navigation as a backup measure
            var nav = _inputField.navigation;
            nav.mode = Navigation.Mode.None;
            _inputField.navigation = nav;
            
            // Add event listeners
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onEndEdit.AddListener(OnEndEdit);
            
            // Set a fixed height for the input field
            _inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
            
            // Ensure the text area has proper masking and layout
            var textAreaRectMask = textArea.GetComponent<RectMask2D>();
            if (textAreaRectMask == null)
            {
                textAreaRectMask = textArea.AddComponent<RectMask2D>();
            }
            
            // Adjust text area padding to prevent text from touching the edges
            textAreaRect.offsetMin = new Vector2(5, 2);
            textAreaRect.offsetMax = new Vector2(-40, -2);
            
            // Copy colors from old input field
            var style = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            var colors = UIManager.GetThemeColorBlock(style.ThemePanel, "InputField", "Input");
            _inputField.colors = colors;
            tmpText.color = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputTextColor");
            _inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputSelectionColor");
            
            // Set background image
            var backgroundGO = new GameObject("Background", typeof(RectTransform), typeof(Image));
            backgroundGO.transform.SetParent(inputFieldGO.transform, false);
            backgroundGO.transform.SetAsFirstSibling();
            var bgRect = backgroundGO.GetComponent<RectTransform>();
            bgRect.anchorMin = UIAnchors.FullStretchStart;
            bgRect.anchorMax = UIAnchors.FullStretch;
            bgRect.sizeDelta = Vector2.zero;
            var bgImage = backgroundGO.GetComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
            
            // Destroy the old input field
            Destroy(oldInputField.gameObject);
            
            // Continue with the rest of the setup
            _panel = transform.Find("Content/Panel").gameObject;
            _chatPanelRect = _panel.GetComponent<RectTransform>();
            _inputFieldRect = _inputField.GetComponent<RectTransform>();
            _contentRect = transform.Find("Content").GetComponent<RectTransform>();
            _scrollbarRect = transform.Find("Content/Scrollbar")?.GetComponent<RectTransform>();
            _scrollRect = GetComponentInChildren<ChatScrollRect>();
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 0f);
            _inputField.text = "";
            if (SettingsManager.UISettings.ChatWidth.Value == 0f)
            {
                _inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            }
            
            // Setup emoji button after input field is initialized
            SetupEmojiButton();
            
            var fontAsset = Resources.Load<TMP_FontAsset>("UI/Fonts/Vegur-Regular-SDF");
            for (int i = 0; i < POOL_SIZE; i++)
            {
                TMP_InputField lineObj = CreateLine(string.Empty);
                var textComponent2 = lineObj.textComponent as TextMeshProUGUI;
                textComponent2.font = fontAsset;
                lineObj.fontAsset = fontAsset;
                lineObj.gameObject.SetActive(false);
                _linesPool.Add(lineObj);
            }
            Sync();

            var content = transform.Find("Content");
            var contentRect = content.GetComponent<RectTransform>();
            var layoutElement = content.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            var scrollbarGo = new GameObject("Scrollbar", typeof(RectTransform));
            scrollbarGo.transform.SetParent(content, false);
            scrollbarGo.SetActive(true);
            var scrollbarRect = scrollbarGo.GetComponent<RectTransform>();
            scrollbarRect.anchorMin = UIAnchors.RightStretch;
            scrollbarRect.anchorMax = UIAnchors.RightStretchEnd;
            scrollbarRect.pivot = UIAnchors.RightCenter;
            scrollbarRect.sizeDelta = new Vector2(10, 0);
            var scrollbar = scrollbarGo.AddComponent<Scrollbar>();
            var slidingArea = new GameObject("Sliding Area", typeof(RectTransform));
            slidingArea.transform.SetParent(scrollbarGo.transform, false);
            var slidingAreaRect = slidingArea.GetComponent<RectTransform>();
            slidingAreaRect.anchorMin = UIAnchors.FullStretchStart;
            slidingAreaRect.anchorMax = UIAnchors.FullStretch;
            slidingAreaRect.sizeDelta = Vector2.zero;
            var handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            handle.transform.SetParent(slidingArea.transform, false);
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = UIAnchors.FullStretchStart;
            handleRect.anchorMax = UIAnchors.FullStretch;
            handleRect.sizeDelta = Vector2.zero;
            var handleImage = handle.GetComponent<Image>();
            handleImage.color = new Color(0.8f, 0.8f, 0.8f, 0.4f);
            scrollbar.handleRect = handleRect;
            scrollbar.targetGraphic = handleImage;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            var scrollRect = content.gameObject.AddComponent<ChatScrollRect>();
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            scrollRect.viewport = contentRect;
            scrollRect.content = _panel.GetComponent<RectTransform>();
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarSpacing = -3;
            var eventTrigger = content.gameObject.AddComponent<EventTrigger>();
            var enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { scrollRect.OnMouseEnter(); });
            eventTrigger.triggers.Add(enterEntry);
            var exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { scrollRect.OnMouseExit(); });
            eventTrigger.triggers.Add(exitEntry);
            var panelRect = _panel.GetComponent<RectTransform>();
            panelRect.offsetMax = new Vector2(-12, panelRect.offsetMax.y);
        }

        private void SetupEmojiButton()
        {
            // Create emoji button
            var emojiButtonGo = new GameObject("EmojiButton", typeof(RectTransform), typeof(Image), typeof(Button));
            emojiButtonGo.transform.SetParent(_inputField.transform, false);
            var emojiButtonRect = emojiButtonGo.GetComponent<RectTransform>();
            emojiButtonRect.anchorMin = UIAnchors.RightCenter;
            emojiButtonRect.anchorMax = UIAnchors.RightCenter;
            emojiButtonRect.pivot = UIAnchors.RightCenter;
            emojiButtonRect.sizeDelta = new Vector2(30, 30);
            emojiButtonRect.anchoredPosition = new Vector2(0, 0);
            
            // Set button appearance
            var emojiButtonImage = emojiButtonGo.GetComponent<Image>();
            emojiButtonImage.color = new Color(0.0f, 0.0f, 0.0f, 0.6f);
            
            // Add click handler
            _emojiButton = emojiButtonGo.GetComponent<Button>();
            _emojiButton.onClick.AddListener(ToggleEmojiPanel);
            
            // Adjust the text area to make room for the button
            var textArea = _inputField.textViewport.gameObject;
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.offsetMax = new Vector2(-40, textAreaRect.offsetMax.y); // Add right padding for the button
            
            // Create emoji panel (initially inactive)
            CreateEmojiPanel();
        }

        private void CreateEmojiPanel()
        {
            // Create panel as a direct child of the canvas
            _emojiPanel = new GameObject("EmojiPanel", typeof(RectTransform), typeof(Image));
            _emojiPanel.transform.SetParent(UIManager.CurrentMenu.transform, false);
            var emojiPanelRect = _emojiPanel.GetComponent<RectTransform>();
            
            // Position it with bottom-left pivot
            emojiPanelRect.anchorMin = UIAnchors.BottomLeft;
            emojiPanelRect.anchorMax = UIAnchors.BottomLeft;
            emojiPanelRect.pivot = UIAnchors.BottomLeft;
            
            // Calculate panel size based on grid content and padding
            float cellSize = 40f;
            float spacing = 5f;
            float padding = 15f; // Consistent padding for all sides
            float gridWidth = (cellSize * 4) + (spacing * 3); // 4 columns with 3 spaces between
            float gridHeight = (cellSize * 4) + (spacing * 3); // 4 rows with 3 spaces between
            float totalWidth = gridWidth + (padding * 2); // Add padding to both sides
            float totalHeight = gridHeight + (padding * 2); // Add padding to top and bottom
            
            // Set panel size to accommodate grid and padding
            emojiPanelRect.sizeDelta = new Vector2(totalWidth, totalHeight);
            
            // Set panel appearance with dark background
            var emojiPanelImage = _emojiPanel.GetComponent<Image>();
            emojiPanelImage.color = new Color(0.118f, 0.118f, 0.118f, 0.95f);
            
            // Add emoji grid
            var emojiGrid = new GameObject("EmojiGrid", typeof(RectTransform), typeof(GridLayoutGroup));
            emojiGrid.transform.SetParent(_emojiPanel.transform, false);
            var gridRect = emojiGrid.GetComponent<RectTransform>();
            gridRect.anchorMin = UIAnchors.FullStretchStart;
            gridRect.anchorMax = UIAnchors.FullStretch;
            gridRect.sizeDelta = Vector2.zero;
            
            // Set consistent padding on all sides
            gridRect.offsetMin = new Vector2(padding, padding);
            gridRect.offsetMax = new Vector2(-padding, -padding);
            
            var gridLayout = emojiGrid.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(spacing, spacing);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 4;
            
            // Add emoji buttons
            AddEmojiButtons(emojiGrid.transform);
            
            // Initially hide the panel
            _emojiPanel.SetActive(false);
        }

        private void AddEmojiButtons(Transform parent)
        {
            // Load the sprite asset once
            var spriteAsset = Resources.Load<TMP_SpriteAsset>("UI/Emojis/EmojiOne.asset");
            
            // Create buttons for sprite indices 0-15
            for (int i = 0; i < 16; i++)
            {
                var emojiButtonGo = new GameObject($"EmojiButton_{i}", typeof(RectTransform), typeof(Button), typeof(Image));
                emojiButtonGo.transform.SetParent(parent, false);
                
                // Set button appearance
                var emojiButtonImage = emojiButtonGo.GetComponent<Image>();
                emojiButtonImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                
                // Create text component with sprite
                var emojiText = new GameObject("EmojiText", typeof(RectTransform), typeof(TextMeshProUGUI));
                emojiText.transform.SetParent(emojiButtonGo.transform, false);
                var emojiTextRect = emojiText.GetComponent<RectTransform>();
                
                // Set proper anchors and pivot
                emojiTextRect.anchorMin = UIAnchors.CenterMiddle;
                emojiTextRect.anchorMax = UIAnchors.CenterMiddle;
                emojiTextRect.pivot = UIAnchors.CenterMiddle;
                
                // Set a fixed size for the text area
                emojiTextRect.sizeDelta = new Vector2(30, 30);
                emojiTextRect.anchoredPosition = Vector2.zero;
                
                var tmpText = emojiText.GetComponent<TextMeshProUGUI>();
                tmpText.text = $"<sprite={i}>";
                tmpText.fontSize = 24;
                tmpText.alignment = TextAlignmentOptions.Center;
                tmpText.enableAutoSizing = true;
                tmpText.fontSizeMin = 12;
                tmpText.fontSizeMax = 24;
                tmpText.overflowMode = TextOverflowModes.Overflow;
                tmpText.margin = new Vector4(0, 0, 0, 0);
                tmpText.spriteAsset = spriteAsset;
                tmpText.richText = true;
                
                // Add click handler
                var button = emojiButtonGo.GetComponent<Button>();
                int spriteIndex = i;
                button.onClick.AddListener(() => InsertEmoji(spriteIndex.ToString()));
            }
        }

        private void ToggleEmojiPanel()
        {
            _emojiPanelActive = !_emojiPanelActive;
            _emojiPanel.SetActive(_emojiPanelActive);
            
            if (_emojiPanelActive)
            {
                // Get the emoji button's position in screen space
                RectTransform buttonRect = _emojiButton.GetComponent<RectTransform>();
                Vector3[] corners = new Vector3[4];
                buttonRect.GetWorldCorners(corners);
                
                // Position panel to the right of the button, aligned with its bottom
                _emojiPanel.transform.position = new Vector3(
                    corners[2].x, // Right side of button
                    corners[0].y, // Bottom of button
                    corners[2].z
                );
                
                // Make sure panel is in front
                _emojiPanel.transform.SetAsLastSibling();
            }
        }

        private void InsertEmoji(string spriteTag)
        {
            // Make sure input field is active and focused
            _inputField.ActivateInputField();
            _inputField.Select();
            
            // Get current text and insert emoji
            string text = _inputField.text;
            string formattedTag = $"<sprite={spriteTag}>";
            int insertPosition = _inputField.isFocused ? _inputField.caretPosition : text.Length;
            string newText = text.Insert(insertPosition, formattedTag);
            
            // Check length limits
            int maxLength = _inputField.characterLimit > 0 ? _inputField.characterLimit : int.MaxValue;
            if (newText.Length > maxLength)
            {
                return;
            }
            
            // Set the text without triggering notifications
            _inputField.SetTextWithoutNotify(newText);
            
            // Update caret position
            int newCaretPos = insertPosition + formattedTag.Length;
            _inputField.caretPosition = newCaretPos;
            _inputField.selectionAnchorPosition = newCaretPos;
            _inputField.selectionFocusPosition = newCaretPos;
            
            // Keep input field focused
            _inputField.ActivateInputField();
        }

        public void Sync()
        {
            ValidatePMState();
            // Don't clear _allMessages, just refresh the display
            RefreshDisplayedMessages();
        }

        private void RefreshDisplayedMessages()
        {
            List<string> linesToShow = new List<string>();
            
            if (_inPMMode && _currentPMTarget != null)
            {
                // Add PM header
                linesToShow.Add(ChatManager.GetFormattedMessage(
                    $"{ChatManager.GetColorString("Private chat with ", ChatTextColor.System)}{ChatManager.GetPlayerIdentifier(_currentPMTarget)}", 
                    DateTime.Now, 
                    true));
                    
                // Filter for PM messages with current target
                for (int i = 0; i < ChatManager.RawMessages.Count; i++)
                {
                    if (ChatManager.PrivateFlags[i] && 
                        (ChatManager.PMPartnerIDs[i] == _currentPMTarget.ActorNumber ||
                         ChatManager.SenderIDs[i] == _currentPMTarget.ActorNumber))
                    {
                        linesToShow.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.SuggestionFlags[i]));
                    }
                }
            }
            else
            {
                // Show non-private and system messages
                for (int i = 0; i < ChatManager.RawMessages.Count; i++)
                {
                    if (!ChatManager.PrivateFlags[i] || ChatManager.SystemFlags[i])
                    {
                        linesToShow.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.SuggestionFlags[i]));
                    }
                }
            }
            
            UpdateVisibleMessages(linesToShow);
        }

        private void UpdateVisibleMessages()
        {
            UpdateVisibleMessages(_allMessages);
        }

        private void UpdateVisibleMessages(List<string> lines)
        {
            if (lines.Count == 0)
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
            int totalMessages = lines.Count;
            float size = Mathf.Min(1f, (float)POOL_SIZE / totalMessages);
            if (_scrollRect && _scrollRect.verticalScrollbar)
            {
                _scrollRect.verticalScrollbar.size = size;
            }
            
            int startIndex = 0;
            if (totalMessages > POOL_SIZE)
            {
                int maxStartIndex = totalMessages - POOL_SIZE;
                startIndex = scrollPos > 0f ? Mathf.Clamp(Mathf.FloorToInt((1f - scrollPos) * maxStartIndex), 0, maxStartIndex) : maxStartIndex;
            }
            
            bool needsCanvasUpdate = false;
            for (int i = 0; i < POOL_SIZE; i++)
            {
                var lineObj = _linesPool[i];
                int messageIndex = startIndex + i;
                if (messageIndex < lines.Count)
                {
                    bool wasActive = lineObj.gameObject.activeSelf;
                    string newText = lines[messageIndex];
                    
                    if (!wasActive)
                    {
                        lineObj.gameObject.SetActive(true);
                        needsCanvasUpdate = true;
                    }
                    if (lineObj.text != newText)
                    {
                        lineObj.text = newText;
                        needsCanvasUpdate = true;
                    }
                    if (lineObj.transform.GetSiblingIndex() != i)
                    {
                        lineObj.transform.SetSiblingIndex(i);
                        needsCanvasUpdate = true;
                    }
                }
                else if (lineObj.gameObject.activeSelf)
                {
                    lineObj.gameObject.SetActive(false);
                    needsCanvasUpdate = true;
                }
            }
            
            if (needsCanvasUpdate)
            {
                _requestCanvasUpdate = true;
            }
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
            // Only process if Enter/Return was pressed
            if (!Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.KeypadEnter))
            {
                IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
                return;
            }

            string input = _inputField.text;
            
            // Don't process empty messages
            if (string.IsNullOrWhiteSpace(input))
            {
                IgnoreNextActivation = true;
                _inputField.DeactivateInputField();
                return;
            }

            // Send message based on mode (PM or normal)
            if (_inPMMode && _currentPMTarget != null)
            {
                ChatManager.SendPrivateMessage(_currentPMTarget, input);
            }
            else
            {
                ChatManager.HandleInput(input);
            }

            // Clear input and deactivate field
            _inputField.text = "";
            _inputField.DeactivateInputField();
            
            // Update the chat display
            Sync();
            
            // Set ignore flag for next activation
            IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
        }

        public void AddLine(string line)
        {
            // Simply add to master list
            _allMessages.Add(line);
            
            // Refresh display using current mode logic
            RefreshDisplayedMessages();
        }

        public void ReplaceLastLine(string line)
        {
            int lastIndex = (_currentLineIndex - 1 + POOL_SIZE) % POOL_SIZE;
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
            if (_inputField != null && !_caretInitialized)
            {
                _caret = _inputField.transform.Find(_inputField.transform.name + " Input Caret");
                if (_caret)
                {
                    if (!_caret.TryGetComponent<Graphic>(out _))
                    {
                        _caret.gameObject.AddComponent<Image>();
                    }
                    _caretInitialized = true;
                }
            }
        }

        private void OnGUI()
        {
            if (IsInputActive())
            {
                Event e = Event.current;
                if (e.type == EventType.KeyDown)
                {
                    if (e.keyCode == KeyCode.Tab)
                    {
                        e.Use();
                        
                        if (_inPMMode && _pmPartners.Count > 0)
                        {
                            _currentPMIndex = (_currentPMIndex + 1) % _pmPartners.Count;
                            EnterPMMode(_pmPartners[_currentPMIndex]);
                        }
                        else
                        {
                            ChatManager.HandleTabComplete();
                        }
                        
                        SetInputText(_inputField.text);
                    }
                    else if (e.keyCode == KeyCode.Escape)
                    {
                        e.Use();
                        if (_inPMMode || _pmPartners.Count > 0)
                        {
                            if (_inPMMode)
                            {
                                ExitPMMode();
                            }
                            else if (_pmPartners.Count > 0)
                            {
                                _currentPMIndex = _pmPartners.Count - 1;
                                EnterPMMode(_pmPartners[_currentPMIndex]);
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (!_caretInitialized && _inputField != null)
            {
                InitializeCaret();
                return;
            }
            bool isInputActive = IsInputActive();
            GameObject newSelectedObject = EventSystem.current.currentSelectedGameObject;
            
            // Close emoji panel when cursor is not in pointer mode
            if (_emojiPanelActive && CursorManager.State != CursorState.Pointer)
            {
                CloseEmojiPanel();
            }
            
            if (newSelectedObject != _currentSelectedObject)
            {
                _currentSelectedObject = newSelectedObject;
            }

            if (Time.unscaledTime - _lastTypeTime >= TYPING_DEBOUNCE)
            {
                ChatManager.HandleTyping(_inputField.text);
            }
            if (_requestCanvasUpdate)
            {
                _requestCanvasUpdate = false;
                Canvas.ForceUpdateCanvases();
            }
        }

        public bool IsPointerOverChatUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePosition = Input.mousePosition;
                // Add check for emoji panel
                if (_emojiPanel != null && _emojiPanel.activeSelf && 
                    RectTransformUtility.RectangleContainsScreenPoint(_emojiPanel.GetComponent<RectTransform>(), mousePosition))
                {
                    return true;
                }
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
            var lineGO = new GameObject("ChatLine", typeof(RectTransform));
            lineGO.transform.SetParent(_panel.transform, false);
            var inputField = lineGO.AddComponent<TMP_InputField>();
            var textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(lineGO.transform, false);
            var textComponent = textArea.AddComponent<TextMeshProUGUI>();
            inputField.textComponent = textComponent;
            inputField.textViewport = textArea.GetComponent<RectTransform>();
            inputField.readOnly = true;
            inputField.richText = true;
            inputField.onFocusSelectAll = false;
            inputField.resetOnDeActivation = false;
            inputField.restoreOriginalTextOnEscape = false;
            inputField.selectionStringAnchorPosition = 0;
            inputField.selectionStringFocusPosition = 0;
            inputField.shouldHideMobileInput = true;
            textComponent.fontSize = SettingsManager.UISettings.ChatFontSize.Value;
            textComponent.color = UIManager.GetThemeColor(style.ThemePanel, "TextColor", "Default");
            textComponent.alignment = TextAlignmentOptions.Left;
            textComponent.enableWordWrapping = true;
            textComponent.richText = true;
            textComponent.enableKerning = true;
            textComponent.isTextObjectScaleStatic = false;
            var clickHandler = lineGO.AddComponent<ChatLineClickHandler>();
            clickHandler.Initialize(textComponent, _inputField);
            var rectTransform = lineGO.GetComponent<RectTransform>();
            rectTransform.anchorMin = UIAnchors.TopStretch;
            rectTransform.anchorMax = UIAnchors.TopStretchEnd;
            rectTransform.pivot = UIAnchors.TopCenter;
            rectTransform.sizeDelta = new Vector2(0, 30);
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = UIAnchors.FullStretchStart;
            textAreaRect.anchorMax = UIAnchors.FullStretch;
            textAreaRect.sizeDelta = Vector2.zero;
            textAreaRect.anchoredPosition = Vector2.zero;
            inputField.text = text;
            return inputField;
        }

        private class ChatLineClickHandler : MonoBehaviour, IPointerClickHandler
        {
            private TextMeshProUGUI _textComponent;
            private TMP_InputField _chatInput;

            public void Initialize(TextMeshProUGUI textComponent, TMP_InputField chatInput)
            {
                _textComponent = textComponent;
                _chatInput = chatInput;
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, eventData.position, null);
                if (linkIndex != -1)
                {
                    TMP_LinkInfo linkInfo = _textComponent.textInfo.linkInfo[linkIndex];
                    string linkID = linkInfo.GetLinkID();
                    
                    if (int.TryParse(linkID, out int playerID))
                    {
                        if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
                            return;
                        var player = PhotonNetwork.CurrentRoom.GetPlayer(playerID);
                        if (player != null)
                        {
                            ChatPanel panel = _chatInput.transform.parent.GetComponent<ChatPanel>();
                            if (panel != null)
                            {
                                panel.EnterPMMode(player);
                            }
                        }
                    }
                }
            }
        }

        private void OnScroll(Vector2 scrollPosition)
        {
            UpdateVisibleMessages();
        }

        private TMP_InputField GetCachedInputField(GameObject obj)
        {
            if (obj == null) return null;
            
            if (!_cachedInputFields.TryGetValue(obj, out TMP_InputField inputField))
            {
                inputField = obj.GetComponent<TMP_InputField>();
                if (inputField != null)
                {
                    _cachedInputFields[obj] = inputField;
                }
            }
            return inputField;
        }

        private void OnDestroy()
        {
            _cachedInputFields.Clear();
        }

        private void OnValueChanged(string text)
        {
            _lastTypeTime = Time.unscaledTime;
            if (text == null)
                text = string.Empty;
            ChatManager.HandleTyping(text);
        }

        public string GetInputText()
        {
            return _inputField.text;
        }

        public void SetInputText(string newText)
        {
            if (_inputField != null)
            {
                _inputField.text = newText;
                
                // Force immediate update of text and caret
                _inputField.textComponent.ForceMeshUpdate();
                _inputField.caretPosition = newText.Length;
                _inputField.selectionAnchorPosition = newText.Length;
                _inputField.selectionFocusPosition = newText.Length;
                
                // Ensure focus and visual state are correct
                _inputField.ActivateInputField();
                _inputField.ForceLabelUpdate();
                
                // Schedule a delayed final caret update
                StartCoroutine(DelayedCaretUpdate());
            }
        }

        private IEnumerator DelayedCaretUpdate()
        {
            yield return new WaitForEndOfFrame();
            
            if (_inputField != null)
            {
                _inputField.caretPosition = _inputField.text.Length;
                _inputField.selectionAnchorPosition = _inputField.text.Length;
                _inputField.selectionFocusPosition = _inputField.text.Length;
                _inputField.ForceLabelUpdate();
            }
        }

        public void EnterPMMode(Player target)
        {
            if (target == null || target.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            if (_inPMMode && _currentPMTarget != null && _currentPMTarget.ActorNumber == target.ActorNumber)
                return;
            _currentPMTarget = target;
            _inPMMode = true;
            _pmToggleActive = true;
            if (_pmToggleCoroutine != null)
                StopCoroutine(_pmToggleCoroutine);
            _pmToggleCoroutine = StartCoroutine(ResetPMToggleActive());
            AddPMPartner(target);
            _inputField.text = "";
            _inputField.Select();
            _inputField.ActivateInputField();
            Sync();
        }

        public void ExitPMMode()
        {
            _currentPMTarget = null;
            _inPMMode = false;
            _currentPMIndex = -1;
            _pmToggleActive = true;
            if (_pmToggleCoroutine != null)
                StopCoroutine(_pmToggleCoroutine);
            _pmToggleCoroutine = StartCoroutine(ResetPMToggleActive());
            _inputField.text = "";
            _inputField.Select();
            _inputField.ActivateInputField();
            Sync();
        }

        public void RemovePMPartner(Player player)
        {
            if (player == null)
                return;
            int index = _pmPartners.FindIndex(p => p.ActorNumber == player.ActorNumber);
            if (index == -1)
                return;
            _pmPartners.RemoveAt(index);
            if (_pmPartners.Count == 0)
            {
                ExitPMMode();
                return;
            }
            if (_currentPMTarget != null && _currentPMTarget.ActorNumber == player.ActorNumber)
            {
                if (_pmPartners.Count > 0)
                {
                    _currentPMIndex = _pmPartners.Count - 1;
                    EnterPMMode(_pmPartners[_currentPMIndex]);
                }
                else
                {
                    ExitPMMode();
                }
            }
            else
            {
                if (_currentPMIndex >= index)
                {
                    _currentPMIndex = Math.Max(0, _currentPMIndex - 1);
                }
            }
        }

        private void ValidatePMState()
        {
            _pmPartners.RemoveAll(p => p == null || !PhotonNetwork.CurrentRoom.Players.ContainsValue(p));
            if (_currentPMTarget != null && 
                !PhotonNetwork.CurrentRoom.Players.ContainsValue(_currentPMTarget))
            {
                _currentPMTarget = null;
            }
            if (_inPMMode && _currentPMTarget == null)
            {
                ExitPMMode();
            }
            _currentPMIndex = Math.Min(_currentPMIndex, _pmPartners.Count - 1);
        }

        public bool IsTogglingPM()
        {
            return _pmToggleActive;
        }

        private IEnumerator ResetPMToggleActive()
        {
            yield return new WaitForSeconds(0.2f);
            _pmToggleActive = false;
        }

        public bool IsInPMMode() => _inPMMode;

        public void AddPMPartner(Player player)
        {
            if (player == null || player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            if (!_pmPartners.Exists(p => p.ActorNumber == player.ActorNumber))
            {
                _pmPartners.Add(player);
                _currentPMIndex = _pmPartners.Count - 1;
            }
        }

        public Player GetCurrentPMTarget()
        {
            return _currentPMTarget;
        }

        public void ResetPMState()
        {
            _inPMMode = false;
            _currentPMTarget = null;
            _pmPartners.Clear();
            _currentPMIndex = -1;
            _pmToggleActive = false;
            if (_pmToggleCoroutine != null)
                StopCoroutine(_pmToggleCoroutine);
        }

        // Change CloseEmojiPanel from private to public
        public void CloseEmojiPanel()
        {
            _emojiPanelActive = false;
            _emojiPanel.SetActive(false);
        }

        // Add this method to handle cursor state changes
        public void HandleCursorStateChange(CursorState newState)
        {
            if (newState != CursorState.Pointer && _emojiPanelActive)
            {
                CloseEmojiPanel();
            }
        }

        // Add this method back, but make it just call SetInputText
        public void MoveCaretToEnd()
        {
            if (_inputField != null)
            {
                SetInputText(_inputField.text);
            }
        }
    }
}

