using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of font scale mode for UI elements.
    /// </summary>
    [CLType(Name = "FontScaleModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicFontScaleModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicFontScaleModeEnum() { }

        /// <summary>
        /// Height: font will scale with the container's height.
        /// </summary>
        [CLProperty]
        public static int Height => (int)FontScaleMode.Height;

        /// <summary>
        /// Width: font will scale with the container's width.
        /// </summary>
        [CLProperty]
        public static int Width => (int)FontScaleMode.Width;
    }

    enum FontScaleMode
    {
        Height,
        Width
    }
}
