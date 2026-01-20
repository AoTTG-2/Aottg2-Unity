using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of human state types.
    /// </summary>
    [CLType(Name = "HumanStateEnum", Static = true, Abstract = true)]
    partial class CustomLogicHumanStateEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHumanStateEnum() { }

        private static readonly string IdleValue = HumanState.Idle.ToString();
        private static readonly string AttackValue = HumanState.Attack.ToString();
        private static readonly string GroundDodgeValue = HumanState.GroundDodge.ToString();
        private static readonly string AirDodgeValue = HumanState.AirDodge.ToString();
        private static readonly string ReloadValue = HumanState.Reload.ToString();
        private static readonly string RefillValue = HumanState.Refill.ToString();
        private static readonly string DieValue = HumanState.Die.ToString();
        private static readonly string GrabValue = HumanState.Grab.ToString();
        private static readonly string EmoteActionValue = HumanState.EmoteAction.ToString();
        private static readonly string SpecialAttackValue = HumanState.SpecialAttack.ToString();
        private static readonly string SpecialActionValue = HumanState.SpecialAction.ToString();
        private static readonly string SlideValue = HumanState.Slide.ToString();
        private static readonly string RunValue = HumanState.Run.ToString();
        private static readonly string LandValue = HumanState.Land.ToString();
        private static readonly string MountingHorseValue = HumanState.MountingHorse.ToString();
        private static readonly string StunValue = HumanState.Stun.ToString();
        private static readonly string WallSlideValue = HumanState.WallSlide.ToString();

        /// <summary>
        /// Human is idle.
        /// </summary>
        [CLProperty]
        public static string Idle => IdleValue;

        /// <summary>
        /// Human is attacking.
        /// </summary>
        [CLProperty]
        public static string Attack => AttackValue;

        /// <summary>
        /// Human is performing a ground dodge.
        /// </summary>
        [CLProperty]
        public static string GroundDodge => GroundDodgeValue;

        /// <summary>
        /// Human is performing an air dodge.
        /// </summary>
        [CLProperty]
        public static string AirDodge => AirDodgeValue;

        /// <summary>
        /// Human is reloading.
        /// </summary>
        [CLProperty]
        public static string Reload => ReloadValue;

        /// <summary>
        /// Human is refilling gas.
        /// </summary>
        [CLProperty]
        public static string Refill => RefillValue;

        /// <summary>
        /// Human is dying.
        /// </summary>
        [CLProperty]
        public static string Die => DieValue;

        /// <summary>
        /// Human is grabbed.
        /// </summary>
        [CLProperty]
        public static string Grab => GrabValue;

        /// <summary>
        /// Human is performing an emote action.
        /// </summary>
        [CLProperty]
        public static string EmoteAction => EmoteActionValue;

        /// <summary>
        /// Human is performing a special attack.
        /// </summary>
        [CLProperty]
        public static string SpecialAttack => SpecialAttackValue;

        /// <summary>
        /// Human is performing a special action.
        /// </summary>
        [CLProperty]
        public static string SpecialAction => SpecialActionValue;

        /// <summary>
        /// Human is sliding.
        /// </summary>
        [CLProperty]
        public static string Slide => SlideValue;

        /// <summary>
        /// Human is running.
        /// </summary>
        [CLProperty]
        public static string Run => RunValue;

        /// <summary>
        /// Human is landing.
        /// </summary>
        [CLProperty]
        public static string Land => LandValue;

        /// <summary>
        /// Human is mounting a horse.
        /// </summary>
        [CLProperty]
        public static string MountingHorse => MountingHorseValue;

        /// <summary>
        /// Human is stunned.
        /// </summary>
        [CLProperty]
        public static string Stun => StunValue;

        /// <summary>
        /// Human is wall sliding.
        /// </summary>
        [CLProperty]
        public static string WallSlide => WallSlideValue;
    }
}
