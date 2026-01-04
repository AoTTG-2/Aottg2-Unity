namespace CustomLogic
{
    /// <summary>
    /// Enumeration of character types.
    /// </summary>
    [CLType(Name = "CharacterTypeEnum", Static = true, Abstract = true)]
    partial class CustomLogicCharacterTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCharacterTypeEnum() { }

        /// <summary>
        /// Human character type.
        /// </summary>
        [CLProperty]
        public static string Human => "Human";

        /// <summary>
        /// Titan character type.
        /// </summary>
        [CLProperty]
        public static string Titan => "Titan";

        /// <summary>
        /// Shifter character type.
        /// </summary>
        [CLProperty]
        public static string Shifter => "Shifter";
    }
}
