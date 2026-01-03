namespace CustomLogic
{
    [CLType(Name = "HandStateEnum", Static = true, Abstract = true, Description = "Enumeration of hand states for WallColossal shifter.")]
    partial class CustomLogicHandStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHandStateEnum() { }

        [CLProperty("Hand is healthy.")]
        public static string Healthy => "Healthy";

        [CLProperty("Hand is broken.")]
        public static string Broken => "Broken";
    }
}
