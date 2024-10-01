using UnityEngine;
using System.Collections;
using Utility;
using System;
using System.Collections.Generic;
using Events;
using ApplicationManagers;
using Characters;
using GameManagers;

namespace GameProgress
{
    class GameProgressManager : MonoBehaviour
    {
        static GameProgressManager _instance;
        public static GameProgressContainer GameProgress;
        private static GameStatHandler _gameStatHandler;
        private static AchievementHandler _achievementHandler;
        private static QuestHandler _questHandler;
        private static List<BaseGameProgressHandler> _handlers = new List<BaseGameProgressHandler>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            GameProgress = new GameProgressContainer();
            _instance.StartCoroutine(_instance.IncrementPlayTime());
            _gameStatHandler = new GameStatHandler(GameProgress.GameStat);
            _achievementHandler = new AchievementHandler(GameProgress.Achievement);
            _questHandler = new QuestHandler(GameProgress.Quest);
            _handlers.Add(_gameStatHandler);
            _handlers.Add(_achievementHandler);
            _handlers.Add(_questHandler);
            EventManager.OnLoadScene += OnLoadScene;
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public static void OnLoadScene(SceneName sceneName)
        {
            if (sceneName == SceneName.MainMenu)
            {
                Save();
                _achievementHandler.ReloadAchievements();
                _questHandler.ReloadQuests();
            }
        }

        private static void Save()
        {
            GameProgress.Save();
        }

        public static int GetExpToNext()
        {
            return _gameStatHandler.GetExpToNext();
        }

        public static void AddExp(int exp)
        {
            _gameStatHandler.AddExp(exp);
        }

        public static void RegisterKill(BaseCharacter player, BaseCharacter enemy)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterKill(player, enemy);
        }

        public static void RegisterDamage(BaseCharacter player, BaseCharacter enemy, int damage)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterDamage(player, enemy, damage);
        }

        public static void RegisterSpeed(float speed)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterSpeed(speed);
        }

        public static void RegisterInteraction(GameObject interact, InteractionType interactionType)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterInteraction(interact, interactionType);
        }

        private IEnumerator IncrementPlayTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                GameProgress.GameStat.PlayTime.Value += 10f;
            }
        }
    }

    public enum KillWeapon
    {
        Blade,
        AHSS,
        Thunderspear,
        APG,
        Other,
        Shifter,
        Titan
    }

    public struct KillMethod
    {
        public const string NullSpecialKey = "None";

        public KillWeapon Weapon;
        public string Special;

        public KillMethod(KillWeapon weapon, string special)
        {
            Weapon = weapon;
            Special = special;
        }

        public static KillMethod FromCurrentCharacter()
        {
            var character = (SceneLoader.CurrentGameManager as InGameManager)?.CurrentCharacter;
            if (!character) throw new InvalidOperationException("Cannot get KillMethod when no Character exists");

            KillMethod killMethod = KillWeapon.Other;
            if (character is Human human)
            {
                if (human.Setup.Weapon == HumanWeapon.AHSS)
                    killMethod = KillWeapon.AHSS;
                else if (human.Setup.Weapon == HumanWeapon.Blade)
                    killMethod = KillWeapon.Blade;
                else if (human.Setup.Weapon == HumanWeapon.Thunderspear)
                    killMethod = KillWeapon.Thunderspear;
                else if (human.Setup.Weapon == HumanWeapon.APG)
                    killMethod = KillWeapon.APG;
                killMethod.Special = human.State == HumanState.SpecialAttack ? human.CurrentSpecial : "";
            }
            else if (character is BasicTitan)
                killMethod = KillWeapon.Titan;
            else if (character is BaseShifter shifter)
            {
                killMethod = KillWeapon.Shifter;
                killMethod.Special = shifter switch
                {
                    ErenShifter => "Eren",
                    AnnieShifter => "Annie",
                    _ => "",
                };
            }

            return killMethod;
        }

        public static implicit operator KillMethod(KillWeapon weapon) => new(weapon, "");

        public readonly string WeaponKey => Weapon.ToString();
        public readonly string SpecialKey => string.IsNullOrEmpty(Special) ? NullSpecialKey : Special;

        public override readonly string ToString() => $"({WeaponKey}, {SpecialKey})";
    }

    public enum InteractionType
    {
        ShareGas,
        CarryHuman
    }
}
