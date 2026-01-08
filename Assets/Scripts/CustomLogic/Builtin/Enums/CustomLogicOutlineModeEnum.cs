namespace CustomLogic
{
    /// <summary>
    /// Enumeration of outline rendering modes for characters.
    /// </summary>
    [CLType(Name = "OutlineModeEnum", Static = true, Abstract = true)]
    partial class CustomLogicOutlineModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicOutlineModeEnum() { }

        /// <summary>
        /// OutlineAll: outlines all parts of the object.
        /// </summary>
        [CLProperty]
        public static string OutlineAll => "OutlineAll";

        /// <summary>
        /// OutlineVisible: outlines only visible parts.
        /// </summary>
        [CLProperty]
        public static string OutlineVisible => "OutlineVisible";

        /// <summary>
        /// OutlineHidden: outlines only hidden parts.
        /// </summary>
        [CLProperty]
        public static string OutlineHidden => "OutlineHidden";

        /// <summary>
        /// OutlineAndSilhouette: combines outline and silhouette.
        /// </summary>
        [CLProperty]
        public static string OutlineAndSilhouette => "OutlineAndSilhouette";

        /// <summary>
        /// SilhouetteOnly: shows only the silhouette.
        /// </summary>
        [CLProperty]
        public static string SilhouetteOnly => "SilhouetteOnly";

        /// <summary>
        /// OutlineAndLightenColor: combines outline with lightened color.
        /// </summary>
        [CLProperty]
        public static string OutlineAndLightenColor => "OutlineAndLightenColor";
    }
}
