namespace CustomLogic
{
    /// <summary>
    /// Enumeration of scale modes for UI icons.
    /// </summary>
    [CLType(Name = "ScaleModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicScaleModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicScaleModeEnum() { }

        /// <summary>
        /// ScaleAndCrop scale mode.
        /// </summary>
        [CLProperty]
        public static string ScaleAndCrop => "ScaleAndCrop";

        /// <summary>
        /// ScaleToFit scale mode.
        /// </summary>
        [CLProperty]
        public static string ScaleToFit => "ScaleToFit";

        /// <summary>
        /// StretchToFill scale mode.
        /// </summary>
        [CLProperty]
        public static string StretchToFill => "StretchToFill";
    }
}
