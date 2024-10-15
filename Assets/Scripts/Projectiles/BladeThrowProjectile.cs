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
using Photon.Pun;

namespace Projectiles
{
    class BladeThrowProjectile : BaseProjectile
    {
        protected override float DestroyDelay => 1.5f;
        protected Transform _blade;
        protected GameObject _model;
        private MeleeWeaponTrail WeaponTrail;
        public Vector3 InitialPlayerVelocity;

        protected override void Awake()
        {
            base.Awake();
            _blade = transform.Find("Blade");
            _model = _blade.Find("Model").gameObject;
            WeaponTrail = GetComponentInChildren<MeleeWeaponTrail>();
            if (SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
                WeaponTrail.Emit = true;
            else
                WeaponTrail.Emit = false;
        }

        protected void Start()
        {
            if (_owner != null && _owner is Human && _owner.IsMine())
                WeaponTrail.SetMaterial(((Human)_owner).Setup.LeftTrail._material);
        }

        protected override void RegisterObjects()
        {
            _hideObjects.Add(_model);
        }

        [PunRPC]
        public override void DisableRPC(PhotonMessageInfo info)
        {
            if (Disabled)
                return;
            if (info.Sender != photonView.Owner)
                return;
            base.DisableRPC(info);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                var character = collision.collider.transform.root.GetComponent<BaseCharacter>();
                if (character == null)
                {
                    EffectSpawner.Spawn(EffectPrefabs.BladeThrowHit, transform.position, Quaternion.LookRotation(_velocity));
                }
                else if (!TeamInfo.SameTeam(character, _team))
                {
                    if (character is BaseTitan)
                    {
                        EffectSpawner.Spawn(EffectPrefabs.Blood1, transform.position, Quaternion.Euler(270f, 0f, 0f));
                    }
                }
                transform.Find("BladeHit").GetComponent<AudioSource>().Play();
                CheckHurtboxes(collision.collider);
                DestroySelf();
            }
        }

        void CheckHurtboxes(Collider firstCollider)
        {
            var radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x * 1.3f;
            var overlap = Physics.OverlapSphere(transform.position, radius, PhysicsLayer.GetMask(PhysicsLayer.Hurtbox, PhysicsLayer.Human));
            List<Collider> colliders = new List<Collider>(overlap);
            if (!colliders.Contains(firstCollider))
                colliders.Add(firstCollider);
            foreach (var collider in colliders)
            {
                var character = collider.transform.root.gameObject.GetComponent<BaseCharacter>();
                var handler = collider.gameObject.GetComponent<CustomLogicCollisionHandler>();
                var damage = CalculateDamage();
                if (handler != null)
                {
                    handler.GetHit(_owner, _owner.Name, damage, "BladeThrow", transform.position);
                    continue;
                }
                if (character == null || character == _owner || TeamInfo.SameTeam(character, _team) || character.Dead)
                    continue;
                if (character is BaseTitan)
                {
                    var titan = (BaseTitan)character;
                    Vector3 position = transform.position;
                    position -= _velocity * Time.fixedDeltaTime * 2f;
                    if (collider == titan.BaseTitanCache.NapeHurtbox)
                    {
                        if (!titan.CheckNapeAngle(position, CharacterData.HumanWeaponInfo["Blade"]["RestrictAngle"].AsFloat))
                            continue;
                        if (_owner != null && _owner is Human)
                        {
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                            ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(titan.BaseTitanCache.Neck.position, damage);
                        }
                        transform.Find("BladeHitNape").GetComponent<AudioSource>().Play();
                    }
                    if (titan.BaseTitanCache.Hurtboxes.Contains(collider))
                    {
                        EffectSpawner.Spawn(EffectPrefabs.CriticalHit, transform.position, Quaternion.Euler(270f, 0f, 0f));
                        if (_owner == null || !(_owner is Human))
                            titan.GetHit("Blade", 100, "BladeThrow", collider.name);
                        else
                        {
                            titan.GetHit(_owner, damage, "BladeThrow", collider.name);
                        }
                    }
                }
                else
                {
                    if (_owner != null && _owner is Human)
                    {
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        character.GetHit(_owner, damage, "BladeThrow", collider.name);
                        ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(character.Cache.Transform.position, damage);
                    }
                    else
                        character.GetHit("Blade", 100, "BladeThrow", collider.name);
                }
            }
        }

        int CalculateDamage()
        {
            int damage = Mathf.Max((int)(this.InitialPlayerVelocity.magnitude * 10f *
                CharacterData.HumanWeaponInfo["Blade"]["DamageMultiplier"].AsFloat), 10);
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
            if (!Disabled)
            {
                float speed = Mathf.Max(GetComponent<Rigidbody>().velocity.magnitude, 80f);
                float rotateSpeed = 1600f * speed;
                _blade.RotateAround(_blade.position, _blade.right, Time.deltaTime * rotateSpeed);
            }
        }
    }
}
