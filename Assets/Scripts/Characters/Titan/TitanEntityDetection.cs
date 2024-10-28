﻿using UnityEngine;
using System.Collections.Generic;
using Utility;
using Projectiles;
using GameManagers;
using Unity.VisualScripting;

namespace Characters
{
    class TitanEntityDetection: MonoBehaviour
    {
        public HashSet<GameObject> _entities = new HashSet<GameObject>();
        public HashSet<GameObject> _humans = new HashSet<GameObject>();
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
            if (character != null && character is Human && !TeamInfo.SameTeam(character, Owner))
            {
                _humans.Add(obj);
            }
            if (character != null && character.IsMine() && (character is Human || !TeamInfo.SameTeam(character, Owner)))
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
            if (_humans.Contains(obj))
                _humans.Remove(obj);
            if (_entities.Count == 0 && _hooks.Count == 0)
                Detect = false;
        }

        protected void FixedUpdate()
        {
            _entities = Util.RemoveNull(_entities);
            _humans = Util.RemoveNull(_humans);
            _hooks.RemoveWhere(hook => !hook || (hook.State != HookState.Hooking && hook.State != HookState.Hooked));
            if (_entities.Count == 0 && _hooks.Count == 0)
                Detect = false;
        }
    }
}
