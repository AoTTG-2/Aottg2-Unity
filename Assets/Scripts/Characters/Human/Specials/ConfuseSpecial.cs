using ApplicationManagers;
using GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    class ConfuseSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 10f;
        protected override bool GroundedOnly => false;
        protected float Range = 250f;
        public List<BasicTitan> AffectedTitans = new List<BasicTitan>();

        public ConfuseSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 30f;
        }
        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.Dodge);
            foreach (var titan in ((InGameManager)SceneLoader.CurrentGameManager).Titans)
            {
                float distance = Vector3.Distance(_human.Cache.Transform.position, titan.Cache.Transform.position);
                if (distance < Range)
                {
                    titan.AttackSpeedMultiplier = titan.AttackSpeedMultiplier*0.67f;
                    AffectedTitans.Add(titan);
                }
            }
        }
        protected override void Deactivate()
        {
            foreach (var titan in AffectedTitans)
            {
                titan.AttackSpeedMultiplier = titan.AttackSpeedMultiplier/0.67f;
            }
            AffectedTitans.Clear();
        }
    }
}
