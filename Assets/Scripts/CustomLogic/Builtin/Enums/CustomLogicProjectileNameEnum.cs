namespace CustomLogic
{
    [CLType(Name = "ProjectileNameEnum", Static = true, Abstract = true, Description = "Enumeration of projectile names that can be spawned.")]
    partial class CustomLogicProjectileNameEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicProjectileNameEnum() { }

        [CLProperty("Thunderspear projectile.")]
        public static string Thunderspear => "Thunderspear";

        [CLProperty("CannonBall projectile.")]
        public static string CannonBall => "CannonBall";

        [CLProperty("Flare projectile.")]
        public static string Flare => "Flare";

        [CLProperty("BladeThrow projectile.")]
        public static string BladeThrow => "BladeThrow";

        [CLProperty("SmokeBomb projectile.")]
        public static string SmokeBomb => "SmokeBomb";

        [CLProperty("Rock1 projectile.")]
        public static string Rock1 => "Rock1";
    }
}
