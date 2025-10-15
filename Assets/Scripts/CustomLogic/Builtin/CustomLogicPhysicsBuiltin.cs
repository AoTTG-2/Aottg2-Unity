using Map;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Static Physics class. Contains some common physics functions
    /// </summary>
    /// <code>
    /// start = Vector3(0);
    /// end = Vector3(10);
    /// # Options: All, MapObjects, Characters, Titans, Humans, Projectiles, Entities, Hitboxes, MapEditor
    /// result = Physics.LineCast(start, end, "Entities");
    /// Game.Print(result.IsCharacter);
    /// Game.Print(result.IsMapObject);
    /// Game.Print(result.Point);
    /// Game.Print(result.Normal);
    /// Game.Print(result.Distance);
    /// Game.Print(result.Collider);
    /// </code>
    [CLType(Name = "Physics", Static = true, Abstract = true)]
    partial class CustomLogicPhysicsBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPhysicsBuiltin()
        {
        }

        [CLProperty("All Collide Mode")]
        public static string CollideWithAll => MapObjectCollideWith.All;

        [CLProperty("MapObject Collide Mode")]
        public static string CollideWithMapObjects => MapObjectCollideWith.MapObjects;

        [CLProperty("Characters Collide Mode")]
        public static string CollideWithCharacters => MapObjectCollideWith.Characters;

        [CLProperty("Titans Collide Mode")]
        public static string CollideWithTitans => MapObjectCollideWith.Titans;

        [CLProperty("Humans Collide Mode")]
        public static string CollideWithHumans => MapObjectCollideWith.Humans;

        [CLProperty("Projectiles Collide Mode")]
        public static string CollideWithProjectiles => MapObjectCollideWith.Projectiles;

        [CLProperty("Entities Collide Mode")]
        public static string CollideWithEntities => MapObjectCollideWith.Entities;

        [CLProperty("Hitboxes Collide Mode")]
        public static string CollideWithHitboxes => MapObjectCollideWith.Hitboxes;

        [CLProperty("MapEditor Collide Mode")]
        public static string CollideWithMapEditor => MapObjectCollideWith.MapEditor;

        [CLMethod("Performs a line cast between two points, returns a LineCastHitResult object")]
        public static CustomLogicLineCastHitResultBuiltin LineCast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, string collideWith)
        {
            RaycastHit hit;
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            if (Physics.Linecast(startPosition, endPosition, out hit, PhysicsLayer.CopyMask(layer).value))
            {
                var collider = CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                if (collider != null)
                {
                    return new CustomLogicLineCastHitResultBuiltin
                    {
                        IsCharacter = collider != null && collider is CustomLogicCharacterBuiltin,
                        IsMapObject = collider != null && collider is CustomLogicMapObjectBuiltin,
                        Point = new CustomLogicVector3Builtin(hit.point),
                        Normal = new CustomLogicVector3Builtin(hit.normal),
                        Distance = hit.distance,
                        Collider = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    };
                }
            }
            return null;
        }

        [CLMethod("Performs a line cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin LineCastAll(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, string collideWith)
        {
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            Vector3 diff = (endPosition - startPosition);
            var hits = Physics.RaycastAll(startPosition, diff.normalized, diff.magnitude, PhysicsLayer.CopyMask(layer).value);

            CustomLogicListBuiltin results = new CustomLogicListBuiltin();

            foreach (var hit in hits)
            {
                var collider = CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                if (collider != null)
                {
                    results.List.Add(new CustomLogicLineCastHitResultBuiltin
                    {
                        IsCharacter = collider != null && collider is CustomLogicCharacterBuiltin,
                        IsMapObject = collider != null && collider is CustomLogicMapObjectBuiltin,
                        Point = new CustomLogicVector3Builtin(hit.point),
                        Normal = new CustomLogicVector3Builtin(hit.normal),
                        Distance = hit.distance,
                        Collider = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).")]
        public static object SphereCast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, float radius, string collideWith)
        {
            RaycastHit hit;
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            var diff = (endPosition - startPosition);
            if (Physics.SphereCast(startPosition, radius, diff.normalized, out hit, diff.magnitude, PhysicsLayer.CopyMask(layer).value))
            {
                return CustomLogicCollisionHandler.GetBuiltin(hit.collider);
            }
            return null;
        }

        [CLMethod("Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin SphereCastAll(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, float radius, string collideWith)
        {
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            Vector3 diff = (endPosition - startPosition);
            var hits = Physics.SphereCastAll(startPosition, radius, diff.normalized, diff.magnitude, PhysicsLayer.CopyMask(layer).value);

            CustomLogicListBuiltin results = new CustomLogicListBuiltin();

            foreach (var hit in hits)
            {
                var collider = CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                if (collider != null)
                {
                    results.List.Add(new CustomLogicLineCastHitResultBuiltin
                    {
                        IsCharacter = collider != null && collider is CustomLogicCharacterBuiltin,
                        IsMapObject = collider != null && collider is CustomLogicMapObjectBuiltin,
                        Point = new CustomLogicVector3Builtin(hit.point),
                        Normal = new CustomLogicVector3Builtin(hit.normal),
                        Distance = hit.distance,
                        Collider = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Performs a box cast between two points, returns the object hit (Human, Titan, etc...).")]
        public static object BoxCast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, CustomLogicVector3Builtin dimensions, CustomLogicQuaternionBuiltin orientation, string collideWith)
        {
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            var diff = (endPosition - startPosition);
            var halfExtents = dimensions.Value / 2;
            if (Physics.BoxCast(startPosition, halfExtents, diff.normalized, out RaycastHit hit, orientation.Value, diff.magnitude, PhysicsLayer.CopyMask(layer).value))
            {
                return CustomLogicCollisionHandler.GetBuiltin(hit.collider);
            }
            return null;
        }

        [CLMethod("Performs a box cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin BoxCastAll(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, CustomLogicVector3Builtin dimensions, CustomLogicQuaternionBuiltin orientation, string collideWith)
        {
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            Vector3 diff = endPosition - startPosition;
            var halfExtents = dimensions.Value / 2;
            var hits = Physics.BoxCastAll(startPosition, halfExtents, diff.normalized, orientation.Value, diff.magnitude, PhysicsLayer.CopyMask(layer).value);
            CustomLogicListBuiltin results = new CustomLogicListBuiltin();

            foreach (var hit in hits)
            {
                var collider = CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                if (collider != null)
                {
                    results.List.Add(new CustomLogicLineCastHitResultBuiltin
                    {
                        IsCharacter = collider != null && collider is CustomLogicCharacterBuiltin,
                        IsMapObject = collider != null && collider is CustomLogicMapObjectBuiltin,
                        Point = new CustomLogicVector3Builtin(hit.point),
                        Normal = new CustomLogicVector3Builtin(hit.normal),
                        Distance = hit.distance,
                        Collider = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Returns a point on the given collider that is closest to the specified location.")]
        public static CustomLogicVector3Builtin ClosestPoint(CustomLogicVector3Builtin point, CustomLogicColliderBuiltin collider, CustomLogicVector3Builtin position, CustomLogicQuaternionBuiltin rotation)
        {
            return new CustomLogicVector3Builtin(Physics.ClosestPoint(point.Value, collider.collider, position.Value, rotation.Value));
        }

        [CLMethod("Compute the minimal translation required to separate the given colliders apart at specified poses.")]
        public static CustomLogicVector3Builtin ComputePenetration(CustomLogicColliderBuiltin colliderA, CustomLogicVector3Builtin positionA, CustomLogicQuaternionBuiltin rotationA, CustomLogicColliderBuiltin colliderB, CustomLogicVector3Builtin positionB, CustomLogicQuaternionBuiltin rotationB)
        {

            bool intersected = Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
            return new CustomLogicVector3Builtin(direction * distance);
        }

        [CLMethod("Check if the the given colliders at specified poses are apart or overlapping.")]
        public static bool AreCollidersOverlapping(CustomLogicColliderBuiltin colliderA, CustomLogicVector3Builtin positionA, CustomLogicQuaternionBuiltin rotationA, CustomLogicColliderBuiltin colliderB, CustomLogicVector3Builtin positionB, CustomLogicQuaternionBuiltin rotationB)
        {

            return Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
        }

    }
}
