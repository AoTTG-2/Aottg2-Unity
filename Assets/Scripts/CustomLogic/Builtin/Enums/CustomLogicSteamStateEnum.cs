namespace CustomLogic
{
    [CLType(Name = "SteamStateEnum", Static = true, Abstract = true, Description = "Enumeration of steam states for WallColossal shifter.")]
    partial class CustomLogicSteamStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicSteamStateEnum() { }

        [CLProperty("Steam is off.")]
        public static string Off => "Off";

        [CLProperty("Steam warning state.")]
        public static string Warning => "Warning";

        [CLProperty("Steam damage state.")]
        public static string Damage => "Damage";
    }
}
