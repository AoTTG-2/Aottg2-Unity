using System;
using UnityEngine;
using ApplicationManagers;
using System.Collections.Generic;

namespace Characters
{
    class AnimationHandler
    {
        private Animation Animation;
        private Animator Animator;
        private SkinnedMeshRenderer Renderer;
        private const float LODBone2Distance = 500f;
        private const float LODBone1Distance = 1000f;
        private Dictionary<string, float> _animationSpeed = new Dictionary<string, float>();
        private string _currentAnimation = string.Empty;
        private float _currentAnimationStartTime = 0f;
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
                Animator.playableGraph.SetTimeUpdateMode(UnityEngine.Playables.DirectorUpdateMode.GameTime);
            }
            Renderer = owner.GetComponentInChildren<SkinnedMeshRenderer>();
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
            else
            {
                float deltaTime = Time.time - _currentAnimationStartTime;
                float normalizedTime = deltaTime / GetTotalTime(_currentAnimation);
                return normalizedTime;
            }
        }

        public bool IsPlaying(string name)
        {
            if (_isLegacy)
                return Animation.IsPlaying(name);
            return _currentAnimation == name;
        }

        public void Play(string name, float startTime, bool reset = false)
        {
            if (_isLegacy)
            {
                Animation.Play(name);
                if (startTime > 0f || reset)
                    Animation[name].normalizedTime = startTime;
            }
            else
            {
                Animator.Play(_animatorStateNames[name], 0, startTime);
                Animator.speed = GetSpeed(name);
            }
            _currentAnimation = name;
            _currentAnimationStartTime = Time.time;
        }

        public void CrossFade(string name, float fade, float startTime)
        {
            if (_isLegacy)
            {
                Animation.CrossFade(name, fade);
                if (startTime > 0f)
                    Animation[name].normalizedTime = startTime;
            }
            else
            {
                if (_currentAnimation != string.Empty)
                    fade = fade / GetLength(_currentAnimation);
                Animator.CrossFade(_animatorStateNames[name], fade, 0, startTime);
                Animator.speed = GetSpeed(name);
            }
            _currentAnimation = name;
            _currentAnimationStartTime = Time.time;
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

        public void OnDistanceUpdate(float distance)
        {
            if (distance > LODBone1Distance)
                SetQuality(SkinQuality.Bone1);
            else if (distance > LODBone2Distance)
                SetQuality(SkinQuality.Bone2);
            else
                SetQuality(SkinQuality.Bone4);
            SetShadows(distance < LODBone1Distance);
        }

        private void SetQuality(SkinQuality quality)
        {
            if (Renderer != null && Renderer.quality != quality)
                Renderer.quality = quality;
        }

        private void SetShadows(bool shadows)
        {
            if (Renderer != null && Renderer.receiveShadows != shadows)
            {
                Renderer.receiveShadows = shadows;
                if (shadows)
                    Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                else
                    Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
    }
}
