namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available particle effect names for humans.
    /// </summary>
    [CLType(Name = "HumanParticleEffectEnum", Static = true, Abstract = true)]
    partial class CustomLogicHumanParticleEffectEnum : BuiltinClassInstance
    {
        internal const string Buff1Value = "Buff1";
        internal const string Buff2Value = "Buff2";
        internal const string Fire1Value = "Fire1";

        [CLConstructor]
        public CustomLogicHumanParticleEffectEnum() { }

        /// <summary>
        /// Buff1 particle effect.
        /// </summary>
        [CLProperty]
        public static string Buff1 => Buff1Value;

        /// <summary>
        /// Buff2 particle effect.
        /// </summary>
        [CLProperty]
        public static string Buff2 => Buff2Value;

        /// <summary>
        /// Fire1 particle effect.
        /// </summary>
        [CLProperty]
        public static string Fire1 => Fire1Value;
    }
}
