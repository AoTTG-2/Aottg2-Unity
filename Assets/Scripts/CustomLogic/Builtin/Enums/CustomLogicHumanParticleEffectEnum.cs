namespace CustomLogic
{
    [CLType(Name = "HumanParticleEffectEnum", Static = true, Abstract = true, Description = "Enumeration of available particle effect names for humans.")]
    partial class CustomLogicHumanParticleEffectEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicHumanParticleEffectEnum() { }

        [CLProperty("Buff1 particle effect.")]
        public static string Buff1 => "Buff1";

        [CLProperty("Buff2 particle effect.")]
        public static string Buff2 => "Buff2";

        [CLProperty("Fire1 particle effect.")]
        public static string Fire1 => "Fire1";
    }
}
