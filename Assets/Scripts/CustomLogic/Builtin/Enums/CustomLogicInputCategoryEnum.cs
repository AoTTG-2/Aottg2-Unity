namespace CustomLogic
{
    /// <summary>
    /// Enumeration of input categories for keybind settings.
    /// </summary>
    [CLType(Name = "InputCategoryEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputCategoryEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputCategoryEnum() { }

        /// <summary>
        /// General input category.
        /// </summary>
        [CLProperty]
        public static string General => "General";

        /// <summary>
        /// Human input category.
        /// </summary>
        [CLProperty]
        public static string Human => "Human";

        /// <summary>
        /// Titan input category.
        /// </summary>
        [CLProperty]
        public static string Titan => "Titan";

        /// <summary>
        /// Interaction input category.
        /// </summary>
        [CLProperty]
        public static string Interaction => "Interaction";
    }
}
