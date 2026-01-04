using Effects;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available effect names that can be spawned. Uses left-hand variable names from EffectPrefabs class.
    /// </summary>
    [CLType(Name = "EffectNameEnum", Static = true, Abstract = true)]
    partial class CustomLogicEffectNameEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicEffectNameEnum() { }

        /// <summary>
        /// ThunderspearExplode effect.
        /// </summary>
        [CLProperty]
        public static string ThunderspearExplode => EffectPrefabs.ThunderspearExplode;

        /// <summary>
        /// GasBurst effect.
        /// </summary>
        [CLProperty]
        public static string GasBurst => EffectPrefabs.GasBurst;

        /// <summary>
        /// GroundShatter effect.
        /// </summary>
        [CLProperty]
        public static string GroundShatter => EffectPrefabs.GroundShatter;

        /// <summary>
        /// Blood1 effect.
        /// </summary>
        [CLProperty]
        public static string Blood1 => EffectPrefabs.Blood1;

        /// <summary>
        /// Blood2 effect.
        /// </summary>
        [CLProperty]
        public static string Blood2 => EffectPrefabs.Blood2;

        /// <summary>
        /// PunchHit effect.
        /// </summary>
        [CLProperty]
        public static string PunchHit => EffectPrefabs.PunchHit;

        /// <summary>
        /// GunExplode effect.
        /// </summary>
        [CLProperty]
        public static string GunExplode => EffectPrefabs.GunExplode;

        /// <summary>
        /// CriticalHit effect.
        /// </summary>
        [CLProperty]
        public static string CriticalHit => EffectPrefabs.CriticalHit;

        /// <summary>
        /// TitanSpawn effect.
        /// </summary>
        [CLProperty]
        public static string TitanSpawn => EffectPrefabs.TitanSpawn;

        /// <summary>
        /// TitanDie1 effect.
        /// </summary>
        [CLProperty]
        public static string TitanDie1 => EffectPrefabs.TitanDie1;

        /// <summary>
        /// TitanDie2 effect.
        /// </summary>
        [CLProperty]
        public static string TitanDie2 => EffectPrefabs.TitanDie2;

        /// <summary>
        /// Boom1 effect.
        /// </summary>
        [CLProperty]
        public static string Boom1 => EffectPrefabs.Boom1;

        /// <summary>
        /// Boom2 effect.
        /// </summary>
        [CLProperty]
        public static string Boom2 => EffectPrefabs.Boom2;

        /// <summary>
        /// Boom3 effect.
        /// </summary>
        [CLProperty]
        public static string Boom3 => EffectPrefabs.Boom3;

        /// <summary>
        /// Boom4 effect.
        /// </summary>
        [CLProperty]
        public static string Boom4 => EffectPrefabs.Boom4;

        /// <summary>
        /// Boom5 effect.
        /// </summary>
        [CLProperty]
        public static string Boom5 => EffectPrefabs.Boom5;

        /// <summary>
        /// Boom6 effect.
        /// </summary>
        [CLProperty]
        public static string Boom6 => EffectPrefabs.Boom6;

        /// <summary>
        /// Boom7 effect.
        /// </summary>
        [CLProperty]
        public static string Boom7 => EffectPrefabs.Boom7;

        /// <summary>
        /// Boom8 effect.
        /// </summary>
        [CLProperty]
        public static string Boom8 => EffectPrefabs.Boom8;

        /// <summary>
        /// Splash effect.
        /// </summary>
        [CLProperty]
        public static string Splash => EffectPrefabs.Splash;

        /// <summary>
        /// TitanBite effect.
        /// </summary>
        [CLProperty]
        public static string TitanBite => EffectPrefabs.TitanBite;

        /// <summary>
        /// ShifterThunder effect.
        /// </summary>
        [CLProperty]
        public static string ShifterThunder => EffectPrefabs.ShifterThunder;

        /// <summary>
        /// BladeThrowHit effect.
        /// </summary>
        [CLProperty]
        public static string BladeThrowHit => EffectPrefabs.BladeThrowHit;

        /// <summary>
        /// APGTrail effect.
        /// </summary>
        [CLProperty]
        public static string APGTrail => EffectPrefabs.APGTrail;

        /// <summary>
        /// SingleSplash effect.
        /// </summary>
        [CLProperty]
        public static string SingleSplash => EffectPrefabs.SingleSplash;

        /// <summary>
        /// Splash1 effect.
        /// </summary>
        [CLProperty]
        public static string Splash1 => EffectPrefabs.Splash1;

        /// <summary>
        /// Splash2 effect.
        /// </summary>
        [CLProperty]
        public static string Splash2 => EffectPrefabs.Splash2;

        /// <summary>
        /// Splash3 effect.
        /// </summary>
        [CLProperty]
        public static string Splash3 => EffectPrefabs.Splash3;

        /// <summary>
        /// WaterWake effect.
        /// </summary>
        [CLProperty]
        public static string WaterWake => EffectPrefabs.WaterWake;

        /// <summary>
        /// ColossalSpawn effect.
        /// </summary>
        [CLProperty]
        public static string ColossalSpawn => EffectPrefabs.ColossalSpawn;

        /// <summary>
        /// ColossalRockSpawn effect.
        /// </summary>
        [CLProperty]
        public static string ColossalRockSpawn => EffectPrefabs.ColossalRockSpawn;

        /// <summary>
        /// ColossalKick effect.
        /// </summary>
        [CLProperty]
        public static string ColossalKick => EffectPrefabs.ColossalKick;
    }
}
