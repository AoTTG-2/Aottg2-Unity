using Anticheat;
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
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                Debug.Log("Running in headless mode!");
                string[] args = System.Environment.GetCommandLineArgs();
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-region":
                            if (i + 1 < args.Length)
                            {
                                string region = args[i + 1];
                                switch (region)
                                {
                                    case "eu":
                                        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.EU);
                                        break;
                                    case "us":
                                        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.US);
                                        break;
                                    case "sa":
                                        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.SA);
                                        break;
                                    case "asia":
                                        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
                                        break;
                                    case "cn":
                                        SettingsManager.MultiplayerSettings.ConnectServer(MultiplayerRegion.CN);
                                        break;
                                }
                            }
                            break;

                        case "-preset":
                            if (i + 1 < args.Length)
                            {
                                string preset = args[i + 1];
                                foreach (InGameSet set in SettingsManager.InGameSettings.InGameSets.Sets.GetItems())
                                {
                                    if (set.Name.Value == preset)
                                    {
                                        SettingsManager.InGameCurrent.Copy(set);
                                    }
                                }
                                SettingsManager.MultiplayerSettings.StartRoom();
                            }
                            break;
                    }
                }
            }
        }
    }
}