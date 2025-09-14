using ApplicationManagers;
using Characters;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using Settings;
using System.Collections.Generic;
using UI;
using Utility;
using UnityEngine;
// using UnityEngine.UI;
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

        [CLMethod]
        public static void SetLabelActive(string label, bool active)
        {
            Menu.SetLabelActive(label, active);
        }

        [CLMethod]
        public static void SetKDRPanelActive(bool active)
        {
            Menu.SetKDRPanelActive(active);
        }

        [CLMethod]
        public static void SetMinimapActive(bool active)
        {
            Menu.SetMinimapActive(active);
        }

        [CLMethod]
        public static void SetChatPanelActive(bool active)
        {
            Menu.SetChatPanelActive(active);
        }

        [CLMethod]
        public static void SetFeedPanelActive(bool active)
        {
            Menu.SetFeedPanelActive(active);
        }

        [CLMethod]
        public static void SetBottomHUDActive(bool active)
        {
            Menu.SetBottomHUDActive(active);
        }

        [CLMethod]
        public static CustomLogicVisualElementBuiltin GetRootVisualElement()
        {
            return new CustomLogicVisualElementBuiltin(Menu.RootVisualElement);
        }

        [CLMethod]
        public static CustomLogicVisualElementBuiltin VisualElement()
        {
            return new CustomLogicVisualElementBuiltin(new VisualElement());
        }

        [CLMethod]
        public static CustomLogicButtonBuiltin Button(string text, UserMethod clickEvent = null)
        {
            return new CustomLogicButtonBuiltin(new Button { text = text }).SetClickEvent(clickEvent);
        }

        [CLMethod]
        public static CustomLogicLabelBuiltin Label(string text)
        {
            return new CustomLogicLabelBuiltin(new Label { text = text });
        }
    }

    [CLType(Name = "VisualElement", Abstract = true)]
    partial class CustomLogicVisualElementBuiltin : BuiltinClassInstance
    {
        private readonly VisualElement _visualElement;

        public CustomLogicVisualElementBuiltin(VisualElement visualElement)
        {
            _visualElement = visualElement;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Opacity(float value)
        {
            _visualElement.style.opacity = Mathf.Clamp01(value / 100f);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Active(bool value)
        {
            _visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Visible(bool value)
        {
            _visualElement.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Absolute(bool value)
        {
            _visualElement.style.position = value ? Position.Absolute : Position.Relative;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Left(float value, bool percentage = false)
        {
            _visualElement.style.left = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Top(float value, bool percentage = false)
        {
            _visualElement.style.top = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Right(float value, bool percentage = false)
        {
            _visualElement.style.right = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Bottom(float value, bool percentage = false)
        {
            _visualElement.style.bottom = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexShrink(float value)
        {
            _visualElement.style.flexShrink = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexGrow(float value)
        {
            _visualElement.style.flexGrow = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexDirection(string value)
        {
            _visualElement.style.flexDirection = value switch
            {
                "Row" => UnityEngine.UIElements.FlexDirection.Row,
                "Column" => UnityEngine.UIElements.FlexDirection.Column,
                "RowReverse" => UnityEngine.UIElements.FlexDirection.RowReverse,
                "ColumnReverse" => UnityEngine.UIElements.FlexDirection.ColumnReverse,
                _ => throw new System.Exception("Unkown flex direction value")
            };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin AlignItems(string value)
        {
            _visualElement.style.alignItems = value switch
            {
                "Auto" => Align.Auto,
                "FlexStart" => Align.FlexStart,
                "Center" => Align.Center,
                "FlexEnd" => Align.FlexEnd,
                "Stretch" => Align.Stretch,
                _ => throw new System.Exception("Unknown align value")
            };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin JustifyContent(string value)
        {
            _visualElement.style.justifyContent = value switch
            {
                "FlexStart" => Justify.FlexStart,
                "Center" => Justify.Center,
                "FlexEnd" => Justify.FlexEnd,
                "SpaceBetween" => Justify.SpaceBetween,
                "SpaceAround" => Justify.SpaceAround,
                "SpaceEvenly" => Justify.SpaceEvenly,
                _ => throw new System.Exception("Unknown justify value")
            };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin AlignSelf(string value)
        {
            _visualElement.style.alignSelf = value switch
            {
                "Auto" => Align.Auto,
                "FlexStart" => Align.FlexStart,
                "Center" => Align.Center,
                "FlexEnd" => Align.FlexEnd,
                "Stretch" => Align.Stretch,
                _ => throw new System.Exception("Unknown align value")
            };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Width(float value, bool percentage = false)
        {
            _visualElement.style.width = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Height(float value, bool percentage = false)
        {
            _visualElement.style.height = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Margin(float value, bool percentage = false)
        {
            return MarginLeft(value, percentage)
                .MarginTop(value, percentage)
                .MarginRight(value, percentage)
                .MarginBottom(value, percentage);
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginLeft(float value, bool percentage = false)
        {
            _visualElement.style.marginLeft = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginTop(float value, bool percentage = false)
        {
            _visualElement.style.marginTop = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginRight(float value, bool percentage = false)
        {
            _visualElement.style.marginRight = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginBottom(float value, bool percentage = false)
        {
            _visualElement.style.marginBottom = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Padding(float value, bool percentage = false)
        {
            return PaddingLeft(value, percentage)
                .PaddingTop(value, percentage)
                .PaddingRight(value, percentage)
                .PaddingBottom(value, percentage);
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingLeft(float value, bool percentage = false)
        {
            _visualElement.style.paddingLeft = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingTop(float value, bool percentage = false)
        {
            _visualElement.style.paddingTop = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingRight(float value, bool percentage = false)
        {
            _visualElement.style.paddingRight = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingBottom(float value, bool percentage = false)
        {
            _visualElement.style.paddingBottom = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin FontStyle(string value)
        {
            _visualElement.style.unityFontStyleAndWeight = value switch
            {
                "Normal" => UnityEngine.FontStyle.Normal,
                "Bold" => UnityEngine.FontStyle.Bold,
                "Italic" => UnityEngine.FontStyle.Italic,
                "BoldAndItalic" => UnityEngine.FontStyle.BoldAndItalic,
                _ => throw new System.Exception("Unknown font style value")
            };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin FontSize(float value, bool percentage = false)
        {
            _visualElement.style.fontSize = GetLength(value, percentage);
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Color(CustomLogicColorBuiltin color)
        {
            _visualElement.style.color = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BackgroundColor(CustomLogicColorBuiltin color)
        {
            _visualElement.style.backgroundColor = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColor(CustomLogicColorBuiltin color)
        {
            return BorderColorLeft(color)
                .BorderColorTop(color)
                .BorderColorRight(color)
                .BorderColorBottom(color);
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorLeft(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderLeftColor = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorTop(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderTopColor = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorRight(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderRightColor = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorBottom(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderBottomColor = color.Value.ToColor();
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidth(float value)
        {
            return BorderWidthLeft(value)
                .BorderWidthTop(value)
                .BorderWidthRight(value)
                .BorderWidthBottom(value);
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthLeft(float value)
        {
            _visualElement.style.borderLeftWidth = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthTop(float value)
        {
            _visualElement.style.borderTopWidth = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthRight(float value)
        {
            _visualElement.style.borderRightWidth = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthBottom(float value)
        {
            _visualElement.style.borderBottomWidth = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadius(float value)
        {
            return BorderRadiusTopLeft(value)
                .BorderRadiusTopRight(value)
                .BorderRadiusBottomLeft(value)
                .BorderRadiusBottomRight(value);
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusTopLeft(float value)
        {
            _visualElement.style.borderTopLeftRadius = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusTopRight(float value)
        {
            _visualElement.style.borderTopRightRadius = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomLeft(float value)
        {
            _visualElement.style.borderBottomLeftRadius = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomRight(float value)
        {
            _visualElement.style.borderBottomRightRadius = value;
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin TransitionDuration(float value, bool seconds = false)
        {
            _visualElement.style.transitionDuration = new List<TimeValue> { new(value, seconds ? TimeUnit.Second : TimeUnit.Millisecond) };
            return this;
        }

        [CLMethod]
        public CustomLogicVisualElementBuiltin Add(CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Add(visualElement._visualElement);
            return this;
        }

        [CLMethod]
        public void Remove(CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Remove(visualElement._visualElement);
        }

        [CLMethod]
        public void RemoveFromHierarchy()
        {
            _visualElement.RemoveFromHierarchy();
        }

        private static Length GetLength(float value, bool percentage)
        {
            return new Length(value, percentage ? LengthUnit.Percent : LengthUnit.Pixel);
        }
    }

    [CLType(Name = "Button", Abstract = true)]
    partial class CustomLogicButtonBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Button _button;

        private UserMethod _clickEvent;

        public CustomLogicButtonBuiltin(Button button) : base(button)
        {
            _button = button;
            _button.clickable.clicked += OnClick;
        }

        private void OnClick()
        {
            if (_clickEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_clickEvent);
        }

        [CLProperty]
        public string Text
        {
            get => _button.text;
            set => _button.text = value;
        }

        [CLMethod]
        public CustomLogicButtonBuiltin SetClickEvent(UserMethod clickEvent)
        {
            _clickEvent = clickEvent;
            return this;
        }
    }

    [CLType(Name = "Label", Abstract = true)]
    partial class CustomLogicLabelBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Label _label;

        public CustomLogicLabelBuiltin(Label label) : base(label)
        {
            _label = label;
            _label.enableRichText = true;
        }

        [CLProperty]
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }
    }
}
