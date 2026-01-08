namespace CustomLogic
{
    /// <summary>
    /// Enumeration of physics material combine modes for friction and bounce.
    /// </summary>
    [CLType(Name = "PhysicMaterialCombineEnum", Static = true, Abstract = true)]
    partial class CustomLogicPhysicMaterialCombineEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPhysicMaterialCombineEnum() { }

        /// <summary>
        /// Minimum combine mode.
        /// </summary>
        [CLProperty]
        public static int Minimum => 0;

        /// <summary>
        /// Multiply combine mode.
        /// </summary>
        [CLProperty]
        public static int Multiply => 1;

        /// <summary>
        /// Maximum combine mode.
        /// </summary>
        [CLProperty]
        public static int Maximum => 2;

        /// <summary>
        /// Average combine mode.
        /// </summary>
        [CLProperty]
        public static int Average => 3;
    }
}
