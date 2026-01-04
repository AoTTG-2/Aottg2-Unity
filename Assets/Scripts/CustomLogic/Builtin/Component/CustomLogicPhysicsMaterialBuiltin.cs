using Map;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a physics material that defines friction and bounciness properties for colliders.
    /// </summary>
    [CLType(Name = "PhysicsMaterialBuiltin", Static = true, Abstract = true, IsComponent = true)]
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

        /// <summary>
        /// PhysicMaterialCombine.Minimum.
        /// </summary>
        [CLProperty]
        public static int FrictionCombineMinimum => (int)PhysicMaterialCombine.Minimum;

        /// <summary>
        /// PhysicMaterialCombine.Multiply.
        /// </summary>
        [CLProperty]
        public static int FrictionCombineMultiply => (int)PhysicMaterialCombine.Multiply;

        /// <summary>
        /// PhysicMaterialCombine.Maximum.
        /// </summary>
        [CLProperty]
        public static int FrictionCombineMaximum => (int)PhysicMaterialCombine.Maximum;

        /// <summary>
        /// PhysicMaterialCombine.Average.
        /// </summary>
        [CLProperty]
        public static int FrictionCombineAverage => (int)PhysicMaterialCombine.Average;

        // Static Friction
        /// <summary>
        /// The static friction of the material.
        /// </summary>
        [CLProperty]
        public float StaticFriction
        {
            get => Value.StaticFriction;
            set => Value.StaticFriction = value;
        }

        // Dynamic Friction
        /// <summary>
        /// The dynamic friction of the material.
        /// </summary>
        [CLProperty]
        public float DynamicFriction
        {
            get => Value.DynamicFriction;
            set => Value.DynamicFriction = value;
        }

        // Bounciness
        /// <summary>
        /// The bounciness of the material.
        /// </summary>
        [CLProperty]
        public float Bounciness
        {
            get => Value.Bounciness;
            set => Value.Bounciness = value;
        }

        // Combine Mode
        /// <summary>
        /// The friction combine mode of the material.
        /// </summary>
        [CLProperty(Enum = typeof(CustomLogicPhysicMaterialCombineEnum))]
        public int FrictionCombine
        {
            get => (int)Value.FrictionCombine;
            set => Value.FrictionCombine = (PhysicMaterialCombine)value;
        }

        // Bounce Combine Mode
        /// <summary>
        /// The bounce combine mode of the material.
        /// </summary>
        [CLProperty(Enum = typeof(CustomLogicPhysicMaterialCombineEnum))]
        public int BounceCombine
        {
            get => (int)Value.BounceCombine;
            set => Value.BounceCombine = (PhysicMaterialCombine)value;
        }


        /// <summary>
        /// Setup the material.
        /// </summary>
        /// <param name="allChildColliders">If true, applies the material to all child colliders as well.</param>
        [CLMethod]
        public void Setup(bool allChildColliders)
        {
            Value.Setup(allChildColliders);
        }
    }
}
