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
            Cooldown = 3f;
        }

        protected override void Activate()
        {
            var owner = (Human)_owner;
            owner.CancelHookBothKey = true;
            owner.CancelHookLeftKey = true;
            owner.CancelHookRightKey = true;
            owner.HookLeft.SetInput(false);
            owner.HookRight.SetInput(false);
            owner.HookLeft.DisableAnyHook();
            owner.HookRight.DisableAnyHook();
            owner.StartSpecialCarry();
        }

        protected override void Deactivate()
        {

        }
    }
}