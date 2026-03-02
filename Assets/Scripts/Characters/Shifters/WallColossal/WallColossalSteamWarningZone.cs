using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace Characters
{
    /// <summary>
    /// Warning zone that triggers the Fire1 particle effect on humans entering the steam area
    /// and applies knockback to projectiles and rigidbody elements
    /// </summary>
    internal class WallColossalSteamWarningZone : MonoBehaviour
    {
        private HashSet<Human> _humansInZone = new HashSet<Human>();
        private HashSet<Rigidbody> _rigidbodiesInZone = new HashSet<Rigidbody>();
        private WallColossalShifter _owner;
        private bool _isActive;
        
        public float KnockbackForce = 100f;
        public float ThunderspearKnockbackMultiplier = 5f;
        public float KnockbackInterval = 0.1f;
        private float _knockbackTimer = 0f;
        private float _projectileCheckTimer = 0f;
        private float _projectileCheckInterval = 0.05f;
        
        private BoxCollider _boxCollider;

        internal void Initialize(WallColossalShifter owner)
        {
            _owner = owner;
            _boxCollider = GetComponent<BoxCollider>();
        }

        internal void SetActive(bool active)
        {
            _isActive = active;
            
            if (!active)
            {
                foreach (var human in _humansInZone)
                {
                    if (human != null && !human.Dead)
                        human.ToggleFire1(false);
                }
                _humansInZone.Clear();
                _rigidbodiesInZone.Clear();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive || _owner == null)
                return;

            if (other.gameObject.layer == PhysicsLayer.Human)
            {
                var human = other.GetComponent<Human>();
                if (human != null && !human.Dead && human.IsMine())
                {
                    _humansInZone.Add(human);
                    human.ToggleFire1(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_owner == null)
                return;

            if (other.gameObject.layer == PhysicsLayer.Human)
            {
                var human = other.GetComponent<Human>();
                if (human != null && _humansInZone.Contains(human))
                {
                    _humansInZone.Remove(human);
                    if (!human.Dead && human.IsMine())
                        human.ToggleFire1(false);
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up all fire effects when destroyed
            foreach (var human in _humansInZone)
            {
                if (human != null && !human.Dead && human.IsMine())
                    human.ToggleFire1(false);
            }
            _humansInZone.Clear();
            _rigidbodiesInZone.Clear();
        }

        private void Update()
        {
            _humansInZone.RemoveWhere(h => h == null || h.Dead);
            _rigidbodiesInZone.RemoveWhere(rb => rb == null);

            if (_isActive && _owner != null && _boxCollider != null)
            {
                _projectileCheckTimer -= Time.deltaTime;
                if (_projectileCheckTimer <= 0f)
                {
                    CheckForProjectiles();
                    _projectileCheckTimer = _projectileCheckInterval;
                }

                _knockbackTimer -= Time.deltaTime;
                if (_knockbackTimer <= 0f)
                {
                    foreach (var rb in _rigidbodiesInZone)
                    {
                        if (rb != null && !rb.isKinematic)
                            ApplyKnockback(rb);
                    }
                    _knockbackTimer = KnockbackInterval;
                }
            }
        }

        private void CheckForProjectiles()
        {
            Vector3 center = transform.TransformPoint(_boxCollider.center);
            Vector3 halfExtents = Vector3.Scale(_boxCollider.size * 0.5f, transform.lossyScale);
            Quaternion rotation = transform.rotation;

            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, rotation, 1 << PhysicsLayer.Projectile);
            
            HashSet<Rigidbody> currentProjectiles = new HashSet<Rigidbody>();
            
            foreach (var collider in hitColliders)
            {
                var rb = collider.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    currentProjectiles.Add(rb);
                    
                    if (!_rigidbodiesInZone.Contains(rb))
                    {
                        _rigidbodiesInZone.Add(rb);
                        ApplyKnockback(rb);
                    }
                }
            }
            
            _rigidbodiesInZone.RemoveWhere(rb => !currentProjectiles.Contains(rb));
        }

        private void ApplyKnockback(Rigidbody rb)
        {
            if (_owner == null || _owner.ColossalCache?.NapeHurtbox == null)
                return;

            Vector3 steamCenter = _owner.ColossalCache.NapeHurtbox.transform.position;
            Vector3 direction = (rb.position - steamCenter).normalized;
            
            float force = KnockbackForce;
            
            // Check if this is a thunderspear and apply extra knockback
            if (rb.name.Contains("Thunderspear") || rb.name.Contains("ThunderSpear"))
            {
                force *= ThunderspearKnockbackMultiplier;
            }
            
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
