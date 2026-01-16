using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for horse characters.
    /// </summary>
    [CLType(Name = "HorseAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicHorseAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHorseAnimationEnum() { }

        /// <summary>
        /// Idle0 animation.
        /// </summary>
        [CLProperty]
        public static string Idle0 => HorseAnimations.Idle0;

        /// <summary>
        /// Idle1 animation.
        /// </summary>
        [CLProperty]
        public static string Idle1 => HorseAnimations.Idle1;

        /// <summary>
        /// Idle2 animation.
        /// </summary>
        [CLProperty]
        public static string Idle2 => HorseAnimations.Idle2;

        /// <summary>
        /// Idle3 animation.
        /// </summary>
        [CLProperty]
        public static string Idle3 => HorseAnimations.Idle3;

        /// <summary>
        /// Crazy animation.
        /// </summary>
        [CLProperty]
        public static string Crazy => HorseAnimations.Crazy;

        /// <summary>
        /// Run animation.
        /// </summary>
        [CLProperty]
        public static string Run => HorseAnimations.Run;

        /// <summary>
        /// Walk animation.
        /// </summary>
        [CLProperty]
        public static string Walk => HorseAnimations.Walk;
    }
}
