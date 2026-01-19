using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of human sounds.
    /// </summary>
    [CLType(Name = "HumanSoundEnum", Static = true, Abstract = true)]
    partial class CustomLogicHumanSoundEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHumanSoundEnum() { }

        /// <summary>
        /// BladeBreak sound.
        /// </summary>
        [CLProperty]
        public static string BladeBreak => HumanSounds.BladeBreak;
        /// <summary>
        /// BladeHit sound.
        /// </summary>
        [CLProperty]
        public static string BladeHit => HumanSounds.BladeHit;
        /// <summary>
        /// OldBladeHit sound.
        /// </summary>
        [CLProperty]
        public static string OldBladeHit => HumanSounds.OldBladeHit;
        /// <summary>
        /// NapeHit sound.
        /// </summary>
        [CLProperty]
        public static string NapeHit => HumanSounds.NapeHit;
        /// <summary>
        /// LimbHit sound.
        /// </summary>
        [CLProperty]
        public static string LimbHit => HumanSounds.LimbHit;
        /// <summary>
        /// OldNapeHit sound.
        /// </summary>
        [CLProperty]
        public static string OldNapeHit => HumanSounds.OldNapeHit;
        /// <summary>
        /// BladeReloadAir sound.
        /// </summary>
        [CLProperty]
        public static string BladeReloadAir => HumanSounds.BladeReloadAir;
        /// <summary>
        /// BladeReloadGround sound.
        /// </summary>
        [CLProperty]
        public static string BladeReloadGround => HumanSounds.BladeReloadGround;
        /// <summary>
        /// GunReload sound.
        /// </summary>
        [CLProperty]
        public static string GunReload => HumanSounds.GunReload;
        /// <summary>
        /// BladeSwing1 sound.
        /// </summary>
        [CLProperty]
        public static string BladeSwing1 => HumanSounds.BladeSwing1;
        /// <summary>
        /// BladeSwing2 sound.
        /// </summary>
        [CLProperty]
        public static string BladeSwing2 => HumanSounds.BladeSwing2;
        /// <summary>
        /// BladeSwing3 sound.
        /// </summary>
        [CLProperty]
        public static string BladeSwing3 => HumanSounds.BladeSwing3;
        /// <summary>
        /// BladeSwing4 sound.
        /// </summary>
        [CLProperty]
        public static string BladeSwing4 => HumanSounds.BladeSwing4;
        /// <summary>
        /// OldBladeSwing sound.
        /// </summary>
        [CLProperty]
        public static string OldBladeSwing => HumanSounds.OldBladeSwing;
        /// <summary>
        /// Dodge sound.
        /// </summary>
        [CLProperty]
        public static string Dodge => HumanSounds.Dodge;
        /// <summary>
        /// FlareLaunch sound.
        /// </summary>
        [CLProperty]
        public static string FlareLaunch => HumanSounds.FlareLaunch;
        /// <summary>
        /// ThunderspearLaunch sound.
        /// </summary>
        [CLProperty]
        public static string ThunderspearLaunch => HumanSounds.ThunderspearLaunch;
        /// <summary>
        /// GasBurst sound.
        /// </summary>
        [CLProperty]
        public static string GasBurst => HumanSounds.GasBurst;
        /// <summary>
        /// HookLaunch sound.
        /// </summary>
        [CLProperty]
        public static string HookLaunch => HumanSounds.HookLaunch;
        /// <summary>
        /// OldHookLaunch sound.
        /// </summary>
        [CLProperty]
        public static string OldHookLaunch => HumanSounds.OldHookLaunch;
        /// <summary>
        /// HookRetractLeft sound.
        /// </summary>
        [CLProperty]
        public static string HookRetractLeft => HumanSounds.HookRetractLeft;
        /// <summary>
        /// HookRetractRight sound.
        /// </summary>
        [CLProperty]
        public static string HookRetractRight => HumanSounds.HookRetractRight;
        /// <summary>
        /// HookImpact sound.
        /// </summary>
        [CLProperty]
        public static string HookImpact => HumanSounds.HookImpact;
        /// <summary>
        /// HookImpactLoud sound.
        /// </summary>
        [CLProperty]
        public static string HookImpactLoud => HumanSounds.HookImpactLoud;
        /// <summary>
        /// GasStart sound.
        /// </summary>
        [CLProperty]
        public static string GasStart => HumanSounds.GasStart;
        /// <summary>
        /// GasLoop sound.
        /// </summary>
        [CLProperty]
        public static string GasLoop => HumanSounds.GasLoop;
        /// <summary>
        /// GasEnd sound.
        /// </summary>
        [CLProperty]
        public static string GasEnd => HumanSounds.GasEnd;
        /// <summary>
        /// ReelIn sound.
        /// </summary>
        [CLProperty]
        public static string ReelIn => HumanSounds.ReelIn;
        /// <summary>
        /// ReelOut sound.
        /// </summary>
        [CLProperty]
        public static string ReelOut => HumanSounds.ReelOut;
        /// <summary>
        /// CrashLand sound.
        /// </summary>
        [CLProperty]
        public static string CrashLand => HumanSounds.CrashLand;
        /// <summary>
        /// Jump sound.
        /// </summary>
        [CLProperty]
        public static string Jump => HumanSounds.Jump;
        /// <summary>
        /// Land sound.
        /// </summary>
        [CLProperty]
        public static string Land => HumanSounds.Land;
        /// <summary>
        /// NoGas sound.
        /// </summary>
        [CLProperty]
        public static string NoGas => HumanSounds.NoGas;
        /// <summary>
        /// Refill sound.
        /// </summary>
        [CLProperty]
        public static string Refill => HumanSounds.Refill;
        /// <summary>
        /// Slide sound.
        /// </summary>
        [CLProperty]
        public static string Slide => HumanSounds.Slide;
        /// <summary>
        /// Footstep1 sound.
        /// </summary>
        [CLProperty]
        public static string Footstep1 => HumanSounds.Footstep1;
        /// <summary>
        /// Footstep2 sound.
        /// </summary>
        [CLProperty]
        public static string Footstep2 => HumanSounds.Footstep2;
        /// <summary>
        /// Death1 sound.
        /// </summary>
        [CLProperty]
        public static string Death1 => HumanSounds.Death1;
        /// <summary>
        /// Death2 sound.
        /// </summary>
        [CLProperty]
        public static string Death2 => HumanSounds.Death2;
        /// <summary>
        /// Death3 sound.
        /// </summary>
        [CLProperty]
        public static string Death3 => HumanSounds.Death3;
        /// <summary>
        /// Death4 sound.
        /// </summary>
        [CLProperty]
        public static string Death4 => HumanSounds.Death4;
        /// <summary>
        /// Death5 sound.
        /// </summary>
        [CLProperty]
        public static string Death5 => HumanSounds.Death5;
        /// <summary>
        /// Checkpoint sound.
        /// </summary>
        [CLProperty]
        public static string Checkpoint => HumanSounds.Checkpoint;
        /// <summary>
        /// GunExplode sound.
        /// </summary>
        [CLProperty]
        public static string GunExplode => HumanSounds.GunExplode;
        /// <summary>
        /// GunExplodeLoud sound.
        /// </summary>
        [CLProperty]
        public static string GunExplodeLoud => HumanSounds.GunExplodeLoud;
        /// <summary>
        /// WaterSplash sound.
        /// </summary>
        [CLProperty]
        public static string WaterSplash => HumanSounds.WaterSplash;
        /// <summary>
        /// Switchback sound.
        /// </summary>
        [CLProperty]
        public static string Switchback => HumanSounds.Switchback;
        /// <summary>
        /// APGShot1 sound.
        /// </summary>
        [CLProperty]
        public static string APGShot1 => HumanSounds.APGShot1;
        /// <summary>
        /// APGShot2 sound.
        /// </summary>
        [CLProperty]
        public static string APGShot2 => HumanSounds.APGShot2;
        /// <summary>
        /// APGShot3 sound.
        /// </summary>
        [CLProperty]
        public static string APGShot3 => HumanSounds.APGShot3;
        /// <summary>
        /// APGShot4 sound.
        /// </summary>
        [CLProperty]
        public static string APGShot4 => HumanSounds.APGShot4;
        /// <summary>
        /// BladeNape1Var1 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape1Var1 => HumanSounds.BladeNape1Var1;
        /// <summary>
        /// BladeNape1Var2 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape1Var2 => HumanSounds.BladeNape1Var2;
        /// <summary>
        /// BladeNape1Var3 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape1Var3 => HumanSounds.BladeNape1Var3;
        /// <summary>
        /// BladeNape2Var1 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape2Var1 => HumanSounds.BladeNape2Var1;
        /// <summary>
        /// BladeNape2Var2 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape2Var2 => HumanSounds.BladeNape2Var2;
        /// <summary>
        /// BladeNape2Var3 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape2Var3 => HumanSounds.BladeNape2Var3;
        /// <summary>
        /// BladeNape3Var1 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape3Var1 => HumanSounds.BladeNape3Var1;
        /// <summary>
        /// BladeNape3Var2 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape3Var2 => HumanSounds.BladeNape3Var2;
        /// <summary>
        /// BladeNape3Var3 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape3Var3 => HumanSounds.BladeNape3Var3;
        /// <summary>
        /// BladeNape4Var1 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape4Var1 => HumanSounds.BladeNape4Var1;
        /// <summary>
        /// BladeNape4Var2 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape4Var2 => HumanSounds.BladeNape4Var2;
        /// <summary>
        /// BladeNape4Var3 sound.
        /// </summary>
        [CLProperty]
        public static string BladeNape4Var3 => HumanSounds.BladeNape4Var3;
        /// <summary>
        /// AHSSGunShot1 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShot1 => HumanSounds.AHSSGunShot1;
        /// <summary>
        /// AHSSGunShot2 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShot2 => HumanSounds.AHSSGunShot2;
        /// <summary>
        /// AHSSGunShot3 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShot3 => HumanSounds.AHSSGunShot3;
        /// <summary>
        /// AHSSGunShot4 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShot4 => HumanSounds.AHSSGunShot4;
        /// <summary>
        /// AHSSGunShotDouble1 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShotDouble1 => HumanSounds.AHSSGunShotDouble1;
        /// <summary>
        /// AHSSGunShotDouble2 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSGunShotDouble2 => HumanSounds.AHSSGunShotDouble2;
        /// <summary>
        /// AHSSNape1Var1 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape1Var1 => HumanSounds.AHSSNape1Var1;
        /// <summary>
        /// AHSSNape1Var2 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape1Var2 => HumanSounds.AHSSNape1Var2;
        /// <summary>
        /// AHSSNape2Var1 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape2Var1 => HumanSounds.AHSSNape2Var1;
        /// <summary>
        /// AHSSNape2Var2 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape2Var2 => HumanSounds.AHSSNape2Var2;
        /// <summary>
        /// AHSSNape3Var1 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape3Var1 => HumanSounds.AHSSNape3Var1;
        /// <summary>
        /// AHSSNape3Var2 sound.
        /// </summary>
        [CLProperty]
        public static string AHSSNape3Var2 => HumanSounds.AHSSNape3Var2;
        /// <summary>
        /// TSLaunch1 sound.
        /// </summary>
        [CLProperty]
        public static string TSLaunch1 => HumanSounds.TSLaunch1;
        /// <summary>
        /// TSLaunch2 sound.
        /// </summary>
        [CLProperty]
        public static string TSLaunch2 => HumanSounds.TSLaunch2;
    }
}
