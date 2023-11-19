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
        public IntSetting BombRange = new IntSetting(3, minValue: 0, maxValue: 3);
        public IntSetting BombSpeed = new IntSetting(6, minValue: 0, maxValue: 10);
        public IntSetting BombCooldown = new IntSetting(1, minValue: 0, maxValue: 6);
        public BoolSetting ShowBombColors = new BoolSetting(false);
        public BoolSetting UseOldEffect = new BoolSetting(false);

        protected override bool Validate()
        {
            return BombRadius.Value + BombRange.Value + BombSpeed.Value + BombCooldown.Value <= 16;
        }
    }
}
