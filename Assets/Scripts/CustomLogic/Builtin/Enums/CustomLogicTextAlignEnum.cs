using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of text alignment values for UI elements.
    /// </summary>
    [CLType(Name = "TextAlignEnum", Static = true, Abstract = true)]
    partial class CustomLogicTextAlignEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTextAlignEnum() { }

        /// <summary>
        /// UpperLeft: text is aligned to the upper left.
        /// </summary>
        [CLProperty]
        public static int UpperLeft => (int)TextAnchor.UpperLeft;

        /// <summary>
        /// UpperCenter: text is aligned to the upper center.
        /// </summary>
        [CLProperty]
        public static int UpperCenter => (int)TextAnchor.UpperCenter;

        /// <summary>
        /// UpperRight: text is aligned to the upper right.
        /// </summary>
        [CLProperty]
        public static int UpperRight => (int)TextAnchor.UpperRight;

        /// <summary>
        /// MiddleLeft: text is aligned to the middle left.
        /// </summary>
        [CLProperty]
        public static int MiddleLeft => (int)TextAnchor.MiddleLeft;

        /// <summary>
        /// MiddleCenter: text is aligned to the middle center.
        /// </summary>
        [CLProperty]
        public static int MiddleCenter => (int)TextAnchor.MiddleCenter;

        /// <summary>
        /// MiddleRight: text is aligned to the middle right.
        /// </summary>
        [CLProperty]
        public static int MiddleRight => (int)TextAnchor.MiddleRight;

        /// <summary>
        /// LowerLeft: text is aligned to the lower left.
        /// </summary>
        [CLProperty]
        public static int LowerLeft => (int)TextAnchor.LowerLeft;

        /// <summary>
        /// LowerCenter: text is aligned to the lower center.
        /// </summary>
        [CLProperty]
        public static int LowerCenter => (int)TextAnchor.LowerCenter;

        /// <summary>
        /// LowerRight: text is aligned to the lower right.
        /// </summary>
        [CLProperty]
        public static int LowerRight => (int)TextAnchor.LowerRight;
    }
}
