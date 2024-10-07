using System;
using UnityEngine;
using Settings;

namespace GameProgress
{
    class GameStatContainer : BaseSettingsContainer
    {
        public IntSetting Level = new IntSetting(1);
        public IntSetting Exp = new IntSetting(0);
        public FloatSetting PlayTime = new FloatSetting(0f);
        public FloatSetting HighestSpeed = new FloatSetting(0f, minValue: 0f, maxValue: 100000f);
        public IntSetting TitansKilledTotal = new IntSetting(0);
        public IntSetting TitansKilledBlade = new IntSetting(0);
        public IntSetting TitansKilledAHSS = new IntSetting(0);
        public IntSetting TitansKilledThunderspear = new IntSetting(0);
        public IntSetting TitansKilledAPG = new IntSetting(0);
        public IntSetting TitansKilledOther = new IntSetting(0);
        public IntSetting HumansKilledTotal = new IntSetting(0);
        public IntSetting HumansKilledBlade = new IntSetting(0);
        public IntSetting HumansKilledAHSS = new IntSetting(0);
        public IntSetting HumansKilledThunderspear = new IntSetting(0);
        public IntSetting HumansKilledAPG = new IntSetting(0);
        public IntSetting HumansKilledTitan = new IntSetting(0);
        public IntSetting HumansKilledOther = new IntSetting(0);
        public DamageSetting Damage = new DamageSetting();
    }
}
