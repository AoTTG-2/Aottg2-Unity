namespace CustomLogic
{
    [CLType(Name = "InputCategoryEnum", Static = true, Abstract = true, Description = "Enumeration of input categories for keybind settings.")]
    partial class CustomLogicInputCategoryEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputCategoryEnum() { }

        [CLProperty("General input category.")]
        public static string General => "General";

        [CLProperty("Human input category.")]
        public static string Human => "Human";

        [CLProperty("Titan input category.")]
        public static string Titan => "Titan";

        [CLProperty("Interaction input category.")]
        public static string Interaction => "Interaction";
    }
}
