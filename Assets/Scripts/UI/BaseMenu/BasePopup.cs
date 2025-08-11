using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    class BasePopup : HeadedPanel
    {
        protected virtual float MinTweenScale => 0.3f;
        protected virtual float MaxTweenScale => 1f;
        protected virtual float MinFadeAlpha => 0f;
        protected virtual float MaxFadeAlpha => 1f;
        protected virtual float SpringDamping => 0.5f;
        protected virtual float SpringStiffness => 0.1f;
        protected virtual float AnimationTime => 0.1f;
        protected virtual bool ShowOnTop => true;
        protected virtual bool UseSound => false;
        protected virtual PopupAnimation PopupAnimationType => PopupAnimation.Tween;
        public float _currentAnimationValue;
        // transforms that should ignore animations
        protected HashSet<Transform> _staticTransforms = new HashSet<Transform>();
        public bool IsActive;

        public override void Show()
        {
            if (IsActive)
                return;
            IsActive = true;
            if (UseSound)
                UIManager.PlaySound(UISound.Forward);
            base.Show();
            if (ShowOnTop)
                transform.SetAsLastSibling();
            StopAllCoroutines();
            if (PopupAnimationType == PopupAnimation.Tween)
                StartCoroutine(TweenIn());
            else if (PopupAnimationType == PopupAnimation.Fade)
                StartCoroutine(FadeIn());
            else if (PopupAnimationType == PopupAnimation.KillPopup)
                StartCoroutine(KillPopupIn());
        }

        public void ShowImmediate()
        {
            if (IsActive)
                return;
            IsActive = true;
            if (UseSound)
                UIManager.PlaySound(UISound.Forward);
            base.Show();
            if (ShowOnTop)
                transform.SetAsLastSibling();
            StopAllCoroutines();
            if (PopupAnimationType == PopupAnimation.Tween)
                SetTransformScale(MaxTweenScale);
            else if (PopupAnimationType == PopupAnimation.Fade)
                SetTransformAlpha(MaxFadeAlpha);
        }

        protected override void HideAllPopups()
        {
            foreach (BasePopup popup in _popups)
                popup.HideImmediate();
        }

        public override void Hide()
        {
            if (!IsActive)
                return;
            IsActive = false;
            if (UseSound)
                UIManager.PlaySound(UISound.Back);
            HideAllPopups();
            StopAllCoroutines();
            if (PopupAnimationType == PopupAnimation.Tween)
                StartCoroutine(TweenOut());
            else if (PopupAnimationType == PopupAnimation.Fade)
                StartCoroutine(FadeOut());
            else if (PopupAnimationType == PopupAnimation.KillPopup)
                StartCoroutine(KillPopupOut());
            else if (PopupAnimationType == PopupAnimation.None)
                FinishHide();
        }

        public virtual void HideImmediate()
        {
            IsActive = false;
            if (!gameObject.activeSelf)
                return;
            HideAllPopups();
            StopAllCoroutines();
            FinishHide();
        }

        protected virtual void OnDisable()
        {
            IsActive = false;
        }

        protected virtual void FinishHide()
        {
            gameObject.SetActive(false);
        }

        protected IEnumerator TweenIn()
        {
            _currentAnimationValue = MinTweenScale;
            while (_currentAnimationValue < MaxTweenScale)
            {
                SetTransformScale(_currentAnimationValue);
                _currentAnimationValue += GetAnimationSpeed(MinTweenScale, MaxTweenScale) * Time.unscaledDeltaTime;
                yield return null;
            }
            SetTransformScale(MaxTweenScale);
        }

        protected IEnumerator TweenOut()
        {
            _currentAnimationValue = MaxTweenScale;
            while (_currentAnimationValue > MinTweenScale)
            {
                SetTransformScale(_currentAnimationValue);
                _currentAnimationValue -= GetAnimationSpeed(MinTweenScale, MaxTweenScale) * Time.unscaledDeltaTime;
                yield return null;
            }
            SetTransformScale(MinTweenScale);
            FinishHide();
        }

        [Serializable]
        public struct AnimationKeyframe
        {
            public float time; // Time percentage (0 to 1)
            public float scale; // Scale value at this keyframe

            public AnimationKeyframe(float time, float scale)
            {
                this.time = time;
                this.scale = scale;
            }
        }


        // Method to interpolate the scale based on the current time percentage
        protected float EvaluateKeyframes(float timePercentage, List<AnimationKeyframe> keyframes)
        {
            if (keyframes == null || keyframes.Count == 0)
                return MinTweenScale;

            // Find the keyframes to interpolate between
            AnimationKeyframe prevFrame = keyframes[0];
            AnimationKeyframe nextFrame = keyframes[keyframes.Count - 1];

            foreach (var frame in keyframes)
            {
                if (frame.time < timePercentage && frame.time > prevFrame.time)
                    prevFrame = frame;
                else if (frame.time >= timePercentage)
                {
                    nextFrame = frame;
                    break;
                }
            }

            // Interpolate between the two keyframes
            float frameDelta = nextFrame.time - prevFrame.time;
            if (frameDelta == 0)
                return prevFrame.scale;

            float interp = (timePercentage - prevFrame.time) / frameDelta;
            return Mathf.Lerp(prevFrame.scale, nextFrame.scale, interp);
        }

        private readonly List<AnimationKeyframe> killPopupKeyframesIn = new List<AnimationKeyframe>
        {
            // Spring Bounce in
            new AnimationKeyframe(0.0f, 0.0f),
            new AnimationKeyframe(0.15f, 1.3f),
            new AnimationKeyframe(0.3f, 0.8f),
            new AnimationKeyframe(0.45f, 1.1f),
            new AnimationKeyframe(0.6f, 0.95f),
            new AnimationKeyframe(0.75f, 1.05f),
            new AnimationKeyframe(1.0f, 1.0f),
        };

        private readonly List<AnimationKeyframe> killPopupKeyframesOut = new List<AnimationKeyframe>
        {
            new AnimationKeyframe(0.0f, 0.0f),
            new AnimationKeyframe(0.6f, 0.0f),
            new AnimationKeyframe(0.7f, 0.85f),
            new AnimationKeyframe(0.85f, 0.8f),
            new AnimationKeyframe(1.0f, 1.0f),
        };

        protected IEnumerator KillPopupIn()
        {
            float startTime = Time.time;
            float endTime = startTime + 0.6f;

            while (Time.time < endTime)
            {
                float currentPercentage = (Time.time - startTime) / 0.6f;
                float scaleValue = EvaluateKeyframes(currentPercentage, killPopupKeyframesIn);
                SetTransformScale(scaleValue);
                yield return null;
            }

            SetTransformScale(MaxTweenScale);
        }

        protected IEnumerator KillPopupOut()
        {
            float startTime = Time.time;
            float endTime = startTime + 1f;

            while (Time.time < endTime)
            {
                float currentPercentage = (Time.time - startTime) / 1f;
                float reversePercentage = 1f - currentPercentage; // Reverse the percentage
                float scaleValue = EvaluateKeyframes(reversePercentage, killPopupKeyframesOut);
                SetTransformScale(scaleValue);
                yield return null;
            }

            SetTransformScale(MinTweenScale);
            FinishHide();
        }

        protected IEnumerator FadeIn()
        {
            _currentAnimationValue = MinFadeAlpha;
            while (_currentAnimationValue < MaxFadeAlpha)
            {
                SetTransformAlpha(_currentAnimationValue);
                _currentAnimationValue += GetAnimationSpeed(MinFadeAlpha, MaxFadeAlpha) * Time.unscaledDeltaTime;
                yield return null;
            }
            SetTransformAlpha(MaxFadeAlpha);
        }

        protected IEnumerator FadeOut()
        {
            _currentAnimationValue = MaxFadeAlpha;
            while (_currentAnimationValue > MinFadeAlpha)
            {
                SetTransformAlpha(_currentAnimationValue);
                _currentAnimationValue -= GetAnimationSpeed(MinFadeAlpha, MaxFadeAlpha) * Time.unscaledDeltaTime;
                yield return null;
            }
            SetTransformAlpha(MinFadeAlpha);
            FinishHide();
        }

        protected void SetTransformScale(float scale)
        {
            IgnoreScaler scaler = transform.GetComponent<IgnoreScaler>();
            if (scaler != null)
                transform.localScale = GetVectorFromScale(scale * scaler.Scale);
            else
                transform.localScale = GetVectorFromScale(scale);
            foreach (Transform transform in _staticTransforms)
            {
                float nativeScale = 1f;
                scaler = transform.GetComponent<IgnoreScaler>();
                if (scaler != null)
                    nativeScale = scaler.Scale;
                transform.localScale = GetVectorFromScale(nativeScale / Mathf.Max(scale, 0.1f));
            }
        }

        protected void SetTransformAlpha(float alpha)
        {
            CanvasGroup group = transform.GetComponent<CanvasGroup>();
            group.alpha = alpha;
        }

        private Vector3 GetVectorFromScale(float scale)
        {
            return new Vector3(scale, scale, scale);
        }

        protected virtual float GetAnimationSpeed(float min, float max)
        {
            return (max - min) / AnimationTime;
        }
    }

    enum PopupAnimation
    {
        None,
        Fade,
        Tween,
        KillPopup,
    }
}
