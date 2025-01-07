using ApplicationManagers;
using Effects;
using Projectiles;
using Settings;
using SimpleJSONFixed;
using System.Threading;
using UI;
using UnityEngine;
using Utility;

namespace Characters
{
    class ThunderspearWeapon : AmmoWeapon
    {
        public ThunderspearProjectile Current;
        public float Radius;
        public float Speed;
        public float TravelTime;
        public float Delay;
        private float _delayTimeLeft;
        private JSONNode TSInfo;

        public ThunderspearWeapon(BaseCharacter owner, int ammo, int ammoPerRound, float cooldown, float radius, float speed,
            float travelTime, float delay, JSONNode tsInfo) : base(owner, ammo, ammoPerRound, cooldown)
        {
            Radius = radius;
            Speed = speed;
            TravelTime = travelTime;
            Delay = delay;
            TSInfo = tsInfo;
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            Vector3 target = human.GetAimPoint();
            Vector3 direction = (target - human.Cache.Transform.position).normalized;
            float cross = Vector3.Cross(human.Cache.Transform.forward, direction).y;
            Vector3 spawnPosition;
            bool hasLeft = IsModelActive(human, true);
            bool hasRight = IsModelActive(human, false);
            bool twoShot = IsTwoShotMode();
            if ((hasLeft && cross < 0f && human.State != HumanState.Land) || !hasRight)
            {
                if(human.CurrentSpecial == "Thunderspears")
                    spawnPosition = human.Setup._part_ts_l.transform.position;
                else
                    spawnPosition = human.Setup._part_blade_l.transform.position;
                human.PlaySound(HumanSounds.GetRandomTSLaunch());
                human.SetThunderspears(false, hasRight || !twoShot);
                if (human.Grounded)
                    human.AttackAnimation = HumanAnimations.TSShootL;
                else
                    human.AttackAnimation = HumanAnimations.TSShootLAir;
            }
            else
            {
                if(human.CurrentSpecial == "Thunderspears")
                    spawnPosition = human.Setup._part_ts_r.transform.position;
                else
                    spawnPosition = human.Setup._part_blade_r.transform.position;
                human.PlaySound(HumanSounds.GetRandomTSLaunch());
                human.SetThunderspears(hasLeft || !twoShot, false);
                if (human.Grounded)
                    human.AttackAnimation = HumanAnimations.TSShootR;
                else
                    human.AttackAnimation = HumanAnimations.TSShootRAir;
            }
            if (human.Grounded)
                spawnPosition = human.Setup._part_head.transform.position + Vector3.up * 1f;
            Vector3 spawnDirection = (target - spawnPosition).normalized;
            if (human.Grounded)
                spawnPosition += spawnDirection * 1f;
            if (human.State != HumanState.Slide)
            {
                if (human.State == HumanState.Attack)
                    human._attackButtonRelease = true;
                human.PlayAnimation(human.AttackAnimation, 0.1f);
                human.State = HumanState.Attack;
                human.TargetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                human._targetRotation = Quaternion.Euler(0f, human.TargetAngle, 0f);
            }
            var initialVelocity = (human.CarryState == HumanCarryState.Carry && human.Carrier != null) ? human.Carrier.CarryVelocity : human.Cache.Rigidbody.velocity;
            float speed = Speed;
            float travelTime = TravelTime;
            if (!SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
            {
                speed = Speed + initialVelocity.magnitude * TSInfo["SpeedMultiplier"].AsFloat;
                travelTime = (Mathf.Clamp(TSInfo["Range"].AsFloat + initialVelocity.magnitude * TSInfo["RangeMultiplier"].AsFloat,
                TSInfo["RangeMin"].AsFloat, TSInfo["RangeMax"].AsFloat)) / speed;
            }
            Current = (ThunderspearProjectile)ProjectileSpawner.Spawn(ProjectilePrefabs.Thunderspear, spawnPosition, Quaternion.LookRotation(spawnDirection),
                spawnDirection * speed, Vector3.zero, travelTime, human.Cache.PhotonView.ViewID, "", new object[] { Radius, SettingsManager.AbilitySettings.BombColor.Value.ToColor() });
            Current.InitialPlayerVelocity = initialVelocity;
            _delayTimeLeft = Delay;
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShootTS();
        }

        public bool HasActiveProjectile()
        {
            return Current != null && !Current.Disabled;
        }

        public override void SetInput(bool key)
        {
            if (key)
            {
                if (HasActiveProjectile() && _delayTimeLeft <= 0f)
                {
                    Current.Explode();
                    Current = null;
                }
                else if (CanUse())
                {
                    Activate();
                    OnUse();
                }
            }
        }

        public override void OnFixedUpdate()
        {
            var human = (Human)_owner;
            if (human.Dead)
            {
                if (Current != null)
                {
                    Current.DestroySelf();
                    Current = null;
                }
            }
            else
            {
                if (CanUse())
                {
                    if (human.State != HumanState.Reload && (!human.Setup._part_blade_l.activeSelf || !human.Setup._part_blade_r.activeSelf))
                    {
                        if (!IsTwoShotMode() || RoundLeft >= 2)
                            human.SetThunderspears(true, true);
                    }
                }
                if (Current != null && !Current.Disabled)
                {
                    _delayTimeLeft -= Time.fixedDeltaTime;
                }
            }
        }

        private bool IsModelActive(Human human, bool left)
        {
            if (left) { 
                if(human.CurrentSpecial == "Thunderspears")
                    return human.Setup._part_ts_l.activeSelf;
                return human.Setup._part_blade_l.activeSelf;
            }
            else {
                if (human.CurrentSpecial == "Thunderspears")
                    return human.Setup._part_ts_r.activeSelf;
                return human.Setup._part_blade_r.activeSelf;
            }
        }

        private bool IsTwoShotMode()
        {
            return !SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value && MaxRound == 2;
        }
        public override void Reset()
        {
            base.Reset();
            var human = (Human)_owner;
            RoundLeft = 2;
            human.SetThunderspears(true, true);
        }
    }
}
