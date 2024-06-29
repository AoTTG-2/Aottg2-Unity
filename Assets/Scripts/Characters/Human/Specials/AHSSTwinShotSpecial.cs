using Effects;
using Settings;
using System.Collections;
using UI;
using UnityEngine;

namespace Characters
{
    class AHSSTwinShot : SimpleUseable
    {
        public AHSSTwinShot(BaseCharacter owner): base(owner)
        {
            Cooldown = 10f;
        }

        public override bool CanUse()
        {
            var weapon = ((Human)_owner).Weapon;
            if (weapon is AmmoWeapon)
            {
                var ammo = (AmmoWeapon)weapon;
                return base.CanUse() && (ammo.RoundLeft >= 2 || ammo.RoundLeft == -1);
            }
            return false;
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            Vector3 target = human.GetAimPoint();
            Vector3 direction = (target - human.Cache.Transform.position).normalized;
            float cross = Vector3.Cross(human.Cache.Transform.forward, direction).y;
            string anim;
            if (human.Grounded)
            {
                anim = HumanAnimations.AHSSShootBoth;
            }
            else
            {
                anim = HumanAnimations.AHSSShootBothAir;
            }
            human.State = HumanState.Attack;
            human.AttackAnimation = anim;
            human.CrossFade(anim, 0.05f);
            human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
            human._targetRotation = Quaternion.Euler(0f, human.TargetAngle, 0f);
            human.Cache.Transform.rotation = Quaternion.Lerp(human.Cache.Transform.rotation, human._targetRotation, Time.deltaTime * 30f);
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
            direction = (target - start).normalized;
            EffectSpawner.Spawn(EffectPrefabs.GunExplode, start, Quaternion.LookRotation(direction), 2f);
            human.PlaySound(HumanSounds.GetRandomAHSSGunShotDouble());
            var ahssInfo = CharacterData.HumanWeaponInfo["AHSS"];
            var capsule = (CapsuleCollider)human.HumanCache.AHSSHit._collider;
            capsule.radius = ahssInfo["Radius"].AsFloat * 2f;
            human.HumanCache.AHSSHit.transform.position = start;
            human.HumanCache.AHSSHit.transform.rotation = Quaternion.LookRotation(direction);
            human.HumanCache.AHSSHit.Activate(0f, 0.1f);
            human.Cache.Rigidbody.AddForce(-direction * ahssInfo["KnockbackForce"].AsFloat * 2f, ForceMode.VelocityChange);
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootAHSS(true, true);
            ((AmmoWeapon)human.Weapon).RoundLeft -= 2;
        }
    }
}
