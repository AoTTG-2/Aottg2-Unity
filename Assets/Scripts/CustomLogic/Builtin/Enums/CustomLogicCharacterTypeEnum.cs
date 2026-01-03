namespace CustomLogic
{
    [CLType(Name = "CharacterTypeEnum", Static = true, Abstract = true, Description = "Enumeration of character types.")]
    partial class CustomLogicCharacterTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCharacterTypeEnum() { }

        [CLProperty("Human character type.")]
        public static string Human => "Human";

        [CLProperty("Titan character type.")]
        public static string Titan => "Titan";

        [CLProperty("Shifter character type.")]
        public static string Shifter => "Shifter";
    }
}
