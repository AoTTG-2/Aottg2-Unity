using UnityEngine;

namespace Map
{
    class CustomPhysicsMaterial : MonoBehaviour
    {
        private PhysicMaterial _material;

        public float Bounciness
        {
            get => _material.bounciness;
            set => _material.bounciness = value;
        }
        
        public float StaticFriction
        {
            get => _material.staticFriction;
            set => _material.staticFriction = value;
        }
        
        public float DynamicFriction
        {
            get => _material.dynamicFriction;
            set => _material.dynamicFriction = value;
        }
        
        public PhysicMaterialCombine FrictionCombine
        {
            get => _material.frictionCombine;
            set => _material.frictionCombine = value;
        }
        
        public PhysicMaterialCombine BounceCombine
        {
            get => _material.bounceCombine;
            set => _material.bounceCombine = value;
        }
        
        public void Setup(bool allChildColliders)
        {
            _material = new PhysicMaterial();

            if (allChildColliders)
            {
                foreach (var col in gameObject.GetComponentsInChildren<Collider>())
                {
                    col.material = _material;
                }
            }
            else
            {
                var col = gameObject.GetComponentInChildren<Collider>();
                if (col != null)
                    col.material = _material;
            }
        }
    }
}