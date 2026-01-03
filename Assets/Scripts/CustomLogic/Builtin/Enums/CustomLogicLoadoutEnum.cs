namespace CustomLogic
{
    [CLType(Name = "LoadoutEnum", Static = true, Abstract = true, Description = "Enumeration of loadout types for humans.")]
    partial class CustomLogicLoadoutEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLoadoutEnum() { }

        [CLProperty("Blades loadout.")]
        public static string Blades => "Blades";

        [CLProperty("AHSS loadout.")]
        public static string AHSS => "AHSS";

        [CLProperty("APG loadout.")]
        public static string APG => "APG";

        [CLProperty("Thunderspears loadout.")]
        public static string Thunderspears => "Thunderspears";
    }
}
