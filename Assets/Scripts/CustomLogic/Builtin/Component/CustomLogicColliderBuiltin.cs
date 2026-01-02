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
        [CLProperty(Description = "The contact offset used by the collider to avoid tunneling.")]
        public float ContactOffset
        {
            get => collider.contactOffset;
            set => collider.contactOffset = value;
        }

        /// <inheritdoc cref="Collider.enabled"/>
        [CLProperty(Description = "Whether the collider is enabled.")]
        new public bool Enabled
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        /// <inheritdoc cref="Collider.excludeLayers"/>
        [CLProperty(Description = "The layers that this Collider should exclude when deciding if the Collider can contact another Collider.")]
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
        [CLProperty(Description = "Whether the collider is a trigger. Triggers don't cause physical collisions.")]
        public bool IsTrigger
        {
            get => collider.isTrigger;
            set => collider.isTrigger = value;
        }

        /// <inheritdoc cref="Bounds.center"/>
        [CLProperty(Description = "The center of the collider's bounding box in world space.")]
        public CustomLogicVector3Builtin Center => new CustomLogicVector3Builtin(collider.bounds.center);

        /// <inheritdoc cref="Collider.providesContacts"/>
        [CLProperty(Description = "Whether the collider provides contact information.")]
        public bool ProvidesContacts
        {
            get => collider.providesContacts;
            set => collider.providesContacts = value;
        }

        [CLProperty(Description = "The name of the physics material on the collider.")]
        public string MaterialName => collider.material.name;

        [CLProperty(Description = "The name of the shared physics material on this collider.")]
        public string SharedMaterialName => collider.sharedMaterial.name;

        [CLProperty(Description = "The collider's transform.")]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(collider.transform);

        [CLProperty(Description = "The transform of the gameobject this collider is attached to.")]
        public CustomLogicTransformBuiltin GameObjectTransform => new CustomLogicTransformBuiltin(collider.gameObject.transform);


        // Methods
        [CLMethod("Gets the closest point on the collider to the given position. Returns: The closest point on the collider.")]
        public CustomLogicVector3Builtin ClosestPoint(
            [CLParam("The position to find the closest point to.")]
            CustomLogicVector3Builtin position)
        {
            return collider.ClosestPoint(position.Value);
        }

        [CLMethod("Gets the closest point on the collider's bounding box to the given position. Returns: The closest point on the bounding box.")]
        public CustomLogicVector3Builtin ClosestPointOnBounds(
            [CLParam("The position to find the closest point on bounds to.")]
            CustomLogicVector3Builtin position)
        {
            return collider.ClosestPointOnBounds(position.Value);
        }

        [CLMethod("Runs a raycast physics check between start to end and checks if it hits any collider with the given collideWith layer. Returns: A LineCastHitResult if it hit something, otherwise returns null.")]
        public CustomLogicLineCastHitResultBuiltin Raycast(
            [CLParam("The start position of the raycast.")]
            CustomLogicVector3Builtin start,
            [CLParam("The end position of the raycast.")]
            CustomLogicVector3Builtin end,
            [CLParam("The layer name to check collisions with.")]
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

        public BuiltinClassInstance Copy()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        [CLMethod("Creates a copy of this collider. Returns: A new Collider with the same values.")]
        public object __Copy__()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        [CLMethod("Checks if two colliders are equal. Returns: True if the colliders are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColliderBuiltin selfCollider, CustomLogicColliderBuiltin otherCollider) =>
                    selfCollider.collider == otherCollider.collider,
                _ => false
            };
        }

        [CLMethod("Gets the hash code of the collider. Returns: The hash code.")]
        public int __Hash__()
        {
            return collider == null ? 0 : collider.GetHashCode();
        }
    }
}
