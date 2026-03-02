namespace CustomLogic
{
    /// <summary>
    /// Enumeration of Titan input keybind settings.
    /// </summary>
    [CLType(Name = "InputTitanEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputTitanEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputTitanEnum() { }

        private static readonly string KickValue = CustomLogicInputCategoryEnum.TitanValue + "/Kick";
        private static readonly string JumpValue = CustomLogicInputCategoryEnum.TitanValue + "/Jump";
        private static readonly string SitValue = CustomLogicInputCategoryEnum.TitanValue + "/Sit";
        private static readonly string WalkValue = CustomLogicInputCategoryEnum.TitanValue + "/Walk";
        private static readonly string SprintValue = CustomLogicInputCategoryEnum.TitanValue + "/Sprint";
        private static readonly string CoverNape1Value = CustomLogicInputCategoryEnum.TitanValue + "/CoverNape1";
        private static readonly string AttackPunchValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackPunch";
        private static readonly string AttackBellyFlopValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackBellyFlop";
        private static readonly string AttackSlapLValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapL";
        private static readonly string AttackSlapRValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapR";
        private static readonly string AttackRockThrowValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackRockThrow";
        private static readonly string AttackBiteLValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackBiteL";
        private static readonly string AttackBiteFValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackBiteF";
        private static readonly string AttackBiteRValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackBiteR";
        private static readonly string AttackHitFaceValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackHitFace";
        private static readonly string AttackHitBackValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackHitBack";
        private static readonly string AttackSlamValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlam";
        private static readonly string AttackStompValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackStomp";
        private static readonly string AttackSwingValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSwing";
        private static readonly string AttackGrabAirFarValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabAirFar";
        private static readonly string AttackGrabAirValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabAir";
        private static readonly string AttackGrabBodyValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabBody";
        private static readonly string AttackGrabCoreValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabCore";
        private static readonly string AttackGrabGroundValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabGround";
        private static readonly string AttackGrabHeadValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabHead";
        private static readonly string AttackGrabHighValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackGrabHigh";
        private static readonly string AttackSlapHighLValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapHighL";
        private static readonly string AttackSlapHighRValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapHighR";
        private static readonly string AttackSlapLowLValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapLowL";
        private static readonly string AttackSlapLowRValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackSlapLowR";
        private static readonly string AttackBrushChestValue = CustomLogicInputCategoryEnum.TitanValue + "/AttackBrushChest";

        /// <summary>
        /// Titan/Kick keybind.
        /// </summary>
        [CLProperty]
        public static string Kick => KickValue;
        /// <summary>
        /// Titan/Jump keybind.
        /// </summary>
        [CLProperty]
        public static string Jump => JumpValue;
        /// <summary>
        /// Titan/Sit keybind.
        /// </summary>
        [CLProperty]
        public static string Sit => SitValue;
        /// <summary>
        /// Titan/Walk keybind.
        /// </summary>
        [CLProperty]
        public static string Walk => WalkValue;
        /// <summary>
        /// Titan/Sprint keybind.
        /// </summary>
        [CLProperty]
        public static string Sprint => SprintValue;
        /// <summary>
        /// Titan/CoverNape1 keybind.
        /// </summary>
        [CLProperty]
        public static string CoverNape1 => CoverNape1Value;
        /// <summary>
        /// Titan/AttackPunch keybind.
        /// </summary>
        [CLProperty]
        public static string AttackPunch => AttackPunchValue;
        /// <summary>
        /// Titan/AttackBellyFlop keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBellyFlop => AttackBellyFlopValue;
        /// <summary>
        /// Titan/AttackSlapL keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapL => AttackSlapLValue;
        /// <summary>
        /// Titan/AttackSlapR keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapR => AttackSlapRValue;
        /// <summary>
        /// Titan/AttackRockThrow keybind.
        /// </summary>
        [CLProperty]
        public static string AttackRockThrow => AttackRockThrowValue;
        /// <summary>
        /// Titan/AttackBiteL keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBiteL => AttackBiteLValue;
        /// <summary>
        /// Titan/AttackBiteF keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBiteF => AttackBiteFValue;
        /// <summary>
        /// Titan/AttackBiteR keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBiteR => AttackBiteRValue;
        /// <summary>
        /// Titan/AttackHitFace keybind.
        /// </summary>
        [CLProperty]
        public static string AttackHitFace => AttackHitFaceValue;
        /// <summary>
        /// Titan/AttackHitBack keybind.
        /// </summary>
        [CLProperty]
        public static string AttackHitBack => AttackHitBackValue;
        /// <summary>
        /// Titan/AttackSlam keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlam => AttackSlamValue;
        /// <summary>
        /// Titan/AttackStomp keybind.
        /// </summary>
        [CLProperty]
        public static string AttackStomp => AttackStompValue;
        /// <summary>
        /// Titan/AttackSwing keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSwing => AttackSwingValue;
        /// <summary>
        /// Titan/AttackGrabAirFar keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAirFar => AttackGrabAirFarValue;
        /// <summary>
        /// Titan/AttackGrabAir keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabAir => AttackGrabAirValue;
        /// <summary>
        /// Titan/AttackGrabBody keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabBody => AttackGrabBodyValue;
        /// <summary>
        /// Titan/AttackGrabCore keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabCore => AttackGrabCoreValue;
        /// <summary>
        /// Titan/AttackGrabGround keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabGround => AttackGrabGroundValue;
        /// <summary>
        /// Titan/AttackGrabHead keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHead => AttackGrabHeadValue;
        /// <summary>
        /// Titan/AttackGrabHigh keybind.
        /// </summary>
        [CLProperty]
        public static string AttackGrabHigh => AttackGrabHighValue;
        /// <summary>
        /// Titan/AttackSlapHighL keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapHighL => AttackSlapHighLValue;
        /// <summary>
        /// Titan/AttackSlapHighR keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapHighR => AttackSlapHighRValue;
        /// <summary>
        /// Titan/AttackSlapLowL keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapLowL => AttackSlapLowLValue;
        /// <summary>
        /// Titan/AttackSlapLowR keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSlapLowR => AttackSlapLowRValue;
        /// <summary>
        /// Titan/AttackBrushChest keybind.
        /// </summary>
        [CLProperty]
        public static string AttackBrushChest => AttackBrushChestValue;
    }
}
