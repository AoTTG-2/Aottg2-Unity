using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of flex direction values for UI elements.
    /// </summary>
    [CLType(Name = "FlexDirectionEnum", Static = true, Abstract = true)]
    partial class CustomLogicFlexDirectionEnum : BuiltinClassInstance
    {

        [CLConstructor]
        public CustomLogicFlexDirectionEnum() { }

        /// <summary>
        /// Row: items are laid out horizontally.
        /// </summary>
        [CLProperty]
        public static int Row => (int)FlexDirection.Row;

        /// <summary>
        /// Column: items are laid out vertically.
        /// </summary>
        [CLProperty]
        public static int Column => (int)FlexDirection.Column;

        /// <summary>
        /// RowReverse: items are laid out horizontally in reverse order.
        /// </summary>
        [CLProperty]
        public static int RowReverse => (int)FlexDirection.RowReverse;

        /// <summary>
        /// ColumnReverse: items are laid out vertically in reverse order.
        /// </summary>
        [CLProperty]
        public static int ColumnReverse => (int)FlexDirection.ColumnReverse;
    }
}
