using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using GameManagers;

namespace UI
{
    class FeedPanel : BasePanel
    {
        private GameObject _panel;
        private List<GameObject> _lines = new List<GameObject>();
        protected override string ThemePanel => "ChatPanel";
        private Transform _horizontalLine;

        public override void Setup(BasePanel parent = null)
        {
            _panel = transform.Find("Content/Panel").gameObject;
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = SettingsManager.UISettings.ChatHeight.Value;
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 0f);
            _horizontalLine = ElementFactory.CreateHorizontalLine(_panel.transform, new ElementStyle(themePanel: ThemePanel), SettingsManager.UISettings.ChatWidth.Value, 2f).transform;
            Sync();
        }

        public void Sync()
        {
            foreach (GameObject go in _lines)
                Destroy(go);
            _lines.Clear();
            AddLines(ChatManager.FeedLines);
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

        protected GameObject CreateLine(string text)
        {
            var style = new ElementStyle(fontSize: SettingsManager.UISettings.ChatFontSize.Value, themePanel: ThemePanel);
            GameObject line = ElementFactory.CreateDefaultLabel(_panel.transform, style, text, alignment: TextAnchor.MiddleLeft);
            line.GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "TextColor", "Default");
            _horizontalLine.SetAsLastSibling();
            return line;
        }
    }
}
