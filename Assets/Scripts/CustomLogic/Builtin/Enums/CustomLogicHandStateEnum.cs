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
        public static string Healthy => "Healthy";

        /// <summary>
        /// Hand is broken.
        /// </summary>
        [CLProperty]
        public static string Broken => "Broken";
    }
}
