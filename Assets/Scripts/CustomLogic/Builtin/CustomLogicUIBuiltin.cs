using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UI;
using Utility;

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
        
        /// <summary>"TopCenter" constant</summary>
        [CLProperty] public static string TopCenter => "TopCenter";
        
        /// <summary>"TopLeft" constant</summary>
        [CLProperty] public static string TopLeft => "TopLeft";
        
        /// <summary>"TopRight" constant</summary>
        [CLProperty] public static string TopRight => "TopRight";
        
        /// <summary>"MiddleCenter" constant</summary>
        [CLProperty] public static string MiddleCenter => "MiddleCenter";
        
        /// <summary>"MiddleLeft" constant</summary>
        [CLProperty] public static string MiddleLeft => "MiddleLeft";
        
        /// <summary>"MiddleRight" constant</summary>
        [CLProperty] public static string MiddleRight => "MiddleRight";
        
        /// <summary>"BottomCenter" constant</summary>
        [CLProperty] public static string BottomCenter => "BottomCenter";
        
        /// <summary>"BottomLeft" constant</summary>
        [CLProperty] public static string BottomLeft => "BottomLeft";
        
        /// <summary>"BottomRight" constant</summary>
        [CLProperty] public static string BottomRight => "BottomRight";
        
        /// <summary>
        /// Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".
        /// </summary>
        [CLMethod] public static void SetLabel(string label, string message) => InGameManager.SetLabel(label, message);
        
        /// <summary>
        /// Sets the label for a certain time, after which it will be cleared.
        /// </summary>
        [CLMethod] public static void SetLabelForTime(string label, string message, float time) => InGameManager.SetLabel(label, message, time);
        
        /// <summary>
        /// Sets the label for all players. Master client only. Be careful not to call this often.
        /// </summary>
        [CLMethod]
        public static void SetLabelAll(string label, string message)
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

        /// <summary>
        /// Sets the label for all players for a certain time. Master client only.
        /// </summary>
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
        
        /// <summary>
        /// Creates a new popup. This popup is hidden until shown.
        /// </summary>
        [CLMethod]
        public static string CreatePopup(string popupName, string title, int width, int height)
        {
            Menu.CreateCustomPopup(popupName, title, width, height);
            return popupName;
        }

        /// <summary>Shows the popup with given name.</summary>
        [CLMethod] public static void ShowPopup(string popupName) => Menu.GetCustomPopup(popupName).Show();
        
        /// <summary>Hides the popup with given name.</summary>
        [CLMethod] public static void HidePopup(string popupName) => Menu.GetCustomPopup(popupName).Hide();
        
        /// <summary>Clears all elements in popup with given name.</summary>
        [CLMethod] public static void ClearPopup(string popupName) => Menu.GetCustomPopup(popupName).Clear();
        
        /// <summary>Adds a text row to the popup with label as content.</summary>
        [CLMethod] public static void AddPopupLabel(string popupName, string label) => Menu.GetCustomPopup(popupName).AddLabel(label);
        
        /// <summary>Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.</summary>
        [CLMethod] public static void AddPopupButton(string popupName, string label, string callback) => Menu.GetCustomPopup(popupName).AddButton(label, callback);
        
        /// <summary>Adds a button to the bottom bar of the popup.</summary>
        [CLMethod] public static void AddPopupBottomButton(string popupName, string label, string callback) => Menu.GetCustomPopup(popupName).AddBottomButton(label, callback);
        
        /// <summary>Adds a list of buttons in a row to the popup.</summary>
        [CLMethod] public static void AddPopupButtons(string popupName, CustomLogicListBuiltin labels, CustomLogicListBuiltin callbacks) => Menu.GetCustomPopup(popupName).AddButtons(labels.List, callbacks.List);
        
        /// <summary>Returns a wrapped string given style and args.</summary>
        [CLMethod]
        public static string WrapStyleTag(string text, string style, string arg = null)
        {
            if (arg == null)
                return $"<{style}>{text}</{style}>";
            return $"<{style}={arg}>{text}</{style}>";
        }
        
        /// <summary>Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.</summary>
        [CLMethod] public static string GetLocale(string cat, string sub, string key) => UIManager.GetLocale(cat, sub, key);

        /// <summary>Returns the current language (e.g. "English" or "简体中文").</summary>
        [CLMethod] public static string GetLanguage() => SettingsManager.GeneralSettings.Language.Value;

        /// <summary>Shows the change character menu if main character is Human.</summary>
        [CLMethod] 
        public static void ShowChangeCharacterMenu()
        {
            var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (inGameManager.CurrentCharacter != null && inGameManager.CurrentCharacter is Human)
            {
                Menu.ShowCharacterChangeMenu();
            }
        }

        /// <summary>Sets the display of the scoreboard header (default "Kills / Deaths...")</summary>
        [CLMethod] public static void SetScoreboardHeader(string header) => CustomLogicManager.Evaluator.ScoreboardHeader = header;

        /// <summary>
        /// Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.
        /// </summary>
        [CLMethod] public static void SetScoreboardProperty(string property) => CustomLogicManager.Evaluator.ScoreboardProperty = $"CL:{property}";
        
        /// <summary>Gets the color of the specified item. See theme json for reference.</summary>
        [CLMethod]
        public static CustomLogicColorBuiltin GetThemeColor(string panel, string category, string item)
        {
            var color = new Color255(UIManager.GetThemeColor(panel, category, item));
            return new CustomLogicColorBuiltin(color);
        }
    }
}
