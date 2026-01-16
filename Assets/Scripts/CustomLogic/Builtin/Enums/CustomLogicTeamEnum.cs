namespace CustomLogic
{
    /// <summary>
    /// Enumeration of team types for characters and players.
    /// </summary>
    [CLType(Name = "TeamEnum", Static = true, Abstract = true)]
    partial class CustomLogicTeamEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTeamEnum() { }

        /// <summary>
        /// No team assigned.
        /// </summary>
        [CLProperty]
        public static string None => "None";

        /// <summary>
        /// Blue team.
        /// </summary>
        [CLProperty]
        public static string Blue => "Blue";

        /// <summary>
        /// Red team.
        /// </summary>
        [CLProperty]
        public static string Red => "Red";

        /// <summary>
        /// Titan team.
        /// </summary>
        [CLProperty]
        public static string Titan => "Titan";

        /// <summary>
        /// Human team.
        /// </summary>
        [CLProperty]
        public static string Human => "Human";
    }
}
