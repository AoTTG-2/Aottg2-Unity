// implement
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomLogic
{
    [CLType(Name = "LodBuiltin", Static = true, Abstract = true, Description = "")]
    partial class CustomLogicLodBuiltin : BuiltinComponentInstance
    {
        public LODGroup Value;
        public CustomLogicMapObjectBuiltin OwnerMapObject;
        public GameObject Owner;
        
        [CLConstructor]
        public CustomLogicLodBuiltin() : base(null) { }
        public CustomLogicLodBuiltin(CustomLogicMapObjectBuiltin owner) : base(owner.Value.GameObject.AddComponent<LODGroup>())
        {
            OwnerMapObject = owner;
            Owner = owner.Value.GameObject;
            Value = (LODGroup)Component;
        }
        [CLProperty(Description = "The LODs of the LODGroup.")]
        public LOD[] Lods
        {
            get => Value.GetLODs();
            set => Value.SetLODs(value);
        }
    }
}