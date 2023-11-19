using Effects;
using GameManagers;
using UI;
using UnityEngine;
using Utility;


namespace Characters
{
    class APGWeapon : AmmoWeapon
    {
        private bool _bigShot;
        private float _bigShotTimeLeft = 0f;

        public APGWeapon(BaseCharacter owner, int ammo, int ammoPerRound, float cooldown) : base(owner, ammo, ammoPerRound, cooldown)
        {
        }

        public override void OnFixedUpdate()
        {
            _bigShotTimeLeft -= Time.fixedDeltaTime;
            if (_bigShotTimeLeft <= 0f)
                _bigShot = false;
        }

        public void SetBigShot(bool shot)
        {
            _bigShot = shot;
            if (_bigShot)
                _bigShotTimeLeft = CharacterData.HumanWeaponInfo["APG"]["Radius2Time"].AsFloat;
            else
                _bigShotTimeLeft = 0f;
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
            human.PlaySound(HumanSounds.GunExplodeSound);
            human.HumanCache.APGHit.transform.position = start;
            human.HumanCache.APGHit.transform.rotation = Quaternion.LookRotation(direction);
            var gunInfo = CharacterData.HumanWeaponInfo["APG"];
            var capsule = (CapsuleCollider)human.HumanCache.APGHit._collider;
            if (_bigShot)
            {
                capsule.radius = gunInfo["Radius2"].AsFloat;
                SetBigShot(false);
            }
            else
                capsule.radius = gunInfo["Radius1"].AsFloat;
            float height = capsule.height * 1.2f;
            float radius = capsule.radius * 4f;
            Vector3 midpoint = 0.5f * (start + start + direction * capsule.height);
            object[] settings = new object[] { midpoint + direction * height * 0.5f, midpoint - direction * height * 0.5f,
            radius, radius, 0.25f};
            EffectSpawner.Spawn(EffectPrefabs.APGTrail, start, Quaternion.identity, settings: settings);
            human.HumanCache.APGHit.Activate(0f, 0.1f);
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootGun();
        }
    }
}
