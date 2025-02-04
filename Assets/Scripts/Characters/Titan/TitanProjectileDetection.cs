using UnityEngine;
using System.Collections.Generic;
using Utility;
using Projectiles;
using GameManagers;
using Unity.VisualScripting;

namespace Characters
{
    class TitanProjectileDetection: MonoBehaviour
    {
        public HashSet<GameObject> _entities = new HashSet<GameObject>();
        public HashSet<Hook> _hooks = new HashSet<Hook>();
        public BaseTitan Owner;
        public bool Detect = false;

        public static TitanProjectileDetection Create(BaseTitan owner)
        {
            GameObject go = new GameObject();
            Collider move = owner.BaseTitanCache.Movebox;
            go.transform.SetParent(move.transform);
            go.transform.localPosition = Vector3.zero;
            TitanProjectileDetection entity = go.AddComponent<TitanProjectileDetection>();
            go.layer = PhysicsLayer.ProjectileDetection;
            if (move is CapsuleCollider)
            {
                CapsuleCollider capsuleMove = (CapsuleCollider)move;
                SphereCollider entityCollider = go.AddComponent<SphereCollider>();
                entityCollider.center = capsuleMove.center;
                entityCollider.radius = capsuleMove.height;
                entityCollider.isTrigger = true;
            }
            entity.Owner = owner;
            return entity;
        }

        public void RegisterHook(Hook hook)
        {
            _hooks.Add(hook);
            Detect = true;
        }

        protected void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.transform.root.gameObject;
            BaseProjectile projectile = obj.GetComponent<BaseProjectile>();
            if (projectile != null && projectile.IsMine())
            {
                _entities.Add(obj);
                Detect = true;
                return;
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            GameObject obj = other.transform.root.gameObject;
            if (_entities.Contains(obj))
                _entities.Remove(obj);
            if (_entities.Count == 0 && _hooks.Count == 0)
                Detect = false;
        }

        protected void FixedUpdate()
        {
            if (_entities.Count > 0)
                _entities = Util.RemoveNull(_entities);
            if (_hooks.Count > 0)
                _hooks.RemoveWhere(hook => !hook || (hook.State != HookState.Hooking && hook.State != HookState.Hooked));
            if (_entities.Count == 0 && _hooks.Count == 0)
                Detect = false;
        }
    }
}
