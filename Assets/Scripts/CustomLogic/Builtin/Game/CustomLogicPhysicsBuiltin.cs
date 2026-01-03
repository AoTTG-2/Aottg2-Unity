using Map;
using UnityEngine;
using Utility;

namespace CustomLogic
{
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
    [CLType(Name = "Physics", Static = true, Abstract = true, Description = "Static Physics class. Contains some common physics functions.")]
    partial class CustomLogicPhysicsBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPhysicsBuiltin(){}

        [CLMethod("Performs a line cast between two points, returns a LineCastHitResult object.")]
        public static CustomLogicLineCastHitResultBuiltin LineCast(
            [CLParam("The start position of the line cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the line cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
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
                        Collider = collider,
                        ColliderInfo = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    };
                }
            }
            return null;
        }

        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" }, Description = "Performs a line cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin LineCastAll(
            [CLParam("The start position of the line cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the line cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
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
                        Collider = collider,
                        ColliderInfo = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).")]
        public static object SphereCast(
            [CLParam("The start position of the sphere cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the sphere cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The radius of the sphere.")]
            float radius,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
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

        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" }, Description = "Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin SphereCastAll(
            [CLParam("The start position of the sphere cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the sphere cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The radius of the sphere.")]
            float radius,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
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
                        Collider = collider,
                        ColliderInfo = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Performs a box cast between two points, returns the object hit (Human, Titan, etc...).")]
        public static object BoxCast(
            [CLParam("The start position of the box cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the box cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The dimensions of the box.")]
            CustomLogicVector3Builtin dimensions,
            [CLParam("The orientation of the box.")]
            CustomLogicQuaternionBuiltin orientation,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
        {
            var startPosition = start.Value;
            var endPosition = end.Value;
            int layer = MapLoader.GetColliderLayer(collideWith);
            var diff = endPosition - startPosition;
            var halfExtents = dimensions.Value / 2;
            if (Physics.BoxCast(startPosition, halfExtents, diff.normalized, out RaycastHit hit, orientation.Value, diff.magnitude, PhysicsLayer.CopyMask(layer).value))
            {
                return CustomLogicCollisionHandler.GetBuiltin(hit.collider);
            }
            return null;
        }

        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" }, Description = "Performs a box cast between two points and returns a LineCastHitResult object for each element hit.")]
        public static CustomLogicListBuiltin BoxCastAll(
            [CLParam("The start position of the box cast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the box cast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The dimensions of the box.")]
            CustomLogicVector3Builtin dimensions,
            [CLParam("The orientation of the box.")]
            CustomLogicQuaternionBuiltin orientation,
            [CLParam("The collision layer to check against.", Enum = typeof(CustomLogicCollideWithEnum))]
            string collideWith)
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
                        Collider = collider,
                        ColliderInfo = new CustomLogicColliderBuiltin(new object[] { hit.collider })
                    });
                }
            }
            return results;
        }

        [CLMethod("Returns a point on the given collider that is closest to the specified location.")]
        public static CustomLogicVector3Builtin ClosestPoint(
            [CLParam("The point to find the closest point to.")]
            CustomLogicVector3Builtin point,
            [CLParam("The collider to check.")]
            CustomLogicColliderBuiltin collider,
            [CLParam("The position of the collider.")]
            CustomLogicVector3Builtin position,
            [CLParam("The rotation of the collider.")]
            CustomLogicQuaternionBuiltin rotation)
        {
            return new CustomLogicVector3Builtin(Physics.ClosestPoint(point.Value, collider.collider, position.Value, rotation.Value));
        }

        [CLMethod("Compute the minimal translation required to separate the given colliders apart at specified poses. Returns Vector3.Zero if the colliders are not intersecting.")]
        public static CustomLogicVector3Builtin ComputePenetration(
            [CLParam("The first collider.")]
            CustomLogicColliderBuiltin colliderA,
            [CLParam("The position of the first collider.")]
            CustomLogicVector3Builtin positionA,
            [CLParam("The rotation of the first collider.")]
            CustomLogicQuaternionBuiltin rotationA,
            [CLParam("The second collider.")]
            CustomLogicColliderBuiltin colliderB,
            [CLParam("The position of the second collider.")]
            CustomLogicVector3Builtin positionB,
            [CLParam("The rotation of the second collider.")]
            CustomLogicQuaternionBuiltin rotationB)
        {
            bool intersected = Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
            if (!intersected)
                return CustomLogicVector3Builtin.Zero;
            return new CustomLogicVector3Builtin(direction * distance);
        }

        [CLMethod("Check if the the given colliders at specified poses are apart or overlapping.")]
        public static bool AreCollidersOverlapping(
            [CLParam("The first collider.")]
            CustomLogicColliderBuiltin colliderA,
            [CLParam("The position of the first collider.")]
            CustomLogicVector3Builtin positionA,
            [CLParam("The rotation of the first collider.")]
            CustomLogicQuaternionBuiltin rotationA,
            [CLParam("The second collider.")]
            CustomLogicColliderBuiltin colliderB,
            [CLParam("The position of the second collider.")]
            CustomLogicVector3Builtin positionB,
            [CLParam("The rotation of the second collider.")]
            CustomLogicQuaternionBuiltin rotationB)
        {
            return Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
        }
    }
}
