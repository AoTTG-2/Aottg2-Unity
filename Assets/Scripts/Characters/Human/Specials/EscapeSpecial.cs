using Effects;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class EscapeSpecial : ExtendedUseable
    {
        protected override float ActiveTime => 0.64f;

        public EscapeSpecial(BaseCharacter owner) : base(owner)
        {
            UsesLeft = MaxUses = 1;
        }

        public override bool CanUse()
        {
            return base.CanUse() && ((Human)_owner).State == HumanState.Grab;
        }

        protected override void Activate()
        {
            ((Human)_owner).CrossFade(HumanAnimations.SpecialJean, 0.1f);
        }

        protected override void Deactivate()
        {
            var human = (Human)_owner;
            if (!human.Dead && human.Grabber != null && human.State == HumanState.Grab)
            {
                human.Ungrab(true, false);
                EffectSpawner.Spawn(EffectPrefabs.Blood1, human.HumanCache.BladeHitLeft.transform.position, Quaternion.Euler(270f, 0f, 0f));
                human.PlaySound(HumanSounds.BladeHit);
                human.SpecialActionState(0.5f);
            }
        }
    }
}
