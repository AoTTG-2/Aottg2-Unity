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
        /// Hand is damaged but not severed.
        /// </summary>
        [CLProperty]
        public static string Damaged => "Damaged";

        /// <summary>
        /// Hand is severed.
        /// </summary>
        [CLProperty]
        public static string Severed => "Severed";

        /// <summary>
        /// Hand is recovering.
        /// </summary>
        [CLProperty]
        public static string Recovering => "Recovering";

        /// <summary>
        /// Hand is broken (deprecated, use Severed instead).
        /// </summary>
        [CLProperty]
        public static string Broken => "Severed";
    }
}
