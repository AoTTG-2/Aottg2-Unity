namespace CustomLogic
{
    /// <summary>
    /// Enumeration of ErenShifter input keybind settings.
    /// </summary>
    [CLType(Name = "InputErenShifterEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputErenShifterEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputErenShifterEnum() { }

        private static readonly string KickValue = CustomLogicInputCategoryEnum.ErenShifterValue + "/Kick";
        private static readonly string JumpValue = CustomLogicInputCategoryEnum.ErenShifterValue + "/Jump";
        private static readonly string WalkValue = CustomLogicInputCategoryEnum.ErenShifterValue + "/Walk";
        private static readonly string AttackComboValue = CustomLogicInputCategoryEnum.ErenShifterValue + "/AttackCombo";

        /// <summary>
        /// ErenShifter/Kick keybind.
        /// </summary>
        [CLProperty]
        public static string Kick => KickValue;
        /// <summary>
        /// ErenShifter/Jump keybind.
        /// </summary>
        [CLProperty]
        public static string Jump => JumpValue;
        /// <summary>
        /// ErenShifter/Walk keybind.
        /// </summary>
        [CLProperty]
        public static string Walk => WalkValue;
        /// <summary>
        /// ErenShifter/AttackCombo keybind.
        /// </summary>
        [CLProperty]
        public static string AttackCombo => AttackComboValue;
    }
}
