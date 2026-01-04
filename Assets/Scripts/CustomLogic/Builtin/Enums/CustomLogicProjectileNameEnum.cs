namespace CustomLogic
{
    /// <summary>
    /// Enumeration of projectile names that can be spawned.
    /// </summary>
    [CLType(Name = "ProjectileNameEnum", Static = true, Abstract = true)]
    partial class CustomLogicProjectileNameEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicProjectileNameEnum() { }

        /// <summary>
        /// Thunderspear projectile.
        /// </summary>
        [CLProperty]
        public static string Thunderspear => "Thunderspear";

        /// <summary>
        /// CannonBall projectile.
        /// </summary>
        [CLProperty]
        public static string CannonBall => "CannonBall";

        /// <summary>
        /// Flare projectile.
        /// </summary>
        [CLProperty]
        public static string Flare => "Flare";

        /// <summary>
        /// BladeThrow projectile.
        /// </summary>
        [CLProperty]
        public static string BladeThrow => "BladeThrow";

        /// <summary>
        /// SmokeBomb projectile.
        /// </summary>
        [CLProperty]
        public static string SmokeBomb => "SmokeBomb";

        /// <summary>
        /// Rock1 projectile.
        /// </summary>
        [CLProperty]
        public static string Rock1 => "Rock1";
    }
}
