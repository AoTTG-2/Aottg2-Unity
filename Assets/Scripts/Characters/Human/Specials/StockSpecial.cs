using Effects;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class StockSpecial : BaseHoldAttackSpecial
    {
        public StockSpecial(BaseCharacter owner) : base(owner)
        {
        }

        public override bool CanUse()
        {
            return base.CanUse() && _human.State == HumanState.Idle;
        }

        protected override void Activate()
        {
            ((Human)_owner).StartBladeSwing();
        }

        protected override void ActiveFixedUpdate()
        {
            base.ActiveFixedUpdate();
            if (_human.Cache.Animation[_human.AttackAnimation].normalizedTime >= 0.32f)
                _human.PauseAnimation();
            if (_human.Grounded)
            {
                IsActive = false;
                Deactivate();
            }
        }

        protected override void Deactivate()
        {
            _human.ContinueAnimation();
        }
    }
}
