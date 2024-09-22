using UnityEngine;
using Characters;
using Effects;
using Utility;

namespace Projectiles
{
    class SmokeBombProjectile : BaseProjectile
    {
        protected override float DestroyDelay => 10f;

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                PhysicsUtils.OverlapSphere(transform.position, 4f, PhysicsLayer.GetMask(PhysicsLayer.Hurtbox));
                foreach (var collider in PhysicsUtils.GetColliders())
                {
                    var root = collider.transform.root.gameObject;
                    if (root.TryGetComponent<BasicTitan>(out var titan))
                    {
                        if (collider == titan.BaseTitanCache.EyesHurtbox)
                        {
                            EffectSpawner.Spawn(EffectPrefabs.CriticalHit, transform.position, Quaternion.Euler(270f, 0f, 0f));
                            titan.GetHit(_owner, 0, "SmokeBomb", collider.name);
                        }
                    }
                }
                DestroySelf();
            }
        }
    }
}
