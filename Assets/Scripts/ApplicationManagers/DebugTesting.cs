using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility;
using System.IO;
using CustomLogic;
using Events;
using Map;
using Effects;
using GameManagers;
using Controllers;
using Unity.VisualScripting;

namespace ApplicationManagers
{
    /// <summary>
    /// Main testing module. This can be used to define developer tests or to create debug commands for external testers.
    /// </summary>
    class DebugTesting : MonoBehaviour
    {
        static DebugTesting _instance;
        public static bool DebugColliders = false;
        public static bool DebugPhase = false;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnLoadScene += OnLoadScene;
        }

        public static void RunTests()
        {
            if (!ApplicationConfig.DevelopmentMode)
                return;
        }

        public static void RunLateTests()
        {
            if (!ApplicationConfig.DevelopmentMode)
                return;
            //CustomLogicManager.Logic = BuiltinLevels.LoadLogic("test2");
            //var settings = CustomLogicManager.GetModeSettings(CustomLogicManager.Logic);
            //CustomLogicManager.StartLogic(settings);
        }

        private static void OnLoadScene(SceneName sceneName)
        {
            if (sceneName != SceneName.DebugTest)
                return;
        }

        public static void Log(object message)
        {
            Debug.Log(message);
        }

        void Update()
        {
        }

        public static void RunDebugCommand(string command)
        {
            if (!ApplicationConfig.DevelopmentMode)
            {
                Debug.Log("Debug commands are not available in release mode.");
                return;
            }
            string[] args = command.Split(' ');
            switch (args[0])
            {
                case "colliders":
                    DebugColliders = !DebugColliders;
                    Debug.Log("Debug colliders enabled: " + DebugColliders.ToString());
                    break;
                case "phase":
                    DebugPhase = !DebugPhase;
                    Debug.Log("Debug phase enabled: " + DebugPhase.ToString());
                    break;
                case "generate_char_previews":
                    ((CharacterEditorGameManager)SceneLoader.CurrentGameManager).GeneratePreviews();
                    break;
                case "generate_titan_keyframes":
                    var titan = ((InGameManager)SceneLoader.CurrentGameManager).CurrentCharacter;
                    titan.AddComponent<DebugAttackKeyframes>();
                    break;
                default:
                    Debug.Log("Invalid debug command.");
                    break;
            }
        }
    }
}
