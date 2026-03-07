using Projectiles;

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
        public static string Thunderspear => ProjectilePrefabs.Thunderspear;

        /// <summary>
        /// CannonBall projectile.
        /// </summary>
        [CLProperty]
        public static string CannonBall => ProjectilePrefabs.CannonBall;

        /// <summary>
        /// Flare projectile.
        /// </summary>
        [CLProperty]
        public static string Flare => ProjectilePrefabs.Flare;

        /// <summary>
        /// BladeThrow projectile.
        /// </summary>
        [CLProperty]
        public static string BladeThrow => ProjectilePrefabs.BladeThrow;

        /// <summary>
        /// SmokeBomb projectile.
        /// </summary>
        [CLProperty]
        public static string SmokeBomb => ProjectilePrefabs.SmokeBomb;

        /// <summary>
        /// Rock1 projectile.
        /// </summary>
        [CLProperty]
        public static string Rock1 => ProjectilePrefabs.Rock1;

        /// <summary>
        /// Rock2 projectile.
        /// </summary>
        [CLProperty]
        public static string Rock2 => ProjectilePrefabs.Rock2;
    }
}
