using Codice.CM.Client.Differences.Graphic;
using Codice.CM.Common;
using Effects;
using log4net.Util;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Characters
{
    class EscapeSpecial : ExtendedUseable
    {
        protected override float ActiveTime => 0.64f;

        public EscapeSpecial(BaseCharacter owner) : base(owner)
        {
            UsesLeft = -1;
            MaxUses = 1;
            Cooldown = 300f;
            ReduceCooldownAmount = 50f;
            SetCooldownLeft(Cooldown);
        }

        public override bool CanUse()
        {
            var human = (Human)_owner;
            if (!human.Weapon.HasDurability()) return false;
            return base.CanUse() && human.State == HumanState.Grab;
        }
        protected override void Activate()
        {
            var human = (Human)_owner;
            if (human.Weapon is BladeWeapon)
            {
                human.CrossFade(HumanAnimations.SpecialJean, 0.1f);
            }
            if (human.Weapon is AHSSWeapon)
            {
                human.CrossFade(HumanAnimations.AHSSShootBoth, 0.1f);
            }
            if (human.Weapon is ThunderspearWeapon)
            {
                human.CrossFade(HumanAnimations.TSShootLAir, 0.1f);
            }
            if (human.Weapon is APGWeapon)
            {
                human.CrossFade(HumanAnimations.AHSSShootBoth, 0.1f);
            }
            
        }

        protected override void Deactivate()
        {
            var human = (Human)_owner;
            if (!human.Dead && human.Grabber != null && human.State == HumanState.Grab)
            {
                human.Ungrab(true, false);
                if(human.Weapon is BladeWeapon)
                {
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, human.HumanCache.BladeHitLeft.transform.position, Quaternion.Euler(270f, 0f, 0f));
                    human.PlaySound(HumanSounds.BladeHit);
                    human.SpecialActionState(0.5f);
                }
                if(human.Weapon is AHSSWeapon)
                {
                    EffectSpawner.Spawn(EffectPrefabs.GunExplode, human.Cache.Transform.position + human.Cache.Transform.up * 0.8f, Quaternion.LookRotation ((human.GetAimPoint() - human.Cache.Transform.position).normalized));
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, human.HumanCache.BladeHitLeft.transform.position, Quaternion.Euler(270f, 0f, 0f));
                    human.PlaySound(HumanSounds.GetRandomAHSSGunShot());
                    human.SpecialActionState(0.5f);
                }
                if (human.Weapon is ThunderspearWeapon)
                {
                    EffectSpawner.Spawn(EffectPrefabs.ThunderspearExplode, human.Cache.Transform.position + human.Cache.Transform.up * 0.8f, Quaternion.LookRotation((human.GetAimPoint() - human.Cache.Transform.position).normalized), 4f, true, new object[] { Color.black, 1, true });
                    EffectSpawner.Spawn(EffectPrefabs.Boom2, human.Cache.Transform.position + human.Cache.Transform.up * 0.8f, Quaternion.LookRotation((human.GetAimPoint() - human.Cache.Transform.position).normalized), 4f, true);
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, human.HumanCache.BladeHitLeft.transform.position, Quaternion.Euler(270f, 0f, 0f));
                    human.PlaySound(HumanSounds.GetRandomTSLaunch());
                    human.SpecialActionState(0.5f);
                }
                if (human.Weapon is APGWeapon)
                {
                    //var gunInfo = CharacterData.HumanWeaponInfo["APG"];
                    //float minRange = gunInfo["MinRange"].AsFloat;
                    //float maxRange = gunInfo["MaxRange"].AsFloat;
                    //if (speed <= gunInfo["Range2Speed"].AsFloat)
                    //    range = range1Const + range1Mult * speed;
                    //else
                    //    range = range2Const + range2Mult * speed;
                    //var capsule = (CapsuleCollider)human.HumanCache.APGHit._collider;

                    //capsule.height = Mathf.Clamp(range, minRange, maxRange);
                    //float height = capsule.height * 1.2f;
                    Vector3 target = human.GetAimPoint();
                    Vector3 direction = (target - human.Cache.Transform.position).normalized;
                    Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 0.8f;
                    direction = (target - start).normalized;
                    //Vector3 midpoint = 0.5f * (start + start + direction * capsule.height);
                    EffectSpawner.Spawn(EffectPrefabs.APGTrail, human.Cache.Transform.position + human.Cache.Transform.up * 0.8f, Quaternion.LookRotation((human.GetAimPoint() - human.Cache.Transform.position).normalized), 4f, true,((APGWeapon)human.Weapon).GetSettings());
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, human.HumanCache.BladeHitLeft.transform.position, Quaternion.Euler(270f, 0f, 0f));
                    human.PlaySound(HumanSounds.GetRandomAPGShot());
                    human.SpecialActionState(0.5f);
                }
            }
            UsesLeft = -1;
            Cooldown = 300;
        }
    }
}
