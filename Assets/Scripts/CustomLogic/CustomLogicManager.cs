using ApplicationManagers;
using Events;
using GameManagers;
using Map;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicManager: Photon.Pun.MonoBehaviourPunCallbacks
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
        public static Dictionary<string, object> RoomData = new Dictionary<string, object>();
        public static Dictionary<string, object> PersistentData = new Dictionary<string, object>();

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
            CameraFOV = 0f;
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
            var lexer = GetLexer(source);
            var parser = new CustomLogicParser(lexer.GetTokens());
            var evaluator = new CustomLogicEvaluator(parser.GetStartAst());
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

        public static void StartLogic(Dictionary<string, BaseSetting> modeSettings)
        {
            var lexer = GetLexer(Logic);
            var parser = new CustomLogicParser(lexer.GetTokens());
            Evaluator = new CustomLogicEvaluator(parser.GetStartAst());
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
