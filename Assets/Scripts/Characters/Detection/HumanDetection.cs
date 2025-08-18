using UnityEngine;
using Cameras;

namespace Characters
{
    class HumanDetection : BaseDetection
    {
        public HumanDetection(BaseCharacter owner, bool enemiesOnly = false, bool titansOnly = true) : base(owner, enemiesOnly, titansOnly)
        {
        }

        protected override void OnRecalculate(BaseCharacter character, float distance)
        {
            if (character is BaseTitan titan)
            {
                float radius = titan.GetColliderToggleRadius();
                titan.TitanColliderToggler.SetNearby(Owner, distance < radius);
            }
        }

        public override void OnFixedUpdate()
        {
            if (ClosestEnemy != null && ClosestEnemy.Dead)
                ClosestEnemy = null; // reset ClosestEnemy if enemy is dead. Otherwise, ai will be in a daze for a while
            base.OnFixedUpdate();
        }
    }
}
