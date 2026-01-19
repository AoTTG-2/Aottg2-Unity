using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of UI label alignment values.
    /// </summary>
    [CLType(Name = "UILabelEnum", Static = true, Abstract = true)]
    partial class CustomLogicUILabelEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicUILabelEnum() { }

        /// <summary>
        /// TopCenter: label is aligned to the top center.
        /// </summary>
        [CLProperty]
        public static string TopCenter => "TopCenter";

        /// <summary>
        /// TopLeft: label is aligned to the top left.
        /// </summary>
        [CLProperty]
        public static string TopLeft => "TopLeft";

        /// <summary>
        /// TopRight: label is aligned to the top right.
        /// </summary>
        [CLProperty]
        public static string TopRight => "TopRight";

        /// <summary>
        /// MiddleCenter: label is aligned to the middle center.
        /// </summary>
        [CLProperty]
        public static string MiddleCenter => "MiddleCenter";

        /// <summary>
        /// MiddleLeft: label is aligned to the middle left.
        /// </summary>
        [CLProperty]
        public static string MiddleLeft => "MiddleLeft";

        /// <summary>
        /// MiddleRight: label is aligned to the middle right.
        /// </summary>
        [CLProperty]
        public static string MiddleRight => "MiddleRight";

        /// <summary>
        /// BottomCenter: label is aligned to the bottom center.
        /// </summary>
        [CLProperty]
        public static string BottomCenter => "BottomCenter";

        /// <summary>
        /// BottomLeft: label is aligned to the bottom left.
        /// </summary>
        [CLProperty]
        public static string BottomLeft => "BottomLeft";

        /// <summary>
        /// BottomRight: label is aligned to the bottom right.
        /// </summary>
        [CLProperty]
        public static string BottomRight => "BottomRight";
    }
}
