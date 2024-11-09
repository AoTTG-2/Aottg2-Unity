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
using Utility;
using CustomLogic;
using Cameras;
using System;

namespace Projectiles
{
    // Create enum
    public enum TSKillType
    {
        Air,
        Ground,
        Kill,
        ArmorHit,
        CloseShot,
        MaxRangeShot
    }

    class ThunderspearProjectile : BaseProjectile
    {
        Color _color;
        float _radius;
        public Vector3 InitialPlayerVelocity;
        Vector3 _lastPosition;
        static LayerMask _collideMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles,
            PhysicsLayer.TitanPushbox);
        static LayerMask _blockMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles,
            PhysicsLayer.TitanPushbox, PhysicsLayer.Human);
        bool _wasImpact = false;
        bool _wasMaxRange = false;
        protected override void SetupSettings(object[] settings)
        {
            _radius = (float)settings[0];
            _color = (Color)settings[1];
            _lastPosition = transform.position;
        }

        protected override void RegisterObjects()
        {
            var trail = transform.Find("Trail").GetComponent<ParticleSystem>();
            var flame = transform.Find("Flame").GetComponent<ParticleSystem>();
            var model = transform.Find("ThunderspearModel").gameObject;
            _hideObjects.Add(flame.gameObject);
            _hideObjects.Add(model);
            if (SettingsManager.AbilitySettings.ShowBombColors.Value)
            {
                var main = trail.main;
                main.startColor = _color;
                main = flame.main;
                main.startColor = _color;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                if (CharacterData.HumanWeaponInfo["Thunderspear"]["Ricochet"].AsBool)
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * _velocity.magnitude * CharacterData.HumanWeaponInfo["Thunderspear"]["RicochetSpeed"].AsFloat;
                else
                {
                    _wasImpact = true;
                    Explode();
                    _rigidbody.velocity = Vector3.zero;
                }
            }
        }

        protected override void OnExceedLiveTime()
        {
            _wasMaxRange = true;
            Explode();
        }

        public void Explode()
        {
            if (!Disabled)
            {
                float effectRadius = _radius * 4f;
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                    effectRadius = _radius * 2f;
                int killedPlayer = KillPlayersInRadius(_radius);
                int killedTitan = KillTitansInRadius(_radius);
                int currentPriority = _wasImpact ? (int)TSKillType.Ground : (int)TSKillType.Air;
                currentPriority = Mathf.Max(currentPriority, (int)killedPlayer);
                currentPriority = Mathf.Max(currentPriority, (int)killedTitan);
                TSKillType soundPriority = (TSKillType)currentPriority;

                EffectSpawner.Spawn(
                    EffectPrefabs.ThunderspearExplode,
                    transform.position,
                    transform.rotation,
                    effectRadius,
                    true,
                    new object[] { _color, soundPriority, _wasImpact }
                );
                StunMyHuman();
                DestroySelf();
            }
        }

        void StunMyHuman()
        {
            if (_owner == null || !(_owner is Human) || !_owner.IsMainCharacter())
                return;
            if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                return;
            float radius = CharacterData.HumanWeaponInfo["Thunderspear"]["StunBlockRadius"].AsFloat;
            float range = CharacterData.HumanWeaponInfo["Thunderspear"]["StunRange"].AsFloat;
            Vector3 direction = _owner.Cache.Transform.position - transform.position;
            RaycastHit hit;
            if (Vector3.Distance(_owner.Cache.Transform.position, transform.position) < range)
            {
                if (((Human)_owner).Grounded)
                {
                    if (Physics.Raycast(transform.position + direction.normalized * 0.1f, direction.normalized, out hit, range, _blockMask))
                    {
                        if (hit.collider.transform.root.gameObject == _owner.gameObject)
                            ((Human)_owner).GetStunnedByTS(transform.position);
                    }
                }
                else
                {
                    if (Physics.SphereCast(transform.position, radius, direction.normalized, out hit, range, _blockMask))
                    {
                        if (hit.collider.transform.root.gameObject == _owner.gameObject)
                            ((Human)_owner).GetStunnedByTS(transform.position);
                    }
                }
            }
        }

        int KillTitansInRadius(float radius)
        {
            var position = transform.position;
            var colliders = Physics.OverlapSphere(position, radius, PhysicsLayer.GetMask(PhysicsLayer.Hurtbox));
            int soundPriority = (int)TSKillType.Air;

            foreach (var collider in colliders)
            {
                var titan = collider.transform.root.gameObject.GetComponent<BaseTitan>();
                var handler = collider.gameObject.GetComponent<CustomLogicCollisionHandler>();
                if (handler != null)
                {
                    var damage = CalculateDamage();
                    handler.GetHit(_owner, _owner.Name, damage, "Thunderspear", transform.position);
                    continue;
                }
                if (titan != null && titan != _owner && !TeamInfo.SameTeam(titan, _team) && !titan.Dead)
                {
                    if (collider == titan.BaseTitanCache.NapeHurtbox && titan.CheckNapeAngle(position, CharacterData.HumanWeaponInfo["Thunderspear"]["RestrictAngle"].AsFloat))
                    {
                        float titanHealth = titan.CurrentHealth;
                        int damage = 100;
                        if (!titan.AI)
                        {
                            damage = 0;
                            titan.GetHit("Thunderspear", damage, "TitanStun", collider.name);
                        }
                        else if (_owner == null || !(_owner is Human))
                        {
                            titan.GetHit("Thunderspear", damage, "Thunderspear", collider.name);
                        }
                        else
                        {
                            damage = CalculateDamage();
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                            ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(titan.BaseTitanCache.Neck.position, damage);
                            titan.GetHit(_owner, damage, "Thunderspear", collider.name);
                        }
                        if (damage >= titanHealth)
                            soundPriority = Mathf.Max(soundPriority, (int)TSKillType.Kill);
                        else
                            soundPriority = Mathf.Max(soundPriority, (int)TSKillType.ArmorHit);
                    }
                }
            }
            return soundPriority;
        }

        int KillPlayersInRadius(float radius)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            var position = transform.position;
            int soundPriority = (int)TSKillType.Air;

            foreach (Human human in gameManager.Humans)
            {
                if (human == null || human.Dead)
                    continue;
                if (Vector3.Distance(human.Cache.Transform.position, position) < radius && human != _owner && !TeamInfo.SameTeam(human, _team))
                {
                    float humanHealth = human.CurrentHealth;
                    if (_owner == null || !(_owner is Human))
                        human.GetHit("", 100, "Thunderspear", "");
                    else
                    {
                        var damage = CalculateDamage(true);
                        human.GetHit(_owner, damage, "Thunderspear", "");
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(human.Cache.Transform.position, damage);
                    }

                    if (human.CurrentHealth <= 0)
                    {
                        // if distance is less than 10, it's a close shot
                        if (Vector3.Distance(position, _owner.Cache.Transform.position) < radius)
                            soundPriority = Mathf.Max(soundPriority, (int)TSKillType.CloseShot);
                        else if (_wasMaxRange)
                            soundPriority = Mathf.Max(soundPriority, (int)TSKillType.MaxRangeShot);
                        else
                            soundPriority = Mathf.Max(soundPriority, (int)TSKillType.Kill);
                    }
                    else
                        soundPriority = Mathf.Max(soundPriority, (int)TSKillType.ArmorHit);

                }
            }
            return soundPriority;
        }

        int CalculateDamage(bool dmgOverride=false)
        {
            float damageMultiplier = CharacterData.HumanWeaponInfo["Thunderspear"]["DamageMultiplier"].AsFloat;
            if (dmgOverride)
            {
                damageMultiplier = 1;
            }
            int damage = Mathf.Max((int)(InitialPlayerVelocity.magnitude * 10f * damageMultiplier), 10);
            if (_owner != null && _owner is Human)
            {
                var human = (Human)_owner;
                if (human.CustomDamageEnabled)
                    return human.CustomDamage;
            }
            return damage;
        }

        protected override void Update()
        {
            base.Update();
            if (_photonView.IsMine)
            {
                if (GetComponent<Rigidbody>().velocity.magnitude > 0f)
                    transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
            }
        }

        protected void FixedUpdate()
        {
            if (_photonView.IsMine)
            {
                RaycastHit hit;
                Vector3 direction = (transform.position - _lastPosition);
                if (Physics.SphereCast(_lastPosition, 0.5f, direction.normalized, out hit, direction.magnitude, _collideMask))
                {
                    transform.position = hit.point;
                }
                _lastPosition = transform.position;
            }
        }
    }
}
