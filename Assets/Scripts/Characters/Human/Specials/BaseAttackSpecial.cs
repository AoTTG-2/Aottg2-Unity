using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class BaseAttackSpecial : ExtendedUseable
    {
        protected Human _human;

        public BaseAttackSpecial(BaseCharacter owner): base(owner)
        {
            _human = (Human)owner;
        }

        public override bool CanUse()
        {
            return base.CanUse() && _human.CanBladeAttack();
        }

        protected bool InSpecial()
        {
            return _human.State == HumanState.SpecialAttack;
        }

        protected override void Deactivate()
        {
            _human.Idle();
        }

        protected override void ActiveFixedUpdate()
        {
            if (!InSpecial())
                IsActive = false;
        }
    }
}
