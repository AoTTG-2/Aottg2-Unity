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
    class Rock1Projectile : BaseProjectile
    {
        protected float _size;

        protected override void RegisterObjects()
        {
            var model = transform.Find("Rubble3Model").gameObject;
            _hideObjects.Add(model);
        }

        protected override void SetupSettings(object[] settings)
        {
            _size = (float)settings[0];
            transform.localScale = Vector3.one * _size;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                var character = collision.collider.gameObject.transform.root.GetComponent<BaseCharacter>();
                var handler = GetComponent<Collider>().gameObject.GetComponent<CustomLogicCollisionHandler>();
                var damage = CalculateDamage();
                if (handler != null)
                {
                    handler.GetHit(_owner, _owner.Name, damage, "Rock", transform.position);
                    return;
                }
                if (character != null && !TeamInfo.SameTeam(character, _team))
                {
                    character.GetHit("Rock", damage, "Rock", collision.collider.name);
                    return;
                }
                KillPlayersInRadius(_size * 2f, damage);
                EffectSpawner.Spawn(EffectPrefabs.Boom7, transform.position, transform.rotation, _size);
                DestroySelf();
            }
        }

        void KillPlayersInRadius(float radius, int damage)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            var position = transform.position;
            foreach (Human human in gameManager.Humans)
            {
                if (human == null || human.Dead)
                    continue;
                if (Vector3.Distance(human.Cache.Transform.position, position) < radius && !TeamInfo.SameTeam(human, _team))
                    human.GetHit("Rock", damage, "Rock", "");
            }
        }

        private int CalculateDamage()
        {
            int damage = 100;

            if (_owner != null && _owner is BaseCharacter character)
            {
                if (character.CustomDamageEnabled)
                    damage = character.CustomDamage;
            }

            return damage;
        }
    }
}
