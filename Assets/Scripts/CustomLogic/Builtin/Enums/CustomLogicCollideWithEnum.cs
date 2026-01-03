using Map;

namespace CustomLogic
{
    [CLType(Name = "CollideWithEnum", Static = true, Abstract = true, Description = "Enumeration of collision layers for physics queries and colliders.")]
    partial class CustomLogicCollideWithEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicCollideWithEnum() { }

        [CLProperty("Collide with all layers.")]
        public static string All => MapObjectCollideWith.All;

        [CLProperty("Collide with map objects.")]
        public static string MapObjects => MapObjectCollideWith.MapObjects;

        [CLProperty("Collide with characters.")]
        public static string Characters => MapObjectCollideWith.Characters;

        [CLProperty("Collide with titans.")]
        public static string Titans => MapObjectCollideWith.Titans;

        [CLProperty("Collide with humans.")]
        public static string Humans => MapObjectCollideWith.Humans;

        [CLProperty("Collide with projectiles.")]
        public static string Projectiles => MapObjectCollideWith.Projectiles;

        [CLProperty("Collide with entities.")]
        public static string Entities => MapObjectCollideWith.Entities;

        [CLProperty("Collide with hitboxes.")]
        public static string Hitboxes => MapObjectCollideWith.Hitboxes;

        [CLProperty("Collide with map editor objects.")]
        public static string MapEditor => MapObjectCollideWith.MapEditor;
    }
}
