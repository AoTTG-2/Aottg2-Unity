using Effects;
using Settings;
using UI;
using UnityEngine;

namespace Characters
{
    class APGRapidFire : BaseHoldAttackSpecial
    {
        int _MaxShotCount;
        float _waitBeforeShot = 0.15f;
        float _lastShotTime = Time.deltaTime;
        public APGRapidFire(BaseCharacter owner) : base(owner)
        {
            Cooldown = 3f;
        }
        public override bool CanUse()
        {
            var weapon = ((Human)_owner).Weapon;
            if (weapon is AmmoWeapon)
            {
                var ammo = (AmmoWeapon)weapon;
                return base.CanUse() && (ammo.RoundLeft > 0 || ammo.RoundLeft == -1);
            }
            return false;
        }
        protected override void Activate()
        {
            var weapon = (AmmoWeapon)((Human)_owner).Weapon;
            _MaxShotCount = weapon.RoundLeft;
            ((Human)_owner).HumanCache.APGHit.Activate(0f);
        }
        protected override void OnUse()
        {
            if (UsesLeft > 0)
                UsesLeft--;
        }
        protected override void Deactivate()
        {
            _lastUseTime = Time.time;
            ((Human)_owner).HumanCache.APGHit.Deactivate();
        }
        protected override void ActiveFixedUpdate()
        {
            if (_MaxShotCount > 0)
            {
                var human = (Human)_owner;

                Vector3 target = human.GetAimPoint();
                Vector3 direction = (target - human.Cache.Transform.position).normalized;
                Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
                direction = (target - start).normalized;
                human.HumanCache.APGHit.transform.position = start;
                human.HumanCache.APGHit.transform.rotation = Quaternion.LookRotation(direction);

                //var gunInfo = CharacterData.HumanWeaponInfo["APGPVP"];
                //var capsule = (CapsuleCollider)human.HumanCache.APGHit._collider;
                //float height = capsule.height * 1.2f;
                //float radius = capsule.radius * 4f;
                //capsule.radius = gunInfo["Radius"].AsFloat;
            _lastShotTime += Time.deltaTime;
            if ((_lastShotTime - _waitBeforeShot >= 0) &&(_MaxShotCount > 0))
            {
                Shoot();
                _MaxShotCount--;
                    ((AmmoWeapon)human.Weapon).RoundLeft--;
                _lastShotTime = Time.deltaTime;
            }
            }
            else
            {
                Deactivate();
            }
        }
        private void Shoot()
        {
            var human = (Human)_owner;
            var capsule = (CapsuleCollider)human.HumanCache.APGHit._collider;
            float height = capsule.height * 1.2f;
            float radius = capsule.radius * 4f;
            
            

            Vector3 target = human.GetAimPoint();
            Vector3 direction = (target - human.Cache.Transform.position).normalized;
            string anim;
            if (human.Grounded)
            {
                if (_MaxShotCount % 2 == 1)
                    anim = HumanAnimations.AHSSShootL;
                else
                    anim = HumanAnimations.AHSSShootR;
            }
            else
            {
                if (_MaxShotCount % 2 == 0)
                    anim = HumanAnimations.AHSSShootLAir;
                else
                    anim = HumanAnimations.AHSSShootRAir;
            }
            human.AttackAnimation = anim;
            human.CrossFade(anim, 0.05f);
            human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
            human._targetRotation = Quaternion.Euler(0f, human.TargetAngle, 0f);
            human.Cache.Transform.rotation = Quaternion.Lerp(human.Cache.Transform.rotation, human._targetRotation, Time.deltaTime * 30f);
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
            direction = (target - start).normalized;
            //EffectSpawner.Spawn(EffectPrefabs.APGTrail, start, Quaternion.LookRotation(direction), 2f);
            Vector3 midpoint = 0.5f * (start + start + direction * capsule.height);
            EffectSpawner.Spawn(EffectPrefabs.APGTrail, human.Cache.Transform.position + human.Cache.Transform.up * 0.8f, Quaternion.LookRotation((human.GetAimPoint() - human.Cache.Transform.position).normalized), 4f, true, new object[] { midpoint + direction * height * 0.5f, midpoint - direction * height * 0.5f, radius, radius, 0.25f });
            human.PlaySound(HumanSounds.GetRandomAPGShot());
            var gunInfo = CharacterData.HumanWeaponInfo["APG"];
            if (SettingsManager.InGameCurrent.Misc.APGPVP.Value)
                gunInfo = CharacterData.HumanWeaponInfo["APGPVP"];
            
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootAPG();
        }
    }
}
