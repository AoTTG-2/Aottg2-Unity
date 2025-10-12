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
        bool _isEmbed = false;
        Transform _embedParent = null;
        Vector3 _embedPosition = Vector3.zero;
        Vector3 _startPosition = Vector3.zero;
        bool _isAA = false;
        float _embedTime;
        bool _usesEmbed = false;
        public static Color CritColor = new Color(0.475f, 0.7f, 1f);

        protected override void SetupSettings(object[] settings)
        {
            _radius = (float)settings[0];
            _color = (Color)settings[1];
            _usesEmbed = (bool)settings[2];
            _lastPosition = transform.position;
            _startPosition = transform.position;
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
                // Check if we're in ThunderspearPVP mode and BombCollision (bouncy) is enabled
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value && SettingsManager.AbilitySettings.BombCollision.Value)
                {
                    // Bouncing behavior - reflect velocity off the collision surface
                    Vector3 reflectedVelocity = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);
                    _rigidbody.velocity = reflectedVelocity * 0.8f; // Apply some energy loss on bounce
                    _velocity = _rigidbody.velocity;
                    return; // Don't proceed with normal collision logic
                }
                
                // Original collision logic for all other cases
                _wasImpact = true;
                _rigidbody.velocity = Vector3.zero;
                foreach (Collider c in _colliders)
                    c.enabled = false;
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value || !_usesEmbed)
                {
                    Explode();
                }
                else
                {
                    _isEmbed = true;
                    _embedTime = Time.fixedTime;
                    _velocity = (-collision.contacts[0].normal + _velocity.normalized).normalized;
                    _embedParent = collision.transform;
                    float embedDistance = 0.1f;
                    if (collision.transform.root.GetComponent<BaseTitan>() != null)
                        embedDistance = 0.5f;
                    _embedPosition = collision.transform.InverseTransformPoint(collision.contacts[0].point + _velocity * embedDistance);
                    _transform.position = collision.contacts[0].point + _velocity * embedDistance;
                    _transform.rotation = Quaternion.LookRotation(_velocity);
                    var travelDistance = Vector3.Distance(_startPosition, _transform.position);
                    float embed1Time = GetStat("Embed1Time") + GetStat("Embed1TimeMultiplier") * InitialPlayerVelocity.magnitude;
                    embed1Time = Mathf.Min(embed1Time, GetStat("Embed1TimeMax"));
                    embed1Time = Mathf.Max(embed1Time, GetStat("Embed1TimeMin"));
                    float embed2Time = Mathf.Max(GetStat("Embed2TimeTotal") - travelDistance * GetStat("Embed2TimeMultiplier"), 0f);
                    _timeLeft = embed1Time + embed2Time;
                    if (Vector3.Distance(_transform.position, _startPosition) < GetStat("AATriggerRange"))
                    {
                        _isAA = true;
                    }
                }
            }
        }


        protected override void OnExceedLiveTime()
        {
            _wasMaxRange = true;
            if (!_isEmbed)
                Explode();
            else
                DestroySelf();
        }

        public void Explode()
        {
            if (!Disabled)
            {
                float effectRadius;
                float restrictAngle = GetStat("RestrictAngle");
                Color color = _color;
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                    effectRadius = _radius * 2f;
                else
                {
                    if (_isEmbed)
                    {
                        float timePassed = Time.fixedTime - _embedTime;
                        float embed1Time = GetStat("Embed1Time") + GetStat("Embed1TimeMultiplier") * InitialPlayerVelocity.magnitude;
                        embed1Time = Mathf.Min(embed1Time, GetStat("Embed1TimeMax"));
                        embed1Time = Mathf.Max(embed1Time, GetStat("Embed1TimeMin"));
                        if (timePassed <= embed1Time)
                        {
                            _radius = _radius * GetStat("RadiusEmbed1Multiplier");
                            restrictAngle = GetStat("RestrictAngleEmbed1");
                            color = CritColor;
                        }
                        else
                        {
                            _radius = _radius * GetStat("RadiusEmbed2Multiplier");
                            restrictAngle = GetStat("RestrictAngleEmbed2");
                        }
                    }
                    effectRadius = _radius * 4f;
                }
                int killedPlayer = KillPlayersInRadius(_radius);
                int killedTitan = KillTitansInRadius(_radius, restrictAngle);
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
                    new object[] { color, soundPriority, _wasImpact }
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
            float range = CharacterData.HumanWeaponInfo["Thunderspear"]["StunRange"].AsFloat;
            RaycastHit hit;
            if (Vector3.Distance(_owner.Cache.Transform.position, transform.position) < range)
            {
                ((Human)_owner).GetStunnedByTS(transform.position);
            }
        }

        int KillTitansInRadius(float radius, float restrictAngle)
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
                    if (collider == titan.BaseTitanCache.NapeHurtbox)
                    {
                        bool angle = titan.CheckNapeAngle(position, restrictAngle);
                        float titanHealth = titan.CurrentHealth;
                        int damage = 100;
                        if (_owner == null || !(_owner is Human))
                        {
                            titan.GetHit("Thunderspear", damage, "Thunderspear", collider.name);
                        }
                        else if (!_isEmbed && angle)
                        {
                            damage = 0;
                            titan.GetHit("Thunderspear", damage, "TitanStun", collider.name);
                        }
                        else if (angle)
                        {
                            damage = CalculateDamage();
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                            ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(titan.BaseTitanCache.Neck.position, damage);
                            titan.GetHit(_owner, damage, "Thunderspear", collider.name);
                        }
                        else if (_isEmbed)
                        {
                            damage = 0;
                            titan.GetHit("Thunderspear", damage, "TitanStun", collider.name);
                        }
                        if(((Human)_owner).Special is RechargeableUseable)
                        {
                            ((RechargeableUseable)((Human)_owner).Special).ReduceCooldown();
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
            float multiplier = CharacterData.HumanWeaponInfo["Thunderspear"]["DamageMultiplier"].AsFloat;
            if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                multiplier = 1f;
            int damage = Mathf.Max((int)(InitialPlayerVelocity.magnitude * 10f * multiplier), 10);
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
                if (_velocity.magnitude > 0f)
                    transform.rotation = Quaternion.LookRotation(_velocity);
            }
        }

        protected void FixedUpdate()
        {
            if (_photonView.IsMine)
            {
                if (_isEmbed)
                {
                    _rigidbody.velocity = Vector3.zero;
                    if (_embedParent != null)
                        _transform.position = _embedParent.TransformPoint(_embedPosition);
                    if (_owner != null && _isAA)
                    {
                        Explode();
                    }
                    return;
                }
                RaycastHit hit;
                Vector3 direction = (transform.position - _lastPosition);
                if (Physics.SphereCast(_lastPosition, 0.5f, direction.normalized, out hit, direction.magnitude, _collideMask))
                {
                    transform.position = hit.point;
                }
                _lastPosition = transform.position;
            }
        }

        float GetStat(string field)
        {
            return CharacterData.HumanWeaponInfo["Thunderspear"][field].AsFloat;
        }
    }
}
