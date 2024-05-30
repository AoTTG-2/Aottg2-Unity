using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class BaseHoldAttackSpecial : HoldUseable
    {
        protected Human _human;
        protected override float ActiveTime => Mathf.Infinity;

        public BaseHoldAttackSpecial(BaseCharacter owner) : base(owner)
        {
            _human = (Human)owner;
        }

        public override bool CanUse()
        {
            return base.CanUse();
        }

        protected override void Deactivate()
        {
            _human.Idle();
        }
    }
}