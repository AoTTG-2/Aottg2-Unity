using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace Characters
{
    class TitanColliderToggler: MonoBehaviour
    {
        public BaseTitan Owner;
        protected TitanEntityDetection _entity;
        protected bool _look = false;
        protected bool _enabled = false;

        public static TitanColliderToggler Create(BaseTitan owner)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(owner.transform);
            TitanColliderToggler toggler = go.AddComponent<TitanColliderToggler>();
            toggler.Owner = owner;
            toggler._entity = TitanEntityDetection.Create(owner);
            toggler.SetColliders(false);
            return toggler;
        }

        public void RegisterLook()
        {
            _look = true;
        }

        protected void FixedUpdate()
        {
            if (_enabled)
            {
                if (!_look && !_entity.Detect)
                    SetColliders(false);
            }
            else if (_look || _entity.Detect)
                SetColliders(true);
            _look = false;
        }

        protected void SetColliders(bool enable)
        {
            foreach (Collider collider in Owner.BaseTitanCache.ToggleColliders)
            {
                if (collider != null)
                {
                    collider.enabled = enable;
                }
            }
            _enabled = enable;
        }
    }
}
