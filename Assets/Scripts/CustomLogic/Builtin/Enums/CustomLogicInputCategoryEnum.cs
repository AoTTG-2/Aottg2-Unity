namespace CustomLogic
{
    /// <summary>
    /// Enumeration of input categories for keybind settings.
    /// </summary>
    [CLType(Name = "InputCategoryEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputCategoryEnum : BuiltinClassInstance
    {
        internal const string GeneralValue = "General";
        internal const string HumanValue = "Human";
        internal const string TitanValue = "Titan";
        internal const string InteractionValue = "Interaction";
        internal const string AnnieShifterValue = "AnnieShifter";
        internal const string ErenShifterValue = "ErenShifter";

        [CLConstructor]
        public CustomLogicInputCategoryEnum() { }

        /// <summary>
        /// General input category.
        /// </summary>
        [CLProperty]
        public static string General => GeneralValue;

        /// <summary>
        /// Human input category.
        /// </summary>
        [CLProperty]
        public static string Human => HumanValue;

        /// <summary>
        /// Titan input category.
        /// </summary>
        [CLProperty]
        public static string Titan => TitanValue;

        /// <summary>
        /// Interaction input category.
        /// </summary>
        [CLProperty]
        public static string Interaction => InteractionValue;

        /// <summary>
        /// AnnieShifter input category.
        /// </summary>
        [CLProperty]
        public static string AnnieShifter => AnnieShifterValue;

        /// <summary>
        /// ErenShifter input category.
        /// </summary>
        [CLProperty]
        public static string ErenShifter => ErenShifterValue;
    }
}
