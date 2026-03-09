using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of aspect ratio for UI elements.
    /// </summary>
    [CLType(Name = "AspectRatioEnum", Static = true, Abstract = true)]
    partial class CustomLogicAspectRatioEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicAspectRatioEnum() { }

        /// <summary>
        /// Height: Scales the container's height to match the width.
        /// </summary>
        [CLProperty]
        public static int Height => (int)ElementAspectRatio.Height;

        /// <summary>
        /// Width: Scales the container's width to match the height.
        /// </summary>
        [CLProperty]
        public static int Width => (int)ElementAspectRatio.Width;
    }

    enum ElementAspectRatio
    {
        Width,
        Height
    }
}
