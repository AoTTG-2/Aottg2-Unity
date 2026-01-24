using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of overflow values for UI elements.
    /// </summary>
    [CLType(Name = "OverflowEnum", Static = true, Abstract = true)]
    partial class CustomLogicOverflowEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicOverflowEnum() { }

        /// <summary>
        /// Visible: content is visible even if it overflows the element's boundary.
        /// </summary>
        [CLProperty]
        public static int Visible => (int)Overflow.Visible;

        /// <summary>
        /// Hidden: content is clipped at the element's boundary.
        /// </summary>
        [CLProperty]
        public static int Hidden => (int)Overflow.Hidden;
    }
}
