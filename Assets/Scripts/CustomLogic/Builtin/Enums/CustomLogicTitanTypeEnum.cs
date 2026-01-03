namespace CustomLogic
{
    [CLType(Name = "TitanTypeEnum", Static = true, Abstract = true, Description = "Enumeration of titan types that can be spawned.")]
    partial class CustomLogicTitanTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTitanTypeEnum() { }

        [CLProperty("Normal titan type.")]
        public static string Normal => "Normal";

        [CLProperty("Abnormal titan type.")]
        public static string Abnormal => "Abnormal";

        [CLProperty("Jumper titan type.")]
        public static string Jumper => "Jumper";

        [CLProperty("Crawler titan type.")]
        public static string Crawler => "Crawler";

        [CLProperty("Thrower titan type.")]
        public static string Thrower => "Thrower";

        [CLProperty("Punk titan type.")]
        public static string Punk => "Punk";

        [CLProperty("Default titan type (uses spawn rate settings).")]
        public static string Default => "Default";

        [CLProperty("Random titan type (randomly selects from available types).")]
        public static string Random => "Random";
    }
}
