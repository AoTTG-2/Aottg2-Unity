using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;
using Utility;

namespace Characters
{
    class SmellSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 30f;
        protected float Range = 600f;
        protected override bool GroundedOnly => false;
        protected float Delay = 1f;

        public SmellSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 60f;
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.SpecialShifter);
            foreach (var titan in ((InGameManager)SceneLoader.CurrentGameManager).Titans)
            {
                float distance = Vector3.Distance(_human.Cache.Transform.position, titan.Cache.Transform.position);
                if (distance < Range)
                {
                    float delay = Util.LinearMap(distance, 0, Range, 0, Delay);
                    titan.Reveal(delay, ActiveTime);
                }
                    
            }
        }

        protected override void Deactivate()
        {
        }
    }
}
