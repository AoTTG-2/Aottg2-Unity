using System.Collections;
using UnityEngine;

namespace Characters
{
    class ShifterTransformSpecial : ExtendedUseable
    {
        protected float LiveTime = 60f;
        protected string _shifter;
        protected override float ActiveTime => 0.8f;

        public ShifterTransformSpecial(BaseCharacter owner, string shifter): base(owner)
        {
            Cooldown = 60f;
            _shifter = shifter;
            SetCooldownLeft(Cooldown);
        }

        protected override void Activate()
        {
            ((Human)_owner).EmoteAnimation(HumanAnimations.SpecialShifter);
        }

        protected override void Deactivate()
        {
            var human = (Human)_owner;
            if (!human.Dead)
                human.TransformShifter(_shifter, LiveTime);
        }
    }
}
