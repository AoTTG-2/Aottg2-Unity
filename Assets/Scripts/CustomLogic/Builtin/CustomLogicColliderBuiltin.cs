using Map;
using NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Collider", Abstract = true)]
    partial class CustomLogicColliderBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collider collider;

        public CustomLogicColliderBuiltin() { }
        public CustomLogicColliderBuiltin(object[] parameters)
        {
            collider = (Collider)parameters[0];
        }

        [CLProperty(Description = "")]
        public CustomLogicTransformBuiltin AttachedArticulationBody => new CustomLogicTransformBuiltin(collider.attachedRigidbody.transform);

        /*[CLProperty(Description = "")]
        public CustomLogicRigidbodyBuiltin AttachedRigidbody {
            get => new CustomLogicRigidbodyBuiltin(collider.attachedRigidbody);
            set => collider.attachedRigidbody = value.Value;
        }*/

        /*[CLProperty(Description = "")]
        public CustomLogicBoundsBuiltin Bounds {
            get {
                return new CustomLogicBoundsBuiltin(collider.bounds);
            }
        }*/

        [CLProperty(Description = "")]
        public float ContactOffset
        {
            get => collider.contactOffset;
            set => collider.contactOffset = value;
        }

        [CLProperty(Description = "")]
        public bool Enabled
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        [CLProperty(Description = "")]
        public int ExludeLayers
        {
            get => collider.gameObject.layer;
            set => collider.gameObject.layer = value;
        }

        [CLProperty(Description = "")]
        public int includeLayers
        {
            get => collider.gameObject.layer;
            set => collider.gameObject.layer = value;
        }

        [CLProperty(Description = "")]
        public bool IsTrigger
        {
            get => collider.isTrigger;
            set => collider.isTrigger = value;
        }

        /*[CLProperty(Description = "")]
        public CustomLogicPhysicMaterialBuiltin Material
        {
            get => new CustomLogicPhysicMaterialBuiltin(collider.material);
            set => collider.material = value.Value;
        }*/

        [CLProperty(Description = "")]
        public CustomLogicVector3Builtin Center => new CustomLogicVector3Builtin(collider.bounds.center);

        /*[CLProperty(Description = "")]
        public CustomLogicMaterialBuiltin Material => new CustomLogicMaterialBuiltin(collider.material);*/

        /*[CLProperty(Description = "")]
        public CustomLogicMaterialBuiltin SharedMaterial => new CustomLogicMaterialBuiltin(collider.sharedMaterial);*/

        [CLProperty(Description = "")]
        public bool ProvidesContacts
        {
            get => collider.providesContacts;
            set => collider.providesContacts = value;
        }

        [CLProperty(Description = "")]
        public string MaterialName => collider.material.name;

        [CLProperty(Description = "")]
        public string SharedMaterialName => collider.sharedMaterial.name;

        [CLProperty(Description = "")]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(collider.transform);

        [CLProperty(Description = "")]
        public CustomLogicTransformBuiltin GameObjectTransform => new CustomLogicTransformBuiltin(collider.gameObject.transform);


        // Methods
        [CLMethod(Description = "")]
        public CustomLogicVector3Builtin ClosestPoint(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPoint(position.Value);
        }

        [CLMethod(Description = "")]
        public CustomLogicVector3Builtin ClosestPointOnBounds(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPointOnBounds(position.Value);
        }

        [CLMethod(Description = "")]
        public CustomLogicLineCastHitResultBuiltin Raycast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, float maxDistance, string collideWith)
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

        public BuiltinClassInstance Copy()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        public object __Copy__()
        {
            throw new NotImplementedException();
        }

        public bool __Eq__(object self, object other)
        {
            throw new NotImplementedException();
        }

        public int __Hash__()
        {
            throw new NotImplementedException();
        }
    }
}
