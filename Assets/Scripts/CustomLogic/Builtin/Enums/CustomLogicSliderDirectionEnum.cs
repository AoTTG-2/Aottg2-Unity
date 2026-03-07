using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of slider directions for UI sliders.
    /// </summary>
    [CLType(Name = "SliderDirectionEnum", Static = true, Abstract = true)]
    partial class CustomLogicSliderDirectionEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicSliderDirectionEnum() { }

        /// <summary>
        /// Horizontal slider direction.
        /// </summary>
        [CLProperty]
        public static int Horizontal => (int)SliderDirection.Horizontal;

        /// <summary>
        /// Vertical slider direction.
        /// </summary>
        [CLProperty]
        public static int Vertical => (int)SliderDirection.Vertical;
    }
}
