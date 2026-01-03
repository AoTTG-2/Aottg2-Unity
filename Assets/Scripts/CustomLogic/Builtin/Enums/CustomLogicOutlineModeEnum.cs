namespace CustomLogic
{
    [CLType(Name = "OutlineModeEnum", Static = true, Abstract = true, Description = "Enumeration of outline rendering modes for characters.")]
    partial class CustomLogicOutlineModeEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicOutlineModeEnum() { }

        [CLProperty("OutlineAll: outlines all parts of the object.")]
        public static string OutlineAll => "OutlineAll";

        [CLProperty("OutlineVisible: outlines only visible parts.")]
        public static string OutlineVisible => "OutlineVisible";

        [CLProperty("OutlineHidden: outlines only hidden parts.")]
        public static string OutlineHidden => "OutlineHidden";

        [CLProperty("OutlineAndSilhouette: combines outline and silhouette.")]
        public static string OutlineAndSilhouette => "OutlineAndSilhouette";

        [CLProperty("SilhouetteOnly: shows only the silhouette.")]
        public static string SilhouetteOnly => "SilhouetteOnly";

        [CLProperty("OutlineAndLightenColor: combines outline with lightened color.")]
        public static string OutlineAndLightenColor => "OutlineAndLightenColor";
    }
}
