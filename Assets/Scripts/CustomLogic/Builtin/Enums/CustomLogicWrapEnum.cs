using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of wrap values for UI elements.
    /// </summary>
    [CLType(Name = "WrapEnum", Static = true, Abstract = true)]
    partial class CustomLogicWrapEnum : BuiltinClassInstance
    {

        [CLConstructor]
        public CustomLogicWrapEnum() { }

        /// <summary>
        /// NoWrap: All items will be on one line.
        /// </summary>
        [CLProperty]
        public static int NoWrap => (int)UnityEngine.UIElements.Wrap.NoWrap;

        /// <summary>
        /// Wrap: Items will wrap onto multiple lines from top to bottom.
        /// </summary>
        [CLProperty]
        public static int Wrap => (int)UnityEngine.UIElements.Wrap.Wrap;

        /// <summary>
        /// WrapReverse: Items will wrap onto multiple lines from bottom to top.
        /// </summary>
        [CLProperty]
        public static int WrapReverse => (int)UnityEngine.UIElements.Wrap.WrapReverse;

    }
}
