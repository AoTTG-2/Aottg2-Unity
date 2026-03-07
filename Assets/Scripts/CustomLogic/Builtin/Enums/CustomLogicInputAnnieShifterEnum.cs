namespace CustomLogic
{
    /// <summary>
    /// Enumeration of AnnieShifter input keybind settings.
    /// </summary>
    [CLType(Name = "InputAnnieShifterEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputAnnieShifterEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputAnnieShifterEnum() { }

        private static readonly string KickValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/Kick";
        private static readonly string JumpValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/Jump";
        private static readonly string WalkValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/Walk";
        private static readonly string AttackComboValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackCombo";
        private static readonly string AttackSwingValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackSwing";
        private static readonly string AttackStompValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackStomp";
        private static readonly string AttackBiteValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackBite";
        private static readonly string AttackHeadValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackHead";
        private static readonly string AttackBrushBackValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackBrushBack";
        private static readonly string AttackBrushFrontValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackBrushFront";
        private static readonly string AttackBrushHeadValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackBrushHead";
        private static readonly string AttackGrabBottomValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackGrabBottom";
        private static readonly string AttackGrabMidValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackGrabMid";
        private static readonly string AttackGrabUpValue = CustomLogicInputCategoryEnum.AnnieShifterValue + "/AttackGrabUp";

        /// <summary>
        /// AnnieShifter/Kick keybind.
        /// </summary>
        [CLProperty]
        public static string Kick => KickValue;
        /// <summary>
        /// AnnieShifter/Jump keybind.
        /// </summary>
        [CLProperty]
        public static string Jump => JumpValue;
        /// <summary>
        /// AnnieShifter/Walk keybind.
        /// </summary>
        [CLProperty]
        public static string Walk => WalkValue;
        /// <summary>
        /// AnnieShifter/AttackCombo keybind.
        /// </summary>
        [CLProperty]
        public static string AttackCombo => AttackComboValue;
        /// <summary>
        /// AnnieShifter/AttackSwing keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSwing => AttackSwingValue;
        /// <summary>
        /// AnnieShifter/AttackStomp keybind.
        /// </summary>
        [CLProperty]
        public static string AttackStomp => AttackStompValue;
        /// <summary>
        /// AnnieShifter/AttackBite keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBite => AttackBiteValue;
        /// <summary>
        /// AnnieShifter/AttackHead keybind.
        /// </summary>
        [CLProperty]
        public static string AttackHead => AttackHeadValue;
        /// <summary>
        /// AnnieShifter/AttackBrushBack keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBrushBack => AttackBrushBackValue;
        /// <summary>
        /// AnnieShifter/AttackBrushFront keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBrushFront => AttackBrushFrontValue;
        /// <summary>
        /// AnnieShifter/AttackBrushHead keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBrushHead => AttackBrushHeadValue;
        /// <summary>
        /// AnnieShifter/AttackGrabBottom keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBottom => AttackGrabBottomValue;
        /// <summary>
        /// AnnieShifter/AttackGrabMid keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabMid => AttackGrabMidValue;
        /// <summary>
        /// AnnieShifter/AttackGrabUp keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabUp => AttackGrabUpValue;
    }
}
