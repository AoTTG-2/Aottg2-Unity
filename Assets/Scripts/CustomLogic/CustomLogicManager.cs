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

        public override void OnJoinedRoom()
        {
            RoomData.Clear();
            PersistentData.Clear();
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

        public static string GetLineNumberString(int line, int internalLogicOffset = 0)
        {
            // Base Logic
            if (line < 0) return $"{line + internalLogicOffset} (Internal Logic)";

            // Map Logic
            if (SettingsManager.InGameCurrent.General.GameMode.Value == BuiltinLevels.UseMapLogic) return $"{line} ({MapManager.MapScript.LogicStart + line} maplogic)";
            return line.ToString();
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            _instance.StopAllCoroutines();
            Evaluator = null;
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
            
            // Clear debugger state when CL is being unloaded
            if (SettingsManager.UISettings.EnableCLDebugger.Value)
            {
                CustomLogic.Debugger.CustomLogicDebugger.Instance.ClearState();
            }
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
                    RPCManager.PhotonView.RPC("LoadBuiltinLogicRPC", RpcTarget.All, new object[] { settings.GameMode.Value });
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
                    RPCManager.PhotonView.RPC("LoadBuiltinLogicRPC", player, new object[] { settings.GameMode.Value });
                else
                    CustomLogicTransfer.Transfer(player);
            }
        }

        public static Dictionary<string, BaseSetting> GetModeSettings(string source)
        {
            var evaluator = GetEditorEvaluator(source);
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

        public static CustomLogicEvaluator GetEditorEvaluator(string source)
        {
            // Rework this to pass around the builtin logic offset dependency bc this is annoying.
            // Also implement a proper stack for error handling via rethrowing exceptions and adding in method calls as stack frames.
            var lexer = GetLexer(source);
            var parser = new CustomLogicParser(lexer.GetTokens(), lexer.BuiltinLogicOffset);
            var evaluator = new CustomLogicEvaluator(parser.GetStartAst(), lexer.BuiltinLogicOffset);
            return evaluator;
        }

        public static string TryParseLogic(string source)
        {
            var lexer = GetLexer(source);
            var parser = new CustomLogicParser(lexer.GetTokens());
            if (lexer.Error != string.Empty)
                return lexer.Error;
            parser.GetStartAst();
            if (parser.Error != string.Empty)
                return parser.Error;
            return "";
        }

        /// <summary>
        /// Need to maintain the following information in the lexer, parser, evaluator, and manager. 
        /// If a line number is negative, it is part of baselogic and we need to remove the offset.
        /// If the mode selected is "Map Logic", the non-negative part is offset by the current maps logic offset.
        /// </summary>
        public static void StartLogic(Dictionary<string, BaseSetting> modeSettings)
        {
            // Wait for debugger connection if enabled and requested
            if (SettingsManager.UISettings.EnableCLDebugger.Value && 
                SettingsManager.UISettings.WaitForDebuggerConnection.Value)
            {
                float timeout = SettingsManager.UISettings.DebuggerConnectionTimeout.Value;
                CustomLogic.Debugger.CustomLogicDebugger.Instance.WaitForConnection(timeout);
            }

            var lexer = GetLexer(Logic);
            var parser = new CustomLogicParser(lexer.GetTokens(), lexer.BuiltinLogicOffset);
            Evaluator = new CustomLogicEvaluator(parser.GetStartAst(), lexer.BuiltinLogicOffset);
            Evaluator.Start(modeSettings);
        }

        private static CustomLogicLexer GetLexer(string logic)
        {
            int baseLogicLines = BaseLogic.Split('\n').Length;
            return new CustomLogicLexer(BaseLogic + "\n" + logic, baseLogicLines);
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
