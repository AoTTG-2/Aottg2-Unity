using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Effects;
using ApplicationManagers;
using GameManagers;
using UI;
using CustomLogic;

namespace Projectiles
{
    class CannonBallProjectile : BaseProjectile
    {
        protected override void RegisterObjects()
        {
            var model = transform.Find("CannonBallModel").gameObject;
            _hideObjects.Add(model);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                var character = collision.collider.gameObject.transform.root.GetComponent<BaseCharacter>();
                var handler = GetComponent<Collider>().gameObject.GetComponent<CustomLogicCollisionHandler>();
                if (handler != null)
                {
                    handler.GetHit(_owner, "CannonBall", 100, "CannonBall");
                    return;
                }
                if (character != null && !TeamInfo.SameTeam(character, _team))
                {
                    if (_owner == null || !(_owner is Human))
                        character.GetHit("CannonBall", 100, "CannonBall", collision.collider.name);
                    else
                    {
                        var human = (Human)_owner;
                        int damage = 100;
                        if (human.CustomDamageEnabled)
                            damage = human.CustomDamage;
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        character.GetHit(_owner, damage, "CannonBall", collision.collider.name);
                    }
                }
                EffectSpawner.Spawn(EffectPrefabs.Boom4, transform.position, Quaternion.LookRotation(_velocity), 0.5f);
                DestroySelf();
            }
        }
    }
}
