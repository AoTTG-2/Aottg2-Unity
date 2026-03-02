using UnityEngine;
using System.Collections.Generic;
using GameManagers;
using CustomLogic;

namespace Characters
{
    class ContinuousDamageHitbox : BaseHitbox
    {
        public int DamagePerSecond = 100;
        public float DamageInterval = 0.1f;

        private Dictionary<BaseCharacter, float> _characterDamageTimers = new Dictionary<BaseCharacter, float>();
        private Dictionary<BaseCharacter, Collider> _characterColliders = new Dictionary<BaseCharacter, Collider>();
        private HashSet<BaseCharacter> _charactersInside = new HashSet<BaseCharacter>();

        private Dictionary<CustomLogicCollisionHandler, float> _handlerDamageTimers = new Dictionary<CustomLogicCollisionHandler, float>();
        private Dictionary<CustomLogicCollisionHandler, Collider> _handlerColliders = new Dictionary<CustomLogicCollisionHandler, Collider>();
        private HashSet<CustomLogicCollisionHandler> _handlersInside = new HashSet<CustomLogicCollisionHandler>();

        public static ContinuousDamageHitbox CreateContinuous(BaseCharacter owner, GameObject obj, Collider collider, int damagePerSecond, float damageInterval)
        {
            ContinuousDamageHitbox hitbox = obj.AddComponent<ContinuousDamageHitbox>();
            hitbox.Owner = owner;

            hitbox._collider = collider;
            hitbox.DamagePerSecond = damagePerSecond;
            hitbox.DamageInterval = damageInterval;
            hitbox.Deactivate();
            return hitbox;
        }

        public new void Activate(float delay = 0f, float length = 0f)
        {
            _characterDamageTimers.Clear();
            _characterColliders.Clear();
            _charactersInside.Clear();
            _handlerDamageTimers.Clear();
            _handlerColliders.Clear();
            _handlersInside.Clear();
            base.Activate(delay, length);
        }

        public new void Deactivate()
        {
            _characterDamageTimers.Clear();
            _characterColliders.Clear();
            _charactersInside.Clear();
            _handlerDamageTimers.Clear();
            _handlerColliders.Clear();
            _handlersInside.Clear();
            base.Deactivate();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var go = other.transform.root.gameObject;
            BaseCharacter character = go.GetComponent<BaseCharacter>();
            CustomLogicCollisionHandler handler = other.gameObject.GetComponent<CustomLogicCollisionHandler>();

            if (character != null && !TeamInfo.SameTeam(Owner, character))
            {
                if (!_charactersInside.Contains(character))
                {
                    _charactersInside.Add(character);
                    _characterDamageTimers[character] = 0f;
                    _characterColliders[character] = other;
                }
            }
            else if (handler != null)
            {
                if (!_handlersInside.Contains(handler))
                {
                    _handlersInside.Add(handler);
                    _handlerDamageTimers[handler] = 0f;
                    _handlerColliders[handler] = other;
                }
            }
        }

        protected override void OnTriggerStay(Collider other)
        {
            var go = other.transform.root.gameObject;
            BaseCharacter character = go.GetComponent<BaseCharacter>();
            CustomLogicCollisionHandler handler = other.gameObject.GetComponent<CustomLogicCollisionHandler>();

            if (character != null && !TeamInfo.SameTeam(Owner, character))
            {
                if (!_charactersInside.Contains(character))
                {
                    _charactersInside.Add(character);
                    _characterDamageTimers[character] = 0f;
                    _characterColliders[character] = other;
                }
            }
            else if (handler != null)
            {
                if (!_handlersInside.Contains(handler))
                {
                    _handlersInside.Add(handler);
                    _handlerDamageTimers[handler] = 0f;
                    _handlerColliders[handler] = other;
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var go = other.transform.root.gameObject;
            BaseCharacter character = go.GetComponent<BaseCharacter>();
            CustomLogicCollisionHandler handler = other.gameObject.GetComponent<CustomLogicCollisionHandler>();

            if (character != null && _charactersInside.Contains(character))
            {
                _charactersInside.Remove(character);
                _characterDamageTimers.Remove(character);
                _characterColliders.Remove(character);
            }
            else if (handler != null && _handlersInside.Contains(handler))
            {
                _handlersInside.Remove(handler);
                _handlerDamageTimers.Remove(handler);
                _handlerColliders.Remove(handler);
            }
        }

        protected virtual void Update()
        {
            if (!IsActive())
                return;

            List<BaseCharacter> charactersToRemove = new List<BaseCharacter>();

            foreach (var character in _charactersInside)
            {
                if (character == null || character.Dead)
                {
                    charactersToRemove.Add(character);
                    continue;
                }

                if (_characterDamageTimers.ContainsKey(character))
                {
                    _characterDamageTimers[character] += Time.deltaTime;

                    if (_characterDamageTimers[character] >= DamageInterval)
                    {
                        if (_characterColliders.ContainsKey(character))
                        {
                            OnHit(character, _characterColliders[character]);
                        }
                        _characterDamageTimers[character] = 0f;
                    }
                }
            }

            foreach (var character in charactersToRemove)
            {
                _charactersInside.Remove(character);
                _characterDamageTimers.Remove(character);
                _characterColliders.Remove(character);
            }

            List<CustomLogicCollisionHandler> handlersToRemove = new List<CustomLogicCollisionHandler>();

            foreach (var handler in _handlersInside)
            {
                if (handler == null)
                {
                    handlersToRemove.Add(handler);
                    continue;
                }

                if (_handlerDamageTimers.ContainsKey(handler))
                {
                    _handlerDamageTimers[handler] += Time.deltaTime;

                    if (_handlerDamageTimers[handler] >= DamageInterval)
                    {
                        if (_handlerColliders.ContainsKey(handler))
                        {
                            OnHit(handler, _handlerColliders[handler]);
                        }
                        _handlerDamageTimers[handler] = 0f;
                    }
                }
            }

            foreach (var handler in handlersToRemove)
            {
                _handlersInside.Remove(handler);
                _handlerDamageTimers.Remove(handler);
                _handlerColliders.Remove(handler);
            }
        }

        protected override void OnHit(BaseCharacter victim, Collider collider)
        {
            Owner.OnHit(this, victim, collider, "", true);
        }

        protected override void OnHit(CustomLogicCollisionHandler handler, Collider collider)
        {
            Owner.OnHit(this, handler, collider, "", true);
        }
    }
}
