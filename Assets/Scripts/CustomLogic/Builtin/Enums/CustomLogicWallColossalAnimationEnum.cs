using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for WallColossal characters.
    /// </summary>
    [CLType(Name = "WallColossalAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicWallColossalAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicWallColossalAnimationEnum() { }

        /// <summary>
        /// Idle animation.
        /// </summary>
        [CLProperty]
        public static string Idle => WallColossalAnimations.IdleValue;

        /// <summary>
        /// Attack wall slap 1 left animation.
        /// </summary>
        [CLProperty]
        public static string AttackWallSlap1L => WallColossalAnimations.AttackWallSlap1LValue;

        /// <summary>
        /// Attack wall slap 1 right animation.
        /// </summary>
        [CLProperty]
        public static string AttackWallSlap1R => WallColossalAnimations.AttackWallSlap1RValue;

        /// <summary>
        /// Attack wall slap 2 left animation.
        /// </summary>
        [CLProperty]
        public static string AttackWallSlap2L => WallColossalAnimations.AttackWallSlap2LValue;

        /// <summary>
        /// Attack wall slap 2 right animation.
        /// </summary>
        [CLProperty]
        public static string AttackWallSlap2R => WallColossalAnimations.AttackWallSlap2RValue;

        /// <summary>
        /// Attack steam animation.
        /// </summary>
        [CLProperty]
        public static string AttackSteam => WallColossalAnimations.AttackSteamValue;

        /// <summary>
        /// Attack sweep animation.
        /// </summary>
        [CLProperty]
        public static string AttackSweep => WallColossalAnimations.AttackSweepValue;

        /// <summary>
        /// Attack kick animation.
        /// </summary>
        [CLProperty]
        public static string AttackKick => WallColossalAnimations.AttackKickValue;
    }
}
