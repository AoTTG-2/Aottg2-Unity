namespace CustomLogic
{
    /// <summary>
    /// Enumeration of loadout types for humans.
    /// </summary>
    [CLType(Name = "LoadoutEnum", Static = true, Abstract = true)]
    partial class CustomLogicLoadoutEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLoadoutEnum() { }

        /// <summary>
        /// Blades loadout.
        /// </summary>
        [CLProperty]
        public static string Blades => "Blades";

        /// <summary>
        /// AHSS loadout.
        /// </summary>
        [CLProperty]
        public static string AHSS => "AHSS";

        /// <summary>
        /// APG loadout.
        /// </summary>
        [CLProperty]
        public static string APG => "APG";

        /// <summary>
        /// Thunderspears loadout.
        /// </summary>
        [CLProperty]
        public static string Thunderspears => "Thunderspears";
    }
}
