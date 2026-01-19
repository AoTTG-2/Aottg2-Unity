using UnityEngine.Rendering;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of shadow casting mode values for LineRenderer.
    /// </summary>
    [CLType(Name = "ShadowCastingModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicShadowCastingModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicShadowCastingModeEnum() { }

        /// <summary>
        /// Off: shadows are disabled.
        /// </summary>
        [CLProperty]
        public static int Off => (int)ShadowCastingMode.Off;

        /// <summary>
        /// On: shadows are enabled.
        /// </summary>
        [CLProperty]
        public static int On => (int)ShadowCastingMode.On;

        /// <summary>
        /// TwoSided: two-sided shadows are enabled.
        /// </summary>
        [CLProperty]
        public static int TwoSided => (int)ShadowCastingMode.TwoSided;

        /// <summary>
        /// ShadowsOnly: only shadows are rendered, not the object itself.
        /// </summary>
        [CLProperty]
        public static int ShadowsOnly => (int)ShadowCastingMode.ShadowsOnly;
    }
}
