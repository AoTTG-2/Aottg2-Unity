namespace CustomLogic
{
    /// <summary>
    /// Enumeration of player status types.
    /// </summary>
    [CLType(Name = "PlayerStatusEnum", Static = true, Abstract = true)]
    partial class CustomLogicPlayerStatusEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPlayerStatusEnum() { }

        /// <summary>
        /// Player is alive.
        /// </summary>
        [CLProperty]
        public static string Alive => "Alive";

        /// <summary>
        /// Player is dead.
        /// </summary>
        [CLProperty]
        public static string Dead => "Dead";

        /// <summary>
        /// Player is spectating.
        /// </summary>
        [CLProperty]
        public static string Spectating => "Spectating";
    }
}
