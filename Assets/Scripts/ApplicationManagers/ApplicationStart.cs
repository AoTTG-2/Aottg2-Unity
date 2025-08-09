﻿using Anticheat;
using Assets.Scripts.ApplicationManagers;
using Characters;
using CustomLogic;
using CustomSkins;
using Events;
using GameManagers;
using GameProgress;
using Map;
using Photon;
using Photon.Pun;
using Settings;
using System.Globalization;
using System.Threading;
using UI;
using UnityEngine;
using Utility;
using Weather;

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
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0;
            PhotonNetwork.UseRpcMonoBehaviourCache = true;
            ApplicationConfig.Init();
            AnticheatManager.Init();
            ChatFilter.Init();
            PhysicsLayer.Init();
            MaterialCache.Init();
            EventManager.Init();
            HumanSetup.Init();
            if (Application.platform == RuntimePlatform.LinuxPlayer)
            {
                DataMigrator.MigrateLinuxSaves();
            }
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
            VoiceChatManager.Init();
            CustomSerialization.Init();

            // debug
            DebugConsole.Init();
            DebugLagSim.Init();
            CustomDebug.Init();
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
            DiscordManager.Init();
        }
    }
}