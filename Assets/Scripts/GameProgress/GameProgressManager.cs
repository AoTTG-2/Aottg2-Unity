using UnityEngine;
using System.Collections;
using Utility;
using System;
using System.Collections.Generic;
using Events;
using ApplicationManagers;
using Characters;

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

        public static void RegisterTitanKill(BasicTitan victim, KillMethod method)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterTitanKill(victim, method);
        }

        public static void RegisterHumanKill(Human victim, KillMethod method)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterHumanKill(victim, method);
        }

        public static void RegisterDamage(GameObject victim, KillMethod method, int damage)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterDamage(victim, method, damage);
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
        public KillWeapon Weapon;
        public string Special;

        public KillMethod(KillWeapon weapon, string special)
        {
            Weapon = weapon;
            Special = special;
        }

        public static implicit operator KillMethod(KillWeapon weapon) => new(weapon, "");
    }

    public enum InteractionType
    {
        ShareGas,
        CarryHuman
    }
}
