using UnityEngine;
using Utility;
using Settings;
using UI;
using Weather;
using System.Collections;
using GameProgress;
using Map;
using GameManagers;
using Events;
using Characters;
using CustomLogic;
using CustomSkins;
using Anticheat;
using Photon;
using Photon.Pun;
using System.Threading;
using System.Globalization;

namespace ApplicationManagers
{
    /// <summary>
    /// Application entry point. Runs on main scene load, and handles loading every other manager.
    /// </summary>
    class ApplicationStart : UnityEngine.MonoBehaviour
    {
        private static bool _firstLaunch = true;

        public void Awake()
        {
            if (_firstLaunch)
            {
                _firstLaunch = false;
                Init();
            }
        }
       
        private static void Init()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            DebugConsole.Init();
            ApplicationConfig.Init();
            AnticheatManager.Init();
            PhysicsLayer.Init();
            MaterialCache.Init();
            EventManager.Init();
            HumanSetup.Init();
            SettingsManager.Init();
            FullscreenHandler.Init();
            UIManager.Init();
            SnapshotManager.Init();
            CursorManager.Init();
            WeatherManager.Init();
            GameProgressManager.Init();
            MapManager.Init();
            CustomLogicManager.Init();
            ChatManager.Init();
            PastebinLoader.Init();
            AssetBundleManager.Init();
            MusicManager.Init();
            CustomSerialization.Init();
            if (ApplicationConfig.DevelopmentMode)
            {
                DebugTesting.Init();
                DebugTesting.RunTests();
            }
            BasicTitanSetup.Init();
            CharacterData.Init();
            MiscInfo.Init();
            PastebinLoader.LoadPastebin();
            SceneLoader.Init();
            EventManager.InvokeFinishInit();
            if (ApplicationConfig.DevelopmentMode)
                DebugTesting.RunLateTests();
        }
    }
}