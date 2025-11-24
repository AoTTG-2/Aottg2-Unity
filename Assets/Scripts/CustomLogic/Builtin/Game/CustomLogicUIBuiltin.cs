using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UI;
using Utility;
using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// UI label functions.
    /// </summary>
    [CLType(Name = "UI", Static = true, Abstract = true)]
    partial class CustomLogicUIBuiltin : BuiltinClassInstance
    {
        private static readonly Dictionary<string, string> LastSetLabels = new();

        private static readonly object[] SetLabelRpcArgs = new object[3];
        private const string SetLabelRpc = nameof(RPCManager.SetLabelRPC);

        private static InGameMenu Menu => (InGameMenu)UIManager.CurrentMenu;

        [CLConstructor]
        public CustomLogicUIBuiltin()
        {
            LastSetLabels.Clear();
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

        [CLMethod("Returns if the given popup is active")]
        public static bool IsPopupActive(string popupName) => Menu.GetCustomPopup(popupName).IsActive;

        [CLProperty("Returns a list of all popups")]
        public static CustomLogicListBuiltin GetPopups
        {
            get
            {
                var result = new CustomLogicListBuiltin();
                foreach (var popup in Menu.GetAllCustomPopups())
                    result.List.Add(popup);
                return result;
            }
        }

        /// <summary>
        /// Sets whether a label is active or not.
        /// </summary>
        [CLMethod]
        public static void SetLabelActive(string label, bool active)
        {
            Menu.SetLabelActive(label, active);
        }

        /// <summary>
        /// Sets whether the KDR panel (top-left) is active or not.
        /// </summary>
        [CLMethod]
        public static void SetKDRPanelActive(bool active)
        {
            Menu.SetKDRPanelActive(active);
        }

        /// <summary>
        /// Sets whether the minimap is active or not.
        /// </summary>
        [CLMethod]
        public static void SetMinimapActive(bool active)
        {
            Menu.SetMinimapActive(active);
        }

        /// <summary>
        /// Sets whether the chat panel is active or not.
        /// </summary>
        [CLMethod]
        public static void SetChatPanelActive(bool active)
        {
            Menu.SetChatPanelActive(active);
        }

        /// <summary>
        /// Sets whether the feed panel is active or not.
        /// </summary>
        [CLMethod]
        public static void SetFeedPanelActive(bool active)
        {
            Menu.SetFeedPanelActive(active);
        }

        /// <summary>
        /// Sets whether the bottom HUD is active or not.
        /// This can only be used when the character is alive.
        /// </summary>
        [CLMethod]
        public static void SetBottomHUDActive(bool active)
        {
            Menu.SetBottomHUDActive(active);
        }

        /// <summary>
        /// Returns the root `VisualElement` which you can add other elements to.
        /// </summary>
        /// <returns>The root `VisualElement`</returns>
        [CLMethod]
        public static CustomLogicVisualElementBuiltin GetRootVisualElement()
        {
            return new CustomLogicVisualElementBuiltin(Menu.RootVisualElement);
        }

        /// <summary>
        /// Creates a new `VisualElement`.
        /// </summary>
        [CLMethod]
        public static CustomLogicVisualElementBuiltin VisualElement()
        {
            return new CustomLogicVisualElementBuiltin(new VisualElement());
        }

        /// <summary>
        /// Creates a new `Button` with optional text and click event.
        /// </summary>
        /// <param name="text">The text that the button displays</param>
        /// <param name="clickEvent">The function that will be called when button is clicked</param>
        [CLMethod]
        public static CustomLogicButtonBuiltin Button(string text = "", UserMethod clickEvent = null)
        {
            return new CustomLogicButtonBuiltin(new Button { text = text }).OnClick(clickEvent);
        }

        /// <summary>
        /// Creates a new `Label` with optional text.
        /// </summary>
        /// <param name="text">The text to be displayed</param>
        [CLMethod]
        public static CustomLogicLabelBuiltin Label(string text = "")
        {
            return new CustomLogicLabelBuiltin(new Label { text = text });
        }

        /// <summary>
        /// Creates a new `TextField` with optional label.
        /// </summary>
        [CLMethod]
        public static CustomLogicTextFieldBuiltin TextField(string label = "")
        {
            return new CustomLogicTextFieldBuiltin(new TextField(label));
        }

        /// <summary>
        /// Creates a new `Toggle` with optional label and value changed event.
        /// </summary>
        /// <param name="label">The label text displayed next to the toggle</param>
        /// <param name="valueChangedEvent">The function that will be called when toggle value changes</param>
        [CLMethod]
        public static CustomLogicToggleBuiltin Toggle(string label = "", UserMethod valueChangedEvent = null)
        {
            return new CustomLogicToggleBuiltin(new Toggle(label)).OnValueChanged(valueChangedEvent);
        }

        /// <summary>
        /// Creates a new `Slider` for floating-point values with optional range, tick interval, and value changed event.
        /// The slider will snap to values at multiples of the tick interval.
        /// </summary>
        /// <param name="lowValue">The minimum value of the slider</param>
        /// <param name="highValue">The maximum value of the slider</param>
        /// <param name="tickInterval">The interval between allowed values. If 0, no snapping occurs. For example, 0.1 will snap to 0.0, 0.1, 0.2, etc.</param>
        /// <param name="label">The label text displayed next to the slider</param>
        /// <param name="valueChangedEvent">The function that will be called when slider value changes</param>
        [CLMethod]
        public static CustomLogicSliderBuiltin Slider(float lowValue = 0f, float highValue = 100f, float tickInterval = 0f, string label = "", UserMethod valueChangedEvent = null)
        {
            var slider = new Slider(label, lowValue, highValue);
            if (tickInterval > 0f)
                slider.pageSize = tickInterval;
            return new CustomLogicSliderBuiltin(slider).OnValueChanged(valueChangedEvent);
        }

        /// <summary>
        /// Creates a new `Slider` for integer values with optional range, tick interval, and value changed event.
        /// The slider will snap to values at multiples of the tick interval.
        /// </summary>
        /// <param name="lowValue">The minimum value of the slider</param>
        /// <param name="highValue">The maximum value of the slider</param>
        /// <param name="tickInterval">The interval between allowed values. For example, 5 will snap to 0, 5, 10, 15, etc.</param>
        /// <param name="label">The label text displayed next to the slider</param>
        /// <param name="valueChangedEvent">The function that will be called when slider value changes</param>
        [CLMethod]
        public static CustomLogicSliderBuiltin SliderInt(int lowValue = 0, int highValue = 100, int tickInterval = 1, string label = "", UserMethod valueChangedEvent = null)
        {
            var sliderInt = new SliderInt(label, lowValue, highValue);
            if (tickInterval > 1)
                sliderInt.pageSize = tickInterval;
            return new CustomLogicSliderBuiltin(sliderInt).OnValueChanged(valueChangedEvent);
        }

        /// <summary>
        /// Creates a new `Dropdown` with a list of choices and optional label and value changed event.
        /// </summary>
        /// <param name="choices">List of string options to display in the dropdown</param>
        /// <param name="defaultIndex">The index of the initially selected option (default: 0)</param>
        /// <param name="label">The label text displayed next to the dropdown</param>
        /// <param name="valueChangedEvent">The function that will be called when dropdown value changes</param>
        [CLMethod]
        public static CustomLogicDropdownBuiltin Dropdown(CustomLogicListBuiltin choices, int defaultIndex = 0, string label = "", UserMethod valueChangedEvent = null)
        {
            var choicesList = new System.Collections.Generic.List<string>();
            foreach (var item in choices.List)
            {
                if (item != null)
                    choicesList.Add(item.ToString());
            }

            if (choicesList.Count == 0)
                choicesList.Add("No options");

            var dropdown = new DropdownField(label, choicesList, defaultIndex);
            return new CustomLogicDropdownBuiltin(dropdown).OnValueChanged(valueChangedEvent);
        }

        /// <summary>
        /// Creates a new `ProgressBar` with optional range, title, and value changed event.
        /// </summary>
        /// <param name="lowValue">The minimum value of the progress bar (default: 0)</param>
        /// <param name="highValue">The maximum value of the progress bar (default: 100)</param>
        /// <param name="title">The title text displayed on the progress bar</param>
        /// <param name="valueChangedEvent">The function that will be called when progress bar value changes</param>
        [CLMethod]
        public static CustomLogicProgressBarBuiltin ProgressBar(float lowValue = 0f, float highValue = 100f, string title = "", UserMethod valueChangedEvent = null)
        {
            var progressBar = new ProgressBar
            {
                lowValue = lowValue,
                highValue = highValue,
                title = title
            };
            return new CustomLogicProgressBarBuiltin(progressBar).OnValueChanged(valueChangedEvent);
        }

        /// <summary>
        /// Creates a new `ScrollView` for scrollable content.
        /// </summary>
        [CLMethod]
        public static CustomLogicScrollViewBuiltin ScrollView()
        {
            return new CustomLogicScrollViewBuiltin(new ScrollView());
        }
    }
}
