namespace CustomLogic
{
    [CLType(Name = "ScaleModeEnum", Static = true, Abstract = true, Description = "Enumeration of scale modes for UI icons.")]
    partial class CustomLogicScaleModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicScaleModeEnum() { }

        [CLProperty("ScaleAndCrop scale mode.")]
        public static string ScaleAndCrop => "ScaleAndCrop";

        [CLProperty("ScaleToFit scale mode.")]
        public static string ScaleToFit => "ScaleToFit";

        [CLProperty("StretchToFill scale mode.")]
        public static string StretchToFill => "StretchToFill";
    }
}
