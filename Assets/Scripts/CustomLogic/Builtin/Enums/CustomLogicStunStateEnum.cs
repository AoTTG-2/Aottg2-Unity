namespace CustomLogic
{
    /// <summary>
    /// Enumeration of stun states for WallColossal shifter.
    /// </summary>
    [CLType(Name = "StunStateEnum", Static = true, Abstract = true)]
    partial class CustomLogicStunStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicStunStateEnum() { }

        /// <summary>
        /// Not stunned.
        /// </summary>
        [CLProperty]
        public static string None => "None";

        /// <summary>
        /// Currently stunned.
        /// </summary>
        [CLProperty]
        public static string Stunned => "Stunned";

        /// <summary>
        /// Recovering from stun.
        /// </summary>
        [CLProperty]
        public static string Recovering => "Recovering";
    }
}
