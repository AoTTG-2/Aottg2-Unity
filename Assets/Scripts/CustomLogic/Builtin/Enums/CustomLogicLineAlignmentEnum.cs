using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of line alignment values for LineRenderer.
    /// </summary>
    [CLType(Name = "LineAlignmentEnum", Static = true, Abstract = true)]
    partial class CustomLogicLineAlignmentEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLineAlignmentEnum() { }

        /// <summary>
        /// View: line is aligned to the camera view.
        /// </summary>
        [CLProperty]
        public static int View => (int)LineAlignment.View;

        /// <summary>
        /// Local: line is aligned to the local transform.
        /// </summary>
        [CLProperty]
        public static int Local => (int)LineAlignment.TransformZ;

        /// <summary>
        /// TransformZ: line is aligned to the transform's Z axis.
        /// </summary>
        [CLProperty]
        public static int TransformZ => (int)LineAlignment.TransformZ;
    }
}
