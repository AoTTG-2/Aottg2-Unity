using GameManagers;

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
        public static string None => TeamInfo.None;

        /// <summary>
        /// Blue team.
        /// </summary>
        [CLProperty]
        public static string Blue => TeamInfo.Blue;

        /// <summary>
        /// Red team.
        /// </summary>
        [CLProperty]
        public static string Red => TeamInfo.Red;

        /// <summary>
        /// Titan team.
        /// </summary>
        [CLProperty]
        public static string Titan => TeamInfo.Titan;

        /// <summary>
        /// Human team.
        /// </summary>
        [CLProperty]
        public static string Human => TeamInfo.Human;
    }
}
