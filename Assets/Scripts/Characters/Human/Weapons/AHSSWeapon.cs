﻿using Effects;
using GameManagers;
using UI;
using UnityEngine;
using Utility;


namespace Characters
{
    class AHSSWeapon : AmmoWeapon
    {
        protected Vector3 _target;

        public AHSSWeapon(BaseCharacter owner, int ammo, int ammoPerRound, float cooldown) : base(owner, ammo, ammoPerRound, cooldown)
        {
        }

        protected override float GetActiveTime()
        {
            return CharacterData.HumanWeaponInfo["AHSS"]["FireDelay"].AsFloat;
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            _target = human.GetAimPoint();
        }

        protected override void Deactivate()
        {
            var human = (Human)_owner;
            Vector3 target = _target;
            Vector3 direction = (target - human.Cache.Transform.position).normalized;
            float cross = Vector3.Cross(human.Cache.Transform.forward, direction).y;
            string anim;
            if (human.Grounded)
            {
                if ((!human.HookLeft.IsHooked() && cross < 0f) || human.HookRight.IsHooked())
                    anim = HumanAnimations.AHSSShootL;
                else
                    anim = HumanAnimations.AHSSShootR;
            }
            else
            {
                if ((!human.HookLeft.IsHooked() && cross < 0f) || human.HookRight.IsHooked())
                    anim = HumanAnimations.AHSSShootLAir;
                else
                    anim = HumanAnimations.AHSSShootRAir;
            }
            human.State = HumanState.Attack;
            human.AttackAnimation = anim;
            human.CrossFade(anim, 0.05f);
            human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
            human._targetRotation = Quaternion.Euler(0f, human.TargetAngle, 0f);
            human.Cache.Transform.rotation = Quaternion.Lerp(human.Cache.Transform.rotation, human._targetRotation, Time.deltaTime * 30f);
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
            direction = (target - start).normalized;
            EffectSpawner.Spawn(EffectPrefabs.GunExplode, start, Quaternion.LookRotation(direction));
            human.PlaySound(HumanSounds.GunExplode);
            var ahssInfo = CharacterData.HumanWeaponInfo["AHSS"];
            var capsule = (CapsuleCollider)human.HumanCache.AHSSHit._collider;
            capsule.radius = ahssInfo["Radius"].AsFloat;
            human.HumanCache.AHSSHit.transform.position = start;
            human.HumanCache.AHSSHit.transform.rotation = Quaternion.LookRotation(direction);
            human.HumanCache.AHSSHit.Activate(0f, 0.1f);
            human.Cache.Rigidbody.AddForce(-direction * ahssInfo["KnockbackForce"].AsFloat, ForceMode.VelocityChange);
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootAHSS(RoundLeft == 1, RoundLeft == 0);
        }
    }
}
