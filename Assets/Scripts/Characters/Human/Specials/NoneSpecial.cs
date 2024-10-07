using Effects;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class NoneSpecial : SimpleUseable
    {
        public NoneSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 0f;
        }

        public override bool CanUse()
        {
            return false;
        }

        protected override void Activate()
        {
        }

        protected override void Deactivate()
        {
        }
    }
}
