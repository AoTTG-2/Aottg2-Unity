using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using System.Diagnostics;
using GameManagers;
using Assets.Scripts.ApplicationManagers;

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
        public static LinkedList<string> _messages = new LinkedList<string>();
        public static LinkedList<string> _messageBuffer = new LinkedList<string>();
        static int _currentCharCount = 0;
        static int _currentCharCountBuffer = 0;
        static Vector2 _scrollPosition = Vector2.zero;
        static string _inputLine = string.Empty;
        static bool _needResetScroll;
        const int MaxMessages = 200;
        const int MaxChars = 5000;
        const int MaxCharsPerMessage = 60;
        const int PositionX = 20;
        const int PositionY = 20;
        const int Width = 500;
        const int Height = 400;
        const int InputHeight = 25;
        const int Padding = 10;
        const string InputControlName = "DebugInput";

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            Application.logMessageReceived += OnUnityDebugLog;
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
            AddMessageBuffer(stackTrace);
            AddMessageBuffer(log);
        }

        static void AddMessageBuffer(string message)
        {
            _messageBuffer.AddLast(message);
            _currentCharCountBuffer += message.Length;
            while (_messageBuffer.Count > MaxMessages || _currentCharCountBuffer > MaxChars)
            {
                _currentCharCountBuffer -= _messageBuffer.First.Value.Length;
                _messageBuffer.RemoveFirst();
            }
            _needResetScroll = true;
        }

        static void AddMessage(string message)
        {
            if (message == string.Empty)
                return;
            if (message.Contains('\n'))
            {
                foreach (string line in message.Split("\n"))
                    AddMessage(line);
                return;
            }
            while (message.Length > MaxCharsPerMessage)
            {
                AddMessage(message.Substring(0, MaxCharsPerMessage));
                message = message.Substring(MaxCharsPerMessage);
            }
            _messages.AddLast(message);
            _currentCharCount += message.Length;
            while (_messages.Count > MaxMessages || _currentCharCount > MaxChars)
            {
                _currentCharCount -= _messages.First.Value.Length;
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
                GUI.Box(new Rect(PositionX, PositionY, Width, Height), "");
                DrawMessageWindow();
                DrawInputWindow();
                HandleInput();
                GUI.depth = 0;
            }
        }

        static void DrawMessageWindow()
        {
            int positionX = PositionX + Padding;
            int positionY = PositionY + Padding;
            int width = Width - Padding * 2;
            GUI.Label(new Rect(positionX, positionY, width, InputHeight), "Debug Console (Press F11 to hide)");
            positionY += InputHeight + Padding;
            int scrollViewHeight = Height - Padding * 4 - InputHeight * 2;
            GUIStyle style = new GUIStyle(GUI.skin.box);
            if (_needResetScroll)
            {
                while (_messageBuffer.Count > 0)
                {
                    AddMessage(_messageBuffer.First.Value);
                    _currentCharCountBuffer -= _messageBuffer.First.Value.Length;
                    _messageBuffer.RemoveFirst();
                }
            }
            string text = "";
            foreach (string message in _messages)
            {
                text += message + "\n";
            }
            text = text.Trim();
            int textWidth = width - Padding * 2;
            int height = (int)style.CalcHeight(new GUIContent(text), textWidth) + Padding;
            _scrollPosition = GUI.BeginScrollView(new Rect(positionX, positionY, width, scrollViewHeight), _scrollPosition,
                new Rect(positionX, positionY, textWidth, height));
            GUI.Label(new Rect(positionX, positionY, textWidth, height), text);
            if (_needResetScroll)
            {
                _needResetScroll = false;
                _scrollPosition = new Vector2(0f, height);
            }
            GUI.EndScrollView();
        }

        static void DrawInputWindow()
        {
            int y = PositionY + Height - InputHeight - Padding;
            GUI.SetNextControlName(InputControlName);
            _inputLine = GUI.TextField(new Rect(PositionX + Padding, y, Width - Padding * 2, InputHeight), _inputLine);
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
    }
}
