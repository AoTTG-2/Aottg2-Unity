using ApplicationManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Physics", Static = true)]
    partial class CustomLogicPhysicsBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicPhysicsBuiltin() : base("Physics")
        {
        }

        [CLMethod("Performs a line cast between two points.")]
        public static object LineCast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, string collideWith)
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

        [CLMethod("Performs a sphere cast between two points.")]
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
    }
}
