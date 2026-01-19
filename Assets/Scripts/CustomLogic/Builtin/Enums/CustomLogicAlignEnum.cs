using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of align values for UI elements (used for align-items and align-self properties).
    /// </summary>
    [CLType(Name = "AlignEnum", Static = true, Abstract = true)]
    partial class CustomLogicAlignEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicAlignEnum() { }

        /// <summary>
        /// Auto: uses the parent's align-items value.
        /// </summary>
        [CLProperty]
        public static int Auto => (int)Align.Auto;

        /// <summary>
        /// FlexStart: aligns items to the start of the cross axis.
        /// </summary>
        [CLProperty]
        public static int FlexStart => (int)Align.FlexStart;

        /// <summary>
        /// Center: aligns items to the center of the cross axis.
        /// </summary>
        [CLProperty]
        public static int Center => (int)Align.Center;

        /// <summary>
        /// FlexEnd: aligns items to the end of the cross axis.
        /// </summary>
        [CLProperty]
        public static int FlexEnd => (int)Align.FlexEnd;

        /// <summary>
        /// Stretch: stretches items to fill the cross axis.
        /// </summary>
        [CLProperty]
        public static int Stretch => (int)Align.Stretch;
    }
}
