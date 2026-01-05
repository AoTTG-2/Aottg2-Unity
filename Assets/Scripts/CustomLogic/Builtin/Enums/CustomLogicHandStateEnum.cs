using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of hand states for WallColossal shifter.
    /// </summary>
    [CLType(Name = "HandStateEnum", Static = true, Abstract = true)]
    partial class CustomLogicHandStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHandStateEnum() { }

        /// <summary>
        /// Hand is healthy.
        /// </summary>
        [CLProperty]
        public static int Healthy => (int)ColossalHandState.Healthy;

        /// <summary>
        /// Hand is broken.
        /// </summary>
        [CLProperty]
        public static int Broken => (int)ColossalHandState.Broken;
    }
}
