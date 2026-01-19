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

        private static readonly string _outlineAll = Outline.Mode.OutlineAll.ToString();
        private static readonly string _outlineVisible = Outline.Mode.OutlineVisible.ToString();
        private static readonly string _outlineHidden = Outline.Mode.OutlineHidden.ToString();
        private static readonly string _outlineAndSilhouette = Outline.Mode.OutlineAndSilhouette.ToString();
        private static readonly string _silhouetteOnly = Outline.Mode.SilhouetteOnly.ToString();
        private static readonly string _outlineAndLightenColor = Outline.Mode.OutlineAndLightenColor.ToString();

        /// <summary>
        /// OutlineAll: outlines all parts of the object.
        /// </summary>
        [CLProperty]
        public static string OutlineAll => _outlineAll;

        /// <summary>
        /// OutlineVisible: outlines only visible parts.
        /// </summary>
        [CLProperty]
        public static string OutlineVisible => _outlineVisible;

        /// <summary>
        /// OutlineHidden: outlines only hidden parts.
        /// </summary>
        [CLProperty]
        public static string OutlineHidden => _outlineHidden;

        /// <summary>
        /// OutlineAndSilhouette: combines outline and silhouette.
        /// </summary>
        [CLProperty]
        public static string OutlineAndSilhouette => _outlineAndSilhouette;

        /// <summary>
        /// SilhouetteOnly: shows only the silhouette.
        /// </summary>
        [CLProperty]
        public static string SilhouetteOnly => _silhouetteOnly;

        /// <summary>
        /// OutlineAndLightenColor: combines outline with lightened color.
        /// </summary>
        [CLProperty]
        public static string OutlineAndLightenColor => _outlineAndLightenColor;
    }
}
