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
        protected virtual bool DestroyOnImpact => true;
        protected virtual float MinImpactVelocity => 0f;
        protected virtual float ImpactCooldown => 1f;
        protected float _impactCooldownLeft = 0f;

        protected override void RegisterObjects()
        {
            var model = transform.Find("Rubble3Model").gameObject;
            _hideObjects.Add(model);
        }

        protected override void Update()
        {
            base.Update();
            _impactCooldownLeft -= Time.deltaTime;
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
                if (_rigidbody.velocity.magnitude < MinImpactVelocity || _impactCooldownLeft > 0f)
                    return;
                _impactCooldownLeft = ImpactCooldown;
                var character = collision.collider.gameObject.transform.root.GetComponent<BaseCharacter>();
                var handler = collision.collider.gameObject.GetComponent<CustomLogicCollisionHandler>();
                var damage = CalculateDamage();
                string name = GetName();
                if (handler != null)
                {
                    handler.GetHit(_owner, name, damage, "Rock", transform.position);
                }
                if (character != null && !TeamInfo.SameTeam(character, _team))
                {
                    character.GetHit(name, damage, "Rock", collision.collider.name);
                }
                KillPlayersInRadius(_size * 2f, damage, character);
                EffectSpawner.Spawn(EffectPrefabs.Boom7, transform.position, transform.rotation, _size);
                if (DestroyOnImpact)
                {
                    DestroySelf();
                }
            }
        }

        void KillPlayersInRadius(float radius, int damage, BaseCharacter damagedHuman)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            var position = transform.position;
            string name = GetName();
            foreach (Human human in gameManager.Humans)
            {
                if (human == null || human.Dead || human == damagedHuman)
                    continue;
                if (Vector3.Distance(human.Cache.Transform.position, position) < radius && !TeamInfo.SameTeam(human, _team))
                    human.GetHit(name, damage, "Rock", "");
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

        private string GetName()
        {
            if (_owner == null)
                return "Rock";
            return _owner.Name + "'s Rock";
        }
    }
}
