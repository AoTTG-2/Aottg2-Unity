using GameManagers;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available weapon types for humans.
    /// </summary>
    [CLType(Name = "WeaponEnum", Static = true, Abstract = true)]
    partial class CustomLogicWeaponEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicWeaponEnum() { }

        /// <summary>
        /// Blade weapon type.
        /// </summary>
        [CLProperty]
        public static string Blade => HumanLoadout.Blade;

        /// <summary>
        /// AHSS weapon type.
        /// </summary>
        [CLProperty]
        public static string AHSS => HumanLoadout.AHSS;

        /// <summary>
        /// APG weapon type.
        /// </summary>
        [CLProperty]
        public static string APG => HumanLoadout.APG;

        /// <summary>
        /// Thunderspear weapon type.
        /// </summary>
        [CLProperty]
        public static string Thunderspear => HumanLoadout.Thunderspear;
    }
}
