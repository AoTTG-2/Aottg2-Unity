using Map;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "PhysicsMaterialBuiltin", Static = true, Abstract = true, Description = "Represents a physics material that defines friction and bounciness properties for colliders.", IsComponent = true)]
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

        [CLProperty("PhysicMaterialCombine.Minimum.")]
        public static int FrictionCombineMinimum => (int)PhysicMaterialCombine.Minimum;

        [CLProperty("PhysicMaterialCombine.Multiply.")]
        public static int FrictionCombineMultiply => (int)PhysicMaterialCombine.Multiply;

        [CLProperty("PhysicMaterialCombine.Maximum.")]
        public static int FrictionCombineMaximum => (int)PhysicMaterialCombine.Maximum;

        [CLProperty("PhysicMaterialCombine.Average.")]
        public static int FrictionCombineAverage => (int)PhysicMaterialCombine.Average;

        // Static Friction
        [CLProperty("The static friction of the material.")]
        public float StaticFriction
        {
            get => Value.StaticFriction;
            set => Value.StaticFriction = value;
        }

        // Dynamic Friction
        [CLProperty("The dynamic friction of the material.")]
        public float DynamicFriction
        {
            get => Value.DynamicFriction;
            set => Value.DynamicFriction = value;
        }

        // Bounciness
        [CLProperty("The bounciness of the material.")]
        public float Bounciness
        {
            get => Value.Bounciness;
            set => Value.Bounciness = value;
        }

        // Combine Mode
        [CLProperty("The friction combine mode of the material.", Enum = typeof(CustomLogicPhysicMaterialCombineEnum))]
        public int FrictionCombine
        {
            get => (int)Value.FrictionCombine;
            set => Value.FrictionCombine = (PhysicMaterialCombine)value;
        }

        // Bounce Combine Mode
        [CLProperty("The bounce combine mode of the material.", Enum = typeof(CustomLogicPhysicMaterialCombineEnum))]
        public int BounceCombine
        {
            get => (int)Value.BounceCombine;
            set => Value.BounceCombine = (PhysicMaterialCombine)value;
        }


        [CLMethod("Setup the material.")]
        public void Setup(
            [CLParam("If true, applies the material to all child colliders as well.")]
            bool allChildColliders)
        {
            Value.Setup(allChildColliders);
        }
    }
}
