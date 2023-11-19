using System;
using UnityEngine;
using UI;
using ApplicationManagers;

namespace Settings
{
    class UISettings: SaveableSettingsContainer
    {
        protected override string FileName { get { return "UI.json"; } }
        public StringSetting UITheme = new StringSetting("Dark");
        public BoolSetting GameFeed = new BoolSetting(false);
        public BoolSetting FeedConsole = new BoolSetting(false);
        public BoolSetting ShowStylebar = new BoolSetting(true);
        public FloatSetting UIMasterScale = new FloatSetting(1f, minValue: 0.75f, maxValue: 1.5f);
        public FloatSetting CrosshairScale = new FloatSetting(1f, minValue: 0f, maxValue: 3f);
        public FloatSetting CrosshairTextScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public StringSetting CrosshairSkin = new StringSetting(string.Empty, maxLength: 200);
        public FloatSetting HUDScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public FloatSetting MinimapScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public FloatSetting StylebarScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public FloatSetting KillScoreScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public FloatSetting KillFeedScale = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public BoolSetting ShowCrosshairDistance = new BoolSetting(true);
        public IntSetting CrosshairStyle = new IntSetting(0);
        public IntSetting Speedometer = new IntSetting((int)SpeedometerType.Off);
        public BoolSetting ShowInterpolation = new BoolSetting(false);
        public BoolSetting ShowCrosshairArrows = new BoolSetting(false);
        public BoolSetting ShowKDR = new BoolSetting(false);
        public BoolSetting ShowPing = new BoolSetting(false);
        public BoolSetting ShowEmotes = new BoolSetting(true);
        public BoolSetting ShowKeybindTip = new BoolSetting(true);
        public BoolSetting ShowGameTime = new BoolSetting(false);
        public IntSetting ShowNames = new IntSetting(0);
        public IntSetting ShowHealthbars = new IntSetting(0);
        public BoolSetting HighVisibilityNames = new BoolSetting(false);
        public BoolSetting FadeMainMenu = new BoolSetting(false);
        public IntSetting ChatWidth = new IntSetting(320, minValue: 50, maxValue: 1000);
        public IntSetting ChatHeight = new IntSetting(295, minValue: 0, maxValue: 500);
        public IntSetting ChatFontSize = new IntSetting(18, minValue: 1, maxValue: 50);

        public override void Apply()
        {
            base.Apply();
            if (UIManager.CurrentMenu != null)
            {
                UIManager.CurrentMenu.ApplyScale(SceneLoader.SceneName);
            }
        }
    }

    public enum SpeedometerType
    {
        Off,
        Speed,
        Damage
    }

    public enum ShowMode
    {
        All,
        Mine,
        Others,
        None
    }
}
