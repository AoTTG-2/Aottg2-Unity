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
    }
}
