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
        public static int Discrete => (int)UnityEngine.CollisionDetectionMode.Discrete;

        /// <summary>
        /// Continuous collision detection mode.
        /// </summary>
        [CLProperty]
        public static int Continuous => (int)UnityEngine.CollisionDetectionMode.Continuous;

        /// <summary>
        /// ContinuousDynamic collision detection mode.
        /// </summary>
        [CLProperty]
        public static int ContinuousDynamic => (int)UnityEngine.CollisionDetectionMode.ContinuousDynamic;

        /// <summary>
        /// ContinuousSpeculative collision detection mode.
        /// </summary>
        [CLProperty]
        public static int ContinuousSpeculative => (int)UnityEngine.CollisionDetectionMode.ContinuousSpeculative;
    }
}
