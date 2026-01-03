namespace CustomLogic
{
    [CLType(Name = "ForceModeEnum", Static = true, Abstract = true, Description = "Enumeration of force modes for applying forces to rigidbodies and characters.")]
    partial class CustomLogicForceModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicForceModeEnum() { }

        [CLProperty("Force mode: applies force continuously over time.")]
        public static string Force => "Force";

        [CLProperty("Acceleration mode: applies force as acceleration, ignoring mass.")]
        public static string Acceleration => "Acceleration";

        [CLProperty("Impulse mode: applies force instantly as an impulse.")]
        public static string Impulse => "Impulse";

        [CLProperty("VelocityChange mode: applies force as a direct velocity change, ignoring mass.")]
        public static string VelocityChange => "VelocityChange";
    }
}
