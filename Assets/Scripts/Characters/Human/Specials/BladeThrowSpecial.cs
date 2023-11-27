using Projectiles;
using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class BladeThrowSpecial : BaseAttackSpecial
    {
        protected override float ActiveTime => 0.8f;
        protected bool _needActivate;
        protected float Speed = 80f;
        protected float LiveTime = 2f;

        public BladeThrowSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 1f;
        }

        protected override void Activate()
        {
            _needActivate = true;
            _human.StartSpecialAttack(HumanAnimations.SpecialPetra);
        }

        protected override void ActiveFixedUpdate()
        {
            base.ActiveFixedUpdate();
            if (_needActivate && _activeTimeLeft < 0.4f)
            {
                _needActivate = false;
                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                    _human.PlaySound(HumanSounds.OldBladeSwing);
                else
                    _human.PlaySound(HumanSounds.BladeSwing4);
                _human.Setup._part_blade_l.SetActive(false);
                _human.Setup._part_blade_r.SetActive(false);
                ((BladeWeapon)_human.Weapon).CurrentDurability = 0f;
                Vector3 target = _human.GetAimPoint();
                Vector3 direction = (target - _human.Cache.Transform.position).normalized;
                _human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                _human._targetRotation = Quaternion.Euler(0f, _human.TargetAngle, 0f);
                _human.Cache.Transform.rotation = Quaternion.Lerp(_human.Cache.Transform.rotation, _human._targetRotation, Time.deltaTime * 30f);
                Vector3 velocity = _human.Cache.Rigidbody.velocity;
                SpawnBladeProjectile(true, target, velocity);
                SpawnBladeProjectile(false, target, velocity);
            }
        }

        protected void SpawnBladeProjectile(bool left, Vector3 target, Vector3 velocity)
        {
            Vector3 position = _human.Cache.Transform.position + Vector3.up * 2.5f;
            if (left)
                position -= _human.Cache.Transform.right * 0.5f;
            else
                position += _human.Cache.Transform.right * 0.5f;
            Vector3 direction = (target - position).normalized;
            float speed = Mathf.Max(Vector3.Dot(velocity, direction), 0f) + Speed;
            var projectile = (BladeThrowProjectile)ProjectileSpawner.Spawn(ProjectilePrefabs.BladeThrow, position,
                Quaternion.LookRotation(direction), direction * speed, Vector3.zero, LiveTime, _human.photonView.ViewID, "");
            projectile.InitialPlayerVelocity = _owner.Cache.Rigidbody.velocity;
        }
    }
}