using System;
using UnityEngine;
using UI;
using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public IntSetting KDR = new IntSetting((int)KDRMode.Off);
        public BoolSetting ShowPing = new BoolSetting(false);
        public BoolSetting ShowEmotes = new BoolSetting(true);
        public BoolSetting ShowKeybindTip = new BoolSetting(true);
        public BoolSetting ShowGameTime = new BoolSetting(false);
        public IntSetting ShowNames = new IntSetting(0);
        public IntSetting ShowHealthbars = new IntSetting(0);
        public IntSetting HumanNameDistance = new IntSetting(500, minValue: 0, maxValue: 100000);
        public IntSetting NameOverrideTarget = new IntSetting((int)ShowMode.None);
        public IntSetting NameBackgroundType = new IntSetting((int)NameStyleType.Off);
        public ColorSetting ForceNameColor = new ColorSetting(new Utility.Color255(255, 255, 255));
        public ColorSetting ForceBackgroundColor = new ColorSetting(new Utility.Color255(0, 0, 0, 100));
        public IntSetting MinNameLength = new IntSetting(0, minValue: 0, maxValue: 100);
        public IntSetting MaxNameLength = new IntSetting(20, minValue: 0, maxValue: 100);
        public BoolSetting FadeMainMenu = new BoolSetting(false);
        public BoolSetting FadeLoadscreen = new BoolSetting(true);
        public IntSetting ChatWidth = new IntSetting(320, minValue: 0, maxValue: 1000);
        public IntSetting ChatHeight = new IntSetting(295, minValue: 0, maxValue: 500);
        public IntSetting ChatFontSize = new IntSetting(18, minValue: 1, maxValue: 50);
        public IntSetting ChatPoolSize = new IntSetting(0, minValue: 0, maxValue: 400);
        public IntSetting KillFeedCount = new IntSetting(3, minValue: 0, maxValue: 10);
        public BoolSetting JoinNotifications = new BoolSetting(true);
        public IntSetting Coordinates = new IntSetting((int)CoordinateMode.Off);
        public BoolSetting ShowChatTimestamp = new BoolSetting(false);
        public ColorSetting ChatBackgroundColor = new ColorSetting(new Utility.Color255(38, 38, 38, 0));


        public override void Apply()
        {
            base.Apply();
            if (UIManager.CurrentMenu != null)
            {
                UIManager.CurrentMenu.ApplyScale(SceneLoader.SceneName);
                if (UIManager.CurrentMenu is InGameMenu)
                {
                    InGameMenu igm = (InGameMenu)UIManager.CurrentMenu;
                    igm.ApplyUISettings();
                    if (igm.ChatPanel != null)
                    {
                        igm.ChatPanel.Sync();
                    }
                }
            }
        }
    }

    public enum NameStyleType
    {
        Off,
        Outline,
        Background
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

    public enum KDRMode
    {
        Off,
        Mine,
        All
    }

    public enum CoordinateMode
    {
        Off,
        Minimap,
        BottomRight
    }
}
