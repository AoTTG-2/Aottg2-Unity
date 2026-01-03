namespace CustomLogic
{
    [CLType(Name = "CollisionDetectionModeEnum", Static = true, Abstract = true, Description = "Enumeration of collision detection modes for rigidbodies.")]
    partial class CustomLogicCollisionDetectionModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollisionDetectionModeEnum() { }

        [CLProperty("Discrete collision detection mode.")]
        public static string Discrete => "Discrete";

        [CLProperty("Continuous collision detection mode.")]
        public static string Continuous => "Continuous";

        [CLProperty("ContinuousDynamic collision detection mode.")]
        public static string ContinuousDynamic => "ContinuousDynamic";

        [CLProperty("ContinuousSpeculative collision detection mode.")]
        public static string ContinuousSpeculative => "ContinuousSpeculative";
    }
}
