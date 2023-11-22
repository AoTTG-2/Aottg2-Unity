using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Effects;
using ApplicationManagers;
using GameManagers;
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
                foreach (var collider in Physics.OverlapSphere(transform.position, 4f, PhysicsLayer.GetMask(PhysicsLayer.Hurtbox)))
                {
                    var root = collider.transform.root.gameObject;
                    var titan = root.GetComponent<BasicTitan>();
                    if (titan != null)
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
