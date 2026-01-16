namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for dummy characters.
    /// </summary>
    [CLType(Name = "DummyAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicDummyAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicDummyAnimationEnum() { }

        /// <summary>
        /// Dummy animation.
        /// </summary>
        [CLProperty]
        public static string Idle => "Armature|dummy_idle";

        /// <summary>
        /// Fall animation.
        /// </summary>
        [CLProperty]
        public static string Fall => "Armature|dummy_fall";

        /// <summary>
        /// Rise animation.
        /// </summary>
        [CLProperty]
        public static string Rise => "Armature|dummy_rise";
    }
}
