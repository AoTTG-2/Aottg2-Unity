﻿using System.Collections.Generic;
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
using System.Linq;
using ApplicationManagers;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private int POOL_SIZE => SettingsManager.UISettings.ChatPoolSize.Value;
        private TMP_InputField _inputField;
        private GameObject _panel;
        private ChatScrollRect _scrollRect;
        private RectTransform _chatPanelRect;
        private RectTransform _inputFieldRect;
        private RectTransform _contentRect;
        private RectTransform _scrollbarRect;
        private Transform _caret;
        protected readonly List<TMP_InputField> _linesPool = new List<TMP_InputField>();
        private readonly Dictionary<GameObject, TMP_InputField> _cachedInputFields = new Dictionary<GameObject, TMP_InputField>();
        private readonly List<string> _allMessages = new List<string>();
        private GameObject _currentSelectedObject;
        private bool _caretInitialized;
        private int _currentLineIndex = 0;
        protected override string ThemePanel => "ChatPanel";
        public bool IgnoreNextActivation;
        private static readonly Regex _richTextPattern = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.Compiled);
        private static readonly Regex _emojiPattern = new Regex(@":([^:\s]+):", RegexOptions.Compiled);
        private float _lastTypeTime = 0f;
        private const float TYPING_DEBOUNCE = 0.2f;
        private bool _requestCanvasUpdate;
        private Player _currentPMTarget;
        private bool _inPMMode;
        private List<Player> _pmPartners = new List<Player>();
        private int _currentPMIndex = -1;
        private bool _pmToggleActive = false;
        private Coroutine _pmToggleCoroutine;
        private GameObject _emojiPanel;
        private bool _emojiPanelActive = false;
        private Button _emojiButton;
        private bool _wasChatUIClicked = false;
        private bool _isInteractingWithChatUI = false;
        private Dictionary<GameObject, RectTransform> _cachedRectTransforms = new Dictionary<GameObject, RectTransform>();
        private int _desiredCaretPosition = 0;
        private static readonly Dictionary<string, int> EmojiNameToIndex = new Dictionary<string, int>();
        private int _emojiPage = 0;
        private const int EMOJIS_PER_PAGE = 16;
        private const int MAX_EMOJI_INDEX = 140;
        private Button _emojiNextButton;
        private Button _emojiBackButton;
        private TextMeshProUGUI _emojiPageText;
        private int _actualPoolSize = 0;
        static ChatPanel()
        {
            for (int i = 0; i <= MAX_EMOJI_INDEX; i++)
            {
                EmojiNameToIndex[i.ToString()] = i;
            }
        }

        private string ProcessEmojiCodes(string text)
        {
            return _emojiPattern.Replace(text, match =>
            {
                string emojiName = match.Groups[1].Value.ToLower();
                if (EmojiNameToIndex.TryGetValue(emojiName, out int spriteIndex))
                {
                    return $"<sprite={spriteIndex}>";
                }
                return match.Value;
            });
        }

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
            public static readonly Vector2 LeftCenter = new Vector2(0f, 0f);     // Centered vertically, anchored to left
            
            // Corner anchors
            public static readonly Vector2 BottomLeft = Vector2.zero;           // Anchored to bottom-left
            public static readonly Vector2 TopRight = Vector2.one;             // Anchored to top-right
        }

        public override void Setup(BasePanel parent = null)
        {
            var oldInputField = transform.Find("InputField").GetComponent<InputField>();
            var inputFieldGO = new GameObject("TMPInputField", typeof(RectTransform), typeof(TMP_InputField));
            inputFieldGO.transform.SetParent(transform, false);
            var oldRect = oldInputField.GetComponent<RectTransform>();
            var newRect = inputFieldGO.GetComponent<RectTransform>();
            newRect.anchorMin = oldRect.anchorMin;
            newRect.anchorMax = oldRect.anchorMax;
            newRect.pivot = oldRect.pivot;
            newRect.sizeDelta = oldRect.sizeDelta;
            newRect.anchoredPosition = oldRect.anchoredPosition;
            _inputField = inputFieldGO.GetComponent<TMP_InputField>();
            _inputField.characterLimit = 500;
            var textArea = new GameObject("Text Area", typeof(RectTransform));
            textArea.transform.SetParent(inputFieldGO.transform, false);
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = UIAnchors.FullStretchStart;
            textAreaRect.anchorMax = UIAnchors.FullStretch;
            textAreaRect.sizeDelta = Vector2.zero;
            textArea.AddComponent<RectMask2D>();
            var textComponent = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textComponent.transform.SetParent(textArea.transform, false);
            var textRect = textComponent.GetComponent<RectTransform>();
            textRect.anchorMin = UIAnchors.FullStretchStart;
            textRect.anchorMax = UIAnchors.FullStretch;
            textRect.sizeDelta = Vector2.zero;
            var tmpText = textComponent.GetComponent<TextMeshProUGUI>();
            tmpText.enableWordWrapping = false;
            tmpText.overflowMode = TextOverflowModes.ScrollRect;
            tmpText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmpText.fontSize = 20.0f;
            _inputField.textViewport = textAreaRect;
            _inputField.textComponent = tmpText;
            _inputField.lineType = TMP_InputField.LineType.SingleLine;
            _inputField.inputType = TMP_InputField.InputType.Standard;
            _inputField.scrollSensitivity = 60f;
            // Add paste handler to strip rich text (except sprites) and line breaks
            _inputField.onValidateInput += (text, charIndex, addedChar) =>
            {
                if (addedChar == '\n' || addedChar == '\r' || addedChar == '\u001B' || addedChar == (char)27)
                    return '\0';
                return addedChar;
            };
            _inputField.onFocusSelectAll = false;
            _inputField.onSelect.AddListener((value) => {
                if (GUIUtility.systemCopyBuffer.Length > 0)
                {
                    string clipText = GUIUtility.systemCopyBuffer;
                    clipText = Regex.Replace(clipText, @"<(?!sprite=)[^>]+>|</[^>]+>", string.Empty) // Keep sprite tags
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\u001B", string.Empty);
                    GUIUtility.systemCopyBuffer = clipText;
                }
            });
            var nav = _inputField.navigation;
            nav.mode = Navigation.Mode.None;
            _inputField.navigation = nav;
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onEndEdit.AddListener(OnEndEdit);
            _inputField.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
            var textAreaRectMask = textArea.GetComponent<RectMask2D>();
            if (textAreaRectMask == null)
            {
                textAreaRectMask = textArea.AddComponent<RectMask2D>();
            }
            textAreaRect.offsetMin = new Vector2(5, 2);
            textAreaRect.offsetMax = new Vector2(-40, -2);
            var style = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            var colors = UIManager.GetThemeColorBlock(style.ThemePanel, "InputField", "Input");
            _inputField.colors = colors;
            tmpText.color = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputTextColor");
            _inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "InputField", "InputSelectionColor");
            var backgroundGO = new GameObject("Background", typeof(RectTransform), typeof(Image));
            backgroundGO.transform.SetParent(inputFieldGO.transform, false);
            backgroundGO.transform.SetAsFirstSibling();
            var bgRect = backgroundGO.GetComponent<RectTransform>();
            bgRect.anchorMin = UIAnchors.FullStretchStart;
            bgRect.anchorMax = UIAnchors.FullStretch;
            bgRect.sizeDelta = Vector2.zero;
            var bgImage = backgroundGO.GetComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
            Destroy(oldInputField.gameObject);
            _panel = transform.Find("Content/Panel").gameObject;
            _chatPanelRect = _panel.GetComponent<RectTransform>();
            _inputFieldRect = _inputField.GetComponent<RectTransform>();
            _contentRect = transform.Find("Content").GetComponent<RectTransform>();
            _scrollbarRect = transform.Find("Content/Scrollbar")?.GetComponent<RectTransform>();
            _scrollRect = GetComponentInChildren<ChatScrollRect>();
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 0f);
            var (text, caretPos) = ChatManager.GetConversation("PUBLIC");
            if (!string.IsNullOrEmpty(text))
            {
                _inputField.text = text;
                _desiredCaretPosition = caretPos;
            }
            else
            {
                var (preservedText, preservedCaret) = ChatManager.GetPreservedInputWithCaret();
                if (!string.IsNullOrEmpty(preservedText))
                {
                    _inputField.text = preservedText;
                    _desiredCaretPosition = preservedCaret;
                }
            }
            SetupEmojiButton();
            var fontAsset = Resources.Load<TMP_FontAsset>("UI/Fonts/Vegur-Regular-SDF");
            foreach (var lineObj in _linesPool)
            {
                if (lineObj != null && lineObj.gameObject != null)
                    Destroy(lineObj.gameObject);
            }
            _linesPool.Clear();
            for (int i = 0; i < POOL_SIZE; i++)
            {
                TMP_InputField lineObj = CreateLine(string.Empty);
                var textComponent2 = lineObj.textComponent as TextMeshProUGUI;
                textComponent2.font = fontAsset;
                lineObj.fontAsset = fontAsset;
                lineObj.gameObject.SetActive(false);
                _linesPool.Add(lineObj);
            }
            _actualPoolSize = _linesPool.Count;
            Sync();
            var content = transform.Find("Content");
            var contentRect = content.GetComponent<RectTransform>();
            var layoutElement = content.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            var panelImage = _panel.GetComponent<Image>();
            if (panelImage == null)
            {
                panelImage = _panel.AddComponent<Image>();
            }
            panelImage.color = SettingsManager.UISettings.ChatBackgroundColor.Value.ToColor();
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
            scrollRect.scrollSensitivity = SettingsManager.UISettings.ChatScrollSensitivity.Value;
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
        }

        private void SetupEmojiButton()
        {
            var emojiButtonGo = new GameObject("EmojiButton", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(Button));
            emojiButtonGo.transform.SetParent(_inputField.transform, false);
            var emojiButtonRect = emojiButtonGo.GetComponent<RectTransform>();
            emojiButtonRect.anchorMin = UIAnchors.RightCenter;
            emojiButtonRect.anchorMax = UIAnchors.RightCenter;
            emojiButtonRect.pivot = UIAnchors.RightCenter;
            emojiButtonRect.sizeDelta = new Vector2(30, 26);
            emojiButtonRect.anchoredPosition = new Vector2(-2, 0);

            var tmpText = emojiButtonGo.GetComponent<TextMeshProUGUI>();
            tmpText.text = "☺";
            tmpText.fontSize = 24;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmpText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            tmpText.margin = new Vector4(0, 0, 0, 0);
            tmpText.color = new Color(1f, 1f, 1f, 0.5f);
            _emojiButton = emojiButtonGo.GetComponent<Button>();
            _emojiButton.onClick.AddListener(ToggleEmojiPanel);
            var textArea = _inputField.textViewport.gameObject;
            var textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.offsetMin = new Vector2(5, 2);
            textAreaRect.offsetMax = new Vector2(-33, -2);
        }

        private void ToggleEmojiPanel()
        {
            if (_emojiPanel == null)
            {
                CreateEmojiPanel();
            }
            _emojiPanelActive = !_emojiPanelActive;
            _emojiPanel.SetActive(_emojiPanelActive);
            if (_emojiPanelActive)
            {
                RectTransform inputFieldRect = _inputField.GetComponent<RectTransform>();
                Vector3[] corners = new Vector3[4];
                inputFieldRect.GetWorldCorners(corners);
                _emojiPanel.transform.position = new Vector3(
                    corners[2].x, // Right edge of input field
                    corners[0].y, // Bottom of input field
                    corners[2].z
                );
                _emojiPanel.transform.SetAsLastSibling();
            }
        }

        private void CreateEmojiPanel()
        {
            _emojiPanel = new GameObject("EmojiPanel", typeof(RectTransform), typeof(Image));
            _emojiPanel.transform.SetParent(UIManager.CurrentMenu.transform, false);
            var emojiPanelRect = _emojiPanel.GetComponent<RectTransform>();
            emojiPanelRect.anchorMin = UIAnchors.BottomLeft;
            emojiPanelRect.anchorMax = UIAnchors.BottomLeft;
            emojiPanelRect.pivot = UIAnchors.BottomLeft;
            float cellSize = 40f;
            float spacing = 5f;
            float padding = 15f;
            float tooltipHeight = 7f;
            float navButtonHeight = 15f;
            float gridWidth = (cellSize * 4) + (spacing * 3);
            float gridHeight = (cellSize * 4) + (spacing * 3);
            float totalWidth = gridWidth + (padding * 2);
            float totalHeight = gridHeight + (padding * 2) + tooltipHeight + navButtonHeight;
            emojiPanelRect.sizeDelta = new Vector2(totalWidth, totalHeight);
            var emojiPanelImage = _emojiPanel.GetComponent<Image>();
            emojiPanelImage.color = new Color(0.118f, 0.118f, 0.118f, 0.95f);
            var tooltipGo = new GameObject("TooltipArea", typeof(RectTransform), typeof(TextMeshProUGUI));
            tooltipGo.transform.SetParent(_emojiPanel.transform, false);
            var tooltipRect = tooltipGo.GetComponent<RectTransform>();
            tooltipRect.anchorMin = new Vector2(0, 1);
            tooltipRect.anchorMax = new Vector2(1, 1);
            tooltipRect.pivot = new Vector2(0.5f, 1f);
            tooltipRect.sizeDelta = new Vector2(0, tooltipHeight);
            tooltipRect.anchoredPosition = new Vector2(0, -padding + 8);
            var tooltipText = tooltipGo.GetComponent<TextMeshProUGUI>();
            tooltipText.fontSize = 14;
            tooltipText.color = new Color(1f, 1f, 1f, 0.8f);
            tooltipText.alignment = TextAlignmentOptions.Center;
            tooltipText.text = "";
            var emojiGrid = new GameObject("EmojiGrid", typeof(RectTransform), typeof(GridLayoutGroup));
            emojiGrid.transform.SetParent(_emojiPanel.transform, false);
            var gridRect = emojiGrid.GetComponent<RectTransform>();
            gridRect.anchorMin = UIAnchors.FullStretchStart;
            gridRect.anchorMax = UIAnchors.FullStretch;
            gridRect.sizeDelta = Vector2.zero;
            gridRect.offsetMin = new Vector2(padding, padding + navButtonHeight);
            gridRect.offsetMax = new Vector2(-padding, -(padding + tooltipHeight));
            var gridLayout = emojiGrid.GetComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridLayout.spacing = new Vector2(spacing, spacing);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 4;
            var navPanel = new GameObject("NavPanel", typeof(RectTransform));
            navPanel.transform.SetParent(_emojiPanel.transform, false);
            var navRect = navPanel.GetComponent<RectTransform>();
            navRect.anchorMin = new Vector2(0, 0);
            navRect.anchorMax = new Vector2(1, 0);
            navRect.pivot = new Vector2(0.5f, 0f);
            navRect.sizeDelta = new Vector2(0, navButtonHeight);
            navRect.anchoredPosition = new Vector2(0, padding / 2);
            var backBtnGo = new GameObject("BackButton", typeof(RectTransform), typeof(Button), typeof(TextMeshProUGUI));
            backBtnGo.transform.SetParent(navPanel.transform, false);
            var backBtnRect = backBtnGo.GetComponent<RectTransform>();
            backBtnRect.anchorMin = new Vector2(0, 0);
            backBtnRect.anchorMax = new Vector2(0, 1);
            backBtnRect.pivot = new Vector2(0, 0.5f);
            backBtnRect.sizeDelta = new Vector2(60, navButtonHeight);
            backBtnRect.anchoredPosition = new Vector2(10, 0);
            var backBtnText = backBtnGo.GetComponent<TextMeshProUGUI>();
            backBtnText.text = "<";
            backBtnText.fontSize = 14;
            backBtnText.alignment = TextAlignmentOptions.Center;
            backBtnText.color = Color.white;
            _emojiBackButton = backBtnGo.GetComponent<Button>();
            _emojiBackButton.onClick.AddListener(() => ChangeEmojiPage(-1, tooltipText));
            var pageTextGo = new GameObject("PageText", typeof(RectTransform), typeof(TextMeshProUGUI));
            pageTextGo.transform.SetParent(navPanel.transform, false);
            var pageTextRect = pageTextGo.GetComponent<RectTransform>();
            pageTextRect.anchorMin = new Vector2(0.5f, 0);
            pageTextRect.anchorMax = new Vector2(0.5f, 1);
            pageTextRect.pivot = new Vector2(0.5f, 0.5f);
            pageTextRect.sizeDelta = new Vector2(60, navButtonHeight);
            pageTextRect.anchoredPosition = new Vector2(0, 0);
            _emojiPageText = pageTextGo.GetComponent<TextMeshProUGUI>();
            _emojiPageText.fontSize = 14;
            _emojiPageText.alignment = TextAlignmentOptions.Center;
            _emojiPageText.color = Color.white;
            var nextBtnGo = new GameObject("NextButton", typeof(RectTransform), typeof(Button), typeof(TextMeshProUGUI));
            nextBtnGo.transform.SetParent(navPanel.transform, false);
            var nextBtnRect = nextBtnGo.GetComponent<RectTransform>();
            nextBtnRect.anchorMin = new Vector2(1, 0);
            nextBtnRect.anchorMax = new Vector2(1, 1);
            nextBtnRect.pivot = new Vector2(1, 0.5f);
            nextBtnRect.sizeDelta = new Vector2(60, navButtonHeight);
            nextBtnRect.anchoredPosition = new Vector2(-10, 0);
            var nextBtnText = nextBtnGo.GetComponent<TextMeshProUGUI>();
            nextBtnText.text = ">";
            nextBtnText.fontSize = 14;
            nextBtnText.alignment = TextAlignmentOptions.Center;
            nextBtnText.color = Color.white;
            _emojiNextButton = nextBtnGo.GetComponent<Button>();
            _emojiNextButton.onClick.AddListener(() => ChangeEmojiPage(1, tooltipText));
            AddEmojiButtons(tooltipText);
            _emojiPanel.SetActive(false);
        }

        private void AddEmojiButtons(TextMeshProUGUI tooltipText)
        {
            var emojiGrid = _emojiPanel.transform.Find("EmojiGrid").GetComponent<GridLayoutGroup>();
            foreach (Transform child in emojiGrid.transform)
            {
                Destroy(child.gameObject);
            }
            int start = _emojiPage * EMOJIS_PER_PAGE;
            int end = Mathf.Min(start + EMOJIS_PER_PAGE, MAX_EMOJI_INDEX + 1);
            for (int i = start; i < end; i++)
            {
                var buttonGo = new GameObject($"EmojiButton_{i}", typeof(RectTransform), typeof(Button), typeof(Image));
                buttonGo.transform.SetParent(emojiGrid.transform, false);
                var buttonImage = buttonGo.GetComponent<Image>();
                buttonImage.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                var emojiText = new GameObject("EmojiText", typeof(RectTransform), typeof(TextMeshProUGUI));
                emojiText.transform.SetParent(buttonGo.transform, false);
                var emojiTextRect = emojiText.GetComponent<RectTransform>();
                emojiTextRect.anchorMin = UIAnchors.CenterMiddle;
                emojiTextRect.anchorMax = UIAnchors.CenterMiddle;
                emojiTextRect.pivot = UIAnchors.CenterMiddle;
                emojiTextRect.sizeDelta = new Vector2(38, 38);
                emojiTextRect.anchoredPosition = new Vector2(0, 0);
                var tmpText = emojiText.GetComponent<TextMeshProUGUI>();
                tmpText.text = $"<sprite={i}>";
                tmpText.fontSize = 30;
                tmpText.alignment = TextAlignmentOptions.Center;
                tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
                tmpText.enableAutoSizing = false;
                tmpText.color = new Color(1f, 1f, 1f, 1f);
                var button = buttonGo.GetComponent<Button>();
                int spriteIndex = i;
                button.onClick.AddListener(() => InsertEmoji(spriteIndex));
                var tooltipTrigger = buttonGo.AddComponent<EventTrigger>();
                var enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                enterEntry.callback.AddListener((data) => { tooltipText.text = $":{spriteIndex}:"; });
                tooltipTrigger.triggers.Add(enterEntry);
                var exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                exitEntry.callback.AddListener((data) => { tooltipText.text = ""; });
                tooltipTrigger.triggers.Add(exitEntry);
            }
            _emojiBackButton.interactable = _emojiPage > 0;
            _emojiNextButton.interactable = (end <= MAX_EMOJI_INDEX);
            if (_emojiPageText != null)
            {
                int maxPage = (MAX_EMOJI_INDEX + 1 + EMOJIS_PER_PAGE - 1) / EMOJIS_PER_PAGE;
                _emojiPageText.text = $"{_emojiPage + 1}/{maxPage}";
            }
        }
        private void ChangeEmojiPage(int delta, TextMeshProUGUI tooltipText)
        {
            int maxPage = (MAX_EMOJI_INDEX + 1 + EMOJIS_PER_PAGE - 1) / EMOJIS_PER_PAGE;
            _emojiPage = Mathf.Clamp(_emojiPage + delta, 0, maxPage - 1);
            AddEmojiButtons(tooltipText);
        }

        private void InsertEmoji(int spriteIndex)
        {
            _inputField.ActivateInputField();
            _inputField.Select();
            string text = _inputField.text;
            string emojiCode = $":{spriteIndex}:";
            
            int insertPosition = text.Length;
            string newText = text.Insert(insertPosition, emojiCode);
            int maxLength = _inputField.characterLimit > 0 ? _inputField.characterLimit : int.MaxValue;
            if (newText.Length > maxLength)
            {
                return;
            }
            string processedText = ProcessEmojiCodes(newText);
            _inputField.SetTextWithoutNotify(processedText);
            int newCaretPos = newText.Length;
            _inputField.caretPosition = newCaretPos;
            _inputField.selectionAnchorPosition = newCaretPos;
            _inputField.selectionFocusPosition = newCaretPos;
            _inputField.ActivateInputField();
        }

        public void Sync()
        {
            RefreshPoolSize();
            ValidatePMState();
            RestorePMPartners();
            RefreshDisplayedMessages();
        }

        private void RefreshDisplayedMessages()
        {
            List<string> regularMessages = new List<string>();
            List<string> suggestions = new List<string>();
            List<string> notifications = new List<string>();
            for (int i = 0; i < ChatManager.RawMessages.Count; i++)
            {
                if (_inPMMode && _currentPMTarget != null)
                {
                    if (ChatManager.PrivateFlags[i] && 
                        (ChatManager.PMPartnerIDs[i] == _currentPMTarget.ActorNumber ||
                         ChatManager.SenderIDs[i] == _currentPMTarget.ActorNumber))
                    {
                        regularMessages.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.SuggestionFlags[i]));
                    }
                }
                else
                {
                    if (ChatManager.NotificationFlags[i])
                    {
                        notifications.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.NotificationFlags[i]));
                    }
                    else if (ChatManager.SuggestionFlags[i])
                    {
                        suggestions.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.SuggestionFlags[i]));
                    }
                    else if ((!ChatManager.PrivateFlags[i] || ChatManager.SystemFlags[i]) && 
                             !ChatManager.SuggestionFlags[i] && !ChatManager.NotificationFlags[i])
                    {
                        regularMessages.Add(ChatManager.GetFormattedMessage(
                            ChatManager.RawMessages[i],
                            ChatManager.Timestamps[i],
                            ChatManager.SuggestionFlags[i]));
                    }
                }
            }
            List<string> linesToShow = new List<string>();
            if (_inPMMode && _currentPMTarget != null)
            {
                linesToShow.Add(ChatManager.GetFormattedMessage(
                    $"{ChatManager.GetColorString("Private chat with ", ChatTextColor.System)}{ChatManager.GetPlayerIdentifier(_currentPMTarget)}", 
                    DateTime.Now, 
                    true));
            }
            linesToShow.AddRange(regularMessages);
            linesToShow.AddRange(notifications);
            linesToShow.AddRange(suggestions);
            UpdateVisibleMessages(linesToShow);
        }

        private void UpdateVisibleMessages()
        {
            UpdateVisibleMessages(_allMessages);
        }

        private int GetEffectivePoolSize()
        {
            int configuredSize = SettingsManager.UISettings.ChatPoolSize.Value;
            return configuredSize == 0 ? _linesPool.Count : configuredSize;
        }

        private void UpdateVisibleMessages(List<string> lines)
        {
            int effectivePoolSize = GetEffectivePoolSize();
            
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
                UpdateBackgroundVisibility(false);
                return;
            }
            float scrollPos = _scrollRect?.verticalNormalizedPosition ?? 0f;
            int totalMessages = lines.Count;
            float size = Mathf.Min(1f, (float)effectivePoolSize / totalMessages);
            if (_scrollRect && _scrollRect.verticalScrollbar)
            {
                _scrollRect.verticalScrollbar.size = size;
            }
            int startIndex = 0;
            if (totalMessages > effectivePoolSize)
            {
                int maxStartIndex = totalMessages - effectivePoolSize;
                startIndex = scrollPos > 0f ? Mathf.Clamp(Mathf.FloorToInt((1f - scrollPos) * maxStartIndex), 0, maxStartIndex) : maxStartIndex;
            }
            bool needsCanvasUpdate = false;
            for (int i = 0; i < _linesPool.Count; i++)
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
            UpdateBackgroundVisibility(true);
            if (needsCanvasUpdate)
            {
                _requestCanvasUpdate = true;
            }
        }

        private void UpdateBackgroundVisibility(bool hasMessages)
        {
            var panelImage = _panel.GetComponent<Image>();
            if (panelImage != null)
            {
                if (hasMessages)
                {
                    panelImage.color = SettingsManager.UISettings.ChatBackgroundColor.Value.ToColor();
                }
                else
                {
                    panelImage.color = Color.clear;
                }
            }
        }

        public void Activate()
        {
            _inputField.Select();
            _inputField.ActivateInputField();
            if (_desiredCaretPosition > 0 && !string.IsNullOrEmpty(_inputField.text))
            {
                int clampedCaretPos = Mathf.Clamp(_desiredCaretPosition, 0, _inputField.text.Length);
                _inputField.caretPosition = clampedCaretPos;
                _inputField.selectionAnchorPosition = clampedCaretPos;
                _inputField.selectionFocusPosition = clampedCaretPos;
                _desiredCaretPosition = 0;
            }
            if (!string.IsNullOrEmpty(_inputField.text))
            {
                ChatManager.ForceSuggestionRefresh();
                ChatManager.HandleTyping(_inputField.text);
            }
        }

        public bool IsInputActive()
        {
            return _inputField.isFocused;
        }

        public void OnEndEdit(string text)
        {
            if (!Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.KeypadEnter))
            {
                Vector2 mousePosition = Input.mousePosition;
                bool isOverChatUI = IsMouseOverAnyChatElement(mousePosition) || 
                                   (_emojiPanel != null && _emojiPanel.activeSelf && 
                                    RectTransformUtility.RectangleContainsScreenPoint(GetCachedRectTransform(_emojiPanel), mousePosition));
                if (!isOverChatUI)
                {
                    ChatManager.ClearLastSuggestions();
                }
                IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
                return;
            }
            string input = ProcessEmojiCodes(_inputField.text);
            if (string.IsNullOrWhiteSpace(input))
            {
                ChatManager.ClearLastSuggestions();
                IgnoreNextActivation = true;
                _inputField.text = "";
                _inputField.DeactivateInputField();
                return;
            }
            ChatManager.ClearLastSuggestions();
            if (_inPMMode && _currentPMTarget != null)
            {
                ChatManager.SendPrivateMessage(_currentPMTarget, input);
                ChatManager.ClearConversation($"PM_{_currentPMTarget.ActorNumber}");
            }
            else
            {
                ChatManager.HandleInput(input);
                ChatManager.ClearConversation("PUBLIC");
            }
            _inputField.text = "";
            _inputField.DeactivateInputField();
            Sync();
            IgnoreNextActivation = SettingsManager.InputSettings.General.Chat.ContainsEnter();
        }

        public void AddLine(string line)
        {
            _allMessages.Add(line);
            RefreshDisplayedMessages();
        }

        public void ReplaceLastLine(string line)
        {
            int lastIndex = (_currentLineIndex - 1 + _actualPoolSize) % _actualPoolSize;
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
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                bool canHandlePMKeys = IsInputActive() || _isInteractingWithChatUI;
                if (e.keyCode == KeyCode.Tab)
                {
                    if (IsInputActive())
                    {
                        e.Use();
                        if (_inPMMode && _pmPartners.Count > 0)
                        {
                            CycleToPMPartner();
                        }
                        else
                        {
                            ChatManager.HandleTabComplete();
                        }
                    }
                    else if (_inPMMode && _pmPartners.Count > 0)
                    {
                        e.Use();
                        CycleToPMPartner();
                    }
                }
                else if (e.keyCode == KeyCode.Escape && canHandlePMKeys)
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
                    else if (IsInputActive())
                    {
                        ChatManager.ClearLastSuggestions();
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
            if (Input.GetMouseButtonDown(0) && !_wasChatUIClicked && EventSystem.current.IsPointerOverGameObject())
            {
                _isInteractingWithChatUI = false;
                ChatManager.ClearLastSuggestions();
            }
            UpdateChatInteractionState();
        }

        public bool IsPointerOverChatUI()
        {
            if (EventSystem.current == null)
                return false;
            Vector2 mousePosition = Input.mousePosition;
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();
            bool isOverChatUI = isOverUI && (
                IsMouseOverAnyChatElement(mousePosition) ||
                (_emojiPanel != null && _emojiPanel.activeSelf && 
                 RectTransformUtility.RectangleContainsScreenPoint(GetCachedRectTransform(_emojiPanel), mousePosition))
            );
            if (Input.GetMouseButtonDown(0))
            {
                _wasChatUIClicked = isOverChatUI;
                if (!isOverChatUI)
                {
                    _isInteractingWithChatUI = false;
                    ChatManager.ClearLastSuggestions();
                    IgnoreNextActivation = false;
                }
            }
            if (isOverChatUI)
            {
                _isInteractingWithChatUI = true;
            }
            return _isInteractingWithChatUI;
        }
        
        private bool IsMouseOverAnyChatElement(Vector2 mousePosition)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_chatPanelRect, mousePosition) ||
                RectTransformUtility.RectangleContainsScreenPoint(_inputFieldRect, mousePosition) ||
                RectTransformUtility.RectangleContainsScreenPoint(_contentRect, mousePosition) ||
                (_scrollbarRect != null && RectTransformUtility.RectangleContainsScreenPoint(_scrollbarRect, mousePosition)))
            {
                return true;
            }
            foreach (var line in _linesPool)
            {
                if (line.gameObject.activeSelf)
                {
                    var rectTransform = GetCachedRectTransform(line.gameObject);
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
                    {
                        return true;
                    }
                }
            }
            if (_emojiButton != null)
            {
                var rectTransform = GetCachedRectTransform(_emojiButton.gameObject);
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
                {
                    return true;
                }
            }
            return false;
        }
        
        public void UpdateChatInteractionState()
        {
            if (IsInputActive() || _emojiPanelActive)
            {
                _isInteractingWithChatUI = true;
            }
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
            clickHandler.Initialize(textComponent, _inputField, this);
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
            textAreaRect.offsetMax = new Vector2(-8, 0);
            inputField.text = text;
            return inputField;
        }

        private class ChatLineClickHandler : MonoBehaviour, IPointerClickHandler
        {
            private TextMeshProUGUI _textComponent;
            private TMP_InputField _chatInput;
            private ChatPanel _chatPanel;
            public void Initialize(TextMeshProUGUI textComponent, TMP_InputField chatInput, ChatPanel chatPanel)
            {
                _textComponent = textComponent;
                _chatInput = chatInput;
                _chatPanel = chatPanel;
            }
            
            public void OnPointerClick(PointerEventData eventData)
            {
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, eventData.position, null);
                if (linkIndex != -1)
                {
                    TMP_LinkInfo linkInfo = _textComponent.textInfo.linkInfo[linkIndex];
                    string linkID = linkInfo.GetLinkID();
                    if (linkID.StartsWith("suggestion_"))
                    {
                        string indexStr = linkID.Substring("suggestion_".Length);
                        if (int.TryParse(indexStr, out int suggestionIndex))
                        {
                            ChatManager.HandleSuggestionClick(suggestionIndex);
                            return;
                        }
                    }
                    else if (int.TryParse(linkID, out int playerID))
                    {
                        if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
                            return;
                        var player = PhotonNetwork.CurrentRoom.GetPlayer(playerID);
                        if (player != null)
                        {
                            _chatPanel.EnterPMMode(player);
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
            if (_inputField != null)
            {
                if (_inPMMode)
                {
                    SaveCurrentConversation();
                }
                ChatManager.PreserveInputText(_inputField.text, _inputField.caretPosition);
            }
            _cachedInputFields.Clear();
            _cachedRectTransforms.Clear();
        }

        private void OnValueChanged(string text)
        {
            _lastTypeTime = Time.unscaledTime;
            if (text == null)
                text = string.Empty;
            string processedText = ProcessEmojiCodes(text);
            if (processedText != text)
            {
                int caretPos = _inputField.caretPosition;
                _inputField.SetTextWithoutNotify(processedText);
                _inputField.caretPosition = caretPos;
            }
            ChatManager.HandleTyping(text);
        }

        public string GetInputText()
        {
            return _inputField.text;
        }

        public void SetInputText(string newText)
        {
            SetTextAndPositionCaret(newText);
        }

        public void EnterPMMode(Player target)
        {
            if (target == null || target.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            if (_inPMMode && _currentPMTarget != null && _currentPMTarget.ActorNumber == target.ActorNumber)
                return;
            SaveCurrentConversation();
            _currentPMTarget = target;
            _inPMMode = true;
            _pmToggleActive = true;
            if (_pmToggleCoroutine != null)
                StopCoroutine(_pmToggleCoroutine);
            _pmToggleCoroutine = StartCoroutine(ResetPMToggleActive());
            AddPMPartner(target);
            _currentPMIndex = _pmPartners.FindIndex(p => p.ActorNumber == target.ActorNumber);
            ChatManager.ClearPMNotification(target.ActorNumber);
            var (text, caretPos) = ChatManager.GetConversation($"PM_{target.ActorNumber}");
            SetTextAndCaretPosition(text, caretPos);
            Sync();
        }

        public void ExitPMMode()
        {
            SaveCurrentConversation();
            _currentPMTarget = null;
            _inPMMode = false;
            _currentPMIndex = -1;
            _pmToggleActive = true;
            if (_pmToggleCoroutine != null)
                StopCoroutine(_pmToggleCoroutine);
            _pmToggleCoroutine = StartCoroutine(ResetPMToggleActive());
            
            var (text, caretPos) = ChatManager.GetConversation("PUBLIC");
            SetTextAndCaretPosition(text, caretPos);
            
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

        private void CycleToPMPartner()
        {
            if (_pmPartners.Count == 0) return;
            int currentIndex = -1;
            if (_currentPMTarget != null)
            {
                currentIndex = _pmPartners.FindIndex(p => p.ActorNumber == _currentPMTarget.ActorNumber);
            }
            if (currentIndex == -1)
            {
                currentIndex = -1;
            }
            int nextIndex = (currentIndex + 1) % _pmPartners.Count;
            Player nextPartner = _pmPartners[nextIndex];
            SaveCurrentConversation();
            _currentPMTarget = nextPartner;
            _currentPMIndex = nextIndex;
            ChatManager.ClearPMNotification(nextPartner.ActorNumber);
            var (text, caretPos) = ChatManager.GetConversation($"PM_{nextPartner.ActorNumber}");
            SetTextAndCaretPosition(text, caretPos);
            Sync();
        }

        public void AddPMPartner(Player player)
        {
            if (player == null || player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            if (!_pmPartners.Exists(p => p.ActorNumber == player.ActorNumber))
            {
                _pmPartners.Add(player);
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

        public void CloseEmojiPanel()
        {
            if (_emojiPanel != null)
            {
                _emojiPanelActive = false;
                _emojiPanel.SetActive(false);
            }
        }

        public void HandleCursorStateChange(CursorState newState)
        {
            if (newState != CursorState.Pointer && _emojiPanelActive)
            {
                CloseEmojiPanel();
            }
        }

        public void MoveCaretToEnd()
        {
            if (_inputField != null)
            {
                string text = _inputField.text;
                int endPos = text.Length;
                _inputField.caretPosition = endPos;
                _inputField.selectionAnchorPosition = endPos;
                _inputField.selectionFocusPosition = endPos;
                _inputField.textComponent.ForceMeshUpdate();
                _inputField.ActivateInputField();
                _inputField.ForceLabelUpdate();
                Canvas.ForceUpdateCanvases();
            }
        }

        public bool ShouldBlockGameInput()
        {
            return IsInputActive() || _emojiPanelActive || _isInteractingWithChatUI;
        }

        public bool ShouldBlockKeybind(KeyCode keyCode)
        {
            if (!ShouldBlockGameInput())
                return false;
                
            if (keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter || 
                keyCode == KeyCode.Escape || keyCode == KeyCode.Tab)
                return false;
                
            return true;
        }

        public void RefreshPoolSize()
        {
            int currentPoolSize = _linesPool.Count;
            int userDefinedPoolSize = SettingsManager.UISettings.ChatPoolSize.Value;
            int targetPoolSize = userDefinedPoolSize;
            
            if (userDefinedPoolSize == 0)
            {
                float chatHeight = SettingsManager.UISettings.ChatHeight.Value;
                float fontSize = SettingsManager.UISettings.ChatFontSize.Value;
                float lineHeight = 30f * (fontSize / 18f);
                targetPoolSize = Mathf.CeilToInt((chatHeight * 2f) / lineHeight);
                targetPoolSize = Mathf.Max(targetPoolSize, 10);
            }
            
            if (currentPoolSize == targetPoolSize)
            {
                _actualPoolSize = _linesPool.Count;
                return;
            }
            if (currentPoolSize > targetPoolSize)
            {
                for (int i = currentPoolSize - 1; i >= targetPoolSize; i--)
                {
                    if (_linesPool[i] != null && _linesPool[i].gameObject != null)
                        Destroy(_linesPool[i].gameObject);
                    _linesPool.RemoveAt(i);
                }
            }
            else if (currentPoolSize < targetPoolSize)
            {
                var fontAsset = Resources.Load<TMP_FontAsset>("UI/Fonts/Vegur-Regular-SDF");
                for (int i = currentPoolSize; i < targetPoolSize; i++)
                {
                    TMP_InputField lineObj = CreateLine(string.Empty);
                    var textComponent2 = lineObj.textComponent as TextMeshProUGUI;
                    textComponent2.font = fontAsset;
                    lineObj.fontAsset = fontAsset;
                    lineObj.gameObject.SetActive(false);
                    _linesPool.Add(lineObj);
                }
            }
            _actualPoolSize = _linesPool.Count;
            RefreshDisplayedMessages();
        }

        public bool IsInteractingWithChatUI()
        {
            return _isInteractingWithChatUI || IsInputActive() || _emojiPanelActive;
        }

        public void SetTextAndPositionCaret(string newText)
        {
            if (_inputField != null)
            {
                _inputField.text = newText;
                _inputField.textComponent.ForceMeshUpdate();
                int endPos = newText.Length;
                _inputField.caretPosition = endPos;
                _inputField.selectionAnchorPosition = endPos;
                _inputField.selectionFocusPosition = endPos;
                _inputField.ActivateInputField();
                _inputField.ForceLabelUpdate();
                Canvas.ForceUpdateCanvases();
            }
        }

        public void SetTextAndCaretPosition(string newText, int caretPosition)
        {
            if (_inputField != null)
            {
                _inputField.text = newText;
                _inputField.textComponent.ForceMeshUpdate();
                int clampedCaretPos = Mathf.Clamp(caretPosition, 0, newText.Length);
                _inputField.caretPosition = clampedCaretPos;
                _inputField.selectionAnchorPosition = clampedCaretPos;
                _inputField.selectionFocusPosition = clampedCaretPos;
                _inputField.ActivateInputField();
                _inputField.ForceLabelUpdate();
                Canvas.ForceUpdateCanvases();
            }
        }

        private RectTransform GetCachedRectTransform(GameObject obj)
        {
            if (obj == null) return null;
            if (!_cachedRectTransforms.TryGetValue(obj, out RectTransform rectTransform))
            {
                rectTransform = obj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    _cachedRectTransforms[obj] = rectTransform;
                }
            }
            return rectTransform;
        }

        private void SaveCurrentConversation()
        {
            if (_inputField == null) return;
            string key = _inPMMode && _currentPMTarget != null ? $"PM_{_currentPMTarget.ActorNumber}" : "PUBLIC";
            if (!_inPMMode && string.IsNullOrEmpty(_inputField.text))
            {
                ChatManager.ClearConversation("PUBLIC");
            }
            else
            {
                ChatManager.SaveConversation(key, _inputField.text, _inputField.caretPosition);
            }
        }

        private void RestorePMPartners()
        {
            HashSet<int> partnerIDs = new HashSet<int>();
            for (int i = 0; i < ChatManager.RawMessages.Count; i++)
            {
                if (ChatManager.PrivateFlags[i])
                {
                    int senderID = ChatManager.SenderIDs[i];
                    int partnerID = ChatManager.PMPartnerIDs[i];
                    if (senderID != PhotonNetwork.LocalPlayer.ActorNumber)
                        partnerIDs.Add(senderID);
                    if (partnerID != PhotonNetwork.LocalPlayer.ActorNumber)
                        partnerIDs.Add(partnerID);
                }
            }
            foreach (int partnerID in partnerIDs)
            {
                var player = PhotonNetwork.CurrentRoom.GetPlayer(partnerID);
                if (player != null && !_pmPartners.Exists(p => p.ActorNumber == partnerID))
                {
                    _pmPartners.Add(player);
                }
            }
            if (_pmPartners.Count > 0)
            {
                _currentPMIndex = _pmPartners.Count - 1;
            }
        }
    }
}
