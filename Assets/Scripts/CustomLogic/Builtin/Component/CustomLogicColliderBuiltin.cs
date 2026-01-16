using Map;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Represents a Collider component for detecting collisions and managing collider properties.
    /// </summary>
    [CLType(Name = "Collider", Abstract = true, IsComponent = true)]
    partial class CustomLogicColliderBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        public Collider collider;

        public CustomLogicColliderBuiltin() { }
        public CustomLogicColliderBuiltin(object[] parameters)
        {
            collider = (Collider)parameters[0];
        }

        /// <summary>
        /// The transform of the rigidbody this collider is attached to.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin AttachedArticulationBody => new CustomLogicTransformBuiltin(collider.attachedRigidbody.transform);


        /// <summary>
        /// The contact offset used by the collider to avoid tunneling.
        /// </summary>
        [CLProperty]
        public float ContactOffset
        {
            get => collider.contactOffset;
            set => collider.contactOffset = value;
        }

        /// <summary>
        /// Whether the collider is enabled.
        /// </summary>
        [CLProperty]
        new public bool Enabled
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        /// <summary>
        /// The layers that this Collider should exclude when deciding if the Collider can contact another Collider.
        /// </summary>
        [CLProperty]
        public int ExcludeLayers
        {
            get => collider.excludeLayers;
            set => collider.excludeLayers = value;
        }

        /// <summary>
        /// The additional layers that this Collider should include when deciding if the Collider can contact another Collider.
        /// </summary>
        [CLProperty]
        public int IncludeLayers
        {
            get => collider.includeLayers;
            set => collider.includeLayers = value;
        }

        /// <summary>
        /// Whether the collider is a trigger. Triggers don't cause physical collisions.
        /// </summary>
        [CLProperty]
        public bool IsTrigger
        {
            get => collider.isTrigger;
            set => collider.isTrigger = value;
        }

        /// <summary>
        /// The center of the collider's bounding box in world space.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Center => new CustomLogicVector3Builtin(collider.bounds.center);

        /// <summary>
        /// Whether the collider provides contact information.
        /// </summary>
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


        /// <summary>
        /// Gets the closest point on the collider to the given position.
        /// </summary>
        /// <param name="position">The position to find the closest point to.</param>
        /// <returns>The closest point on the collider.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin ClosestPoint(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPoint(position.Value);
        }

        /// <summary>
        /// Gets the closest point on the collider's bounding box to the given position.
        /// </summary>
        /// <param name="position">The position to find the closest point on bounds to.</param>
        /// <returns>The closest point on the bounding box.</returns>
        [CLMethod]
        public CustomLogicVector3Builtin ClosestPointOnBounds(CustomLogicVector3Builtin position)
        {
            return collider.ClosestPointOnBounds(position.Value);
        }

        /// <summary>
        /// Runs a raycast physics check between start to end and checks if it hits any collider with the given collideWith layer.
        /// </summary>
        /// <param name="start">The start position of the raycast.</param>
        /// <param name="end">The end position of the raycast.</param>
        /// <param name="collideWith">The layer name to check collisions with.</param>
        /// <returns>A LineCastHitResult if it hit something, otherwise returns null.</returns>
        [CLMethod]
        public CustomLogicLineCastHitResultBuiltin Raycast(
            CustomLogicVector3Builtin start,
            CustomLogicVector3Builtin end,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))] string collideWith)
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

        /// <summary>
        /// Creates a copy of this collider.
        /// </summary>
        /// <returns>A new Collider with the same values.</returns>
        [CLMethod]
        public object __Copy__()
        {
            return new CustomLogicColliderBuiltin(new object[] { collider });
        }

        /// <summary>
        /// Checks if two colliders are equal.
        /// </summary>
        /// <param name="self">The first collider.</param>
        /// <param name="other">The second collider.</param>
        /// <returns>True if the colliders are equal, false otherwise.</returns>
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

        /// <summary>
        /// Gets the hash code of the collider.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__()
        {
            return collider == null ? 0 : collider.GetHashCode();
        }
    }
}
