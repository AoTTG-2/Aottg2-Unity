using Assets.Scripts.ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
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

            public LogMessage(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
            {
                Message = message;
                Type = type;
                IsCustomLogic = isCustomLogic;
                StackTrace = stackTrace;
                Count = 1;

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

            public string GetFormattedMessage(bool showStackTraces)
            {
                string countSuffix = Count > 1 ? $" (x{Count})" : "";
                string result = Prefix + Message + countSuffix;
                
                if (showStackTraces && !string.IsNullOrEmpty(StackTrace))
                {
                    result += "\n" + StackTrace;
                }
                
                return result;
            }

            public bool IsDuplicateOf(LogMessage other)
            {
                return other != null &&
                       Message == other.Message &&
                       Type == other.Type &&
                       IsCustomLogic == other.IsCustomLogic &&
                       StackTrace == other.StackTrace;
            }
        }

        public static LinkedList<LogMessage> _messages = new LinkedList<LogMessage>();
        public static LinkedList<LogMessage> _messageBuffer = new LinkedList<LogMessage>();
        static int _currentCharCount = 0;
        static int _currentCharCountBuffer = 0;
        static Vector2 _scrollPosition = Vector2.zero;
        static string _inputLine = string.Empty;
        static bool _needResetScroll;

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

        const int MaxMessages = 200;
        const int MaxChars = 5000;
        const int InputHeight = 25;
        const int Padding = 10;
        const int TabHeight = 25;
        const int ResizeHandleSize = 15;
        const int MinWidth = 400;
        const int MinHeight = 300;
        const string InputControlName = "DebugInput";


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
        }

        public static void Log(string message, bool showInChat = false)
        {
            UnityEngine.Debug.Log(message);
            if (showInChat && ChatManager.IsChatAvailable())
                ChatManager.AddException(message);
        }

        public static void LogTimeSince(float start, string prefix = "")
        {
            UnityEngine.Debug.Log(prefix + ": " + (Time.realtimeSinceStartup - start).ToString());
        }

        static void OnUnityDebugLog(string log, string stackTrace, LogType type)
        {
            // Check if this is a Custom Logic error - be more comprehensive in detection
            bool isCustomLogic = log.Contains("Custom logic runtime error") ||
                                log.Contains("Custom logic error") ||
                                log.Contains("Custom logic parsing error");

            // Custom Logic errors should be treated as errors, not info
            if (isCustomLogic && type == LogType.Log)
                type = LogType.Error;

            AddMessageBuffer(log, type, stackTrace, isCustomLogic);
        }

        static void AddMessageBuffer(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
        {
            // Check if this is a duplicate of the last message
            if (_messageBuffer.Count > 0)
            {
                var lastMessage = _messageBuffer.Last.Value;
                var tempMessage = new LogMessage(message, type, stackTrace, isCustomLogic);
                
                if (lastMessage.IsDuplicateOf(tempMessage))
                {
                    lastMessage.Count++;
                    _needResetScroll = true;
                    return;
                }
            }

            var logMessage = new LogMessage(message, type, stackTrace, isCustomLogic);
            _messageBuffer.AddLast(logMessage);
            _currentCharCountBuffer += message.Length + stackTrace.Length;
            
            while (_messageBuffer.Count > MaxMessages || _currentCharCountBuffer > MaxChars)
            {
                var first = _messageBuffer.First.Value;
                _currentCharCountBuffer -= first.Message.Length + first.StackTrace.Length;
                _messageBuffer.RemoveFirst();
            }
            _needResetScroll = true;
        }

        static void AddMessage(string message, LogType type, string stackTrace = "", bool isCustomLogic = false)
        {
            if (message == string.Empty)
                return;

            // Check if this is a duplicate of the last message
            if (_messages.Count > 0)
            {
                var lastMessage = _messages.Last.Value;
                var tempMessage = new LogMessage(message, type, stackTrace, isCustomLogic);
                
                if (lastMessage.IsDuplicateOf(tempMessage))
                {
                    lastMessage.Count++;
                    return;
                }
            }

            var logMessage = new LogMessage(message, type, stackTrace, isCustomLogic);
            _messages.AddLast(logMessage);
            _currentCharCount += message.Length + stackTrace.Length;

            while (_messages.Count > MaxMessages || _currentCharCount > MaxChars)
            {
                var first = _messages.First.Value;
                _currentCharCount -= first.Message.Length + first.StackTrace.Length;
                _messages.RemoveFirst();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
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
                _messages.Clear();
                _messageBuffer.Clear();
                _currentCharCount = 0;
                _currentCharCountBuffer = 0;
            }
            buttonX += buttonWidth + buttonSpacing;

            // Stack trace toggle button
            string toggleLabel = _showStackTraces ? "Hide Traces" : "Show Traces";
            if (GUI.Button(new Rect(buttonX, positionY, buttonWidth, InputHeight), toggleLabel))
            {
                _showStackTraces = !_showStackTraces;
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
            }
            tabX += tabWidth + Padding;

            // Info tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Info"))
            {
                _currentTab = LogTab.Info;
                _needResetScroll = true;
            }
            tabX += tabWidth + Padding;

            // Warning tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Warning"))
            {
                _currentTab = LogTab.Warning;
                _needResetScroll = true;
            }
            tabX += tabWidth + Padding;

            // Error tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "Error"))
            {
                _currentTab = LogTab.Error;
                _needResetScroll = true;
            }
            tabX += tabWidth + Padding;

            // Custom Logic tab
            if (GUI.Button(new Rect(tabX, positionY, tabWidth, TabHeight), "CL"))
            {
                _currentTab = LogTab.CustomLogic;
                _needResetScroll = true;
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

            if (_needResetScroll)
            {
                while (_messageBuffer.Count > 0)
                {
                    var logMessage = _messageBuffer.First.Value;
                    AddMessage(logMessage.Message, logMessage.Type, logMessage.StackTrace, logMessage.IsCustomLogic);
                    var first = _messageBuffer.First.Value;
                    _currentCharCountBuffer -= first.Message.Length + first.StackTrace.Length;
                    _messageBuffer.RemoveFirst();
                }
            }

            // Filter messages based on current tab
            string text = "";
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
                    text += logMessage.GetFormattedMessage(_showStackTraces) + "\n";
                }
            }
            text = text.Trim();

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

            _scrollPosition = GUI.BeginScrollView(new Rect(positionX, positionY, width, scrollViewHeight), _scrollPosition,
                new Rect(positionX, positionY, contentWidth, textHeight));

            // Use TextArea instead of Label to allow text selection
            GUI.TextArea(new Rect(positionX, positionY, contentWidth, textHeight), text, style);

            if (_needResetScroll)
            {
                _needResetScroll = false;
                _scrollPosition = new Vector2(0f, textHeight);
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
            else if (IsEnterUp())
            {
                GUI.FocusControl(InputControlName);
            }
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
