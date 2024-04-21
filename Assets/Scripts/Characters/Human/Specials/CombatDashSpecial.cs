using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class CombatDashSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 2f;
        protected float Range = 200f;
        protected override bool GroundedOnly => false;

        public CombatDashSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 5f;
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.Dash);

            // Add a force in the direction of the cursor
            var target = _human.GetAimPoint();
            var direction = (target - _human.Cache.Transform.position).normalized;
            _human.Cache.Rigidbody.velocity = direction * 100f;
        }

        protected override void Deactivate()
        {

        }
    }
}