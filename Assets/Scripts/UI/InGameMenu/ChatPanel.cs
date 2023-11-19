using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using GameManagers;

namespace UI
{
    class ChatPanel : BasePanel
    {
        private InputField _inputField;
        private GameObject _panel;
        private List<GameObject> _lines = new List<GameObject>();
        protected override string ThemePanel => "ChatPanel";
        protected Transform _caret;
        public bool IgnoreNextActivation;

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
            _inputField.text = "";
            Sync();
        }

        public void Sync()
        {
            foreach (GameObject go in _lines)
                Destroy(go);
            _lines.Clear();
            AddLines(ChatManager.Lines);
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

        public void AddLine(string line)
        {
            _lines.Add(CreateLine(line));
            Canvas.ForceUpdateCanvases();
            ClearExcessLines();
        }

        public void AddLines(List<string> lines)
        {
            foreach (string line in lines)
                _lines.Add(CreateLine(line));
            Canvas.ForceUpdateCanvases();
            ClearExcessLines();
        }

        protected void ClearExcessLines()
        {
            int maxHeight = SettingsManager.UISettings.ChatHeight.Value;
            float currentHeight = 0;
            for (int i = 0; i < _lines.Count; i++)
                currentHeight += _lines[i].GetComponent<RectTransform>().sizeDelta.y;
            float heightToRemove = Mathf.Max(currentHeight - maxHeight, 0f);
            while (heightToRemove > 0f && _lines.Count > 0)
            {
                float height = _lines[0].GetComponent<RectTransform>().sizeDelta.y;
                heightToRemove -= height;
                if (heightToRemove > 0f)
                {
                    Destroy(_lines[0]);
                    _lines.RemoveAt(0);
                }
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
        }

        protected GameObject CreateLine(string text)
        {
            var style = new ElementStyle(fontSize: SettingsManager.UISettings.ChatFontSize.Value, themePanel: ThemePanel);
            GameObject line = ElementFactory.CreateDefaultLabel(_panel.transform, style, text, alignment: TextAnchor.MiddleLeft);
            line.GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "TextColor", "Default");
            return line;
        }
    }
}
