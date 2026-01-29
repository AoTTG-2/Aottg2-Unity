using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class DistractSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 1f;
        protected float Range = 300f;
        protected override bool GroundedOnly => false;

        public DistractSpecial(BaseCharacter owner ): base(owner)
        {
            Cooldown = 20f;
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.SpecialMarco0);
        }

        protected override void Deactivate()
        {
            if (InSpecial())
            {
                foreach (var titan in ((InGameManager)SceneLoader.CurrentGameManager).Titans)
                {
                    if (Vector3.Distance(_human.Cache.Transform.position, titan.Cache.Transform.position) < Range)
                        titan.Distract(_human);
                }
            }
        }
    }
}
