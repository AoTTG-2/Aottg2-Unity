using Projectiles;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of Thunderspear kill sound types for effect spawning.
    /// </summary>
    [CLType(Name = "TSKillSoundEnum", Static = true, Abstract = true)]
    partial class CustomLogicTSKillSoundEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTSKillSoundEnum() { }

        private static readonly string _kill = TSKillType.Kill.ToString();
        private static readonly string _air = TSKillType.Air.ToString();
        private static readonly string _ground = TSKillType.Ground.ToString();
        private static readonly string _armorHit = TSKillType.ArmorHit.ToString();
        private static readonly string _closeShot = TSKillType.CloseShot.ToString();
        private static readonly string _maxRangeShot = TSKillType.MaxRangeShot.ToString();

        /// <summary>
        /// Kill sound type.
        /// </summary>
        [CLProperty]
        public static string Kill => _kill;

        /// <summary>
        /// Air sound type.
        /// </summary>
        [CLProperty]
        public static string Air => _air;

        /// <summary>
        /// Ground sound type.
        /// </summary>
        [CLProperty]
        public static string Ground => _ground;

        /// <summary>
        /// ArmorHit sound type.
        /// </summary>
        [CLProperty]
        public static string ArmorHit => _armorHit;

        /// <summary>
        /// CloseShot sound type.
        /// </summary>
        [CLProperty]
        public static string CloseShot => _closeShot;

        /// <summary>
        /// MaxRangeShot sound type.
        /// </summary>
        [CLProperty]
        public static string MaxRangeShot => _maxRangeShot;
    }
}
