using ApplicationManagers;
using GameManagers;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class AirAcrobaticsSpecial : BaseEmoteSpecial
    {
        protected override float ActiveTime => 3f;
        protected override bool GroundedOnly => false;
        protected int BaseCooldown = 4;
        protected int MaxDashes = 3;
        protected int DashesLeft;
        private const float GrabIFrameDuration = 0.5f;
        private float _previousSpeed = 0f;

        public AirAcrobaticsSpecial(BaseCharacter owner) : base(owner)
        {
            Cooldown = 4f;
            DashesLeft = MaxDashes;
        }

        public void RegisterDash()
        {
            DashesLeft--;
            if (DashesLeft <= 0)
            {
                _activeTimeLeft = 0f;
                Deactivate();
            }
            else
            {
                ((Human)_owner).DodgeWall();
                _human.PlaySound(HumanSounds.Switchback);

                if (DashesLeft == 1)
                {
                    _previousSpeed = _human.Cache.Rigidbody.velocity.magnitude;
                }
            }
        }

        protected override void Activate()
        {
            ((Human)_owner).StartGrabImmunity(GrabIFrameDuration);
            base.Activate();
            ((Human)_owner).DodgeWall();
        }

        protected override void Deactivate()
        {
            if (DashesLeft == 0)
            {
                _human.Cache.Rigidbody.velocity = _human.Cache.Rigidbody.velocity.normalized * Mathf.Min(_previousSpeed, _human.Cache.Rigidbody.velocity.magnitude);
                _human.Cache.Rigidbody.drag = 0f;
            }
            DashesLeft = MaxDashes;
        }
    }
}
