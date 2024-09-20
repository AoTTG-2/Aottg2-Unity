using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class CarrySpecial : ExtendedUseable
    {
        protected override float ActiveTime => 0.64f;
        protected float CarryDistance => 10f;
        public CarrySpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 2f;
        }

        protected override void Activate()
        {
            var owner = (Human)_owner;
            if (owner.BackHuman != null)
            {
                owner.StopCarrySpecial();
                return;
            }
            var target = owner.GetCarryOption(CarryDistance);
            if (target != null)
                owner.StartCarrySpecial(target);
        }

        protected override void Deactivate()
        {

        }
    }
}