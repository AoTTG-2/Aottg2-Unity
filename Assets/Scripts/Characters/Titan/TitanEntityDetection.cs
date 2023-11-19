using UnityEngine;
using System.Collections.Generic;
using Utility;
using Projectiles;
using GameManagers;

namespace Characters
{
    class TitanEntityDetection: MonoBehaviour
    {
        public HashSet<GameObject> _entities = new HashSet<GameObject>();
        public HashSet<Hook> _hooks = new HashSet<Hook>();
        public BaseTitan Owner;
        public bool Detect = false;

        public static TitanEntityDetection Create(BaseTitan owner)
        {
            GameObject go = new GameObject();
            Collider move = owner.BaseTitanCache.Movebox;
            go.transform.SetParent(move.transform);
            go.transform.localPosition = Vector3.zero;
            TitanEntityDetection entity = go.AddComponent<TitanEntityDetection>();
            go.layer = PhysicsLayer.EntityDetection;
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
            BaseCharacter character = obj.GetComponent<BaseCharacter>();
            if (character != null && character.IsMine() && !TeamInfo.SameTeam(character, Owner))
            {
                _entities.Add(obj);
                Detect = true;
                return;
            }
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
            _entities = Util.RemoveNull(_entities);
            var newHooks = new HashSet<Hook>();
            foreach (var item in _hooks)
            {
                if (item != null && (item.State == HookState.Hooking || item.State == HookState.Hooked))
                    newHooks.Add(item);
            }
            _hooks = newHooks;
            if (_entities.Count == 0 && _hooks.Count == 0)
                Detect = false;
        }
    }
}
