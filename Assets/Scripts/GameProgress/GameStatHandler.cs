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
        const float ExpPerLevelMultiplier = 1.2f;
        const int MaxLevel = 20;
        private List<int> _expPerLevel = new List<int>();
        private GameStatContainer _gameStat;

        public GameStatHandler(GameStatContainer gameStat)
        {
            _gameStat = gameStat;
            _expPerLevel.Add(ExpPerLevelBase);
            for (int i = 1; i < MaxLevel; i++)
                _expPerLevel.Add((int)(_expPerLevel[i - 1] * ExpPerLevelMultiplier));
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
                _gameStat.Exp.Value -= _expPerLevel[_gameStat.Level.Value];
                _gameStat.Exp.Value = Math.Max(_gameStat.Exp.Value, 0);
                CheckLevelUp();
            }
        }

        public override void RegisterTitanKill(GameObject character, BasicTitan victim, KillWeapon weapon)
        {
            switch (weapon)
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

        public override void RegisterHumanKill(GameObject character, Human victim, KillWeapon weapon)
        {
            switch (weapon)
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

        public override void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
        {
            if (weapon == KillWeapon.Blade || weapon == KillWeapon.AHSS || weapon == KillWeapon.Thunderspear || weapon == KillWeapon.APG)
            {
                _gameStat.DamageHighestOverall.Value = Math.Max(_gameStat.DamageHighestOverall.Value, damage);
                _gameStat.DamageTotalOverall.Value += damage;
                if (weapon == KillWeapon.Blade)
                {
                    _gameStat.DamageHighestBlade.Value = Math.Max(_gameStat.DamageHighestBlade.Value, damage);
                    _gameStat.DamageTotalBlade.Value += damage;
                }
                else if (weapon == KillWeapon.AHSS)
                {
                    _gameStat.DamageHighestAHSS.Value = Math.Max(_gameStat.DamageHighestAHSS.Value, damage);
                    _gameStat.DamageTotalAHSS.Value += damage;
                }
                else if (weapon == KillWeapon.APG)
                {
                    _gameStat.DamageHighestAPG.Value = Math.Max(_gameStat.DamageHighestAPG.Value, damage);
                    _gameStat.DamageTotalAPG.Value += damage;
                }
                else if (weapon == KillWeapon.Thunderspear)
                {
                    _gameStat.DamageHighestThunderspear.Value = Math.Max(_gameStat.DamageHighestThunderspear.Value, damage);
                    _gameStat.DamageTotalThunderspear.Value += damage;
                }
            }
        }

        public override void RegisterSpeed(GameObject character, float speed)
        {
            _gameStat.HighestSpeed.Value = Mathf.Max(_gameStat.HighestSpeed.Value, speed);
        }

        public override void RegisterInteraction(GameObject character, GameObject interact, InteractionType type)
        {
        }
    }
}
