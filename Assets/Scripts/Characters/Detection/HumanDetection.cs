using UnityEngine;
using Cameras;

namespace Characters
{
    class HumanDetection: BaseDetection
    {
        public HumanDetection(BaseCharacter owner, bool enemiesOnly = false, bool titansOnly = true): base(owner, enemiesOnly, titansOnly)
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
    }
}
