using Effects;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class StockSpecial : HoldUseable
    {
        public StockSpecial(BaseCharacter owner) : base(owner)
        {
        }

        public override bool CanUse()
        {
            return base.CanUse() && ((Human)_owner).State == HumanState.Idle;
        }

        protected override void Activate()
        {
            ((Human)_owner).StartBladeSwing();
        }

        protected override void Deactivate()
        {
        }
    }
}
