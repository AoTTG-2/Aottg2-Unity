using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class Spin1Special : BaseAttackSpecial
    {
        protected override float ActiveTime => 0.8f;
        protected float AnimationLoopStartTime = 0.35f;
        protected float AnimationLoopEndTime = 0.5f;
        protected int Loops = 3;
        protected int _stage;

        public Spin1Special(BaseCharacter owner): base(owner)
        {
            Cooldown = 5f;
        }

        protected override void Activate()
        {
            _stage = 0;
            _human.StartSpecialAttack(HumanAnimations.SpecialLevi);
        }

        protected override void ActiveFixedUpdate()
        {
            base.ActiveFixedUpdate();
            if (!_human.Cache.Animation.IsPlaying(HumanAnimations.SpecialLevi))
                return;
            float time = GetAnimationTime();
            if (_stage == 0 && time > AnimationLoopStartTime)
            {
                _human.ActivateBlades();
                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                    _human.PlaySound(HumanSounds.OldBladeSwing);
                else
                    _human.PlaySound(HumanSounds.BladeSwing3);
                _stage += 1;
            }
            else if (_stage < Loops && time > AnimationLoopEndTime)
            {
                _human.PlayAnimation(HumanAnimations.SpecialLevi, AnimationLoopStartTime);
                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                    _human.PlaySound(HumanSounds.OldBladeSwing);
                else
                    _human.PlaySound(HumanSounds.BladeSwing3);
                _stage += 1;
            }
        }

        protected float GetAnimationTime()
        {
            return _human.Cache.Animation[HumanAnimations.SpecialLevi].normalizedTime;
        }
    }
}
