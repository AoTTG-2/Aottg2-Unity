using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;
using Spawnables;

namespace Characters
{
    class SupplySpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 0.5f;
        protected override bool GroundedOnly => false;

        public SupplySpecial(BaseCharacter owner): base(owner)
        {
            UsesLeft = -1;
            MaxUses = 1;

            Cooldown = 300;
            SetCooldownLeft(Cooldown);
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.EmoteWave);
        }

        public override bool CanUse()
        {
            return base.CanUse() && (_human.ActOnHorseback || _human.Grounded);
        }

        protected override void Deactivate()
        {
            var rotation = _human.Cache.Transform.rotation.eulerAngles;
            SpawnableSpawner.Spawn(SpawnablePrefabs.Supply, _human.Cache.Transform.position + _human.Cache.Transform.forward * 2f + Vector3.up * 0.5f, 
                Quaternion.Euler(0f, rotation.y, 90f));
            UsesLeft = -1;
            SetCooldownLeft(Cooldown);
        }

        public override void Reset()
        {
            base.Reset();
            SetCooldownLeft(0f);
        }
    }
}
