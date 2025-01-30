using UnityEngine;
using Cameras;

namespace Characters
{
    class TitanDetection: BaseDetection
    {
        public TitanDetection (BaseCharacter owner): base(owner, true, false)
        {

        }

        protected override void OnRecalculate(BaseCharacter character, float distance)
        {
            if (character is BaseTitan)
            {
                var titan = (BaseTitan)character;
                float radius = titan.GetColliderToggleRadius();
                titan.TitanColliderToggler.SetNearby(Owner, distance < radius);
            }
        }
    }
}
