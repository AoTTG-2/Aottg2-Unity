using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of text overflow values for UI elements.
    /// </summary>
    [CLType(Name = "TextOverflowEnum", Static = true, Abstract = true)]
    partial class CustomLogicTextOverflowEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTextOverflowEnum() { }

        /// <summary>
        /// Clip: text is clipped at the element's boundary.
        /// </summary>
        [CLProperty]
        public static int Clip => (int)TextOverflow.Clip;

        /// <summary>
        /// Ellipsis: text is truncated with an ellipsis.
        /// </summary>
        [CLProperty]
        public static int Ellipsis => (int)TextOverflow.Ellipsis;
    }
}
