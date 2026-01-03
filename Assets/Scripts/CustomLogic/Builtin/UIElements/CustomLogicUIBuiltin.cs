using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UI;
using UnityEngine.UIElements;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "UI", Static = true, Abstract = true, Description = "UI label functions.")]
    partial class CustomLogicUIBuiltin : BuiltinClassInstance
    {
        private static readonly Dictionary<string, string> LastSetLabels = new();

        private static readonly object[] SetLabelRpcArgs = new object[3];
        private const string SetLabelRpc = nameof(RPCManager.SetLabelRPC);

        private static InGameMenu Menu => (InGameMenu)UIManager.CurrentMenu;

        [CLConstructor]
        public CustomLogicUIBuiltin(){}

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

        public static void ClearLabels()
        {
            LastSetLabels.Clear();
        }

        [CLProperty(Description = "\"TopCenter\" constant")]
        public static string TopCenter => "TopCenter";

        [CLProperty(Description = "\"TopLeft\" constant")]
        public static string TopLeft => "TopLeft";

        [CLProperty(Description = "\"TopRight\" constant")]
        public static string TopRight => "TopRight";

        [CLProperty(Description = "\"MiddleCenter\" constant")]
        public static string MiddleCenter => "MiddleCenter";

        [CLProperty(Description = "\"MiddleLeft\" constant")]
        public static string MiddleLeft => "MiddleLeft";

        [CLProperty(Description = "\"MiddleRight\" constant")]
        public static string MiddleRight => "MiddleRight";

        [CLProperty(Description = "\"BottomCenter\" constant")]
        public static string BottomCenter => "BottomCenter";

        [CLProperty(Description = "\"BottomLeft\" constant")]
        public static string BottomLeft => "BottomLeft";

        [CLProperty(Description = "\"BottomRight\" constant")]
        public static string BottomRight => "BottomRight";

        [CLMethod(Description = "Sets the label at a certain location. Valid types: \"TopCenter\", \"TopLeft\", \"TopRight\", \"MiddleCenter\", \"MiddleLeft\", \"MiddleRight\", \"BottomLeft\", \"BottomRight\", \"BottomCenter\".")]
        public static void SetLabel(
            [CLParam("The label location.")]
            string label,
            [CLParam("The message to display.")]
            string message)
            => InGameManager.SetLabel(label, message);

        [CLMethod(Description = "Sets the label for a certain time, after which it will be cleared.")]
        public static void SetLabelForTime(
            [CLParam("The label location.")]
            string label,
            [CLParam("The message to display.")]
            string message,
            [CLParam("The time in seconds before the label is cleared.")]
            float time)
            => InGameManager.SetLabel(label, message, time);

        [CLMethod(Description = "Sets the label for all players. Master client only. Be careful not to call this often.")]
        public static void SetLabelAll(
            [CLParam("The label location.")]
            string label,
            [CLParam("The message to display.")]
            string message)
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

        [CLMethod(Description = "Sets the label for all players for a certain time. Master client only.")]
        public static void SetLabelForTimeAll(
            [CLParam("The label location.")]
            string label,
            [CLParam("The message to display.")]
            string message,
            [CLParam("The time in seconds before the label is cleared.")]
            float time)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SetLabelRpcArgs[0] = label;
                SetLabelRpcArgs[1] = message;
                SetLabelRpcArgs[2] = time;
                RPCManager.PhotonView.RPC(SetLabelRpc, RpcTarget.All, SetLabelRpcArgs);
            }
        }

        [CLMethod(Description = "Creates a new popup. This popup is hidden until shown.")]
        public static string CreatePopup(
            [CLParam("The name of the popup.")]
            string popupName,
            [CLParam("The title of the popup.")]
            string title,
            [CLParam("The width of the popup.")]
            int width,
            [CLParam("The height of the popup.")]
            int height)
        {
            Menu.CreateCustomPopup(popupName, title, width, height);
            return popupName;
        }

        [CLMethod(Description = "Shows the popup with given name.")]
        public static void ShowPopup(
            [CLParam("The name of the popup to show.")]
            string popupName)
            => Menu.GetCustomPopup(popupName).Show();

        [CLMethod(Description = "Hides the popup with given name.")]
        public static void HidePopup(
            [CLParam("The name of the popup to hide.")]
            string popupName)
            => Menu.GetCustomPopup(popupName).Hide();

        [CLMethod(Description = "Clears all elements in popup with given name.")]
        public static void ClearPopup(
            [CLParam("The name of the popup to clear.")]
            string popupName)
            => Menu.GetCustomPopup(popupName).Clear();

        [CLMethod(Description = "Adds a text row to the popup with label as content.")]
        public static void AddPopupLabel(
            [CLParam("The name of the popup.")]
            string popupName,
            [CLParam("The label text to add.")]
            string label)
            => Menu.GetCustomPopup(popupName).AddLabel(label);

        [CLMethod(Description = "Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.")]
        public static void AddPopupButton(
            [CLParam("The name of the popup.")]
            string popupName,
            [CLParam("The button display text.")]
            string label,
            [CLParam("The callback name that will be passed to OnButtonClick in Main.")]
            string callback)
            => Menu.GetCustomPopup(popupName).AddButton(label, callback);

        [CLMethod(Description = "Adds a button to the bottom bar of the popup.")]
        public static void AddPopupBottomButton(
            [CLParam("The name of the popup.")]
            string popupName,
            [CLParam("The button display text.")]
            string label,
            [CLParam("The callback name that will be passed to OnButtonClick in Main.")]
            string callback)
            => Menu.GetCustomPopup(popupName).AddBottomButton(label, callback);

        [CLMethod(Description = "Adds a list of buttons in a row to the popup.")]
        public static void AddPopupButtons(
            [CLParam("The name of the popup.")]
            string popupName,
            [CLParam("List of button display texts.", Type = "List<string>")]
            CustomLogicListBuiltin labels,
            [CLParam("List of callback names that will be passed to OnButtonClick in Main.", Type = "List<void>")]
            CustomLogicListBuiltin callbacks)
            => Menu.GetCustomPopup(popupName).AddButtons(labels.List, callbacks.List);

        [CLMethod(Description = "Returns a wrapped string given style and args.")]
        public static string WrapStyleTag(
            [CLParam("The text to wrap.")]
            string text,
            [CLParam("The style tag name.")]
            string style,
            [CLParam("Optional style argument.")]
            string arg = null)
        {
            if (arg == null)
                return $"<{style}>{text}</{style}>";
            return $"<{style}={arg}>{text}</{style}>";
        }

        [CLMethod(Description = "Shows the change character menu if main character is Human.")]
        public static void ShowChangeCharacterMenu()
        {
            var inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            if (inGameManager.CurrentCharacter != null && inGameManager.CurrentCharacter is Human)
            {
                Menu.ShowCharacterChangeMenu();
            }
        }

        [CLMethod(Description = "Sets the display of the scoreboard header (default \"Kills / Deaths...\")")]
        public static void SetScoreboardHeader(
            [CLParam("The header text to display.")]
            string header)
            => CustomLogicManager.Evaluator.ScoreboardHeader = header;

        [CLMethod(Description = "Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default \"Kills / Deaths...\" display.")]
        public static void SetScoreboardProperty(
            [CLParam("The property name to read from Player custom properties.")]
            string property)
            => CustomLogicManager.Evaluator.ScoreboardProperty = $"CL:{property}";

        [CLMethod(Description = "Gets the color of the specified item. See theme json for reference.")]
        public static CustomLogicColorBuiltin GetThemeColor(
            [CLParam("The panel name.")]
            string panel,
            [CLParam("The category name.")]
            string category,
            [CLParam("The item name.")]
            string item)
        {
            var color = new Color255(UIManager.GetThemeColor(panel, category, item));
            return new CustomLogicColorBuiltin(color);
        }

        [CLMethod(Description = "Returns if the given popup is active")]
        public static bool IsPopupActive(
            [CLParam("The name of the popup to check.")]
            string popupName)
            => Menu.GetCustomPopup(popupName).IsActive;

        [CLProperty(Description = "Returns a list of all popups", TypeArguments = new[] { "string" })]
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

        [CLMethod(Description = "Sets whether a label is active or not.")]
        public static void SetLabelActive(
            [CLParam("The label name.")]
            string label,
            [CLParam("Whether the label should be active.")]
            bool active)
        {
            Menu.SetLabelActive(label, active);
        }

        [CLMethod(Description = "Sets whether the KDR panel (top-left) is active or not.")]
        public static void SetKDRPanelActive(
            [CLParam("Whether the KDR panel should be active.")]
            bool active)
        {
            Menu.SetKDRPanelActive(active);
        }

        [CLMethod(Description = "Sets whether the minimap is active or not.")]
        public static void SetMinimapActive(
            [CLParam("Whether the minimap should be active.")]
            bool active)
        {
            Menu.SetMinimapActive(active);
        }

        [CLMethod(Description = "Sets whether the chat panel is active or not.")]
        public static void SetChatPanelActive(
            [CLParam("Whether the chat panel should be active.")]
            bool active)
        {
            Menu.SetChatPanelActive(active);
        }

        [CLMethod(Description = "Sets whether the feed panel is active or not.")]
        public static void SetFeedPanelActive(
            [CLParam("Whether the feed panel should be active.")]
            bool active)
        {
            Menu.SetFeedPanelActive(active);
        }

        [CLMethod(Description = "Sets whether the bottom HUD is active or not. This can only be used when the character is alive.")]
        public static void SetBottomHUDActive(
            [CLParam("Whether the bottom HUD should be active.")]
            bool active)
        {
            Menu.SetBottomHUDActive(active);
        }

        [CLMethod(Description = "Returns the root `VisualElement` which you can add other elements to. Returns: The root `VisualElement`")]
        public static CustomLogicVisualElementBuiltin GetRootVisualElement()
        {
            return new CustomLogicVisualElementBuiltin(Menu.RootVisualElement);
        }

        [CLMethod(Description = "Creates a new `VisualElement`.")]
        public static CustomLogicVisualElementBuiltin VisualElement()
        {
            return new CustomLogicVisualElementBuiltin(new VisualElement());
        }

        [CLMethod(Description = "Creates a new `Button` with optional text and click event.")]
        public static CustomLogicButtonBuiltin Button(
            [CLParam("The text that the button displays")]
            string text = "",
            [CLParam("The function that will be called when button is clicked")]
            UserMethod clickEvent = null)
        {
            return new CustomLogicButtonBuiltin(new Button { text = text }).OnClick(clickEvent);
        }

        [CLMethod(Description = "Creates a new `Label` with optional text.")]
        public static CustomLogicLabelBuiltin Label(
            [CLParam("The text to be displayed")]
            string text = "")
        {
            return new CustomLogicLabelBuiltin(new Label { text = text });
        }

        [CLMethod(Description = "Creates a new `TextField` with optional label.")]
        public static CustomLogicTextFieldBuiltin TextField(
            [CLParam("The label text displayed next to the TextField (default: empty).")]
            string label = "")
        {
            return new CustomLogicTextFieldBuiltin(new TextField(label));
        }

        [CLMethod(Description = "Creates a new `Toggle` with optional label and value changed event.")]
        public static CustomLogicToggleBuiltin Toggle(
            [CLParam("The label text displayed next to the toggle")]
            string label = "",
            [CLParam("The function that will be called when toggle value changes")]
            UserMethod valueChangedEvent = null)
        {
            return new CustomLogicToggleBuiltin(new Toggle(label)).OnValueChanged(valueChangedEvent);
        }

        [CLMethod(Description = "Creates a new `Slider` for floating-point values with optional range, tick interval, and value changed event. The slider will snap to values at multiples of the tick interval.")]
        public static CustomLogicSliderBuiltin Slider(
            [CLParam("The minimum value of the slider")]
            float lowValue = 0f,
            [CLParam("The maximum value of the slider")]
            float highValue = 100f,
            [CLParam("The interval between allowed values. If 0, no snapping occurs. For example, 0.1 will snap to 0.0, 0.1, 0.2, etc.")]
            float tickInterval = 0f,
            [CLParam("The label text displayed next to the slider")]
            string label = "",
            [CLParam("The function that will be called when slider value changes")]
            UserMethod valueChangedEvent = null)
        {
            var slider = new Slider(label, lowValue, highValue);
            if (tickInterval > 0f)
                slider.pageSize = tickInterval;
            return new CustomLogicSliderBuiltin(slider).OnValueChanged(valueChangedEvent);
        }

        [CLMethod(Description = "Creates a new `Slider` for integer values with optional range, tick interval, and value changed event. The slider will snap to values at multiples of the tick interval.")]
        public static CustomLogicSliderBuiltin SliderInt(
            [CLParam("The minimum value of the slider")]
            int lowValue = 0,
            [CLParam("The maximum value of the slider")]
            int highValue = 100,
            [CLParam("The interval between allowed values. For example, 5 will snap to 0, 5, 10, 15, etc.")]
            int tickInterval = 1,
            [CLParam("The label text displayed next to the slider")]
            string label = "",
            [CLParam("The function that will be called when slider value changes")]
            UserMethod valueChangedEvent = null)
        {
            var sliderInt = new SliderInt(label, lowValue, highValue);
            if (tickInterval > 1)
                sliderInt.pageSize = tickInterval;
            return new CustomLogicSliderBuiltin(sliderInt).OnValueChanged(valueChangedEvent);
        }

        [CLMethod(Description = "Creates a new `Dropdown` with a list of choices and optional label and value changed event.")]
        public static CustomLogicDropdownBuiltin Dropdown(
            [CLParam("List of string options to display in the dropdown", Type = "List<string>")]
            CustomLogicListBuiltin choices,
            [CLParam("The index of the initially selected option (default: 0)")]
            int defaultIndex = 0,
            [CLParam("The label text displayed next to the dropdown")]
            string label = "",
            [CLParam("The function that will be called when dropdown value changes")]
            UserMethod valueChangedEvent = null)
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

        [CLMethod(Description = "Creates a new `ProgressBar` with optional range, title, and value changed event.")]
        public static CustomLogicProgressBarBuiltin ProgressBar(
            [CLParam("The minimum value of the progress bar (default: 0)")]
            float lowValue = 0f,
            [CLParam("The maximum value of the progress bar (default: 100)")]
            float highValue = 100f,
            [CLParam("The title text displayed on the progress bar")]
            string title = "",
            [CLParam("The function that will be called when progress bar value changes")]
            UserMethod valueChangedEvent = null)
        {
            var progressBar = new ProgressBar
            {
                lowValue = lowValue,
                highValue = highValue,
                title = title
            };
            return new CustomLogicProgressBarBuiltin(progressBar).OnValueChanged(valueChangedEvent);
        }

        [CLMethod(Description = "Creates a new `ScrollView` for scrollable content.")]
        public static CustomLogicScrollViewBuiltin ScrollView()
        {
            return new CustomLogicScrollViewBuiltin(new ScrollView());
        }

        [CLMethod(Description = "Creates a new `Icon` element for displaying images/icons.")]
        public static CustomLogicIconBuiltin Icon(
            [CLParam("Path to the icon resource (e.g., \"Icons/Game/BladeIcon\")")]
            string iconPath = "")
        {
            var icon = new CustomLogicIconBuiltin(new Image());
            if (!string.IsNullOrEmpty(iconPath))
                icon.SetIcon(iconPath);
            return icon;
        }

        [CLMethod(Description = "Creates a new `Image` element for displaying images/icons.")]
        public static CustomLogicImageBuiltin Image(
            [CLParam("Path to the icon resource (e.g., \"Icons/Game/BladeIcon\")")]
            string iconPath = "")
        {
            var image = new CustomLogicImageBuiltin();
            if (!string.IsNullOrEmpty(iconPath))
                image.SetImage(iconPath);
            return image;
        }
    }
}
