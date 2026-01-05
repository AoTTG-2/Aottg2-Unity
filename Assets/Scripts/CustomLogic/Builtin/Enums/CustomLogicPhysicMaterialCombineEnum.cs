using UnityEngine;

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
        public static int Minimum => (int)PhysicMaterialCombine.Minimum;

        /// <summary>
        /// Multiply combine mode.
        /// </summary>
        [CLProperty]
        public static int Multiply => (int)PhysicMaterialCombine.Multiply;

        /// <summary>
        /// Maximum combine mode.
        /// </summary>
        [CLProperty]
        public static int Maximum => (int)PhysicMaterialCombine.Maximum;

        /// <summary>
        /// Average combine mode.
        /// </summary>
        [CLProperty]
        public static int Average => (int)PhysicMaterialCombine.Average;
    }
}
