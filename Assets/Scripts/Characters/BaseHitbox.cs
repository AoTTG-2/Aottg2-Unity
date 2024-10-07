using UnityEngine;
using System.Collections.Generic;
using Utility;
using GameManagers;
using System.Collections;
using ApplicationManagers;
using Map;
using CustomLogic;

namespace Characters
{
    class BaseHitbox: MonoBehaviour
    {
        public BaseCharacter Owner;
        public bool TwoFixedUpdates = false;
        protected HashSet<GameObject> _hitGameObjects = new HashSet<GameObject>();
        protected HashSet<Collider> _firstFrameColliders = new HashSet<Collider>();
        public Collider _collider;
        protected bool _firstHit = false;
        public GameObject _debugObject;

        public static BaseHitbox Create(BaseCharacter owner, GameObject obj, Collider collider = null)
        {
            BaseHitbox hitbox = obj.AddComponent<BaseHitbox>();
            hitbox.Owner = owner;
            if (collider == null)
                collider = obj.GetComponent<Collider>();
            hitbox._collider = collider;
            hitbox.Deactivate();
            if (collider is SphereCollider)
                hitbox.UpdateDebugCollider((SphereCollider)collider);
            return hitbox;
        }

        public void UpdateSphereCollider(float radius)
        {
            if (_collider is SphereCollider)
            {
                SphereCollider collider = (SphereCollider)_collider;
                collider.radius = radius;
                UpdateDebugCollider(collider);
            }
        }

        protected void UpdateDebugCollider(SphereCollider collider)
        {
            if (DebugTesting.DebugColliders)
            {
                if (_debugObject != null)
                    Destroy(_debugObject);
                var sphere = ResourceManager.InstantiateAsset<GameObject>("Game", "TestSphere");
                sphere.transform.parent = gameObject.transform;
                sphere.transform.localPosition = collider.center;
                sphere.transform.localScale = Vector3.one * collider.radius * 2f;
                sphere.GetComponent<Renderer>().material.color = Color.red;
                _debugObject = sphere;
                _debugObject.SetActive(false);
            }
        }

        public bool IsActive()
        {
            return _collider.enabled;
        }
        
        public void Activate(float delay = 0f, float length = 0f)
        {
            _hitGameObjects.Clear();
            _firstHit = true;
            _firstFrameColliders.Clear();
            if (delay == 0f)
            {
                _collider.enabled = true;
                ToggleDebug(true);
            }
            else
                StartCoroutine(WaitAndActivate(delay));
            if (length > 0f)
                StartCoroutine(WaitAndDeactivate(delay + length));
        }

        public void Deactivate()
        {
            StopAllCoroutines();
            _collider.enabled = false;
            ToggleDebug(false);
        }

        protected IEnumerator WaitAndActivate(float delay)
        {
            yield return new WaitForSeconds(delay);
            _collider.enabled = true;
            ToggleDebug(true);
        }

        protected IEnumerator WaitAndDeactivate(float delay)
        {
            yield return new WaitForSeconds(delay);
            _collider.enabled = false;
            ToggleDebug(false);
            
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (TwoFixedUpdates)
                return;
            OnTrigger(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!TwoFixedUpdates)
                return;
            OnTrigger(other);
        }

        protected virtual void OnTrigger(Collider other)
        {
            if (TwoFixedUpdates && !_firstFrameColliders.Contains(other))
            {
                _firstFrameColliders.Add(other);
                return;
            }
            var go = other.transform.root.gameObject;
            BaseCharacter character = go.GetComponent<BaseCharacter>();
            CustomLogicCollisionHandler handler = other.gameObject.GetComponent<CustomLogicCollisionHandler>();
            if (character != null && !TeamInfo.SameTeam(Owner, character) && !_hitGameObjects.Contains(other.gameObject))
            {
                _hitGameObjects.Add(other.gameObject);
                OnHit(character, other);
            }
            else if (handler != null && !_hitGameObjects.Contains(other.gameObject))
            {
                _hitGameObjects.Add(other.gameObject);
                OnHit(handler, other);
            }
        }

        protected virtual void OnHit(BaseCharacter victim, Collider collider)
        {
            Owner.OnHit(this, victim, collider, "", _firstHit);
            _firstHit = false;
        }

        protected virtual void OnHit(CustomLogicCollisionHandler handler, Collider collider)
        {
            Owner.OnHit(this, handler, collider, "", _firstHit);
            _firstHit = false;
        }

        protected void ToggleDebug(bool toggle)
        {
            if (_debugObject != null)
                _debugObject.SetActive(toggle);
        }
    }
}
