using Effects;

namespace CustomLogic
{
    // Refer to Assets/Scripts/Effects/EffectPrefabs.cs for the complete list of effects.
    [CLType(Name = "EffectNameEnum", Static = true, Abstract = true, Description = "Enumeration of available effect names that can be spawned. Uses left-hand variable names from EffectPrefabs class.")]
    partial class CustomLogicEffectNameEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicEffectNameEnum() { }

        [CLProperty("ThunderspearExplode effect.")]
        public static string ThunderspearExplode => EffectPrefabs.ThunderspearExplode;

        [CLProperty("GasBurst effect.")]
        public static string GasBurst => EffectPrefabs.GasBurst;

        [CLProperty("GroundShatter effect.")]
        public static string GroundShatter => EffectPrefabs.GroundShatter;

        [CLProperty("Blood1 effect.")]
        public static string Blood1 => EffectPrefabs.Blood1;

        [CLProperty("Blood2 effect.")]
        public static string Blood2 => EffectPrefabs.Blood2;

        [CLProperty("PunchHit effect.")]
        public static string PunchHit => EffectPrefabs.PunchHit;

        [CLProperty("GunExplode effect.")]
        public static string GunExplode => EffectPrefabs.GunExplode;

        [CLProperty("CriticalHit effect.")]
        public static string CriticalHit => EffectPrefabs.CriticalHit;

        [CLProperty("TitanSpawn effect.")]
        public static string TitanSpawn => EffectPrefabs.TitanSpawn;

        [CLProperty("TitanDie1 effect.")]
        public static string TitanDie1 => EffectPrefabs.TitanDie1;

        [CLProperty("TitanDie2 effect.")]
        public static string TitanDie2 => EffectPrefabs.TitanDie2;

        [CLProperty("Boom1 effect.")]
        public static string Boom1 => EffectPrefabs.Boom1;

        [CLProperty("Boom2 effect.")]
        public static string Boom2 => EffectPrefabs.Boom2;

        [CLProperty("Boom3 effect.")]
        public static string Boom3 => EffectPrefabs.Boom3;

        [CLProperty("Boom4 effect.")]
        public static string Boom4 => EffectPrefabs.Boom4;

        [CLProperty("Boom5 effect.")]
        public static string Boom5 => EffectPrefabs.Boom5;

        [CLProperty("Boom6 effect.")]
        public static string Boom6 => EffectPrefabs.Boom6;

        [CLProperty("Boom7 effect.")]
        public static string Boom7 => EffectPrefabs.Boom7;

        [CLProperty("Boom8 effect.")]
        public static string Boom8 => EffectPrefabs.Boom8;

        [CLProperty("Splash effect.")]
        public static string Splash => EffectPrefabs.Splash;

        [CLProperty("TitanBite effect.")]
        public static string TitanBite => EffectPrefabs.TitanBite;

        [CLProperty("ShifterThunder effect.")]
        public static string ShifterThunder => EffectPrefabs.ShifterThunder;

        [CLProperty("BladeThrowHit effect.")]
        public static string BladeThrowHit => EffectPrefabs.BladeThrowHit;

        [CLProperty("APGTrail effect.")]
        public static string APGTrail => EffectPrefabs.APGTrail;

        [CLProperty("SingleSplash effect.")]
        public static string SingleSplash => EffectPrefabs.SingleSplash;

        [CLProperty("Splash1 effect.")]
        public static string Splash1 => EffectPrefabs.Splash1;

        [CLProperty("Splash2 effect.")]
        public static string Splash2 => EffectPrefabs.Splash2;

        [CLProperty("Splash3 effect.")]
        public static string Splash3 => EffectPrefabs.Splash3;

        [CLProperty("WaterWake effect.")]
        public static string WaterWake => EffectPrefabs.WaterWake;

        [CLProperty("ColossalSpawn effect.")]
        public static string ColossalSpawn => EffectPrefabs.ColossalSpawn;

        [CLProperty("ColossalRockSpawn effect.")]
        public static string ColossalRockSpawn => EffectPrefabs.ColossalRockSpawn;

        [CLProperty("ColossalKick effect.")]
        public static string ColossalKick => EffectPrefabs.ColossalKick;
    }
}
