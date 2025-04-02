using Projectiles;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class FlareItem1 : SimpleUseable
    {
        Color _color;
        float Speed = 450f;
        Vector3 Gravity = Vector3.down * 20f;

        public FlareItem1(BaseCharacter owner, string name, Color color, float cooldown): base(owner)
        {
            Name = name;
            _color = color;
            Cooldown = cooldown;
        }

        protected override void Activate()
        {
            var human = (Human)_owner;
            Vector3 target = human.GetAimPoint();
            Vector3 start = human.Cache.Transform.position + human.Cache.Transform.up * 2f;
            Vector3 direction = (target - start).normalized;
            ProjectileSpawner.Spawn(ProjectilePrefabs.FlareLight, start, Quaternion.identity, direction * Speed, Gravity, 6.5f, _owner.Cache.PhotonView.ViewID,
                "", new object[] { _color });
            human.PlaySound(HumanSounds.FlareLaunch);
        }
    }
}
