using UnityEngine;
using Utility;
using System;
using System.Collections.Generic;
using System.Collections;
using Characters;

namespace GameProgress
{
    class GameStatHandler : BaseGameProgressHandler
    {
        const int ExpPerKill = 10;
        const int ExpPerLevelBase = 500;
        const int MaxExpPerLevel = 2000;
        const float ExpPerLevelMultiplier = 1.2f;
        const int MaxLevel = 50;
        private List<int> _expPerLevel = new List<int>();
        private GameStatContainer _gameStat;

        public GameStatHandler(GameStatContainer gameStat)
        {
            _gameStat = gameStat;
            _expPerLevel.Add(ExpPerLevelBase);
            for (int i = 1; i < MaxLevel; i++)
                _expPerLevel.Add((int)(Mathf.Min(_expPerLevel[i - 1] * ExpPerLevelMultiplier, MaxExpPerLevel)));
        }

        public int GetExpToNext()
        {
            if (_gameStat.Level.Value >= MaxLevel)
                return 0;
            return _expPerLevel[_gameStat.Level.Value];
        }

        public void AddExp(int exp)
        {
            _gameStat.Exp.Value += exp;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            if (_gameStat.Level.Value >= MaxLevel)
            {
                _gameStat.Exp.Value = 0;
                _gameStat.Level.Value = MaxLevel;
                return;
            }
            if (_gameStat.Exp.Value <= 0)
                return;
            if (_gameStat.Exp.Value >= _expPerLevel[_gameStat.Level.Value])
            {
                _gameStat.Level.Value += 1;
                _gameStat.Exp.Value = 0;
            }
        }

        public override void RegisterTitanKill(BasicTitan victim, KillMethod method)
        {
            switch (method.Weapon)
            {
                case KillWeapon.Blade:
                    _gameStat.TitansKilledBlade.Value++;
                    break;
                case KillWeapon.AHSS:
                    _gameStat.TitansKilledAHSS.Value++;
                    break;
                case KillWeapon.APG:
                    _gameStat.TitansKilledAPG.Value++;
                    break;
                case KillWeapon.Thunderspear:
                    _gameStat.TitansKilledThunderspear.Value++;
                    break;
                default:
                    _gameStat.TitansKilledOther.Value++;
                    break;
            }
            _gameStat.TitansKilledTotal.Value++;
            AddExp(ExpPerKill);
        }

        public override void RegisterHumanKill(Human victim, KillMethod method)
        {
            switch (method.Weapon)
            {
                case KillWeapon.Blade:
                    _gameStat.HumansKilledBlade.Value++;
                    break;
                case KillWeapon.AHSS:
                    _gameStat.HumansKilledAHSS.Value++;
                    break;
                case KillWeapon.APG:
                    _gameStat.HumansKilledAPG.Value++;
                    break;
                case KillWeapon.Thunderspear:
                    _gameStat.HumansKilledThunderspear.Value++;
                    break;
                case KillWeapon.Titan:
                    _gameStat.HumansKilledTitan.Value++;
                    break;
                default:
                    _gameStat.HumansKilledOther.Value++;
                    break;
            }
            _gameStat.HumansKilledTotal.Value++;
            AddExp(ExpPerKill);
        }

        public override void RegisterDamage(GameObject victim, KillMethod method, int damage)
        {
            _gameStat.Damage.Register(method, (ulong)damage);
        }

        public override void RegisterSpeed(float speed)
        {
            _gameStat.HighestSpeed.Value = Mathf.Max(_gameStat.HighestSpeed.Value, speed);
        }

        public override void RegisterInteraction(GameObject interact, InteractionType type)
        {
        }
    }
}
