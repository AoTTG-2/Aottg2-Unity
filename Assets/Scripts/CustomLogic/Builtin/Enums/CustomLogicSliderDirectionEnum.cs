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
        public static string Horizontal => "Horizontal";

        /// <summary>
        /// Vertical slider direction.
        /// </summary>
        [CLProperty]
        public static string Vertical => "Vertical";
    }
}
