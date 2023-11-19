using ApplicationManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class BasePopup: HeadedPanel
    {
        protected virtual float MinTweenScale => 0.3f;
        protected virtual float MaxTweenScale => 1f;
        protected virtual float MinFadeAlpha => 0f;
        protected virtual float MaxFadeAlpha => 1f;
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

        protected void OnDisable()
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
                _currentAnimationValue += GetAnimmationSpeed(MinTweenScale, MaxTweenScale) * Time.unscaledDeltaTime;
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
                _currentAnimationValue -= GetAnimmationSpeed(MinTweenScale, MaxTweenScale) * Time.unscaledDeltaTime;
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
                _currentAnimationValue += GetAnimmationSpeed(MinFadeAlpha, MaxFadeAlpha) * Time.unscaledDeltaTime;
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
                _currentAnimationValue -= GetAnimmationSpeed(MinFadeAlpha, MaxFadeAlpha) * Time.unscaledDeltaTime;
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

        private float GetAnimmationSpeed(float min, float max)
        {
            return (max - min) / AnimationTime;
        }
    }

    enum PopupAnimation
    {
        None,
        Fade,
        Tween
    }
}
