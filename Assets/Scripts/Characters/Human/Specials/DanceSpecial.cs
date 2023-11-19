using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class DanceSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 2f;
        protected float Range = 200f;

        public DanceSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 5f;
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.SpecialArmin);
        }

        protected override void Deactivate()
        {
            if (InSpecial())
            {
                foreach (var titan in ((InGameManager)SceneLoader.CurrentGameManager).Titans)
                {
                    if (Vector3.Distance(_human.Cache.Transform.position, titan.Cache.Transform.position) < Range)
                        titan.Laugh(_human);
                }
            }
        }
    }
}
