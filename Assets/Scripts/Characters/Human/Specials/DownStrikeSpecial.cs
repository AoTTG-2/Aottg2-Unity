using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class DownStrikeSpecial : BaseHoldAttackSpecial
    {
        protected bool _needActivate;

        public DownStrikeSpecial(BaseCharacter owner): base(owner)
        {
            Cooldown = 5f;
        }

        protected override void Activate()
        {
            _needActivate = true;
            _human.StartSpecialAttack(HumanAnimations.SpecialMikasa1);
        }

        protected override void ActiveFixedUpdate()
        {
            base.ActiveFixedUpdate();
            if (_needActivate)
            {
                _needActivate = false;
                _human.Cache.Rigidbody.velocity = Vector3.zero;
                _human.ActivateBlades();
                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                    _human.PlaySound(HumanSounds.OldBladeSwing);
                else
                    _human.PlaySound(HumanSounds.BladeSwing4);
            }

            _human.Cache.Rigidbody.AddForce(Vector3.down * 10f, ForceMode.VelocityChange);

            if (_human.Grounded || _human.HookLeft.IsActive || _human.HookRight.IsActive)
            {
                IsActive = false;
                Deactivate();
            }
        }

        public override bool CanUse()
        {
            return base.CanUse() && !_human.Grounded && !_human.HookLeft.IsActive && !_human.HookRight.IsActive;
        }
    }
}
