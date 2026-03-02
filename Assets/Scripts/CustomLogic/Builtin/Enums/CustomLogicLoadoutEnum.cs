using GameManagers;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of loadout types for all character types.
    /// </summary>
    [CLType(Name = "LoadoutEnum", Static = true, Abstract = true)]
    partial class CustomLogicLoadoutEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicLoadoutEnum() { }

        #region Human Loadouts

        /// <summary>
        /// Blades loadout (human).
        /// </summary>
        [CLProperty]
        public static string HumanBlades => HumanLoadout.Blade;

        /// <summary>
        /// AHSS loadout (human).
        /// </summary>
        [CLProperty]
        public static string HumanAHSS => HumanLoadout.AHSS;

        /// <summary>
        /// APG loadout (human).
        /// </summary>
        [CLProperty]
        public static string HumanAPG => HumanLoadout.APG;

        /// <summary>
        /// Thunderspears loadout (human).
        /// </summary>
        [CLProperty]
        public static string HumanThunderspears => HumanLoadout.Thunderspear;

        #endregion

        #region Titan Loadouts

        /// <summary>
        /// Small titan size.
        /// </summary>
        [CLProperty]
        public static string TitanSmall => TitanLoadout.Small;

        /// <summary>
        /// Medium titan size.
        /// </summary>
        [CLProperty]
        public static string TitanMedium => TitanLoadout.Medium;

        /// <summary>
        /// Large titan size.
        /// </summary>
        [CLProperty]
        public static string TitanLarge => TitanLoadout.Large;

        #endregion

        #region Shifter Loadouts

        /// <summary>
        /// Annie shifter type.
        /// </summary>
        [CLProperty]
        public static string ShifterAnnie = ShifterLoadout.Annie;

        /// <summary>
        /// Eren shifter type.
        /// </summary>
        [CLProperty]
        public static string ShifterEren = ShifterLoadout.Eren;

        /// <summary>
        /// Armored shifter type.
        /// </summary>
        [CLProperty]
        public static string ShifterArmored = ShifterLoadout.Armored;

        /// <summary>
        /// WallColossal shifter type.
        /// </summary>
        [CLProperty]
        public static string ShifterWallColossal = ShifterLoadout.WallColossal;

        #endregion
    }
}
