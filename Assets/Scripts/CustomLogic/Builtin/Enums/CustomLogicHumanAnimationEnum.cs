using Characters;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available animations for human characters.
    /// </summary>
    [CLType(Name = "HumanAnimationEnum", Static = true, Abstract = true)]
    partial class CustomLogicHumanAnimationEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHumanAnimationEnum() { }

        /// <summary>
        /// Horse mount animation.
        /// </summary>
        [CLProperty]
        public static string HorseMount => HumanAnimations.HorseMount;

        /// <summary>
        /// Horse dismount animation.
        /// </summary>
        [CLProperty]
        public static string HorseDismount => HumanAnimations.HorseDismount;

        /// <summary>
        /// Horse idle animation.
        /// </summary>
        [CLProperty]
        public static string HorseIdle => HumanAnimations.HorseIdle;

        /// <summary>
        /// Horse run animation.
        /// </summary>
        [CLProperty]
        public static string HorseRun => HumanAnimations.HorseRun;

        /// <summary>
        /// Idle female animation.
        /// </summary>
        [CLProperty]
        public static string IdleF => HumanAnimations.IdleF;

        /// <summary>
        /// Idle male animation.
        /// </summary>
        [CLProperty]
        public static string IdleM => HumanAnimations.IdleM;

        /// <summary>
        /// Idle AHSS male animation.
        /// </summary>
        [CLProperty]
        public static string IdleAHSSM => HumanAnimations.IdleAHSSM;

        /// <summary>
        /// Idle AHSS female animation.
        /// </summary>
        [CLProperty]
        public static string IdleAHSSF => HumanAnimations.IdleAHSSF;

        /// <summary>
        /// Idle Thunderspear female animation.
        /// </summary>
        [CLProperty]
        public static string IdleTSF => HumanAnimations.IdleTSF;

        /// <summary>
        /// Idle Thunderspear male animation.
        /// </summary>
        [CLProperty]
        public static string IdleTSM => HumanAnimations.IdleTSM;

        /// <summary>
        /// Jump animation.
        /// </summary>
        [CLProperty]
        public static string Jump => HumanAnimations.Jump;

        /// <summary>
        /// Run animation.
        /// </summary>
        [CLProperty]
        public static string Run => HumanAnimations.Run;

        /// <summary>
        /// Run Thunderspear animation.
        /// </summary>
        [CLProperty]
        public static string RunTS => HumanAnimations.RunTS;

        /// <summary>
        /// Run buffed animation.
        /// </summary>
        [CLProperty]
        public static string RunBuffed => HumanAnimations.RunBuffed;

        /// <summary>
        /// Dodge animation.
        /// </summary>
        [CLProperty]
        public static string Dodge => HumanAnimations.Dodge;

        /// <summary>
        /// Land animation.
        /// </summary>
        [CLProperty]
        public static string Land => HumanAnimations.Land;

        /// <summary>
        /// Slide animation.
        /// </summary>
        [CLProperty]
        public static string Slide => HumanAnimations.Slide;

        /// <summary>
        /// Grabbed animation.
        /// </summary>
        [CLProperty]
        public static string Grabbed => HumanAnimations.Grabbed;

        /// <summary>
        /// Dash animation.
        /// </summary>
        [CLProperty]
        public static string Dash => HumanAnimations.Dash;

        /// <summary>
        /// Refill animation.
        /// </summary>
        [CLProperty]
        public static string Refill => HumanAnimations.Refill;

        /// <summary>
        /// To roof animation.
        /// </summary>
        [CLProperty]
        public static string ToRoof => HumanAnimations.ToRoof;

        /// <summary>
        /// Wall run animation.
        /// </summary>
        [CLProperty]
        public static string WallRun => HumanAnimations.WallRun;

        /// <summary>
        /// On wall animation.
        /// </summary>
        [CLProperty]
        public static string OnWall => HumanAnimations.OnWall;

        /// <summary>
        /// Change blade animation.
        /// </summary>
        [CLProperty]
        public static string ChangeBlade => HumanAnimations.ChangeBlade;

        /// <summary>
        /// Change blade air animation.
        /// </summary>
        [CLProperty]
        public static string ChangeBladeAir => HumanAnimations.ChangeBladeAir;

        /// <summary>
        /// AHSS hook forward both animation.
        /// </summary>
        [CLProperty]
        public static string AHSSHookForwardBoth => HumanAnimations.AHSSHookForwardBoth;

        /// <summary>
        /// AHSS hook forward left animation.
        /// </summary>
        [CLProperty]
        public static string AHSSHookForwardL => HumanAnimations.AHSSHookForwardL;

        /// <summary>
        /// AHSS hook forward right animation.
        /// </summary>
        [CLProperty]
        public static string AHSSHookForwardR => HumanAnimations.AHSSHookForwardR;

        /// <summary>
        /// AHSS shoot right animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootR => HumanAnimations.AHSSShootR;

        /// <summary>
        /// AHSS shoot left animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootL => HumanAnimations.AHSSShootL;

        /// <summary>
        /// AHSS shoot both animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootBoth => HumanAnimations.AHSSShootBoth;

        /// <summary>
        /// AHSS shoot right air animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootRAir => HumanAnimations.AHSSShootRAir;

        /// <summary>
        /// AHSS shoot left air animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootLAir => HumanAnimations.AHSSShootLAir;

        /// <summary>
        /// AHSS shoot both air animation.
        /// </summary>
        [CLProperty]
        public static string AHSSShootBothAir => HumanAnimations.AHSSShootBothAir;

        /// <summary>
        /// AHSS gun reload both animation.
        /// </summary>
        [CLProperty]
        public static string AHSSGunReloadBoth => HumanAnimations.AHSSGunReloadBoth;

        /// <summary>
        /// AHSS gun reload both air animation.
        /// </summary>
        [CLProperty]
        public static string AHSSGunReloadBothAir => HumanAnimations.AHSSGunReloadBothAir;

        /// <summary>
        /// Thunderspear shoot right animation.
        /// </summary>
        [CLProperty]
        public static string TSShootR => HumanAnimations.TSShootR;

        /// <summary>
        /// Thunderspear shoot left animation.
        /// </summary>
        [CLProperty]
        public static string TSShootL => HumanAnimations.TSShootL;

        /// <summary>
        /// Thunderspear shoot right air animation.
        /// </summary>
        [CLProperty]
        public static string TSShootRAir => HumanAnimations.TSShootRAir;

        /// <summary>
        /// Thunderspear shoot left air animation.
        /// </summary>
        [CLProperty]
        public static string TSShootLAir => HumanAnimations.TSShootLAir;

        /// <summary>
        /// Air hook left just animation.
        /// </summary>
        [CLProperty]
        public static string AirHookLJust => HumanAnimations.AirHookLJust;

        /// <summary>
        /// Air hook right just animation.
        /// </summary>
        [CLProperty]
        public static string AirHookRJust => HumanAnimations.AirHookRJust;

        /// <summary>
        /// Air hook left animation.
        /// </summary>
        [CLProperty]
        public static string AirHookL => HumanAnimations.AirHookL;

        /// <summary>
        /// Air hook right animation.
        /// </summary>
        [CLProperty]
        public static string AirHookR => HumanAnimations.AirHookR;

        /// <summary>
        /// Air hook animation.
        /// </summary>
        [CLProperty]
        public static string AirHook => HumanAnimations.AirHook;

        /// <summary>
        /// Air release animation.
        /// </summary>
        [CLProperty]
        public static string AirRelease => HumanAnimations.AirRelease;

        /// <summary>
        /// Air fall animation.
        /// </summary>
        [CLProperty]
        public static string AirFall => HumanAnimations.AirFall;

        /// <summary>
        /// Air rise animation.
        /// </summary>
        [CLProperty]
        public static string AirRise => HumanAnimations.AirRise;

        /// <summary>
        /// Air 2 animation.
        /// </summary>
        [CLProperty]
        public static string Air2 => HumanAnimations.Air2;

        /// <summary>
        /// Air 2 right animation.
        /// </summary>
        [CLProperty]
        public static string Air2Right => HumanAnimations.Air2Right;

        /// <summary>
        /// Air 2 left animation.
        /// </summary>
        [CLProperty]
        public static string Air2Left => HumanAnimations.Air2Left;

        /// <summary>
        /// Air 2 backward animation.
        /// </summary>
        [CLProperty]
        public static string Air2Backward => HumanAnimations.Air2Backward;

        /// <summary>
        /// Attack 1 hook left 1 animation.
        /// </summary>
        [CLProperty]
        public static string Attack1HookL1 => HumanAnimations.Attack1HookL1;

        /// <summary>
        /// Attack 1 hook left 2 animation.
        /// </summary>
        [CLProperty]
        public static string Attack1HookL2 => HumanAnimations.Attack1HookL2;

        /// <summary>
        /// Attack 1 hook right 1 animation.
        /// </summary>
        [CLProperty]
        public static string Attack1HookR1 => HumanAnimations.Attack1HookR1;

        /// <summary>
        /// Attack 1 hook right 2 animation.
        /// </summary>
        [CLProperty]
        public static string Attack1HookR2 => HumanAnimations.Attack1HookR2;

        /// <summary>
        /// Attack 1 animation.
        /// </summary>
        [CLProperty]
        public static string Attack1 => HumanAnimations.Attack1;

        /// <summary>
        /// Attack 2 animation.
        /// </summary>
        [CLProperty]
        public static string Attack2 => HumanAnimations.Attack2;

        /// <summary>
        /// Attack 4 animation.
        /// </summary>
        [CLProperty]
        public static string Attack4 => HumanAnimations.Attack4;

        /// <summary>
        /// Special Armin animation.
        /// </summary>
        [CLProperty]
        public static string SpecialArmin => HumanAnimations.SpecialArmin;

        /// <summary>
        /// Special Marco 0 animation.
        /// </summary>
        [CLProperty]
        public static string SpecialMarco0 => HumanAnimations.SpecialMarco0;

        /// <summary>
        /// Special Marco 1 animation.
        /// </summary>
        [CLProperty]
        public static string SpecialMarco1 => HumanAnimations.SpecialMarco1;

        /// <summary>
        /// Special Sasha animation.
        /// </summary>
        [CLProperty]
        public static string SpecialSasha => HumanAnimations.SpecialSasha;

        /// <summary>
        /// Special Mikasa 1 animation.
        /// </summary>
        [CLProperty]
        public static string SpecialMikasa1 => HumanAnimations.SpecialMikasa1;

        /// <summary>
        /// Special Mikasa 2 animation.
        /// </summary>
        [CLProperty]
        public static string SpecialMikasa2 => HumanAnimations.SpecialMikasa2;

        /// <summary>
        /// Special Levi animation.
        /// </summary>
        [CLProperty]
        public static string SpecialLevi => HumanAnimations.SpecialLevi;

        /// <summary>
        /// Special Petra animation.
        /// </summary>
        [CLProperty]
        public static string SpecialPetra => HumanAnimations.SpecialPetra;

        /// <summary>
        /// Special Jean animation.
        /// </summary>
        [CLProperty]
        public static string SpecialJean => HumanAnimations.SpecialJean;

        /// <summary>
        /// Special Shifter animation.
        /// </summary>
        [CLProperty]
        public static string SpecialShifter => HumanAnimations.SpecialShifter;

        /// <summary>
        /// Emote salute animation.
        /// </summary>
        [CLProperty]
        public static string EmoteSalute => HumanAnimations.EmoteSalute;

        /// <summary>
        /// Emote no animation.
        /// </summary>
        [CLProperty]
        public static string EmoteNo => HumanAnimations.EmoteNo;

        /// <summary>
        /// Emote yes animation.
        /// </summary>
        [CLProperty]
        public static string EmoteYes => HumanAnimations.EmoteYes;

        /// <summary>
        /// Emote wave animation.
        /// </summary>
        [CLProperty]
        public static string EmoteWave => HumanAnimations.EmoteWave;
    }
}
