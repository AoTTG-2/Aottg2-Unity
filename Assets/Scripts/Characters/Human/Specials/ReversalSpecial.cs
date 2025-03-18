using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class ReversalSpecial : SwitchbackSpecial
    {
        private const float GrabIFrameDuration = 0.5f;
        protected override float ActiveTime => 0.3f;

        public ReversalSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 2f;
        }

        override public bool RegisterCollision(Human human, Collision collision, float speed)
        {
            if (IsActive)
            {
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                human.Cache.Rigidbody.velocity = -1 * cameraRay.direction.normalized * Mathf.Max(speed, 20f);
                _activeTimeLeft = 0f;
                IsActive = false;
                human.PlaySound(HumanSounds.Switchback);
                Deactivate();
                return true;
            }
            return false;
        }

        protected override void Activate()
        {
            ((Human)_owner).StartGrabImmunity(GrabIFrameDuration);
            base.Activate();
        }

        protected override void Deactivate()
        {
            ((Human)_owner).DodgeWall();
        }
    }
}
