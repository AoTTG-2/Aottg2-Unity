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
        public Animation Animation;
        private const float UpdateDelay = 0.1f;
        private const float CullDistance = 200f;
        private Dictionary<string, float> _animationSpeed = new Dictionary<string, float>();
        private bool _manual = false;
        private float _currentUpdateTime = 0f;
        private Transform _transform;
        private string _currentAnimation = string.Empty;
        private float _currentAnimationStartTime = 0f;

        public AnimationHandler(GameObject owner)
        {
            Animation = owner.GetComponent<Animation>();
            _transform = owner.transform;
            foreach (AnimationState state in Animation)
                _animationSpeed[state.name] = state.speed;
        }

        public string GetCurrentAnimation()
        {
            foreach (AnimationState state in Animation)
            {
                if (Animation.IsPlaying(state.name))
                    return state.name;
            }
            return "";
        }

        public float GetLength(string name)
        {
            return Animation[name].length;
        }

        public float GetSpeed(string name)
        {
            return _animationSpeed[name];
        }

        public float GetTotalTime(string name)
        {
            return Animation[name].length / _animationSpeed[name];
        }

        public float GetNormalizedTime(string name)
        {
            return Animation[name].normalizedTime;
        }

        public bool IsPlaying(string name)
        {
            return Animation.IsPlaying(name);
        }

        public void Play(string name)
        {
            Animation.Play(name);
            _currentAnimation = name;
            _currentAnimationStartTime = Time.time;
        }

        public void CrossFade(string name, float fade)
        {
            if (_manual)
                Animation.Play(name);
            else
                Animation.CrossFade(name, fade);
            _currentAnimation = name;
            _currentAnimationStartTime = Time.time;
        }

        public void SetNormalizedTime(string name, float time)
        {
            Animation[name].normalizedTime = time;
        }

        public void SetTime(string name, float time)
        {
            Animation[name].time = time;
        }

        public void SetSpeed(string name, float speed)
        {
            _animationSpeed[name] = speed;
            if (!_manual)
                Animation[name].speed = speed;
        }

        public void SetSpeedAll(float speed)
        {
            foreach (AnimationState animation in Animation)
            {
                _animationSpeed[animation.name] = speed;
                if (!_manual)
                    animation.speed = speed;
            }
        }

        public void OnLateUpdate()
        {
            return;

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
            if (_manual)
                UpdateManual();
        }

        private void UpdateManual()
        {
            if (_currentAnimation == string.Empty)
                return;
            if (GetSpeed(_currentAnimation) == 0f)
                return;
            float time = Time.time - _currentAnimationStartTime;
            SetTime(_currentAnimation, time);
        }

        private void SetManual(bool manual)
        {
            if (_manual == manual)
                return;
            _manual = manual;
            if (_manual)
            {
                foreach (AnimationState animation in Animation)
                    animation.speed = 0f;
            }
            else
            {
                foreach (AnimationState animation in Animation)
                    animation.speed = _animationSpeed[animation.name];
            }
        }
    }
}
