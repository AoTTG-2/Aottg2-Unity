using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UI;

namespace CustomLogic
{
    [CLType(Static = true, Abstract = true)]
    class CustomLogicUIBuiltin: CustomLogicClassInstanceBuiltin
    {
        private static readonly Dictionary<string, string> LastSetLabels = new();

        private static readonly object[] SetLabelRpcArgs = new object[3];
        private const string SetLabelRpc = nameof(RPCManager.SetLabelRPC);

        private static InGameMenu Menu => (InGameMenu)UIManager.CurrentMenu;

        public CustomLogicUIBuiltin(): base("UI")
        {
        }

        public void OnPlayerJoin(Player player)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var key in LastSetLabels.Keys)
                {
                    SetLabelRpcArgs[0] = key;
                    SetLabelRpcArgs[1] = LastSetLabels[key];
                    SetLabelRpcArgs[2] = 0f;
                    RPCManager.PhotonView.RPC(SetLabelRpc, player, SetLabelRpcArgs);
                }
            }
        }
        
        [CLProperty("TopCenter label type")]
        public static string TopCenter => "TopCenter";
        
        [CLProperty("TopLeft label type")]
        public static string TopLeft => "TopLeft";
        
        [CLProperty("TopRight label type")]
        public static string TopRight => "TopRight";
        
        [CLProperty("MiddleCenter label type")]
        public static string MiddleCenter => "MiddleCenter";
        
        [CLProperty("MiddleLeft label type")]
        public static string MiddleLeft => "MiddleLeft";
        
        [CLProperty("MiddleRight label type")]
        public static string MiddleRight => "MiddleRight";
        
        [CLProperty("BottomCenter label type")]
        public static string BottomCenter => "BottomCenter";
        
        [CLProperty("BottomLeft label type")]
        public static string BottomLeft => "BottomLeft";
        
        [CLProperty("BottomRight label type")]
        public static string BottomRight => "BottomRight";
        
        /// <summary>
        /// Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".
        /// </summary>
        /// <param name="label"></param>
        /// <param name="message"></param>
        [CLMethod] public static void SetLabel(string label, string message) => InGameManager.SetLabel(label, message);
        [CLMethod] public static void SetLabelForTime(string label, string message, float time) => InGameManager.SetLabel(label, message, time);
        
        [CLMethod]
        public static void SeLabelAll(string label, string message)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!LastSetLabels.ContainsKey(label) || message != LastSetLabels[label])
                {
                    SetLabelRpcArgs[0] = label;
                    SetLabelRpcArgs[1] = message;
                    SetLabelRpcArgs[2] = 0f;
                    RPCManager.PhotonView.RPC(SetLabelRpc, RpcTarget.All, SetLabelRpcArgs);
                }
                LastSetLabels[label] = message;
            }
        }

        [CLMethod]
        public static void SetLabelForTimeAll(string label, string message, float time)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetLabelRpcArgs[0] = label;
                SetLabelRpcArgs[1] = message;
                SetLabelRpcArgs[2] = time;
                RPCManager.PhotonView.RPC(SetLabelRpc, RpcTarget.All, SetLabelRpcArgs);
            }
        }
        
        [CLMethod]
        public static string CreatePopup(string popupName, string title, int width, int height)
        {
            Menu.CreateCustomPopup(popupName, title, width, height);
            return popupName;
        }

        [CLMethod] public static void ShowPopup(string popupName) => Menu.GetCustomPopup(popupName).Show();
        
        [CLMethod] public static void HidePopup(string popupName) => Menu.GetCustomPopup(popupName).Hide();
        
        [CLMethod] public static void ClearPopup(string popupName) => Menu.GetCustomPopup(popupName).Clear();
        
        [CLMethod] public static void AddPopupLabel(string popupName, string label) => Menu.GetCustomPopup(popupName).AddLabel(label);
        
        [CLMethod] public static void AddPopupButton(string popupName, string label, string callback) => Menu.GetCustomPopup(popupName).AddButton(label, callback);
        
        [CLMethod] public static void AddPopupBottomButton(string popupName, string label, string callback) => Menu.GetCustomPopup(popupName).AddBottomButton(label, callback);
        
        [CLMethod] public static void AddPopupButtons(string popupName, List<object> labels, List<object> callbacks) => Menu.GetCustomPopup(popupName).AddButtons(labels, callbacks); // todo: test c# type (List) to CL type (CLList) conversion
        
        [CLMethod]
        public static string WrapStyleTag(string text, string style, string arg = null) // todo: optional arg doesn't work, fix it
        {
            if (arg == null)
                return $"<{style}>{text}</{style}>";
            return $"<{style}={arg}>{text}</{style}>";
        }
        
        [CLMethod] public static string GetLocale(string cat, string sub, string key) => UIManager.GetLocale(cat, sub, key);

        [CLMethod] public static string GetLanguage() => SettingsManager.GeneralSettings.Language.Value;

        [CLMethod] 
        public static void ShowChangeCharacterMenu()
        {
            var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (inGameManager.CurrentCharacter != null && inGameManager.CurrentCharacter is Human)
            {
                Menu.ShowCharacterChangeMenu();
            }
        }

        [CLMethod] public static void SetScoreboardHeader(string header) => CustomLogicManager.Evaluator.ScoreboardHeader = header;

        [CLMethod] public static void SetScoreboardProperty(string property) => CustomLogicManager.Evaluator.ScoreboardProperty = $"CL:{property}";
    }
}
