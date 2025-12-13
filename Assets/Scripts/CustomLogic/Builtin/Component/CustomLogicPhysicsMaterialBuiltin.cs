using Map;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "PhysicsMaterialBuiltin", Static = true, Abstract = true, Description = "", IsComponent = true)]
    partial class CustomLogicPhysicsMaterialBuiltin : BuiltinComponentInstance
    {
        public CustomPhysicsMaterial Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;

        [CLConstructor]
        public CustomLogicPhysicsMaterialBuiltin() : base(null) { }

        public CustomLogicPhysicsMaterialBuiltin(CustomLogicMapObjectBuiltin owner) : base(GetOrAddComponent<CustomPhysicsMaterial>(owner.Value.GameObject))
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (CustomPhysicsMaterial)Component;
        }

        [CLProperty(Description = "PhysicMaterialCombine.Minimum")]
        public static int FrictionCombineMinimum => (int)PhysicMaterialCombine.Minimum;

        [CLProperty(Description = "PhysicMaterialCombine.Multiply")]
        public static int FrictionCombineMultiply => (int)PhysicMaterialCombine.Multiply;

        [CLProperty(Description = "PhysicMaterialCombine.Maximum")]
        public static int FrictionCombineMaximum => (int)PhysicMaterialCombine.Maximum;

        [CLProperty(Description = "PhysicMaterialCombine.Average")]
        public static int FrictionCombineAverage => (int)PhysicMaterialCombine.Average;

        // Static Friction
        [CLProperty(Description = "The static friction of the material.")]
        public float StaticFriction
        {
            get => Value.StaticFriction;
            set => Value.StaticFriction = value;
        }

        // Dynamic Friction
        [CLProperty(Description = "The dynamic friction of the material.")]
        public float DynamicFriction
        {
            get => Value.DynamicFriction;
            set => Value.DynamicFriction = value;
        }

        // Bounciness
        [CLProperty(Description = "The bounciness of the material.")]
        public float Bounciness
        {
            get => Value.Bounciness;
            set => Value.Bounciness = value;
        }

        // Combine Mode
        [CLProperty(Description = "The friction combine mode of the material.")]
        public int FrictionCombine
        {
            get => (int)Value.FrictionCombine;
            set => Value.FrictionCombine = (PhysicMaterialCombine)value;
        }

        // Bounce Combine Mode
        [CLProperty(Description = "The bounce combine mode of the material.")]
        public int BounceCombine
        {
            get => (int)Value.BounceCombine;
            set => Value.BounceCombine = (PhysicMaterialCombine)value;
        }


        [CLMethod(description: "Setup the material.")]
        public void Setup(bool allChildColliders)
        {
            Value.Setup(allChildColliders);
        }
    }
}
