namespace CustomLogic
{
    /// <summary>
    /// Enumeration of available particle effect names for humans.
    /// </summary>
    [CLType(Name = "HumanParticleEffectEnum", Static = true, Abstract = true)]
    partial class CustomLogicHumanParticleEffectEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHumanParticleEffectEnum() { }

        /// <summary>
        /// Buff1 particle effect.
        /// </summary>
        [CLProperty]
        public static string Buff1 => "Buff1";

        /// <summary>
        /// Buff2 particle effect.
        /// </summary>
        [CLProperty]
        public static string Buff2 => "Buff2";

        /// <summary>
        /// Fire1 particle effect.
        /// </summary>
        [CLProperty]
        public static string Fire1 => "Fire1";
    }
}
