namespace CustomLogic
{
    /// <summary>
    /// Enumeration of force modes for applying forces to rigidbodies and characters.
    /// </summary>
    [CLType(Name = "ForceModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicForceModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicForceModeEnum() { }

        /// <summary>
        /// Force mode: applies force continuously over time.
        /// </summary>
        [CLProperty]
        public static string Force => "Force";

        /// <summary>
        /// Acceleration mode: applies force as acceleration, ignoring mass.
        /// </summary>
        [CLProperty]
        public static string Acceleration => "Acceleration";

        /// <summary>
        /// Impulse mode: applies force instantly as an impulse.
        /// </summary>
        [CLProperty]
        public static string Impulse => "Impulse";

        /// <summary>
        /// VelocityChange mode: applies force as a direct velocity change, ignoring mass.
        /// </summary>
        [CLProperty]
        public static string VelocityChange => "VelocityChange";
    }
}
