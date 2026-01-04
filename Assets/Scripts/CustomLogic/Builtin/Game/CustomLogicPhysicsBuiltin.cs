using Map;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Static Physics class. Contains some common physics functions.
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
        public CustomLogicPhysicsBuiltin(){}

        /// <summary>
        /// Performs a line cast between two points.
        /// </summary>
        /// <param name="start">The start position of the line cast.</param>
        /// <param name="end">The end position of the line cast.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>A LineCastHitResult object.</returns>
        [CLMethod]
        public static CustomLogicLineCastHitResultBuiltin LineCast(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
        /// </summary>
        /// <param name="start">The start position of the line cast.</param>
        /// <param name="end">The end position of the line cast.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>A list of LineCastHitResult objects.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" })]
        public static CustomLogicListBuiltin LineCastAll(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Performs a sphere cast between two points.
        /// </summary>
        /// <param name="start">The start position of the sphere cast.</param>
        /// <param name="end">The end position of the sphere cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>The object hit (Human, Titan, etc...), or null if nothing was hit.</returns>
        [CLMethod]
        public static object SphereCast(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            float radius,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
        /// </summary>
        /// <param name="start">The start position of the sphere cast.</param>
        /// <param name="end">The end position of the sphere cast.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>A list of LineCastHitResult objects.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" })]
        public static CustomLogicListBuiltin SphereCastAll(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            float radius,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Performs a box cast between two points.
        /// </summary>
        /// <param name="start">The start position of the box cast.</param>
        /// <param name="end">The end position of the box cast.</param>
        /// <param name="dimensions">The dimensions of the box.</param>
        /// <param name="orientation">The orientation of the box.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>The object hit (Human, Titan, etc...), or null if nothing was hit.</returns>
        [CLMethod]
        public static object BoxCast(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            CustomLogicVector3Builtin dimensions,
            CustomLogicQuaternionBuiltin orientation,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Performs a box cast between two points and returns a LineCastHitResult object for each element hit.
        /// </summary>
        /// <param name="start">The start position of the box cast.</param>
        /// <param name="end">The end position of the box cast.</param>
        /// <param name="dimensions">The dimensions of the box.</param>
        /// <param name="orientation">The orientation of the box.</param>
        /// <param name="collideWith">The collision layer to check against.</param>
        /// <returns>A list of LineCastHitResult objects.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "LineCastHitResult" })]
        public static CustomLogicListBuiltin BoxCastAll(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            CustomLogicVector3Builtin dimensions,
            CustomLogicQuaternionBuiltin orientation,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))]
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

        /// <summary>
        /// Returns a point on the given collider that is closest to the specified location.
        /// </summary>
        /// <param name="point">The point to find the closest point to.</param>
        /// <param name="collider">The collider to check.</param>
        /// <param name="position">The position of the collider.</param>
        /// <param name="rotation">The rotation of the collider.</param>
        /// <returns>The closest point on the collider.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin ClosestPoint(
            CustomLogicVector3Builtin point,
            CustomLogicColliderBuiltin collider,
            CustomLogicVector3Builtin position,
            CustomLogicQuaternionBuiltin rotation)
        {
            return new CustomLogicVector3Builtin(Physics.ClosestPoint(point.Value, collider.collider, position.Value, rotation.Value));
        }

        /// <summary>
        /// Compute the minimal translation required to separate the given colliders apart at specified poses.
        /// </summary>
        /// <param name="colliderA">The first collider.</param>
        /// <param name="positionA">The position of the first collider.</param>
        /// <param name="rotationA">The rotation of the first collider.</param>
        /// <param name="colliderB">The second collider.</param>
        /// <param name="positionB">The position of the second collider.</param>
        /// <param name="rotationB">The rotation of the second collider.</param>
        /// <returns>Vector3.Zero if the colliders are not intersecting, otherwise the minimal translation vector.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin ComputePenetration(
            CustomLogicColliderBuiltin colliderA,
            CustomLogicVector3Builtin positionA,
            CustomLogicQuaternionBuiltin rotationA,
            CustomLogicColliderBuiltin colliderB,
            CustomLogicVector3Builtin positionB,
            CustomLogicQuaternionBuiltin rotationB)
        {
            bool intersected = Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
            if (!intersected)
                return CustomLogicVector3Builtin.Zero;
            return new CustomLogicVector3Builtin(direction * distance);
        }

        /// <summary>
        /// Check if the the given colliders at specified poses are apart or overlapping.
        /// </summary>
        /// <param name="colliderA">The first collider.</param>
        /// <param name="positionA">The position of the first collider.</param>
        /// <param name="rotationA">The rotation of the first collider.</param>
        /// <param name="colliderB">The second collider.</param>
        /// <param name="positionB">The position of the second collider.</param>
        /// <param name="rotationB">The rotation of the second collider.</param>
        /// <returns>True if the colliders are overlapping, false otherwise.</returns>
        [CLMethod]
        public static bool AreCollidersOverlapping(
            CustomLogicColliderBuiltin colliderA,
            CustomLogicVector3Builtin positionA,
            CustomLogicQuaternionBuiltin rotationA,
            CustomLogicColliderBuiltin colliderB,
            CustomLogicVector3Builtin positionB,
            CustomLogicQuaternionBuiltin rotationB)
        {
            return Physics.ComputePenetration(colliderA.collider, positionA.Value, rotationA.Value, colliderB.collider, positionB, rotationB, out Vector3 direction, out float distance);
        }
    }
}
