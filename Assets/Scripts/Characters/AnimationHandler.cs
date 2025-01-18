using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using System.Collections.Generic;
using Settings;
using System.Collections;
using CustomLogic;
using UI;
using Cameras;
using Photon.Pun;
using Photon.Realtime;
using GameProgress;

namespace Characters
{
    class AnimationHandler
    {
        private Animation Animation;
        private Animator Animator;
        private const float UpdateDelay = 0.1f;
        private const float CullFrameDelay = 0.2f;
        private const float CullDistance = 500f;
        private Dictionary<string, float> _animationSpeed = new Dictionary<string, float>();
        private bool _manual = false;
        private float _currentUpdateTime = 0f;
        private float _currentCullTime = 0f;
        private Transform _transform;
        private string _currentAnimation = string.Empty;
        private bool _isLegacy;
        private Dictionary<string, AnimationClip> _animatorClips = new Dictionary<string, AnimationClip>();
        private Dictionary<string, string> _animatorStateNames = new Dictionary<string, string>();

        public AnimationHandler(GameObject owner)
        {
            Animation = owner.GetComponent<Animation>();
            Animator = owner.GetComponent<Animator>();
            if (Animation != null)
            {
                _isLegacy = true;
                foreach (AnimationState state in Animation)
                    _animationSpeed[state.name] = state.speed;
            }
            else
            {
                foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                {
                    _animatorStateNames[clip.name] = clip.name.Replace('.', '_');
                    _animatorClips[clip.name] = clip;
                    _animationSpeed[clip.name] = 1f;
                }
                Animator.playableGraph.SetTimeUpdateMode(UnityEngine.Playables.DirectorUpdateMode.Manual);
            }
            _transform = owner.transform;
        }

        public string GetCurrentAnimation()
        {
            if (_isLegacy)
            {
                foreach (AnimationState state in Animation)
                {
                    if (Animation.IsPlaying(state.name))
                        return state.name;
                }
            }
            else
            {
                return _currentAnimation;
            }
            return "";
        }

        public float GetLength(string name)
        {
            if (_isLegacy)
                return Animation[name].length;
            return _animatorClips[name].length;
        }

        public float GetSpeed(string name)
        {
            return _animationSpeed[name];
        }

        public float GetTotalTime(string name)
        {
            return GetLength(name) / _animationSpeed[name];
        }

        public float GetNormalizedTime(string name)
        {
            if (_isLegacy)
                return Animation[name].normalizedTime;
            throw new Exception("GetNormalizedTime only available for legacy animations.");
        }

        public float GetCurrentNormalizedTime()
        {
            if (_isLegacy)
                return Animation[_currentAnimation].normalizedTime;
            return Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public bool IsPlaying(string name)
        {
            if (_isLegacy)
                return Animation.IsPlaying(name);
            return _currentAnimation == name;
        }

        public void Play(string name, float startTime)
        {
            if (_isLegacy)
            {
                Animation.Play(name);
                if (startTime > 0f)
                    Animation[name].normalizedTime = startTime;
            }
            else
            {
                Animator.Play(_animatorStateNames[name], 0, startTime);
                Animator.speed = GetSpeed(name);
            }
            _currentAnimation = name;
        }

        public void CrossFade(string name, float fade, float startTime)
        {
            if (_isLegacy)
            {
                if (_manual)
                    Animation.Play(name);
                else
                    Animation.CrossFade(name, fade);
                if (startTime > 0f)
                    Animation[name].normalizedTime = startTime;
            }
            else
            {
                if (_manual)
                    Animator.Play(_animatorStateNames[name], 0, startTime);
                else
                    Animator.CrossFade(_animatorStateNames[name], fade, 0, startTime);
                Animator.speed = GetSpeed(name);
            }
            _currentAnimation = name;
        }

        public void ManuallyStepAnimator(float time)
        {
            Animator.playableGraph.Evaluate(time);
        }

        public void SetSpeed(string name, float speed)
        {
            _animationSpeed[name] = speed;
            if (_isLegacy)
                Animation[name].speed = speed;
            else
            {
                if (_currentAnimation == name)
                    Animator.speed = speed;
            }
        }

        public void SetSpeedAll(float speed)
        {
            if (_isLegacy)
            {
                foreach (AnimationState animation in Animation)
                {
                    _animationSpeed[animation.name] = speed;
                    animation.speed = speed;
                }
            }
            else
            {
                foreach (string key in _animationSpeed.Keys)
                {
                    _animationSpeed[key] = speed;
                    if (_currentAnimation == key)
                        Animator.speed = speed;
                }
            }
        }

        public void SetCullingType(bool alwaysAnimate)
        {
            if (_isLegacy)
            {
                if (alwaysAnimate)
                    Animation.cullingType = AnimationCullingType.AlwaysAnimate;
                else
                    Animation.cullingType = AnimationCullingType.BasedOnRenderers;
            }
            else
            {
                if (alwaysAnimate)
                    Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                else
                    Animator.cullingMode = AnimatorCullingMode.CullCompletely;
            }
        }

        public void OnLateUpdate()
        {
            if (_isLegacy)
                return;
            if (_manual)
            {
                _currentCullTime += Time.deltaTime;
                if (_currentCullTime > CullFrameDelay)
                {
                    ManuallyStepAnimator(_currentCullTime);
                    _currentCullTime = 0f;
                }
            }
            else
                ManuallyStepAnimator(Time.deltaTime);

            _currentUpdateTime += Time.deltaTime;
            if (_currentUpdateTime < UpdateDelay)
                return;
            _currentUpdateTime = 0;
            var cameraPosition = SceneLoader.CurrentCamera.Cache.Transform.position;
            float distance = Vector3.Distance(cameraPosition, _transform.position);
            if (distance > CullDistance)
                SetManual(true);
            else
                SetManual(false);
        }

        private void SetManual(bool manual)
        {
            if (_manual == manual)
                return;
            _manual = manual;
            _currentCullTime = 0f;
        }
    }
}
