using Characters;

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
        public static int None => (int)ColossalStunState.None;

        /// <summary>
        /// Currently stunned.
        /// </summary>
        [CLProperty]
        public static int Stunned => (int)ColossalStunState.Stunned;

        /// <summary>
        /// Recovering from stun.
        /// </summary>
        [CLProperty]
        public static int Recovering => (int)ColossalStunState.Recovering;
    }
}
