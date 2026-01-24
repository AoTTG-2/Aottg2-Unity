using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for Eren characters.
    /// </summary>
    [CLType(Name = "ErenAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicErenAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicErenAnimationEnum() { }

        /// <summary>
        /// Idle animation.
        /// </summary>
        [CLProperty]
        public static string Idle => ErenAnimations.IdleValue;

        /// <summary>
        /// Run animation.
        /// </summary>
        [CLProperty]
        public static string Run => ErenAnimations.RunValue;

        /// <summary>
        /// Walk animation.
        /// </summary>
        [CLProperty]
        public static string Walk => ErenAnimations.WalkValue;

        /// <summary>
        /// Jump animation.
        /// </summary>
        [CLProperty]
        public static string Jump => ErenAnimations.JumpValue;

        /// <summary>
        /// Fall animation.
        /// </summary>
        [CLProperty]
        public static string Fall => ErenAnimations.FallValue;

        /// <summary>
        /// Land animation.
        /// </summary>
        [CLProperty]
        public static string Land => ErenAnimations.LandValue;

        /// <summary>
        /// Die animation.
        /// </summary>
        [CLProperty]
        public static string Die => ErenAnimations.DieValue;

        /// <summary>
        /// Attack combo animation.
        /// </summary>
        [CLProperty]
        public static string AttackCombo => ErenAnimations.AttackComboValue;

        /// <summary>
        /// Attack kick animation.
        /// </summary>
        [CLProperty]
        public static string AttackKick => ErenAnimations.AttackKickValue;

        /// <summary>
        /// Stun animation.
        /// </summary>
        [CLProperty]
        public static string Stun => ErenAnimations.StunValue;

        /// <summary>
        /// Emote nod animation.
        /// </summary>
        [CLProperty]
        public static string EmoteNod => ErenAnimations.EmoteNodValue;

        /// <summary>
        /// Emote roar animation.
        /// </summary>
        [CLProperty]
        public static string EmoteRoar => ErenAnimations.EmoteRoarValue;

        /// <summary>
        /// Rock lift animation.
        /// </summary>
        [CLProperty]
        public static string RockLift => ErenAnimations.RockLiftValue;

        /// <summary>
        /// Rock lift 001 animation.
        /// </summary>
        [CLProperty]
        public static string RockLift001 => ErenAnimations.RockLift001Value;

        /// <summary>
        /// Rock walk animation.
        /// </summary>
        [CLProperty]
        public static string RockWalk => ErenAnimations.RockWalkValue;

        /// <summary>
        /// Rock fix hole animation.
        /// </summary>
        [CLProperty]
        public static string RockFixHole => ErenAnimations.RockFixHoleValue;
    }
}
