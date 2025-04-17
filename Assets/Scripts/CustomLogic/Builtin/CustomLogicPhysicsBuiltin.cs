using ApplicationManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Physics class for custom logic.
    /// </summary>
    /// <example>
    /// start = Vector3(0);
    /// end = Vector3(10);
    /// result = Physics.LineCast(start, end, "Entities");
    /// Game.Print(result.IsCharacter);
    /// Game.Print(result.IsMapObject);
    /// Game.Print(result.Point);
    /// Game.Print(result.Normal);
    /// Game.Print(result.Distance);
    /// Game.Print(result.Collider);
    /// </example>
    [CLType(Name = "Physics", Static = true, Abstract = true)]
    partial class CustomLogicPhysicsBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPhysicsBuiltin()
        {
        }


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
    }
}
