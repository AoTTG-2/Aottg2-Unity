namespace CustomLogic
{
    /// <summary>
    /// Enumeration of General input keybind settings.
    /// </summary>
    [CLType(Name = "InputGeneralEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputGeneralEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputGeneralEnum() { }

        private static readonly string ForwardValue = CustomLogicInputCategoryEnum.GeneralValue + "/Forward";
        private static readonly string BackValue = CustomLogicInputCategoryEnum.GeneralValue + "/Back";
        private static readonly string LeftValue = CustomLogicInputCategoryEnum.GeneralValue + "/Left";
        private static readonly string RightValue = CustomLogicInputCategoryEnum.GeneralValue + "/Right";
        private static readonly string UpValue = CustomLogicInputCategoryEnum.GeneralValue + "/Up";
        private static readonly string DownValue = CustomLogicInputCategoryEnum.GeneralValue + "/Down";
        private static readonly string ModifierValue = CustomLogicInputCategoryEnum.GeneralValue + "/Modifier";
        private static readonly string AutorunValue = CustomLogicInputCategoryEnum.GeneralValue + "/Autorun";
        private static readonly string PauseValue = CustomLogicInputCategoryEnum.GeneralValue + "/Pause";
        private static readonly string ChangeCharacterValue = CustomLogicInputCategoryEnum.GeneralValue + "/ChangeCharacter";
        private static readonly string RestartGameValue = CustomLogicInputCategoryEnum.GeneralValue + "/RestartGame";
        private static readonly string ToggleScoreboardValue = CustomLogicInputCategoryEnum.GeneralValue + "/ToggleScoreboard";
        private static readonly string ToggleMapValue = CustomLogicInputCategoryEnum.GeneralValue + "/ToggleMap";
        private static readonly string ChatValue = CustomLogicInputCategoryEnum.GeneralValue + "/Chat";
        private static readonly string PushToTalkValue = CustomLogicInputCategoryEnum.GeneralValue + "/PushToTalk";
        private static readonly string ChangeCameraValue = CustomLogicInputCategoryEnum.GeneralValue + "/ChangeCamera";
        private static readonly string HideCursorValue = CustomLogicInputCategoryEnum.GeneralValue + "/HideCursor";
        private static readonly string SpectatePreviousPlayerValue = CustomLogicInputCategoryEnum.GeneralValue + "/SpectatePreviousPlayer";
        private static readonly string SpectateNextPlayerValue = CustomLogicInputCategoryEnum.GeneralValue + "/SpectateNextPlayer";
        private static readonly string SkipCutsceneValue = CustomLogicInputCategoryEnum.GeneralValue + "/SkipCutscene";
        private static readonly string HideUIValue = CustomLogicInputCategoryEnum.GeneralValue + "/HideUI";
        private static readonly string DebugWindowValue = CustomLogicInputCategoryEnum.GeneralValue + "/DebugWindow";

        /// <summary>
        /// General/Forward keybind.
        /// </summary>
        [CLProperty]
        public static string Forward => ForwardValue;
        /// <summary>
        /// General/Back keybind.
        /// </summary>
        [CLProperty]
        public static string Back => BackValue;
        /// <summary>
        /// General/Left keybind.
        /// </summary>
        [CLProperty]
        public static string Left => LeftValue;
        /// <summary>
        /// General/Right keybind.
        /// </summary>
        [CLProperty]
        public static string Right => RightValue;
        /// <summary>
        /// General/Up keybind.
        /// </summary>
        [CLProperty]
        public static string Up => UpValue;
        /// <summary>
        /// General/Down keybind.
        /// </summary>
        [CLProperty]
        public static string Down => DownValue;
        /// <summary>
        /// General/Modifier keybind.
        /// </summary>
        [CLProperty]
        public static string Modifier => ModifierValue;
        /// <summary>
        /// General/Autorun keybind.
        /// </summary>
        [CLProperty]
        public static string Autorun => AutorunValue;
        /// <summary>
        /// General/Pause keybind.
        /// </summary>
        [CLProperty]
        public static string Pause => PauseValue;
        /// <summary>
        /// General/ChangeCharacter keybind.
        /// </summary>
        [CLProperty]
        public static string ChangeCharacter => ChangeCharacterValue;
        /// <summary>
        /// General/RestartGame keybind.
        /// </summary>
        [CLProperty]
        public static string RestartGame => RestartGameValue;
        /// <summary>
        /// General/ToggleScoreboard keybind.
        /// </summary>
        [CLProperty]
        public static string ToggleScoreboard => ToggleScoreboardValue;
        /// <summary>
        /// General/ToggleMap keybind.
        /// </summary>
        [CLProperty]
        public static string ToggleMap => ToggleMapValue;
        /// <summary>
        /// General/Chat keybind.
        /// </summary>
        [CLProperty]
        public static string Chat => ChatValue;
        /// <summary>
        /// General/PushToTalk keybind.
        /// </summary>
        [CLProperty]
        public static string PushToTalk => PushToTalkValue;
        /// <summary>
        /// General/ChangeCamera keybind.
        /// </summary>
        [CLProperty]
        public static string ChangeCamera => ChangeCameraValue;
        /// <summary>
        /// General/HideCursor keybind.
        /// </summary>
        [CLProperty]
        public static string HideCursor => HideCursorValue;
        /// <summary>
        /// General/SpectatePreviousPlayer keybind.
        /// </summary>
        [CLProperty]
        public static string SpectatePreviousPlayer => SpectatePreviousPlayerValue;
        /// <summary>
        /// General/SpectateNextPlayer keybind.
        /// </summary>
        [CLProperty]
        public static string SpectateNextPlayer => SpectateNextPlayerValue;
        /// <summary>
        /// General/SkipCutscene keybind.
        /// </summary>
        [CLProperty]
        public static string SkipCutscene => SkipCutsceneValue;
        /// <summary>
        /// General/HideUI keybind.
        /// </summary>
        [CLProperty]
        public static string HideUI => HideUIValue;
        /// <summary>
        /// General/DebugWindow keybind.
        /// </summary>
        [CLProperty]
        public static string DebugWindow => DebugWindowValue;
    }
}
