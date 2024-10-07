using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Settings;
using GameManagers;
using Photon.Realtime;

namespace UI
{
    class VoiceChatPanel : BasePanel
    {
        protected override string ThemePanel => "VoiceChatPanel";
        private GameObject _panel;
        private Dictionary<int, GameObject> _playersTalking = new Dictionary<int, GameObject>();

        public override void Setup(BasePanel parent = null)
        {
            _panel = transform.Find("Content/Panel").gameObject;
            transform.Find("Content").GetComponent<LayoutElement>().preferredHeight = 100;
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(SettingsManager.UISettings.ChatWidth.Value, 100f);
        }


        public void AddPlayer(Player player)
        {
            if (_playersTalking.ContainsKey(player.ActorNumber))
                return;
            var style = new ElementStyle(fontSize: SettingsManager.UISettings.ChatFontSize.Value, themePanel: ThemePanel);
            string playerName = ChatManager.GetIDString(player.ActorNumber) + player.GetStringProperty(PlayerProperty.Name);
            GameObject line = ElementFactory.CreateDefaultLabel(_panel.transform, style, playerName, alignment: TextAnchor.MiddleLeft);
            line.GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "TextColor", "Default");
            _playersTalking.Add(player.ActorNumber, line);
            Canvas.ForceUpdateCanvases();
        }

        public void RemovePlayer(Player player)
        {
            if (_playersTalking.ContainsKey(player.ActorNumber))
            {
                Destroy(_playersTalking[player.ActorNumber]);
                _playersTalking.Remove(player.ActorNumber);
            }
            Canvas.ForceUpdateCanvases();
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
