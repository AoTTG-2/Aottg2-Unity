using System;
using UnityEngine;
using Utility;

namespace Settings
{
    class AbilitySettings : SaveableSettingsContainer
    {
        protected override string FileName { get { return "Ability.json"; } }
        public ColorSetting BombColor = new ColorSetting(new Color255(255, 255, 255, 255), minAlpha: 128);
        public IntSetting BombRadius = new IntSetting(6, minValue: 0, maxValue: 10);
        public IntSetting BombRange = new IntSetting(3, minValue: 0, maxValue: 10);
        public IntSetting BombSpeed = new IntSetting(6, minValue: 0, maxValue: 10);
        public IntSetting BombCooldown = new IntSetting(1, minValue: 0, maxValue: 10);
        public BoolSetting CursorCooldown = new BoolSetting(false);
        public BoolSetting ShowBombColors = new BoolSetting(false);
        public BoolSetting UseOldEffect = new BoolSetting(false);
        public BoolSetting BombCollision = new BoolSetting(false);
    }
}

