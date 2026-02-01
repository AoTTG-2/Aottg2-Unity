using ApplicationManagers;
using Events;
using GameManagers;
using Map;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicManager : Photon.Pun.MonoBehaviourPunCallbacks
    {
        public static CustomLogicManager _instance;
        public static CustomLogicEvaluator Evaluator;
        public static bool LogicLoaded;
        public static string Logic;
        public static string LogicHash;
        public static string BaseLogic;
        public static CustomLogicCompiler Compiler;
        public static bool Cutscene;
        public static bool ManualCamera;
        public static float CameraFOV;
        public static bool SkipCutscene;
        public static Vector3 CameraPosition;
        public static Vector3 CameraRotation;
        public static Vector3 CameraVelocity;
        public static CameraInputMode? CameraMode = null;
        public static bool CameraLocked;
        public static bool CursorVisible;
        public static HashSet<KeybindSetting> KeybindDefaultDisabled = new HashSet<KeybindSetting>();
        public static HashSet<KeybindSetting> KeybindHold = new HashSet<KeybindSetting>();
        public static Dictionary<string, object> RoomData = new Dictionary<string, object>();
        public static Dictionary<string, object> PersistentData = new Dictionary<string, object>();
        public static HashSet<string> GeneralComponents = new HashSet<string>();
        public static HashSet<string> InternalComponents = new HashSet<string>();
        public static bool IsWaitingForRestart => _hasRestarted;
        private static bool _hasRestarted = false;

        public static void WaitForRestart()
        {
            _hasRestarted = false;
            Evaluator = null;
        }

        public override void OnJoinedRoom()
        {
            RoomData.Clear();
            PersistentData.Clear();
            CameraLocked = false;
            CursorVisible = false;
        }

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            CustomLogicSymbols.Init();
            CustomLogicTransfer.Init();
            EventManager.OnLoadScene += OnLoadScene;
            EventManager.OnPreLoadScene += OnPreLoadScene;
            BaseLogic = ResourceManager.TryLoadText(ResourcePaths.Modes, "BaseLogic");
            string[] lines = BaseLogic.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("# general") || lines[i].StartsWith("# internal"))
                {
                    if (i < lines.Length - 1 && lines[i + 1].StartsWith("component"))
                    {
                        string component = lines[i + 1].Substring("component ".Length).Trim();
                        if (lines[i].StartsWith("# general"))
                            GeneralComponents.Add(component);
                        else if (lines[i].StartsWith("# internal"))
                            InternalComponents.Add(component);
                    }
                }
            }
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            _instance.StopAllCoroutines();
            _hasRestarted = true;
            Evaluator = null;
            Compiler = null;
            LogicLoaded = false;
            Cutscene = false;
            SkipCutscene = false;
            ManualCamera = false;
            CameraPosition = Vector3.zero;
            CameraRotation = Vector3.zero;
            CameraVelocity = Vector3.zero;
            CameraMode = null;
            CameraFOV = 0f;
            KeybindDefaultDisabled.Clear();
            KeybindHold.Clear();
            CustomLogicUIBuiltin.ClearLabels();
            CameraLocked = false;
            CursorVisible = false;
        }

        public static void ToggleCutscene(bool cutscene)
        {
            if (cutscene != Cutscene)
            {
                Cutscene = cutscene;
                if (Cutscene)
                    ((InGameMenu)UIManager.CurrentMenu).SetCharacterMenu(false);
                else if (SettingsManager.InGameCharacterSettings.ChooseStatus.Value == (int)ChooseCharacterStatus.Choosing)

                    ((InGameMenu)UIManager.CurrentMenu).SetCharacterMenu(true);
            }
        }

        private static void OnLoadScene(SceneName sceneName)
        {
            if (sceneName == SceneName.InGame)
                StartInGame();
            else
                LogicLoaded = true;
        }

        public static void StartInGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InGameGeneralSettings settings = SettingsManager.InGameCurrent.General;
                if (BuiltinLevels.IsLogicBuiltin(settings.GameMode.Value))
                {
                    CustomLogicTransfer.LogicTransferReady = true;
                    RPCManager.PhotonView.RPC(nameof(RPCManager.LoadBuiltinLogicRPC), RpcTarget.All, new object[] { settings.GameMode.Value });
                }
                else
                {
                    Logic = BuiltinLevels.LoadLogic(settings.GameMode.Value);
                    LogicHash = Util.CreateMD5(Logic);
                    CustomLogicTransfer.Start();
                    OnLoadCachedLogicRPC(Util.CreateLocalPhotonInfo());
                }
            }
        }

        public static void OnLoadBuiltinLogicRPC(string name, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            Logic = BuiltinLevels.LoadLogic(name);
            LogicHash = Util.CreateMD5(Logic);
            CustomLogicTransfer.LogicHash = string.Empty;
            FinishLoadLogic();
        }

        public static void OnLoadCachedLogicRPC(PhotonMessageInfo info)
        {
            if (info.Sender != null && !info.Sender.IsMasterClient)
                return;
            FinishLoadLogic();
        }

        public static void FinishLoadLogic()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperty(PlayerProperty.CustomLogicHash, CustomLogicTransfer.LogicHash);
            LogicLoaded = true;
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient && CustomLogicTransfer.LogicTransferReady)
            {
                InGameGeneralSettings settings = SettingsManager.InGameCurrent.General;
                if (BuiltinLevels.IsLogicBuiltin(settings.GameMode.Value))
                    RPCManager.PhotonView.RPC(nameof(RPCManager.LoadBuiltinLogicRPC), player, new object[] { settings.GameMode.Value });
                else
                    CustomLogicTransfer.Transfer(player);
            }
        }

        public static Dictionary<string, BaseSetting> GetModeSettings(string source)
        {
            var evaluator = GetEditorEvaluator(source, false);
            return evaluator.GetModeSettings();
        }

        public static string GetModeDescription(Dictionary<string, BaseSetting> settings)
        {
            if (settings.ContainsKey("Description") && settings["Description"] is StringSetting)
            {
                return ((StringSetting)settings["Description"]).Value;
            }
            return "";
        }

        public static CustomLogicEvaluator GetEditorEvaluator(string source, bool loadBaseLogic = true)
        {
            var compiler = new CustomLogicCompiler();
            
            // Add base logic
            if (loadBaseLogic)
                compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", BaseLogic, CustomLogicSourceType.BaseLogic));
            
            // Add the user source
            compiler.AddSourceFile(new CustomLogicSourceFile("UserSource.cl", source, CustomLogicSourceType.ModeLogic));
            
            // Compile
            string combinedSource = compiler.Compile();
            
            var lexer = new CustomLogicLexer(combinedSource, compiler);
            var parser = new CustomLogicParser(lexer.GetTokens(), compiler);
            var evaluator = new CustomLogicEvaluator(parser.GetStartAst(), compiler);
            return evaluator;
        }

        public static string TryParseLogic(string source)
        {
            var compiler = new CustomLogicCompiler();
            
            // Add base logic
            compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", BaseLogic, CustomLogicSourceType.BaseLogic));
            
            // Add the user source
            compiler.AddSourceFile(new CustomLogicSourceFile("UserSource.cl", source, CustomLogicSourceType.ModeLogic));
            
            // Compile
            string combinedSource = compiler.Compile();
            
            var lexer = new CustomLogicLexer(combinedSource, compiler);
            var parser = new CustomLogicParser(lexer.GetTokens(), compiler);
            if (lexer.Error != string.Empty)
                return lexer.Error;
            parser.GetStartAst();
            if (parser.Error != string.Empty)
                return parser.Error;
            return "";
        }

        /// <summary>
        /// Starts the custom logic system with the given mode settings.
        /// Loads files in the following order:
        /// 1. C# bindings (implicit)
        /// 2. BaseLogic
        /// 3. Addons (if any)
        /// 4. Map logic (if applicable)
        /// 5. Mode logic
        /// </summary>
        public static void StartLogic(Dictionary<string, BaseSetting> modeSettings)
        {
            Compiler = new CustomLogicCompiler();
            
            // Add base logic (always first)
            Compiler.AddSourceFile(new CustomLogicSourceFile("BaseLogic.cl", BaseLogic, CustomLogicSourceType.BaseLogic));
            
            // TODO: Add addon support here in the future
            // For now, addons can be added via BuiltinLevels.GetAddonFiles()
            
            // Add map logic if using "Map Logic" mode
            InGameGeneralSettings settings = SettingsManager.InGameCurrent.General;
            if (settings.GameMode.Value == BuiltinLevels.UseMapLogic && MapManager.MapScript != null)
            {
                string mapLogic = MapManager.MapScript.Logic;
                if (!string.IsNullOrEmpty(mapLogic))
                {
                    string mapName = settings.MapName.Value;
                    Compiler.AddSourceFile(new CustomLogicSourceFile($"{mapName}.txt", mapLogic, CustomLogicSourceType.MapLogic));
                }
            }
            
            // Add mode logic (always last, unless it's Map Logic mode and map has no logic)
            if (!string.IsNullOrEmpty(Logic))
            {
                string modeName = settings.GameMode.Value;
                if (modeName == BuiltinLevels.UseMapLogic)
                {
                    // No mode logic if using MapLogic -> note after rewrite but before refactor we still loaded modelogic but maplogic came after.
                    modeName = "MapLogic";
                }
                else
                {
                    Compiler.AddSourceFile(new CustomLogicSourceFile($"{modeName}.cl", Logic, CustomLogicSourceType.ModeLogic));
                }
            }
            
            // Compile all sources
            string combinedSource = Compiler.Compile();
            
            var lexer = new CustomLogicLexer(combinedSource, Compiler);
            var parser = new CustomLogicParser(lexer.GetTokens(), Compiler);
            Evaluator = new CustomLogicEvaluator(parser.GetStartAst(), Compiler);
            Evaluator.Start(modeSettings);
        }

        private void FixedUpdate()
        {
            if (Evaluator != null)
                Evaluator.OnTick();
        }

        private void Update()
        {
            if (Evaluator != null)
            {
                Evaluator.OnFrame();
                if (Cutscene || ManualCamera)
                    CameraPosition += CameraVelocity * Time.deltaTime;
            }
            if (SkipCutscene && !Cutscene)
                SkipCutscene = false;
            if (Cutscene && (SettingsManager.GeneralSettings.SkipCutscenes.Value ||
                (!ChatManager.IsChatActive() && !InGameMenu.InMenu() && SettingsManager.InputSettings.General.SkipCutscene.GetKeyDown())))
            {
                SkipCutscene = true;
            }
        }

        private void LateUpdate()
        {
            if (Evaluator != null)
                Evaluator.OnLateFrame();
        }
    }
}
