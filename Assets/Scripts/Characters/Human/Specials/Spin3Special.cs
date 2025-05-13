﻿using Settings;
using System.Collections;
using UnityEngine;

namespace Characters
{
    class Spin3Special : BaseAttackSpecial
    {
        protected override float ActiveTime => 1f;
        protected float AnimationLoopStartTime = 0.35f;
        protected float AnimationLoopEndTime = 0.5f;
        protected int Loops = 3;
        protected int _stage;
        protected Vector3 _aimPoint;
        protected bool _pulled;
        protected bool _startSpin;
        protected float PullForce = 13f;

        public Spin3Special(BaseCharacter owner) : base(owner)
        {
            Cooldown = 3.5f;
        }

        protected override void Activate()
        {
            if (_human.MountState != HumanMountState.None)
            {
                _human.Unmount(true);
            }
            _stage = 0;
            _human.HookLeft.DisableAnyHook();
            _human.HookRight.DisableAnyHook();
            if (_human.Stats.CurrentGas > 0f)
                _human.HookRight.SetInput(true);
            _aimPoint = _human.GetAimPoint();
            _pulled = false;
            _startSpin = false;
        }

        protected override void ActiveFixedUpdate()
        {
            if (!_startSpin)
            {
                if (_activeTimeLeft <= 1f)
                {
                    _human.StartSpecialAttack(HumanAnimations.SpecialLevi);
                    _human.Cache.Rigidbody.velocity += (Vector3)(Vector3.up * 5f);
                    _startSpin = true;
                }
                return;
            }
            base.ActiveFixedUpdate();
            if (_human.Animation.IsPlaying(HumanAnimations.SpecialLevi))
            {
                float time = GetAnimationTime();
                if (_human.Grounded && time > 0.4f && time < 0.61f)
                {
                    _human.Cache.Rigidbody.AddForce((Vector3)(_human.transform.forward * 200f));
                }
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
                if (_stage == 3)
                {
                    _human.HookRight.SetInput(false);
                    _human.HookLeft.DisableAnyHook();
                    _human.HookRight.DisableAnyHook();
                    _stage += 1;
                }
            }
            if (_human.HookRight.HasHook())
            {
                var state = _human.HookRight.GetHookState();
                if (state == HookState.DisablingHooked || state == HookState.Hooked)
                {
                    if (!_pulled)
                    {
                        _pulled = true;
                        var position = _human.HookRight.GetHookPosition();
                        _human.Cache.Rigidbody.AddForce((position - _human.Cache.Rigidbody.position).normalized * PullForce, ForceMode.Impulse);
                    }
                }
            }
        }

        protected float GetAnimationTime()
        {
            return _human.Animation.GetNormalizedTime(HumanAnimations.SpecialLevi);
        }
    }
}
