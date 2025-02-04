using UnityEngine;
using Cameras;

namespace Characters
{
    class HumanDetection: BaseDetection
    {
        public HumanDetection(BaseCharacter owner): base(owner, false, true)
        {
        }

        protected override void OnRecalculate(BaseCharacter character, float distance)
        {
            var titan = (BaseTitan)character;
            float radius = titan.GetColliderToggleRadius();
            titan.TitanColliderToggler.SetNearby(Owner, distance < radius);
        }
    }
}
