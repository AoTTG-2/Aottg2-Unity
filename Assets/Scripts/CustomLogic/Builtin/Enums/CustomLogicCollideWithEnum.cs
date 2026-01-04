using Map;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of collision layers for physics queries and colliders.
    /// </summary>
    [CLType(Name = "CollideWithEnum", Static = true, Abstract = true)]
    partial class CustomLogicCollideWithEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollideWithEnum() { }

        /// <summary>
        /// Collide with all layers.
        /// </summary>
        [CLProperty]
        public static string All => MapObjectCollideWith.All;

        /// <summary>
        /// Collide with map objects.
        /// </summary>
        [CLProperty]
        public static string MapObjects => MapObjectCollideWith.MapObjects;

        /// <summary>
        /// Collide with characters.
        /// </summary>
        [CLProperty]
        public static string Characters => MapObjectCollideWith.Characters;

        /// <summary>
        /// Collide with titans.
        /// </summary>
        [CLProperty]
        public static string Titans => MapObjectCollideWith.Titans;

        /// <summary>
        /// Collide with humans.
        /// </summary>
        [CLProperty]
        public static string Humans => MapObjectCollideWith.Humans;

        /// <summary>
        /// Collide with projectiles.
        /// </summary>
        [CLProperty]
        public static string Projectiles => MapObjectCollideWith.Projectiles;

        /// <summary>
        /// Collide with entities.
        /// </summary>
        [CLProperty]
        public static string Entities => MapObjectCollideWith.Entities;

        /// <summary>
        /// Collide with hitboxes.
        /// </summary>
        [CLProperty]
        public static string Hitboxes => MapObjectCollideWith.Hitboxes;

        /// <summary>
        /// Collide with map editor objects.
        /// </summary>
        [CLProperty]
        public static string MapEditor => MapObjectCollideWith.MapEditor;
    }
}
