using UnityEngine;
using UnityEngine.AI;

namespace CustomLogic
{
    /// <summary>
    /// Represents a NavMesh obstacle component that can carve or block navigation mesh paths.
    /// </summary>
    [CLType(Name = "NavmeshObstacleBuiltin", Static = true, Abstract = true, IsComponent = true)]
    partial class CustomLogicNavmeshObstacleBuiltin : BuiltinComponentInstance
    {
        public NavMeshObstacle Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;
        [CLConstructor]
        public CustomLogicNavmeshObstacleBuiltin() : base(null) { }
        public CustomLogicNavmeshObstacleBuiltin(CustomLogicMapObjectBuiltin owner) : base(GetOrAddComponent<NavMeshObstacle>(owner.Value.GameObject))
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (NavMeshObstacle)Component;
        }

        // Expose static properties for each NavMeshObstacleShape
        /// <summary>
        /// The NavMeshObstacleShape Box.
        /// </summary>
        [CLProperty]
        public static int ShapeBox => (int)NavMeshObstacleShape.Box;

        /// <summary>
        /// The NavMeshObstacleShape Capsule.
        /// </summary>
        [CLProperty]
        public static int ShapeCapsule => (int)NavMeshObstacleShape.Capsule;

        /// <summary>
        /// The radius of the obstacle.
        /// </summary>
        [CLProperty]
        public float Radius
        {
            get => Value.radius;
            set => Value.radius = value;
        }

        /// <summary>
        /// The height of the obstacle.
        /// </summary>
        [CLProperty]
        public float Height
        {
            get => Value.height;
            set => Value.height = value;
        }

        /// <summary>
        /// The scale of the obstacle.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Scale
        {
            get => new CustomLogicVector3Builtin(Value.size);
            set => Value.size = value.Value;
        }

        /// <summary>
        /// The center of the obstacle.
        /// </summary>
        [CLProperty]
        public Vector3 Center
        {
            get => Value.center;
            set => Value.center = value;
        }

        // carving
        /// <summary>
        /// Whether the obstacle carves the NavMesh.
        /// </summary>
        [CLProperty]
        public bool Carving
        {
            get => Value.carving;
            set => Value.carving = value;
        }

        /// <summary>
        /// Whether the obstacle only carves when stationary.
        /// </summary>
        [CLProperty]
        public bool CarveOnlyStationary
        {
            get => Value.carveOnlyStationary;
            set => Value.carveOnlyStationary = value;
        }

        // Shape
        /// <summary>
        /// The shape of the obstacle.
        /// </summary>
        [CLProperty]
        public int Shape
        {
            get => (int)Value.shape;
            set => Value.shape = (NavMeshObstacleShape)value;
        }

        /// <summary>
        /// Auto scales the obstacle to fit the colliders.
        /// </summary>
        [CLMethod]
        public void AutoScale()
        {
            Bounds bounds = OwnerMapObject.Value.colliderCache[0].bounds;
            foreach (var collider in OwnerMapObject.Value.colliderCache)
            {
                bounds.Encapsulate(collider.bounds);
            }
            Value.shape = NavMeshObstacleShape.Box;
            Value.size = bounds.size;
            Value.center = bounds.center;
        }
    }
}
