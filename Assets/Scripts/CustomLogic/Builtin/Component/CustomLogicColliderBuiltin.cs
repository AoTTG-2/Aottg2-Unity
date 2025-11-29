using Map;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Collider", Abstract = true, IsComponent = true)]
    partial class CustomLogicColliderBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collider collider;

        public CustomLogicColliderBuiltin() { }
        public CustomLogicColliderBuiltin(object[] parameters)
        {
            collider = (Collider)parameters[0];
        }

        [CLProperty(Description = "The transform of the rigidbody this collider is attached to.")]
        public CustomLogicTransformBuiltin AttachedArticulationBody => new CustomLogicTransformBuiltin(collider.attachedRigidbody.transform);


        /// <inheritdoc cref="Collider.contactOffset"/>
        [CLProperty]
        public float ContactOffset
        {
            get => collider.contactOffset;
            set => collider.contactOffset = value;
        }

        /// <inheritdoc cref="Collider.enabled"/>
        [CLProperty]
        new public bool Enabled
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        /// <inheritdoc cref="Collider.excludeLayers"/>
        [CLProperty]
        public int ExludeLayers
        {
            get => collider.excludeLayers;
            set => collider.excludeLayers = value;
        }

        /// <inheritdoc cref="Collider.includeLayers"/>
        [CLProperty(Description = "The additional layers that this Collider should include when deciding if the Collider can contact another Collider.")]
        public int IncludeLayers
        {
            get => collider.includeLayers;
            set => collider.includeLayers = value;
        }

        /// <inheritdoc cref="Collider.isTrigger"/>
        [CLProperty]
        public bool IsTrigger
        {
            get => collider.isTrigger;
            set => collider.isTrigger = value;
        }

        /// <inheritdoc cref="Bounds.center"/>
        [CLProperty]
        public CustomLogicVector3Builtin Center => new CustomLogicVector3Builtin(collider.bounds.center);

        /// <inheritdoc cref="Collider.providesContacts"/>
        [CLProperty]
        public bool ProvidesContacts
        {
            get => collider.providesContacts;
            set => collider.providesContacts = value;
        }

        /// <summary>
        /// The name of the physics material on the collider.
        /// </summary>
        [CLProperty]
        public string MaterialName => collider.material.name;

        /// <summary>
        /// The name of the shared physics material on this collider.
        /// </summary>
        [CLProperty]
        public string SharedMaterialName => collider.sharedMaterial.name;

        /// <summary>
        /// The collider's transform.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(collider.transform);

        /// <summary>
        /// The transform of the gameobject this collider is attached to.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin GameObjectTransform => new CustomLogicTransformBuiltin(collider.gameObject.transform);


        // Methods
        /// <inheritdoc cref="Collider.ClosestPoint(Vector3)"/>
        [CLMethod]
        public CustomLogicVector3Builtin ClosestPoint(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPoint(position.Value);
        }

        /// <inheritdoc cref="Collider.ClosestPointOnBounds(Vector3)"/>
        [CLMethod]
        public CustomLogicVector3Builtin ClosestPointOnBounds(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPointOnBounds(position.Value);
        }

        /// <summary>
        /// Runs a raycast physics check between start to end and checks if it hits any collider with the given collideWith layer.
        /// Returns a Collider object if it hit something otherwise returns null.
        /// </summary>
        [CLMethod]
        public CustomLogicLineCastHitResultBuiltin Raycast(CustomLogicVector3Builtin start, CustomLogicVector3Builtin end, string collideWith)
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

        public BuiltinClassInstance Copy()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        [CLMethod]
        public object __Copy__()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColliderBuiltin selfCollider, CustomLogicColliderBuiltin otherCollider) =>
                    selfCollider.collider == otherCollider.collider,
                _ => false
            };
        }

        [CLMethod]
        public int __Hash__()
        {
            return collider == null ? 0 : collider.GetHashCode();
        }
    }
}
