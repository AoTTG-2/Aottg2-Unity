using Effects;
using Settings;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Characters
{
    class SpinKennySpecial : BaseAttackSpecial
    {
        protected override float ActiveTime => 0.8f;
        protected float AnimationLoopStartTime = 0.35f;
        protected float AnimationLoopEndTime = 0.5f;
        protected int Loops = 3;
        protected int _stage;
        protected AmmoWeapon _weapon;
        CapsuleCollider capsule;
        public SpinKennySpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 5f;
            _weapon = (AmmoWeapon)((Human)owner).Weapon;
            capsule = (CapsuleCollider)_human.HumanCache.APGHit._collider;
        }

        protected override void Activate()
        {
            _stage = 0;
            _human.StartSpecialAttack(HumanAnimations.SpecialLevi);
            Debug.Log("Activate");
        }

        protected override void ActiveFixedUpdate()
        {
            Debug.Log("Active Update");
            base.ActiveFixedUpdate();
            if (!_human.Cache.Animation.IsPlaying(HumanAnimations.SpecialLevi))
                return;
            float time = GetAnimationTime();


            if (_stage == 0 && time > AnimationLoopStartTime)
            {
                if (_weapon is APGWeapon)
                {
                    ShootAPG(_stage);
                    //EffectSpawner.Spawn(EffectPrefabs.APGTrail, _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 0, 0), 4f, true, new object[] { _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 0, 0), 0.4f, 0.4f, 0.25f });
                }
                else if (_weapon is AHSSWeapon)
                {
                    ShootAHSS(_stage);
                }
                else if (_weapon is ThunderspearWeapon)
                {
                    ShootAPG(_stage);
                    _human.PlaySound(HumanSounds.GetRandomTSLaunch());
                }
                _stage += 1;
            }
            else if (_stage < Loops && time > AnimationLoopEndTime)
            {
                Vector3 direction = new Vector3(-1, 0, -1); 
                if (_stage == 1)
                {
                    direction = new Vector3(1, 0, 1);
                }
                else if (_stage == 2) 
                {
                    direction = new Vector3(1, 0, 1);
                }
                _human.PlayAnimation(HumanAnimations.SpecialLevi, AnimationLoopStartTime);
                if (_weapon is APGWeapon)
                {
                    ShootAPG(_stage);
                    //EffectSpawner.Spawn(EffectPrefabs.APGTrail, _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 120 * _stage, 0), 4f, true, new object[] { _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 120 * _stage, 0), 0.4f, 0.4f, 0.25f });
                }
                else if (_weapon is AHSSWeapon)
                {
                    ShootAHSS(_stage);
                }
                else if (_weapon is ThunderspearWeapon)
                {
                    ShootAPG(_stage);
                    _human.PlaySound(HumanSounds.GetRandomTSLaunch());
                }
                _stage += 1;
            }
            Debug.Log(_owner.Cache.Transform.rotation.x);
        }
        public override bool CanUse()
        {
            return (Time.time - _lastUseTime) >= Cooldown && (UsesLeft == -1 || UsesLeft > 0);
        }
        protected float GetAnimationTime()
        {
            return _human.Cache.Animation[HumanAnimations.SpecialLevi].normalizedTime;
        }
        void ShootAPG(int stage)
        {
            Vector3 target = _human.GetAimPoint();
            Vector3 direction = (target - _human.Cache.Transform.position).normalized;
            Vector3 start = _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f;
            direction = (target - start).normalized;
            var capsule = (CapsuleCollider)_human.HumanCache.APGHit._collider;
            capsule.radius = 0.1f;
            float height = capsule.height * 1.2f;
            float radius = capsule.radius * 4f;
            Vector3 midpoint = 0.5f * (start + start + direction * capsule.height);
            EffectSpawner.Spawn(EffectPrefabs.APGTrail, _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 120 * _stage, 0), 4f, true, new object[] { midpoint + direction * height * 0.5f, midpoint - direction * height * 0.5f, radius, radius, 0.25f });
            //EffectSpawner.Spawn(EffectPrefabs.APGTrail, _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 120 * stage, 0), 4f, true, new object[] { _human.transform.position , midpoint - direction * height * 0.5f, 0.4f, 0.4f, 0.25f });
            _human.PlaySound(HumanSounds.GetRandomAPGShot());
            ((CapsuleCollider)_human.HumanCache.APGHit._collider).center = _human.transform.position;
            ((CapsuleCollider)_human.HumanCache.APGHit._collider).direction = 0;
            ((CapsuleCollider)_human.HumanCache.APGHit._collider).height = 4f;
            ((CapsuleCollider)_human.HumanCache.APGHit._collider).radius = 10f;
            _human.HumanCache.APGHit.Activate(0f, 0.1f);
        }
        void ShootAHSS(int stage)
        {
            EffectSpawner.Spawn(EffectPrefabs.GunExplode, _human.Cache.Transform.position + _human.Cache.Transform.up * 0.8f, Quaternion.Euler(0, 120 * stage, 0));
            _human.PlaySound(HumanSounds.GetRandomAHSSGunShot());
            ((CapsuleCollider)_human.HumanCache.AHSSHit._collider).center = _human.transform.position;
            ((CapsuleCollider)_human.HumanCache.AHSSHit._collider).direction = 0;
            ((CapsuleCollider)_human.HumanCache.AHSSHit._collider).height = 4f;
            ((CapsuleCollider)_human.HumanCache.AHSSHit._collider).radius = 10f;
            _human.HumanCache.AHSSHit.Activate(0f, 0.1f);
        }
    }
}
