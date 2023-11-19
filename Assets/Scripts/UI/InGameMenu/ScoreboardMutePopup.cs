using GameManagers;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ScoreboardMutePopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocale("ScoreboardPopup", "MutePopup", "Title");
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;
        protected override float VerticalSpacing => 20f;
        protected override float Width => 370f;
        protected override float Height => 250f;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected BoolSetting _muteEmote = new BoolSetting(false);
        protected BoolSetting _muteText = new BoolSetting(false);
        protected Player _player;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "ScoreboardPopup";
            string sub = "MutePopup";
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle style = new ElementStyle(titleWidth: 240f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Confirm"), onClick: () => OnButtonClick("Confirm"));
            ElementFactory.CreateToggleSetting(SinglePanel, style, _muteEmote, UIManager.GetLocale(cat, sub, "MuteEmote"));
            ElementFactory.CreateToggleSetting(SinglePanel, style, _muteText, UIManager.GetLocale(cat, sub, "MuteText"));
        }

        public void Show(Player player)
        {
            base.Show();
            _player = player;
            _muteEmote.Value = InGameManager.MuteEmote.Contains(player.ActorNumber);
            _muteText.Value = InGameManager.MuteText.Contains(player.ActorNumber);
            SyncSettingElements();
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Confirm")
            {
                if (_muteEmote.Value)
                    ChatManager.MutePlayer(_player, true);
                else
                    ChatManager.UnmutePlayer(_player, true);
                if (_muteText.Value)
                    ChatManager.MutePlayer(_player, false);
                else
                    ChatManager.UnmutePlayer(_player, false);
                Hide();
            }
        }
    }
}
