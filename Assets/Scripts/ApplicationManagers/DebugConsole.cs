using Assets.Scripts.ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
    /// <summary>
    /// A GUI console used by testers to view debug logs and type debug commands.
    /// Can be activated by pressing F11.
    /// See DebugTesting for debug command implementations.
    /// </summary>
    class DebugConsole : MonoBehaviour
    {
        static DebugConsole _instance;
        public static bool Enabled;

        // Message structure to hold log data with type
        public class LogMessage
        {
            public string Message;
            public LogType Type;
            public string Prefix;
            public bool IsCustomLogic;
            public string StackTrace;
            public int Count;
            private string _cachedFormatted;
            private bool _lastShowStackTraces;
            private int _lastCount;

            public void Initialize(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
            {
                // Message and stackTrace should already be truncated by AddMessageBuffer
                Message = message ?? string.Empty;
                StackTrace = stackTrace ?? string.Empty;
                Type = type;
                IsCustomLogic = isCustomLogic;
                Count = 1;
                _cachedFormatted = null;
                _lastShowStackTraces = false;
                _lastCount = 0;

                // Set prefix with UTF-8 icons and rich text colors
                switch (type)
                {
                    case LogType.Error:
                        Prefix = "<color=red>✖</color> ";
                        break;
                    case LogType.Warning:
                        Prefix = "<color=yellow>⚠</color> ";
                        break;
                    case LogType.Log:
                        Prefix = "<color=white>ℹ</color> ";
                        break;
                    case LogType.Exception:
                        Prefix = "<color=red>⛔</color> ";
                        break;
                    case LogType.Assert:
                        Prefix = "<color=orange>!</color> ";
                        break;
                    default:
                        Prefix = "";
                        break;
                }
            }

            public void Reset()
            {
                Message = null;
                StackTrace = null;
                Prefix = null;
                _cachedFormatted = null;
                Count = 1;
            }

            public string GetFormattedMessage(bool showStackTraces)
            {
                // Cache the formatted string to avoid allocations
                if (_cachedFormatted == null || _lastShowStackTraces != showStackTraces || _lastCount != Count)
                {
                    _lastShowStackTraces = showStackTraces;
                    _lastCount = Count;
                    
                    string countSuffix = Count > 1 ? $" (x{Count})" : "";
                    string result = Prefix + Message + countSuffix;

                    if (showStackTraces && !string.IsNullOrEmpty(StackTrace))
                    {
                        result += "\n" + StackTrace;
                    }
                    _cachedFormatted = result;
                }

                return _cachedFormatted;
            }

            public bool IsDuplicateOf(string message, LogType type, bool isCustomLogic, string stackTrace)
            {
                return Message == message &&
                       Type == type &&
                       IsCustomLogic == isCustomLogic &&
                       StackTrace == stackTrace;
            }
        }

        // Object pool for LogMessage
        static class LogMessagePool
        {
            private static readonly Stack<LogMessage> _pool = new Stack<LogMessage>(PoolSize);
            private const int PoolSize = 512;

            static LogMessagePool()
            {
                for (int i = 0; i < PoolSize; i++)
                {
                    _pool.Push(new LogMessage());
                }
            }

            public static LogMessage Get(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
            {
                LogMessage logMessage;
                if (_pool.Count > 0)
                {
                    logMessage = _pool.Pop();
                }
                else
                {
                    logMessage = new LogMessage();
                }
                logMessage.Initialize(message, type, stackTrace, isCustomLogic);
                return logMessage;
            }

            public static void Return(LogMessage logMessage)
            {
                if (logMessage == null) return;
                logMessage.Reset();
                if (_pool.Count < PoolSize)
                {
                    _pool.Push(logMessage);
                }
            }
        }

        public static LinkedList<LogMessage> _messages = new LinkedList<LogMessage>();
        public static LinkedList<LogMessage> _messageBuffer = new LinkedList<LogMessage>();
        static int _currentCharCount = 0;
        static int _currentCharCountBuffer = 0;
        static Vector2 _scrollPosition = Vector2.zero;
        static string _inputLine = string.Empty;
        static bool _needResetScroll;

        // Virtualized rendering cache
        static List<LogMessage> _filteredMessages = new List<LogMessage>(MaxMessages);
        static bool _filterDirty = true;
        static LogTab _lastFilterTab;
        static StringBuilder _displayBuilder = new StringBuilder(8192);
        static string _cachedDisplayText = "";
        static bool _displayDirty = true;
        static int _lastFilteredCount = 0;
        static bool _lastShowStackTraces = false;

        // Throttling for buffer processing
        const int MaxBufferProcessPerFrame = 50;

        // Window position and size
        static float _windowX = 20;
        static float _windowY = 20;
        static float _windowWidth = 600;
        static float _windowHeight = 400;

        // Dragging and resizing state
        static bool _isDragging = false;
        static bool _isResizing = false;
        static Vector2 _dragOffset;
        static Vector2 _resizeStartSize;
        static Vector2 _resizeStartMousePos;

        // Tab filtering
        enum LogTab
        {
            All,
            Info,
            Warning,
            Error,
            CustomLogic
        }
        static LogTab _currentTab;
        static bool _showStackTraces = false;
        static bool _solidBackground = true;
        static bool _wordWrap = true;

        const int MaxMessages = 1024;
        const int MaxChars = 500000;
        const int MaxMessageLength = 1024;
        const int InputHeight = 25;
        const int Padding = 10;
        const int TabHeight = 25;
        const int ResizeHandleSize = 15;
        const int MinWidth = 400;
        const int MinHeight = 300;
        const string InputControlName = "DebugInput";
        const string CustomLogicErrorPrefix = "[Custom Logic Error] ";


        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            Application.logMessageReceived += OnUnityDebugLog;

            // Set default tab based on build type
            // In editor or debug builds, show all logs by default
            // In release builds, show only Custom Logic errors by default (most relevant to end users)
#if UNITY_EDITOR
            _currentTab = LogTab.All;
#else
            _currentTab = Debug.isDebugBuild ? LogTab.All : LogTab.CustomLogic;
#endif
            _lastFilterTab = _currentTab;
        }

        public static void Log(string message, bool showInChat = false)
        {
            UnityEngine.Debug.Log(message);
            if (showInChat && ChatManager.IsChatAvailable())
                ChatManager.AddException(message);
        }

        public static void LogCustomLogic(string message, bool showInChat = false)
        {
            // Prefix the message internally for detection, but don't show the prefix
            UnityEngine.Debug.LogError(CustomLogicErrorPrefix + message);
            if (showInChat && ChatManager.IsChatAvailable())
                ChatManager.AddException(message);
        }

        public static void LogTimeSince(float start, string prefix = "")
        {
            UnityEngine.Debug.Log(prefix + ": " + (Time.realtimeSinceStartup - start).ToString());
        }

        static void OnUnityDebugLog(string log, string stackTrace, LogType type)
        {
            // Check if this is a Custom Logic error by looking for the prefix
            bool isCustomLogic = log.StartsWith(CustomLogicErrorPrefix);
            
            // Remove the prefix from the message before displaying
            if (isCustomLogic)
            {
                log = log.Substring(CustomLogicErrorPrefix.Length);
                // Custom Logic errors should be treated as errors
                if (type == LogType.Log)
                    type = LogType.Error;
            }

            AddMessageBuffer(log, type, stackTrace, isCustomLogic);
        }

        static void AddMessageBuffer(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
        {
            // Null safety
            message = message ?? string.Empty;
            stackTrace = stackTrace ?? string.Empty;
            
            // Truncate message and stack trace before duplicate check and storage
            string truncatedMessage = message.Length > MaxMessageLength 
                ? message.Substring(0, MaxMessageLength) + "..." 
                : message;
            int remainingLen = MaxMessageLength - Mathf.Min(message.Length, MaxMessageLength);
            string truncatedStackTrace = stackTrace.Length > remainingLen 
                ? stackTrace.Substring(0, Mathf.Max(0, remainingLen)) + "..." 
                : stackTrace;

            // Check if this is a duplicate of the last message (using truncated versions)
            if (_messageBuffer.Count > 0)
            {
                var lastMessage = _messageBuffer.Last.Value;
                if (lastMessage.IsDuplicateOf(truncatedMessage, type, isCustomLogic, truncatedStackTrace))
                {
                    lastMessage.Count++;
                    _needResetScroll = true;
                    _displayDirty = true;
                    return;
                }
            }

            var logMessage = LogMessagePool.Get(truncatedMessage, type, truncatedStackTrace, isCustomLogic);
            _messageBuffer.AddLast(logMessage);
            _currentCharCountBuffer += truncatedMessage.Length + truncatedStackTrace.Length;

            while (_messageBuffer.Count > MaxMessages || _currentCharCountBuffer > MaxChars)
            {
                var first = _messageBuffer.First.Value;
                _currentCharCountBuffer -= first.Message.Length + first.StackTrace.Length;
                _messageBuffer.RemoveFirst();
                LogMessagePool.Return(first);
            }
            _needResetScroll = true;
            _filterDirty = true;
            _displayDirty = true;
        }

        static void AddMessage(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
        {
            if (message == string.Empty)
                return;

            // Check if this is a duplicate of the last message
            if (_messages.Count > 0)
            {
                var lastMessage = _messages.Last.Value;
                if (lastMessage.IsDuplicateOf(message, type, isCustomLogic, stackTrace))
                {
                    lastMessage.Count++;
                    _displayDirty = true;
                    return;
                }
            }

            var logMessage = LogMessagePool.Get(message, type, stackTrace, isCustomLogic);
            _messages.AddLast(logMessage);
            _currentCharCount += message.Length + stackTrace.Length;

            while (_messages.Count > MaxMessages || _currentCharCount > MaxChars)
            {
                var first = _messages.First.Value;
                _currentCharCount -= first.Message.Length + first.StackTrace.Length;
                _messages.RemoveFirst();
                LogMessagePool.Return(first);
            }
            _filterDirty = true;
            _displayDirty = true;
        }

        static void ProcessMessageBuffer()
        {
            int processed = 0;
            while (_messageBuffer.Count > 0 && processed < MaxBufferProcessPerFrame)
            {
                var logMessage = _messageBuffer.First.Value;
                _messageBuffer.RemoveFirst();
                
                // Transfer to main messages - reuse the pooled object
                if (_messages.Count > 0)
                {
                    var lastMessage = _messages.Last.Value;
                    if (lastMessage.IsDuplicateOf(logMessage.Message, logMessage.Type, logMessage.IsCustomLogic, logMessage.StackTrace))
                    {
                        lastMessage.Count += logMessage.Count;
                        _currentCharCountBuffer -= logMessage.Message.Length + logMessage.StackTrace.Length;
                        LogMessagePool.Return(logMessage);
                        processed++;
                        continue;
                    }
                }

                _currentCharCountBuffer -= logMessage.Message.Length + logMessage.StackTrace.Length;
                _currentCharCount += logMessage.Message.Length + logMessage.StackTrace.Length;
                _messages.AddLast(logMessage);

                while (_messages.Count > MaxMessages || _currentCharCount > MaxChars)
                {
                    var first = _messages.First.Value;
                    _currentCharCount -= first.Message.Length + first.StackTrace.Length;
                    _messages.RemoveFirst();
                    LogMessagePool.Return(first);
                }
                processed++;
            }

            if (processed > 0)
            {
                _filterDirty = true;
                _displayDirty = true;
            }
        }

        static void UpdateFilteredMessages()
        {
            if (!_filterDirty && _lastFilterTab == _currentTab)
                return;

            _filteredMessages.Clear();
            foreach (var logMessage in _messages)
            {
                bool includeMessage = false;

                switch (_currentTab)
                {
                    case LogTab.All:
                        includeMessage = true;
                        break;
                    case LogTab.Info:
                        includeMessage = logMessage.Type == LogType.Log && !logMessage.IsCustomLogic;
                        break;
                    case LogTab.Warning:
                        includeMessage = logMessage.Type == LogType.Warning && !logMessage.IsCustomLogic;
                        break;
                    case LogTab.Error:
                        includeMessage = !logMessage.IsCustomLogic &&
                                       (logMessage.Type == LogType.Error ||
                                        logMessage.Type == LogType.Exception ||
                                        logMessage.Type == LogType.Assert);
                        break;
                    case LogTab.CustomLogic:
                        includeMessage = logMessage.IsCustomLogic;
                        break;
                }

                if (includeMessage)
                {
                    _filteredMessages.Add(logMessage);
                }
            }

            _filterDirty = false;
            _lastFilterTab = _currentTab;
            _displayDirty = true;
        }

        static string GetDisplayText()
        {
            // Check if we need to rebuild the display text
            if (!_displayDirty && 
                _lastFilteredCount == _filteredMessages.Count && 
                _lastShowStackTraces == _showStackTraces)
            {
                return _cachedDisplayText;
            }

            _displayBuilder.Clear();
            for (int i = 0; i < _filteredMessages.Count; i++)
            {
                if (i > 0)
                    _displayBuilder.Append('\n');
                _displayBuilder.Append(_filteredMessages[i].GetFormattedMessage(_showStackTraces));
            }

            _cachedDisplayText = _displayBuilder.ToString();
            _lastFilteredCount = _filteredMessages.Count;
            _lastShowStackTraces = _showStackTraces;
            _displayDirty = false;

            return _cachedDisplayText;
        }

        void Update()
        {
            if (SettingsManager.InputSettings.General.DebugWindow.GetKeyDown())
            {
                Enabled = !Enabled;
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                DebugLagSim.Toggle();
            }
        }

        void OnGUI()
        {
            if (Enabled)
            {
                // draw debug console over everything else
                GUI.depth = 1;

                // Handle dragging and resizing
                HandleWindowInteraction();

                // Draw background - solid or transparent based on toggle
                if (_solidBackground)
                {
                    // Draw a solid dark background
                    Color oldColor = GUI.color;
                    GUI.color = new Color(0.15f, 0.15f, 0.15f, 1f);
                    GUI.DrawTexture(new Rect(_windowX, _windowY, _windowWidth, _windowHeight), Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }

                // Always draw the window border/frame
                GUI.Box(new Rect(_windowX, _windowY, _windowWidth, _windowHeight), "");

                DrawTabs();
                DrawMessageWindow();
                DrawInputWindow();
                HandleInput();

                // Draw resize handle
                DrawResizeHandle();

                GUI.depth = 0;
            }
        }

        static void HandleWindowInteraction()
        {
            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;

            // Check if mouse is in resize handle area (bottom-right corner)
            Rect resizeHandleRect = new Rect(_windowX + _windowWidth - ResizeHandleSize,
                                             _windowY + _windowHeight - ResizeHandleSize,
                                             ResizeHandleSize, ResizeHandleSize);

            // Check if mouse is in title bar area (for dragging) - adjusted for extra spacing
            Rect titleBarRect = new Rect(_windowX, _windowY, _windowWidth, InputHeight + Padding * 3 + TabHeight);

            // Calculate button area to exclude from dragging - now 4 buttons on top row
            int buttonWidth = 80;
            int buttonSpacing = 5;
            int totalButtonWidth = buttonWidth * 4 + buttonSpacing * 3;
            Rect buttonAreaRect = new Rect(_windowX + _windowWidth - totalButtonWidth - Padding * 2,
                                           _windowY + Padding,
                                           totalButtonWidth + Padding,
                                           InputHeight);

            // Calculate tab area to exclude from dragging
            Rect tabAreaRect = new Rect(_windowX + Padding,
                                        _windowY + Padding * 2 + InputHeight,
                                        _windowWidth - Padding * 2,
                                        TabHeight);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (resizeHandleRect.Contains(mousePos))
                {
                    _isResizing = true;
                    _resizeStartSize = new Vector2(_windowWidth, _windowHeight);
                    _resizeStartMousePos = mousePos;
                    e.Use();
                }
                else if (titleBarRect.Contains(mousePos) && !buttonAreaRect.Contains(mousePos) && !tabAreaRect.Contains(mousePos))
                {
                    // Only start dragging if not clicking on buttons or tabs
                    _isDragging = true;
                    _dragOffset = new Vector2(mousePos.x - _windowX, mousePos.y - _windowY);
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                _isDragging = false;
                _isResizing = false;
            }
            else if (e.type == EventType.MouseDrag)
            {
                if (_isDragging)
                {
                    _windowX = mousePos.x - _dragOffset.x;
                    _windowY = mousePos.y - _dragOffset.y;
                    e.Use();
                }
                else if (_isResizing)
                {
                    Vector2 delta = mousePos - _resizeStartMousePos;
                    _windowWidth = Mathf.Max(MinWidth, _resizeStartSize.x + delta.x);
                    _windowHeight = Mathf.Max(MinHeight, _resizeStartSize.y + delta.y);
                    e.Use();
                }
            }
        }

        static void DrawTabs()
        {
            int positionX = (int)_windowX + Padding;
            int positionY = (int)_windowY + Padding;
            int width = (int)_windowWidth - Padding * 2;

            // Title and toggle buttons on the right (top row)
            int buttonWidth = 90;
            int buttonSpacing = 5;
            int totalButtonWidth = buttonWidth * 4 + buttonSpacing * 3; // Clear + Stack Traces + Background + Wrap
            GUI.Label(new Rect(positionX, positionY, width - totalButtonWidth - Padding, InputHeight), "Debug Console (Press F11 to hide)");

            int buttonX = positionX + width - totalButtonWidth;

            // Clear button
            if (GUI.Button(new Rect(buttonX, positionY, buttonWidth, InputHeight), "Clear"))
            {
                // Return all messages to the pool
                foreach (var msg in _messages)
                    LogMessagePool.Return(msg);
                foreach (var msg in _messageBuffer)
                    LogMessagePool.Return(msg);
                    
                _messages.Clear();
                _messageBuffer.Clear();
                _filteredMessages.Clear();
                _currentCharCount = 0;
                _currentCharCountBuffer = 0;
                _filterDirty = true;
                _displayDirty = true;
                _cachedDisplayText = "";
            }
            buttonX += buttonWidth + buttonSpacing;

            // Stack trace toggle button
            string toggleLabel = _showStackTraces ? "Hide Traces" : "Show Traces";
            if (GUI.Button(new Rect(buttonX, positionY, buttonWidth, InputHeight), toggleLabel))
            {
                _showStackTraces = !_showStackTraces;
                _displayDirty = true;
            }
            buttonX += buttonWidth + buttonSpacing;

            // Background toggle button
            string bgLabel = _solidBackground ? "Glass" : "Opaque";
            if (GUI.Button(new Rect(buttonX, positionY, buttonWidth, InputHeight), bgLabel))
            {
                _solidBackground = !_solidBackground;
            }
            buttonX += buttonWidth + buttonSpacing;

            // Word wrap toggle button
            string wrapLabel = _wordWrap ? "No Wrap" : "Wrap";
            if (GUI.Button(new Rect(buttonX, positionY, buttonWidth, InputHeight), wrapLabel))
            {
                _wordWrap = !_wordWrap;
            }

            // Add extra spacing between title row and tabs
            positionY += InputHeight + Padding;

            // Second row: Log type tabs
            int tabWidth = (width - Padding * 4) / 5;
            int tabX = positionX;

            // All tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "All"))
            {
                _currentTab = LogTab.All;
                _needResetScroll = true;
                _filterDirty = true;
            }
            tabX += tabWidth + Padding;

            // Info tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Info"))
            {
                _currentTab = LogTab.Info;
                _needResetScroll = true;
                _filterDirty = true;
            }
            tabX += tabWidth + Padding;

            // Warning tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Warning"))
            {
                _currentTab = LogTab.Warning;
                _needResetScroll = true;
                _filterDirty = true;
            }
            tabX += tabWidth + Padding;

            // Error tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Error"))
            {
                _currentTab = LogTab.Error;
                _needResetScroll = true;
                _filterDirty = true;
            }
            tabX += tabWidth + Padding;

            // Custom Logic tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "CL"))
            {
                _currentTab = LogTab.CustomLogic;
                _needResetScroll = true;
                _filterDirty = true;
            }

            // Highlight current tab
            GUI.Box(new Rect(positionX + (int)_currentTab * (tabWidth + Padding), positionY, tabWidth, TabHeight), "");
        }

        static void DrawMessageWindow()
        {
            int positionX = (int)_windowX + Padding;
            int positionY = (int)_windowY + Padding * 2 + InputHeight + TabHeight + Padding; // Added extra Padding for spacing
            int width = (int)_windowWidth - Padding * 2;
            int scrollViewHeight = (int)_windowHeight - Padding * 6 - InputHeight * 2 - TabHeight; // Adjusted for extra spacing
            GUIStyle style = new GUIStyle(GUI.skin.textArea);
            style.wordWrap = _wordWrap;
            style.richText = true; // Enable rich text for colored icons

            // Process buffer with throttling
            if (_needResetScroll || _messageBuffer.Count > 0)
            {
                ProcessMessageBuffer();
            }

            // Update filtered messages list
            UpdateFilteredMessages();

            // Get cached display text
            string text = GetDisplayText();

            int textWidth = width - Padding * 2;
            int textHeight;
            int contentWidth;

            if (_wordWrap)
            {
                // When word wrapping is enabled, calculate height based on wrapped content
                textHeight = (int)style.CalcHeight(new GUIContent(text), textWidth) + Padding;
                contentWidth = textWidth;
            }
            else
            {
                // When word wrapping is disabled, calculate the maximum line width for horizontal scrolling
                string[] lines = text.Split('\n');
                float maxLineWidth = 0;
                foreach (string line in lines)
                {
                    float lineWidth = style.CalcSize(new GUIContent(line)).x;
                    if (lineWidth > maxLineWidth)
                        maxLineWidth = lineWidth;
                }
                contentWidth = Mathf.Max(textWidth, (int)maxLineWidth + Padding);
                textHeight = (int)style.CalcHeight(new GUIContent(text), contentWidth) + Padding;
            }

            // Content rect should use (0,0) as origin for scroll view internal coordinates
            _scrollPosition = GUI.BeginScrollView(new Rect(positionX, positionY, width, scrollViewHeight), _scrollPosition,
                new Rect(0, 0, contentWidth, textHeight));

            // Draw TextArea at (0,0) relative to scroll view content
            GUI.TextArea(new Rect(0, 0, contentWidth, textHeight), text, style);

            if (_needResetScroll)
            {
                _needResetScroll = false;
                // Scroll to bottom: max scroll = content height - visible height
                _scrollPosition = new Vector2(0f, Mathf.Max(0, textHeight - scrollViewHeight));
            }
            GUI.EndScrollView();
        }

        static void DrawInputWindow()
        {
            int y = (int)(_windowY + _windowHeight) - InputHeight - Padding;

            // Draw input field taking the full width
            GUI.SetNextControlName(InputControlName);
            _inputLine = GUI.TextField(new Rect((int)_windowX + Padding, y, (int)_windowWidth - Padding * 2, InputHeight), _inputLine);
        }

        static void HandleInput()
        {
            if (GUI.GetNameOfFocusedControl() == InputControlName)
            {
                if (IsEnterUp())
                {
                    if (_inputLine != string.Empty)
                    {
                        UnityEngine.Debug.Log(_inputLine);
                        if (_inputLine.StartsWith("/"))
                            DebugTesting.RunDebugCommand(_inputLine.Substring(1));
                        else
                            UnityEngine.Debug.Log("Invalid debug command.");
                        _inputLine = string.Empty;
                    }
                    GUI.FocusControl(string.Empty);
                }
            }
            /*else if (IsEnterUp())
            {
                GUI.FocusControl(InputControlName);
            }*/
        }

        static bool IsEnterUp()
        {
            return Event.current.type == EventType.KeyUp && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter);
        }

        static void HandleWindowDraggingAndResizing()
        {
            // Handle dragging
            if (_isDragging)
            {
                Vector2 mousePos = Event.current.mousePosition;
                _windowX = mousePos.x + _dragOffset.x;
                _windowY = mousePos.y + _dragOffset.y;
            }

            // Handle resizing
            if (_isResizing)
            {
                Vector2 mousePos = Event.current.mousePosition;
                float width = _resizeStartSize.x + (mousePos.x - _resizeStartMousePos.x);
                float height = _resizeStartSize.y + (mousePos.y - _resizeStartMousePos.y);

                // Apply minimum size constraints
                width = Mathf.Max(width, MinWidth);
                height = Mathf.Max(height, MinHeight);

                _windowWidth = width;
                _windowHeight = height;
            }

            // Begin window drag on left mouse down
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Vector2 mousePos = Event.current.mousePosition;
                Rect titleBarRect = new Rect(_windowX, _windowY, _windowWidth, InputHeight + TabHeight + Padding * 2);

                // Check if we're clicking on the title bar
                if (titleBarRect.Contains(mousePos))
                {
                    _dragOffset = new Vector2(_windowX - mousePos.x, _windowY - mousePos.y);
                    _isDragging = true;
                    Event.current.Use();
                }
                else
                {
                    // Check if we're clicking on the resize handle
                    Rect resizeHandleRect = new Rect(_windowX + _windowWidth - ResizeHandleSize, _windowY + _windowHeight - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize);
                    if (resizeHandleRect.Contains(mousePos))
                    {
                        _resizeStartSize = new Vector2(_windowWidth, _windowHeight);
                        _resizeStartMousePos = mousePos;
                        _isResizing = true;
                        Event.current.Use();
                    }
                }
            }

            // End dragging/resizing on mouse up
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                _isDragging = false;
                _isResizing = false;
            }

            // Ensure the window stays within the screen bounds
            _windowX = Mathf.Clamp(_windowX, 0, Screen.width - _windowWidth);
            _windowY = Mathf.Clamp(_windowY, 0, Screen.height - _windowHeight);
        }

        static void DrawResizeHandle()
        {
            // Draw a visual indicator for the resize handle
            Rect resizeHandleRect = new Rect(_windowX + _windowWidth - ResizeHandleSize,
                                             _windowY + _windowHeight - ResizeHandleSize,
                                             ResizeHandleSize, ResizeHandleSize);

            GUI.Box(resizeHandleRect, "⋰");
        }
    }
}
