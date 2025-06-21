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
        [CLProperty(Description = "The center of the obstacle.")]
        public Vector3 Center
        {
            get => Value.center;
            set => Value.center = value;
        }
    }
}
