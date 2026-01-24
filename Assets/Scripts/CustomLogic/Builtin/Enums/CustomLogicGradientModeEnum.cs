using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of gradient mode values for LineRenderer.
    /// </summary>
    [CLType(Name = "GradientModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicGradientModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicGradientModeEnum() { }

        /// <summary>
        /// Blend: gradient colors are blended.
        /// </summary>
        [CLProperty]
        public static int Blend => (int)GradientMode.Blend;

        /// <summary>
        /// Fixed: gradient colors are fixed.
        /// </summary>
        [CLProperty]
        public static int Fixed => (int)GradientMode.Fixed;
    }
}
