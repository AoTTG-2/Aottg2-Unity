using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class BaseHoldAttackSpecial : HoldUseable
    {
        protected Human _human;
        protected override float ActiveTime => Mathf.Infinity;
        protected bool _keyIsReset = true;

        public BaseHoldAttackSpecial(BaseCharacter owner) : base(owner)
        {
            _human = (Human)owner;
        }

        public override void SetInput(bool key)
        {
            if (key)
            {
                if (!IsActive && CanUse() && _keyIsReset)
                {
                    IsActive = true;
                    _activeTimeLeft = ActiveTime;
                    Activate();
                    OnUse();
                }
            }
            else if (IsActive)
            {
                IsActive = false;
                Deactivate();
            }
            _keyIsReset = !key;
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