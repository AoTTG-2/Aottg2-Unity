namespace CustomLogic
{
    /// <summary>
    /// Enumeration of Interaction input keybind settings.
    /// </summary>
    [CLType(Name = "InputInteractionEnum", Static = true, Abstract = true)]
    partial class CustomLogicInputInteractionEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicInputInteractionEnum() { }

        private static readonly string InteractValue = CustomLogicInputCategoryEnum.InteractionValue + "/Interact";
        private static readonly string Interact2Value = CustomLogicInputCategoryEnum.InteractionValue + "/Interact2";
        private static readonly string Interact3Value = CustomLogicInputCategoryEnum.InteractionValue + "/Interact3";
        private static readonly string ItemMenuValue = CustomLogicInputCategoryEnum.InteractionValue + "/ItemMenu";
        private static readonly string EmoteMenuValue = CustomLogicInputCategoryEnum.InteractionValue + "/EmoteMenu";
        private static readonly string MenuNextValue = CustomLogicInputCategoryEnum.InteractionValue + "/MenuNext";
        private static readonly string QuickSelect1Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect1";
        private static readonly string QuickSelect2Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect2";
        private static readonly string QuickSelect3Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect3";
        private static readonly string QuickSelect4Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect4";
        private static readonly string QuickSelect5Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect5";
        private static readonly string QuickSelect6Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect6";
        private static readonly string QuickSelect7Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect7";
        private static readonly string QuickSelect8Value = CustomLogicInputCategoryEnum.InteractionValue + "/QuickSelect8";
        private static readonly string Function1Value = CustomLogicInputCategoryEnum.InteractionValue + "/Function1";
        private static readonly string Function2Value = CustomLogicInputCategoryEnum.InteractionValue + "/Function2";
        private static readonly string Function3Value = CustomLogicInputCategoryEnum.InteractionValue + "/Function3";
        private static readonly string Function4Value = CustomLogicInputCategoryEnum.InteractionValue + "/Function4";

        /// <summary>
        /// Interaction/Interact keybind.
        /// </summary>
        [CLProperty]
        public static string Interact => InteractValue;
        /// <summary>
        /// Interaction/Interact2 keybind.
        /// </summary>
        [CLProperty]
        public static string Interact2 => Interact2Value;
        /// <summary>
        /// Interaction/Interact3 keybind.
        /// </summary>
        [CLProperty]
        public static string Interact3 => Interact3Value;
        /// <summary>
        /// Interaction/ItemMenu keybind.
        /// </summary>
        [CLProperty]
        public static string ItemMenu => ItemMenuValue;
        /// <summary>
        /// Interaction/EmoteMenu keybind.
        /// </summary>
        [CLProperty]
        public static string EmoteMenu => EmoteMenuValue;
        /// <summary>
        /// Interaction/MenuNext keybind.
        /// </summary>
        [CLProperty]
        public static string MenuNext => MenuNextValue;
        /// <summary>
        /// Interaction/QuickSelect1 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect1 => QuickSelect1Value;
        /// <summary>
        /// Interaction/QuickSelect2 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect2 => QuickSelect2Value;
        /// <summary>
        /// Interaction/QuickSelect3 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect3 => QuickSelect3Value;
        /// <summary>
        /// Interaction/QuickSelect4 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect4 => QuickSelect4Value;
        /// <summary>
        /// Interaction/QuickSelect5 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect5 => QuickSelect5Value;
        /// <summary>
        /// Interaction/QuickSelect6 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect6 => QuickSelect6Value;
        /// <summary>
        /// Interaction/QuickSelect7 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect7 => QuickSelect7Value;
        /// <summary>
        /// Interaction/QuickSelect8 keybind.
        /// </summary>
        [CLProperty]
        public static string QuickSelect8 => QuickSelect8Value;
        /// <summary>
        /// Interaction/Function1 keybind.
        /// </summary>
        [CLProperty]
        public static string Function1 => Function1Value;
        /// <summary>
        /// Interaction/Function2 keybind.
        /// </summary>
        [CLProperty]
        public static string Function2 => Function2Value;
        /// <summary>
        /// Interaction/Function3 keybind.
        /// </summary>
        [CLProperty]
        public static string Function3 => Function3Value;
        /// <summary>
        /// Interaction/Function4 keybind.
        /// </summary>
        [CLProperty]
        public static string Function4 => Function4Value;
    }
}
