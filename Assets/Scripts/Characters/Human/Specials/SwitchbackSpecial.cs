using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class SwitchbackSpecial : ExtendedUseable
    {
        protected override float ActiveTime => 0.3f;

        public SwitchbackSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 2f;
        }

        public bool RegisterCollision(Human human, Collision collision, float speed)
        {
            if (IsActive)
            {
                human.Cache.Rigidbody.velocity = collision.contacts[0].normal.normalized * Mathf.Max(speed, 20f);
                _activeTimeLeft = 0f;
                IsActive = false;
                human.PlaySound(HumanSounds.Switchback);
                Deactivate();
                return true;
            }
            return false;
        }

        protected override void Deactivate()
        {
            ((Human)_owner).DodgeWall();
        }
    }
}
