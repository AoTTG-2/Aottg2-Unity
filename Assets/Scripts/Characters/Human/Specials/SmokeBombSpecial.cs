using Projectiles;
using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class SmokeBombSpecial : SimpleUseable
    {
        float Speed = 150f;
        Vector3 Gravity = Vector3.down * 15f;

        public SmokeBombSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 15f;
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            Vector3 target = human.GetAimPoint();
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 2f;
            Vector3 direction = (target - start).normalized;
            ProjectileSpawner.Spawn(ProjectilePrefabs.SmokeBomb, start, Quaternion.identity, direction * Speed, Gravity, 6.5f, _owner.Cache.PhotonView.ViewID,
                "", new object[0]);
            human.PlaySound(HumanSounds.FlareLaunch);
        }
    }
}
