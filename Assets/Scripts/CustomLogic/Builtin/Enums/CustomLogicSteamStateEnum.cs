using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of steam states for WallColossal shifter.
    /// </summary>
    [CLType(Name = "SteamStateEnum", Static = true, Abstract = true)]
    partial class CustomLogicSteamStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicSteamStateEnum() { }

        /// <summary>
        /// Steam is off.
        /// </summary>
        [CLProperty]
        public static int Off => (int)ColossalSteamState.Off;

        /// <summary>
        /// Steam warning state.
        /// </summary>
        [CLProperty]
        public static int Warning => (int)ColossalSteamState.Warning;

        /// <summary>
        /// Steam damage state.
        /// </summary>
        [CLProperty]
        public static int Damage => (int)ColossalSteamState.Damage;
    }
}
