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

        public static void RegisterTitanKill(GameObject character, BasicTitan victim, KillWeapon weapon)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterTitanKill(character, victim, weapon);
        }

        public static void RegisterHumanKill(GameObject character, Human victim, KillWeapon weapon)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterHumanKill(character, victim, weapon);
        }

        public static void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterDamage(character, victim, weapon, damage);
        }

        public static void RegisterSpeed(GameObject character, float speed)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterSpeed(character, speed);
        }

        public static void RegisterInteraction(GameObject character, GameObject interact, InteractionType interactionType)
        {
            foreach (BaseGameProgressHandler handler in _handlers)
                handler.RegisterInteraction(character, interact, interactionType);
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

    public enum InteractionType
    {
        ShareGas,
        CarryHuman
    }
}
