namespace CustomLogic
{
    /// <summary>
    /// Enumeration of Human input keybind settings.
    /// </summary>
    [CLType(Name = "InputHumanEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputHumanEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputHumanEnum() { }

        private static readonly string AttackDefaultValue = CustomLogicInputCategoryEnum.HumanValue + "/AttackDefault";
        private static readonly string AttackSpecialValue = CustomLogicInputCategoryEnum.HumanValue + "/AttackSpecial";
        private static readonly string HookLeftValue = CustomLogicInputCategoryEnum.HumanValue + "/HookLeft";
        private static readonly string HookRightValue = CustomLogicInputCategoryEnum.HumanValue + "/HookRight";
        private static readonly string HookBothValue = CustomLogicInputCategoryEnum.HumanValue + "/HookBoth";
        private static readonly string DashValue = CustomLogicInputCategoryEnum.HumanValue + "/Dash";
        private static readonly string ReelInValue = CustomLogicInputCategoryEnum.HumanValue + "/ReelIn";
        private static readonly string ReelOutValue = CustomLogicInputCategoryEnum.HumanValue + "/ReelOut";
        private static readonly string DodgeValue = CustomLogicInputCategoryEnum.HumanValue + "/Dodge";
        private static readonly string JumpValue = CustomLogicInputCategoryEnum.HumanValue + "/Jump";
        private static readonly string ReloadValue = CustomLogicInputCategoryEnum.HumanValue + "/Reload";
        private static readonly string HorseMountValue = CustomLogicInputCategoryEnum.HumanValue + "/HorseMount";
        private static readonly string HorseWalkValue = CustomLogicInputCategoryEnum.HumanValue + "/HorseWalk";
        private static readonly string HorseJumpValue = CustomLogicInputCategoryEnum.HumanValue + "/HorseJump";
        private static readonly string NapeLockValue = CustomLogicInputCategoryEnum.HumanValue + "/NapeLock";

        /// <summary>
        /// Human/AttackDefault keybind.
        /// </summary>
        [CLProperty]
        public static string AttackDefault => AttackDefaultValue;
        /// <summary>
        /// Human/AttackSpecial keybind.
        /// </summary>
        [CLProperty]
        public static string AttackSpecial => AttackSpecialValue;
        /// <summary>
        /// Human/HookLeft keybind.
        /// </summary>
        [CLProperty]
        public static string HookLeft => HookLeftValue;
        /// <summary>
        /// Human/HookRight keybind.
        /// </summary>
        [CLProperty]
        public static string HookRight => HookRightValue;
        /// <summary>
        /// Human/HookBoth keybind.
        /// </summary>
        [CLProperty]
        public static string HookBoth => HookBothValue;
        /// <summary>
        /// Human/Dash keybind.
        /// </summary>
        [CLProperty]
        public static string Dash => DashValue;
        /// <summary>
        /// Human/ReelIn keybind.
        /// </summary>
        [CLProperty]
        public static string ReelIn => ReelInValue;
        /// <summary>
        /// Human/ReelOut keybind.
        /// </summary>
        [CLProperty]
        public static string ReelOut => ReelOutValue;
        /// <summary>
        /// Human/Dodge keybind.
        /// </summary>
        [CLProperty]
        public static string Dodge => DodgeValue;
        /// <summary>
        /// Human/Jump keybind.
        /// </summary>
        [CLProperty]
        public static string Jump => JumpValue;
        /// <summary>
        /// Human/Reload keybind.
        /// </summary>
        [CLProperty]
        public static string Reload => ReloadValue;
        /// <summary>
        /// Human/HorseMount keybind.
        /// </summary>
        [CLProperty]
        public static string HorseMount => HorseMountValue;
        /// <summary>
        /// Human/HorseWalk keybind.
        /// </summary>
        [CLProperty]
        public static string HorseWalk => HorseWalkValue;
        /// <summary>
        /// Human/HorseJump keybind.
        /// </summary>
        [CLProperty]
        public static string HorseJump => HorseJumpValue;
        /// <summary>
        /// Human/NapeLock keybind.
        /// </summary>
        [CLProperty]
        public static string NapeLock => NapeLockValue;
    }
}
