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
        /// StretchToFill scale mode.
        /// </summary>
        [CLProperty]
        public static int StretchToFill => (int)UnityEngine.ScaleMode.StretchToFill;

        /// <summary>
        /// ScaleAndCrop scale mode.
        /// </summary>
        [CLProperty]
        public static int ScaleAndCrop => (int)UnityEngine.ScaleMode.ScaleAndCrop;

        /// <summary>
        /// ScaleToFit scale mode.
        /// </summary>
        [CLProperty]
        public static int ScaleToFit => (int)UnityEngine.ScaleMode.ScaleToFit;
    }
}
