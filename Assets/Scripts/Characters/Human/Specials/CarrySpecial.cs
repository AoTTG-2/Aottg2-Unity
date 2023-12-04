using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class CarrySpecial : ExtendedUseable
    {
        protected override float ActiveTime => 0.64f;
        public CarrySpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 5f;
        }

        protected override void Activate()
        {
            ((Human)_owner).StartSpecialCarry();
        }

        protected override void Deactivate()
        {

        }
    }
}