using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of font style values for UI elements.
    /// </summary>
    [CLType(Name = "FontStyleEnum", Static = true, Abstract = true)]
    partial class CustomLogicFontStyleEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicFontStyleEnum() { }

        /// <summary>
        /// Normal font style.
        /// </summary>
        [CLProperty]
        public static int Normal => (int)FontStyle.Normal;

        /// <summary>
        /// Bold font style.
        /// </summary>
        [CLProperty]
        public static int Bold => (int)FontStyle.Bold;

        /// <summary>
        /// Italic font style.
        /// </summary>
        [CLProperty]
        public static int Italic => (int)FontStyle.Italic;

        /// <summary>
        /// Bold and italic font style.
        /// </summary>
        [CLProperty]
        public static int BoldAndItalic => (int)FontStyle.BoldAndItalic;
    }
}
