namespace CustomLogic
{
    /// <summary>
    /// Enumeration of collision detection modes for rigidbodies.
    /// </summary>
    [CLType(Name = "CollisionDetectionModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicCollisionDetectionModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollisionDetectionModeEnum() { }

        /// <summary>
        /// Discrete collision detection mode.
        /// </summary>
        [CLProperty]
        public static string Discrete => "Discrete";

        /// <summary>
        /// Continuous collision detection mode.
        /// </summary>
        [CLProperty]
        public static string Continuous => "Continuous";

        /// <summary>
        /// ContinuousDynamic collision detection mode.
        /// </summary>
        [CLProperty]
        public static string ContinuousDynamic => "ContinuousDynamic";

        /// <summary>
        /// ContinuousSpeculative collision detection mode.
        /// </summary>
        [CLProperty]
        public static string ContinuousSpeculative => "ContinuousSpeculative";
    }
}
