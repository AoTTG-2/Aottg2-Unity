using Effects;
using GameManagers;
using Settings;
using UI;
using UnityEngine;
using Utility;


namespace Characters
{
    class APGWeapon : AmmoWeapon
    {
        public APGWeapon(BaseCharacter owner, int ammo, int ammoPerRound, float cooldown) : base(owner, ammo, ammoPerRound, cooldown)
        {
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            string anim = "";
            bool left = !human.HookLeft.IsHooked();
            if (human.Grounded)
            {
                if (left)
                    anim = HumanAnimations.AHSSShootL;
                else
                    anim = HumanAnimations.AHSSShootR;
            }
            else
            {
                if (left)
                    anim = HumanAnimations.AHSSShootLAir;
                else
                    anim = HumanAnimations.AHSSShootRAir;
            }
            human.State = HumanState.Attack;
            human.AttackAnimation = anim;
            human.CrossFade(anim, 0.05f);
            Vector3 target = human.GetAimPoint();
            Vector3 direction = (target - human.Cache.Transform.position).normalized;
            human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
            human._targetRotation = Quaternion.Euler(0f, human.TargetAngle, 0f);
            human.Cache.Transform.rotation = Quaternion.Lerp(human.Cache.Transform.rotation, human._targetRotation, Time.deltaTime * 30f);
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
            direction = (target - start).normalized;
            EffectSpawner.Spawn(EffectPrefabs.GunExplode, start, Quaternion.LookRotation(direction), 0.2f);
            human.PlaySound(HumanSounds.GetRandomAPGShot());
            human.HumanCache.APGHit.transform.position = start;
            human.HumanCache.APGHit.transform.rotation = Quaternion.LookRotation(direction);
            var gunInfo = CharacterData.HumanWeaponInfo["APG"];
            if (SettingsManager.InGameCurrent.Misc.APGPVP.Value)
                gunInfo = CharacterData.HumanWeaponInfo["APGPVP"];
            var capsule = (CapsuleCollider)human.HumanCache.APGHit._collider;
            capsule.radius = gunInfo["Radius"].AsFloat;
            float range1Mult = gunInfo["Range1Multiplier"].AsFloat;
            float range2Mult = gunInfo["Range2Multiplier"].AsFloat;
            float range1Const = gunInfo["Range1Constant"].AsFloat;
            float range2Const = gunInfo["Range2Constant"].AsFloat;
            float minRange = gunInfo["MinRange"].AsFloat;
            float maxRange = gunInfo["MaxRange"].AsFloat;
            float speed = human.Cache.Rigidbody.velocity.magnitude;
            float range;
            if (speed <= gunInfo["Range2Speed"].AsFloat)
                range = range1Const + range1Mult * speed;
            else
                range = range2Const + range2Mult * speed;
            capsule.height = Mathf.Clamp(range, minRange, maxRange);
            capsule.center = new Vector3(0f, 0f, capsule.height * 0.5f + 0.5f);
            float height = capsule.height * 1.2f;
            float radius = capsule.radius * 4f;
            Vector3 midpoint = 0.5f * (start + start + direction * capsule.height);
            object[] settings = new object[] { midpoint + direction * height * 0.5f, midpoint - direction * height * 0.5f,
            radius, radius, 0.25f};
            EffectSpawner.Spawn(EffectPrefabs.APGTrail, start, Quaternion.identity, settings: settings);
            human.HumanCache.APGHit.Activate(0f, 0.1f);
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootAPG();
        }
    }
}
