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
        protected override float Width => 400f;
        protected override float Height => 350f;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected BoolSetting _muteEmote = new BoolSetting(false);
        protected BoolSetting _muteText = new BoolSetting(false);
        protected BoolSetting _muteVoice = new BoolSetting(false);
        protected FloatSetting _voiceVolume = new FloatSetting(1f, minValue: 0f, maxValue: 1f);
        protected Player _player;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "ScoreboardPopup";
            string sub = "MutePopup";
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle style = new ElementStyle(titleWidth: 250f, themePanel: ThemePanel);
            ElementStyle sliderStyle = new ElementStyle(titleWidth: 75f, themePanel: ThemePanel, spacing: 0f);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Confirm"), onClick: () => OnButtonClick("Confirm"));
            ElementFactory.CreateToggleSetting(SinglePanel, style, _muteEmote, UIManager.GetLocale(cat, sub, "MuteEmote"));
            ElementFactory.CreateToggleSetting(SinglePanel, style, _muteText, UIManager.GetLocale(cat, sub, "MuteText"));
            ElementFactory.CreateToggleSetting(SinglePanel, style, _muteVoice, UIManager.GetLocale(cat, sub, "MuteVoice"));
            ElementFactory.CreateSliderSetting(SinglePanel, sliderStyle, _voiceVolume, UIManager.GetLocale(cat, sub, "VoiceVolume"));

        }

        public void Show(Player player)
        {
            base.Show();
            _player = player;
            _muteEmote.Value = InGameManager.MuteEmote.Contains(player.ActorNumber);
            _muteText.Value = InGameManager.MuteText.Contains(player.ActorNumber);
            _muteVoice.Value = InGameManager.MuteVoiceChat.Contains(player.ActorNumber);
            SyncSettingElements();
        }

        protected void HandleMute(Player player, string type, bool mute, bool isMuted)
        {
            if (mute && !isMuted)
            {
                ChatManager.MutePlayer(player, type);
            }
            else if (!mute && isMuted)
            {
                ChatManager.UnmutePlayer(player, type);
            }
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Confirm")
            {
                bool prevMuteEmote = InGameManager.MuteEmote.Contains(_player.ActorNumber);
                bool prevMuteText = InGameManager.MuteText.Contains(_player.ActorNumber);
                bool prevMuteVoice = InGameManager.MuteVoiceChat.Contains(_player.ActorNumber);

                HandleMute(_player, "emote", _muteEmote.Value, prevMuteEmote);
                HandleMute(_player, "text", _muteText.Value, prevMuteText);
                HandleMute(_player, "voice", _muteVoice.Value, prevMuteVoice);
                ChatManager.SetPlayerVolume(_player, _voiceVolume.Value);

                Hide();
            }
        }
    }
}
