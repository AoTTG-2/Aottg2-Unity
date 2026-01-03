namespace CustomLogic
{
    [CLType(Name = "WeaponEnum", Static = true, Abstract = true, Description = "Enumeration of available weapon types for humans.")]
    partial class CustomLogicWeaponEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicWeaponEnum() { }

        [CLProperty("Blade weapon type.")]
        public static string Blade => "Blade";

        [CLProperty("AHSS weapon type.")]
        public static string AHSS => "AHSS";

        [CLProperty("APG weapon type.")]
        public static string APG => "APG";

        [CLProperty("Thunderspear weapon type.")]
        public static string Thunderspear => "Thunderspear";
    }
}
