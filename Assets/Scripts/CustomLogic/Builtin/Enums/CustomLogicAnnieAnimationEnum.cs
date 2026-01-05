using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for Annie characters.
    /// </summary>
    [CLType(Name = "AnnieAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicAnnieAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicAnnieAnimationEnum() { }

        /// <summary>
        /// Idle animation.
        /// </summary>
        [CLProperty]
        public static string Idle => AnnieAnimations.IdleValue;

        /// <summary>
        /// Run animation.
        /// </summary>
        [CLProperty]
        public static string Run => AnnieAnimations.RunValue;

        /// <summary>
        /// Walk animation.
        /// </summary>
        [CLProperty]
        public static string Walk => AnnieAnimations.WalkValue;

        /// <summary>
        /// Jump animation.
        /// </summary>
        [CLProperty]
        public static string Jump => AnnieAnimations.JumpValue;

        /// <summary>
        /// Fall animation.
        /// </summary>
        [CLProperty]
        public static string Fall => AnnieAnimations.FallValue;

        /// <summary>
        /// Land animation.
        /// </summary>
        [CLProperty]
        public static string Land => AnnieAnimations.LandValue;

        /// <summary>
        /// Die animation.
        /// </summary>
        [CLProperty]
        public static string Die => AnnieAnimations.DieValue;

        /// <summary>
        /// Stun animation.
        /// </summary>
        [CLProperty]
        public static string Stun => AnnieAnimations.StunValue;

        /// <summary>
        /// Sit fall animation.
        /// </summary>
        [CLProperty]
        public static string SitFall => AnnieAnimations.SitFallValue;

        /// <summary>
        /// Sit idle animation.
        /// </summary>
        [CLProperty]
        public static string SitIdle => AnnieAnimations.SitIdleValue;

        /// <summary>
        /// Sit up animation.
        /// </summary>
        [CLProperty]
        public static string SitUp => AnnieAnimations.SitUpValue;

        /// <summary>
        /// Attack combo animation.
        /// </summary>
        [CLProperty]
        public static string AttackCombo => AnnieAnimations.AttackComboValue;

        /// <summary>
        /// Attack combo blind animation.
        /// </summary>
        [CLProperty]
        public static string AttackComboBlind => AnnieAnimations.AttackComboBlindValue;

        /// <summary>
        /// Attack swing animation.
        /// </summary>
        [CLProperty]
        public static string AttackSwing => AnnieAnimations.AttackSwingValue;

        /// <summary>
        /// Attack brush back animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushBack => AnnieAnimations.AttackBrushBackValue;

        /// <summary>
        /// Attack brush front left animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushFrontL => AnnieAnimations.AttackBrushFrontLValue;

        /// <summary>
        /// Attack brush front right animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushFrontR => AnnieAnimations.AttackBrushFrontRValue;

        /// <summary>
        /// Attack brush head left animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushHeadL => AnnieAnimations.AttackBrushHeadLValue;

        /// <summary>
        /// Attack brush head right animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushHeadR => AnnieAnimations.AttackBrushHeadRValue;

        /// <summary>
        /// Attack grab bottom left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBottomLeft => AnnieAnimations.AttackGrabBottomLeftValue;

        /// <summary>
        /// Attack grab bottom right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBottomRight => AnnieAnimations.AttackGrabBottomRightValue;

        /// <summary>
        /// Attack grab mid left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabMidLeft => AnnieAnimations.AttackGrabMidLeftValue;

        /// <summary>
        /// Attack grab mid right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabMidRight => AnnieAnimations.AttackGrabMidRightValue;

        /// <summary>
        /// Attack grab up animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabUp => AnnieAnimations.AttackGrabUpValue;

        /// <summary>
        /// Attack grab up left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabUpLeft => AnnieAnimations.AttackGrabUpLeftValue;

        /// <summary>
        /// Attack grab up right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabUpRight => AnnieAnimations.AttackGrabUpRightValue;

        /// <summary>
        /// Attack kick animation.
        /// </summary>
        [CLProperty]
        public static string AttackKick => AnnieAnimations.AttackKickValue;

        /// <summary>
        /// Attack stomp animation.
        /// </summary>
        [CLProperty]
        public static string AttackStomp => AnnieAnimations.AttackStompValue;

        /// <summary>
        /// Attack head animation.
        /// </summary>
        [CLProperty]
        public static string AttackHead => AnnieAnimations.AttackHeadValue;

        /// <summary>
        /// Attack bite animation.
        /// </summary>
        [CLProperty]
        public static string AttackBite => AnnieAnimations.AttackBiteValue;

        /// <summary>
        /// Emote salute animation.
        /// </summary>
        [CLProperty]
        public static string EmoteSalute => AnnieAnimations.EmoteSaluteValue;

        /// <summary>
        /// Emote taunt animation.
        /// </summary>
        [CLProperty]
        public static string EmoteTaunt => AnnieAnimations.EmoteTauntValue;

        /// <summary>
        /// Emote wave animation.
        /// </summary>
        [CLProperty]
        public static string EmoteWave => AnnieAnimations.EmoteWaveValue;

        /// <summary>
        /// Emote roar animation.
        /// </summary>
        [CLProperty]
        public static string EmoteRoar => AnnieAnimations.EmoteRoarValue;
    }
}
