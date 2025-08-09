using UnityEngine;
using UnityEngine.AI;

namespace CustomLogic
{
    [CLType(Name = "NavmeshObstacleBuiltin", Static = true, Abstract = true, Description = "")]
    partial class CustomLogicNavmeshObstacleBuiltin : BuiltinComponentInstance
    {
        public NavMeshObstacle Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;
        [CLConstructor]
        public CustomLogicNavmeshObstacleBuiltin() : base(null) { }
        public CustomLogicNavmeshObstacleBuiltin(CustomLogicMapObjectBuiltin owner) : base(owner.Value.GameObject.AddComponent<NavMeshObstacle>())
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (NavMeshObstacle)Component;
        }

        // Expose static properties for each NavMeshObstacleShape
        [CLProperty(Description = "The NavMeshObstacleShape Box.", Static = true)]
        public static int ShapeBox => (int)NavMeshObstacleShape.Box;

        [CLProperty(Description = "The NavMeshObstacleShape Capsule.", Static = true)]
        public static int ShapeCapsule => (int)NavMeshObstacleShape.Capsule;

        [CLProperty(Description = "The radius of the obstacle.")]
        public float Radius
        {
            get => Value.radius;
            set => Value.radius = value;
        }

        [CLProperty(Description = "The height of the obstacle.")]
        public float Height
        {
            get => Value.height;
            set => Value.height = value;
        }

        [CLProperty(Description = "The scale of the obstacle")]
        public CustomLogicVector3Builtin Scale
        {
            get => new CustomLogicVector3Builtin(Value.size);
            set => Value.size = value.Value;
        }

        [CLProperty(Description = "The center of the obstacle.")]
        public Vector3 Center
        {
            get => Value.center;
            set => Value.center = value;
        }

        // carving
        [CLProperty(Description = "Whether the obstacle carves the NavMesh.")]
        public bool Carving
        {
            get => Value.carving;
            set => Value.carving = value;
        }

        [CLProperty(Description = "Whether the obstacle only carves when stationary.")]
        public bool CarveOnlyStationary
        {
            get => Value.carveOnlyStationary;
            set => Value.carveOnlyStationary = value;
        }

        // Shape
        [CLProperty(Description = "The shape of the obstacle.")]
        public int Shape
        {
            get => (int)Value.shape;
            set => Value.shape = (NavMeshObstacleShape)value;
        }

        [CLMethod(Description = "Auto scales the obstacle to fit the colliders.")]
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
