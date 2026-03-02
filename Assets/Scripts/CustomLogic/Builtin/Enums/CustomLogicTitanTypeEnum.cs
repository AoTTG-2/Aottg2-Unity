using GameManagers;

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
        public static string Normal => TitanType.Normal;

        /// <summary>
        /// Abnormal titan type.
        /// </summary>
        [CLProperty]
        public static string Abnormal => TitanType.Abnormal;

        /// <summary>
        /// Jumper titan type.
        /// </summary>
        [CLProperty]
        public static string Jumper => TitanType.Jumper;

        /// <summary>
        /// Crawler titan type.
        /// </summary>
        [CLProperty]
        public static string Crawler => TitanType.Crawler;

        /// <summary>
        /// Thrower titan type.
        /// </summary>
        [CLProperty]
        public static string Thrower => TitanType.Thrower;

        /// <summary>
        /// Punk titan type.
        /// </summary>
        [CLProperty]
        public static string Punk => TitanType.Punk;

        /// <summary>
        /// Default titan type (uses spawn rate settings).
        /// </summary>
        [CLProperty]
        public static string Default => TitanType.Default;

        /// <summary>
        /// Random titan type (randomly selects from available types).
        /// </summary>
        [CLProperty]
        public static string Random => TitanType.Random;
    }
}
