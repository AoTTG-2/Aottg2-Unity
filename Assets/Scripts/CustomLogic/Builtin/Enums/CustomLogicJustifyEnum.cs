using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of justify content values for UI elements.
    /// </summary>
    [CLType(Name = "JustifyEnum", Static = true, Abstract = true)]
    partial class CustomLogicJustifyEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicJustifyEnum() { }

        /// <summary>
        /// FlexStart: aligns items to the start of the main axis.
        /// </summary>
        [CLProperty]
        public static int FlexStart => (int)Justify.FlexStart;

        /// <summary>
        /// Center: aligns items to the center of the main axis.
        /// </summary>
        [CLProperty]
        public static int Center => (int)Justify.Center;

        /// <summary>
        /// FlexEnd: aligns items to the end of the main axis.
        /// </summary>
        [CLProperty]
        public static int FlexEnd => (int)Justify.FlexEnd;

        /// <summary>
        /// SpaceBetween: distributes items evenly with space between them.
        /// </summary>
        [CLProperty]
        public static int SpaceBetween => (int)Justify.SpaceBetween;

        /// <summary>
        /// SpaceAround: distributes items evenly with space around them.
        /// </summary>
        [CLProperty]
        public static int SpaceAround => (int)Justify.SpaceAround;

        /// <summary>
        /// SpaceEvenly: distributes items evenly with equal space around them.
        /// </summary>
        [CLProperty]
        public static int SpaceEvenly => (int)Justify.SpaceEvenly;
    }
}
