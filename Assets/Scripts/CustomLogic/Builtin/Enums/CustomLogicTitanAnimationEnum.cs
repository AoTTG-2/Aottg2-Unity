using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for titan characters.
    /// </summary>
    [CLType(Name = "TitanAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicTitanAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicTitanAnimationEnum() { }

        /// <summary>
        /// Idle animation.
        /// </summary>
        [CLProperty]
        public static string Idle => BasicTitanAnimations.IdleValue;

        /// <summary>
        /// Run abnormal animation.
        /// </summary>
        [CLProperty]
        public static string RunAbnormal => BasicTitanAnimations.RunAbnormalValue;

        /// <summary>
        /// Sprint abnormal animation.
        /// </summary>
        [CLProperty]
        public static string SprintAbnormal => BasicTitanAnimations.RunAbnormal1Value;

        /// <summary>
        /// Run crawler animation.
        /// </summary>
        [CLProperty]
        public static string RunCrawler => BasicTitanAnimations.RunCrawlerValue;

        /// <summary>
        /// Idle crawler animation.
        /// </summary>
        [CLProperty]
        public static string IdleCrawler => BasicTitanAnimations.IdleCrawlerValue;

        /// <summary>
        /// Jump crawler animation.
        /// </summary>
        [CLProperty]
        public static string JumpCrawler => BasicTitanAnimations.JumpCrawlerValue;

        /// <summary>
        /// Fall crawler animation.
        /// </summary>
        [CLProperty]
        public static string FallCrawler => BasicTitanAnimations.FallCrawlerValue;

        /// <summary>
        /// Land crawler animation.
        /// </summary>
        [CLProperty]
        public static string LandCrawler => BasicTitanAnimations.LandCrawlerValue;

        /// <summary>
        /// Walk animation.
        /// </summary>
        [CLProperty]
        public static string Walk => BasicTitanAnimations.WalkValue;

        /// <summary>
        /// Jump animation.
        /// </summary>
        [CLProperty]
        public static string Jump => BasicTitanAnimations.JumpValue;

        /// <summary>
        /// Fall animation.
        /// </summary>
        [CLProperty]
        public static string Fall => BasicTitanAnimations.FallValue;

        /// <summary>
        /// Land animation.
        /// </summary>
        [CLProperty]
        public static string Land => BasicTitanAnimations.LandValue;

        /// <summary>
        /// Stun animation.
        /// </summary>
        [CLProperty]
        public static string Stun => BasicTitanAnimations.StunValue;

        /// <summary>
        /// Stun left animation.
        /// </summary>
        [CLProperty]
        public static string StunLeft => BasicTitanAnimations.StunLeftValue;

        /// <summary>
        /// Stun right animation.
        /// </summary>
        [CLProperty]
        public static string StunRight => BasicTitanAnimations.StunRightValue;

        /// <summary>
        /// Die back animation.
        /// </summary>
        [CLProperty]
        public static string DieBack => BasicTitanAnimations.DieBackValue;

        /// <summary>
        /// Die front animation.
        /// </summary>
        [CLProperty]
        public static string DieFront => BasicTitanAnimations.DieFrontValue;

        /// <summary>
        /// Die ground animation.
        /// </summary>
        [CLProperty]
        public static string DieGround => BasicTitanAnimations.DieGroundValue;

        /// <summary>
        /// Die crawler animation.
        /// </summary>
        [CLProperty]
        public static string DieCrawler => BasicTitanAnimations.DieCrawlerValue;

        /// <summary>
        /// Die sit animation.
        /// </summary>
        [CLProperty]
        public static string DieSit => BasicTitanAnimations.DieSitValue;

        /// <summary>
        /// Attack punch combo animation.
        /// </summary>
        [CLProperty]
        public static string AttackPunchCombo => BasicTitanAnimations.AttackPunchComboValue;

        /// <summary>
        /// Attack punch animation.
        /// </summary>
        [CLProperty]
        public static string AttackPunch => BasicTitanAnimations.AttackPunchValue;

        /// <summary>
        /// Attack slam animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlam => BasicTitanAnimations.AttackSlamValue;

        /// <summary>
        /// Attack belly flop animation.
        /// </summary>
        [CLProperty]
        public static string AttackBellyFlop => BasicTitanAnimations.AttackBellyFlopValue;

        /// <summary>
        /// Attack belly flop getup animation.
        /// </summary>
        [CLProperty]
        public static string AttackBellyFlopGetup => BasicTitanAnimations.AttackBellyFlopGetupValue;

        /// <summary>
        /// Attack kick animation.
        /// </summary>
        [CLProperty]
        public static string AttackKick => BasicTitanAnimations.AttackKickValue;

        /// <summary>
        /// Attack stomp animation.
        /// </summary>
        [CLProperty]
        public static string AttackStomp => BasicTitanAnimations.AttackStompValue;

        /// <summary>
        /// Attack swing left animation.
        /// </summary>
        [CLProperty]
        public static string AttackSwingL => BasicTitanAnimations.AttackSwingLValue;

        /// <summary>
        /// Attack swing right animation.
        /// </summary>
        [CLProperty]
        public static string AttackSwingR => BasicTitanAnimations.AttackSwingRValue;

        /// <summary>
        /// Attack bite front animation.
        /// </summary>
        [CLProperty]
        public static string AttackBiteF => BasicTitanAnimations.AttackBiteFValue;

        /// <summary>
        /// Attack bite left animation.
        /// </summary>
        [CLProperty]
        public static string AttackBiteL => BasicTitanAnimations.AttackBiteLValue;

        /// <summary>
        /// Attack bite right animation.
        /// </summary>
        [CLProperty]
        public static string AttackBiteR => BasicTitanAnimations.AttackBiteRValue;

        /// <summary>
        /// Attack grab air far left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAirFarL => BasicTitanAnimations.AttackGrabAirFarLValue;

        /// <summary>
        /// Attack grab air far right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAirFarR => BasicTitanAnimations.AttackGrabAirFarRValue;

        /// <summary>
        /// Attack grab air left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAirL => BasicTitanAnimations.AttackGrabAirLValue;

        /// <summary>
        /// Attack grab air right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAirR => BasicTitanAnimations.AttackGrabAirRValue;

        /// <summary>
        /// Attack grab back left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBackL => BasicTitanAnimations.AttackGrabBackLValue;

        /// <summary>
        /// Attack grab back right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBackR => BasicTitanAnimations.AttackGrabBackRValue;

        /// <summary>
        /// Attack grab core left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabCoreL => BasicTitanAnimations.AttackGrabCoreLValue;

        /// <summary>
        /// Attack grab core right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabCoreR => BasicTitanAnimations.AttackGrabCoreRValue;

        /// <summary>
        /// Attack grab ground back left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabGroundBackL => BasicTitanAnimations.AttackGrabGroundBackLValue;

        /// <summary>
        /// Attack grab ground back right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabGroundBackR => BasicTitanAnimations.AttackGrabGroundBackRValue;

        /// <summary>
        /// Attack grab ground front left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabGroundFrontL => BasicTitanAnimations.AttackGrabGroundFrontLValue;

        /// <summary>
        /// Attack grab ground front right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabGroundFrontR => BasicTitanAnimations.AttackGrabGroundFrontRValue;

        /// <summary>
        /// Attack grab head back left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHeadBackL => BasicTitanAnimations.AttackGrabHeadBackLValue;

        /// <summary>
        /// Attack grab head back right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHeadBackR => BasicTitanAnimations.AttackGrabHeadBackRValue;

        /// <summary>
        /// Attack grab head front left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHeadFrontL => BasicTitanAnimations.AttackGrabHeadFrontLValue;

        /// <summary>
        /// Attack grab head front right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHeadFrontR => BasicTitanAnimations.AttackGrabHeadFrontRValue;

        /// <summary>
        /// Attack grab high left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHighL => BasicTitanAnimations.AttackGrabHighLValue;

        /// <summary>
        /// Attack grab high right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHighR => BasicTitanAnimations.AttackGrabHighRValue;

        /// <summary>
        /// Attack grab stomach left animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabStomachL => BasicTitanAnimations.AttackGrabStomachLValue;

        /// <summary>
        /// Attack grab stomach right animation.
        /// </summary>
        [CLProperty]
        public static string AttackGrabStomachR => BasicTitanAnimations.AttackGrabStomachRValue;

        /// <summary>
        /// Attack eat left animation.
        /// </summary>
        [CLProperty]
        public static string AttackEatL => BasicTitanAnimations.AttackEatLValue;

        /// <summary>
        /// Attack eat right animation.
        /// </summary>
        [CLProperty]
        public static string AttackEatR => BasicTitanAnimations.AttackEatRValue;

        /// <summary>
        /// Attack slap high left animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapHighL => BasicTitanAnimations.AttackSlapHighLValue;

        /// <summary>
        /// Attack slap high right animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapHighR => BasicTitanAnimations.AttackSlapHighRValue;

        /// <summary>
        /// Attack slap left animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapL => BasicTitanAnimations.AttackSlapLValue;

        /// <summary>
        /// Attack slap right animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapR => BasicTitanAnimations.AttackSlapRValue;

        /// <summary>
        /// Attack slap low left animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapLowL => BasicTitanAnimations.AttackSlapLowLValue;

        /// <summary>
        /// Attack slap low right animation.
        /// </summary>
        [CLProperty]
        public static string AttackSlapLowR => BasicTitanAnimations.AttackSlapLowRValue;

        /// <summary>
        /// Attack brush chest left animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushChestL => BasicTitanAnimations.AttackBrushChestLValue;

        /// <summary>
        /// Attack brush chest right animation.
        /// </summary>
        [CLProperty]
        public static string AttackBrushChestR => BasicTitanAnimations.AttackBrushChestRValue;

        /// <summary>
        /// Attack hit back animation.
        /// </summary>
        [CLProperty]
        public static string AttackHitBack => BasicTitanAnimations.AttackHitBackValue;

        /// <summary>
        /// Attack hit face animation.
        /// </summary>
        [CLProperty]
        public static string AttackHitFace => BasicTitanAnimations.AttackHitFaceValue;

        /// <summary>
        /// Attack rock throw animation.
        /// </summary>
        [CLProperty]
        public static string AttackRockThrow => BasicTitanAnimations.AttackRockThrowValue;

        /// <summary>
        /// Attack jump animation.
        /// </summary>
        [CLProperty]
        public static string AttackJump => BasicTitanAnimations.AttackJumpValue;

        /// <summary>
        /// Attack jump crawler animation.
        /// </summary>
        [CLProperty]
        public static string AttackJumpCrawler => BasicTitanAnimations.AttackJumpCrawlerValue;

        /// <summary>
        /// Sit idle animation.
        /// </summary>
        [CLProperty]
        public static string SitIdle => BasicTitanAnimations.SitIdleValue;

        /// <summary>
        /// Sit idle crawler animation.
        /// </summary>
        [CLProperty]
        public static string SitIdleCrawler => BasicTitanAnimations.SitIdleCrawlerValue;

        /// <summary>
        /// Sit down animation.
        /// </summary>
        [CLProperty]
        public static string SitDown => BasicTitanAnimations.SitDownValue;

        /// <summary>
        /// Sit up animation.
        /// </summary>
        [CLProperty]
        public static string SitUp => BasicTitanAnimations.SitUpValue;

        /// <summary>
        /// Sit up crawler animation.
        /// </summary>
        [CLProperty]
        public static string SitUpCrawler => BasicTitanAnimations.SitUpCrawlerValue;

        /// <summary>
        /// Sit fall animation.
        /// </summary>
        [CLProperty]
        public static string SitFall => BasicTitanAnimations.SitFallValue;

        /// <summary>
        /// Sit fall crawler animation.
        /// </summary>
        [CLProperty]
        public static string SitFallCrawler => BasicTitanAnimations.SitFallCrawlerValue;

        /// <summary>
        /// Turn 90 left animation.
        /// </summary>
        [CLProperty]
        public static string Turn90L => BasicTitanAnimations.Turn90LValue;

        /// <summary>
        /// Turn 90 right animation.
        /// </summary>
        [CLProperty]
        public static string Turn90R => BasicTitanAnimations.Turn90RValue;

        /// <summary>
        /// Turn 90 left crawler animation.
        /// </summary>
        [CLProperty]
        public static string Turn90LCrawler => BasicTitanAnimations.Turn90LCrawlerValue;

        /// <summary>
        /// Turn 90 right crawler animation.
        /// </summary>
        [CLProperty]
        public static string Turn90RCrawler => BasicTitanAnimations.Turn90RCrawlerValue;

        /// <summary>
        /// Blind animation.
        /// </summary>
        [CLProperty]
        public static string Blind => BasicTitanAnimations.BlindValue;

        /// <summary>
        /// Sit blind animation.
        /// </summary>
        [CLProperty]
        public static string SitBlind => BasicTitanAnimations.SitBlindValue;

        /// <summary>
        /// Blind crawler animation.
        /// </summary>
        [CLProperty]
        public static string BlindCrawler => BasicTitanAnimations.BlindCrawlerValue;

        /// <summary>
        /// Arm hurt left animation.
        /// </summary>
        [CLProperty]
        public static string ArmHurtL => BasicTitanAnimations.ArmHurtLValue;

        /// <summary>
        /// Arm hurt right animation.
        /// </summary>
        [CLProperty]
        public static string ArmHurtR => BasicTitanAnimations.ArmHurtRValue;

        /// <summary>
        /// Cover nape animation.
        /// </summary>
        [CLProperty]
        public static string CoverNape => BasicTitanAnimations.CoverNapeValue;

        /// <summary>
        /// Emote laugh animation.
        /// </summary>
        [CLProperty]
        public static string EmoteLaugh => BasicTitanAnimations.EmoteLaughValue;

        /// <summary>
        /// Emote nod animation.
        /// </summary>
        [CLProperty]
        public static string EmoteNod => BasicTitanAnimations.EmoteNodValue;

        /// <summary>
        /// Emote shake animation.
        /// </summary>
        [CLProperty]
        public static string EmoteShake => BasicTitanAnimations.EmoteShakeValue;

        /// <summary>
        /// Emote roar animation.
        /// </summary>
        [CLProperty]
        public static string EmoteRoar => BasicTitanAnimations.EmoteRoarValue;
    }
}
