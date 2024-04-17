using ApplicationManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicPhysicsBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicPhysicsBuiltin(): base("Physics")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "LineCast")
            {
                RaycastHit hit;
                var start = ((CustomLogicVector3Builtin)parameters[0]).Value;
                var end = ((CustomLogicVector3Builtin)parameters[1]).Value;
                string collideWith = (string)parameters[2];
                int layer = MapLoader.GetColliderLayer(collideWith);
                if (Physics.Linecast(start, end, out hit, PhysicsLayer.CopyMask(layer).value))
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
                            Collider = collider
                        };
                    }
                }
                return null;
            }
            if (name == "SphereCast")
            {
                RaycastHit hit;
                var start = ((CustomLogicVector3Builtin)parameters[0]).Value;
                var end = ((CustomLogicVector3Builtin)parameters[1]).Value;
                var radius = (float)parameters[2];
                string collideWith = (string)parameters[3];
                int layer = MapLoader.GetColliderLayer(collideWith);
                var diff = (end - start);
                if (Physics.SphereCast(start, radius, diff.normalized, out hit, diff.magnitude, PhysicsLayer.CopyMask(layer).value))
                {
                    return CustomLogicCollisionHandler.GetBuiltin(hit.collider);
                }
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            return base.GetField(name);
        }
    }
}
