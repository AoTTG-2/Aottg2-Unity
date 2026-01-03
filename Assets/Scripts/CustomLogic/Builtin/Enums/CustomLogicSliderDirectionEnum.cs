namespace CustomLogic
{
    [CLType(Name = "SliderDirectionEnum", Static = true, Abstract = true, Description = "Enumeration of slider directions for UI sliders.")]
    partial class CustomLogicSliderDirectionEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicSliderDirectionEnum() { }

        [CLProperty("Horizontal slider direction.")]
        public static string Horizontal => "Horizontal";

        [CLProperty("Vertical slider direction.")]
        public static string Vertical => "Vertical";
    }
}
