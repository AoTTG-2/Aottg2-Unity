namespace CustomLogic
{
    /// <summary>
    /// Enumeration of titan types that can be spawned.
    /// </summary>
    [CLType(Name = "TitanTypeEnum", Static = true, Abstract = true)]
    partial class CustomLogicTitanTypeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTitanTypeEnum() { }

        /// <summary>
        /// Normal titan type.
        /// </summary>
        [CLProperty]
        public static string Normal => "Normal";

        /// <summary>
        /// Abnormal titan type.
        /// </summary>
        [CLProperty]
        public static string Abnormal => "Abnormal";

        /// <summary>
        /// Jumper titan type.
        /// </summary>
        [CLProperty]
        public static string Jumper => "Jumper";

        /// <summary>
        /// Crawler titan type.
        /// </summary>
        [CLProperty]
        public static string Crawler => "Crawler";

        /// <summary>
        /// Thrower titan type.
        /// </summary>
        [CLProperty]
        public static string Thrower => "Thrower";

        /// <summary>
        /// Punk titan type.
        /// </summary>
        [CLProperty]
        public static string Punk => "Punk";

        /// <summary>
        /// Default titan type (uses spawn rate settings).
        /// </summary>
        [CLProperty]
        public static string Default => "Default";

        /// <summary>
        /// Random titan type (randomly selects from available types).
        /// </summary>
        [CLProperty]
        public static string Random => "Random";
    }
}
