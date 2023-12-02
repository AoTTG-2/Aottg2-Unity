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

        public SupplySpecial(BaseCharacter owner): base(owner)
        {
            UsesLeft = MaxUses = 1;
            Cooldown = 300;
        }

        protected override void Activate()
        {
            _human.EmoteAnimation(HumanAnimations.EmoteWave);
        }

        protected override void Deactivate()
        {
            var rotation = _human.Cache.Transform.rotation.eulerAngles;
            SpawnableSpawner.Spawn(SpawnablePrefabs.Supply, _human.Cache.Transform.position + _human.Cache.Transform.forward * 2f + Vector3.up * 0.5f, 
                Quaternion.Euler(0f, rotation.y, 90f));
        }
    }
}
