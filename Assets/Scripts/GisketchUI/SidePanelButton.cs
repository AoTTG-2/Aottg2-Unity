using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UI;

namespace GisketchUI
{
    public class SidePanelButton : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private Text label;
        [SerializeField] private Image icon;
        [SerializeField] private Image hitArea;

        public RectTransform rectTransform;
        private RectTransform labelRect;
        private Vector3 originalPosition;
        private Vector2 backgroundOriginalPosition;
        private Vector2 labelOriginalPosition;
        private bool isActive = false;

        [SerializeField] private float hoverOffset = 10f;
        [SerializeField] private float pressOffset = -2f;

        public event Action OnClick;

        public enum ButtonVariant
        {
            Default,
            Blue,
            Orange,
            Green,
            Red,
            Purple
        }

        [SerializeField] private ButtonVariant variant = ButtonVariant.Default;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            labelRect = label.GetComponent<RectTransform>();
            originalPosition = rectTransform.anchoredPosition;
            backgroundOriginalPosition = background.anchoredPosition;
            labelOriginalPosition = labelRect.anchoredPosition;

            SetupHitArea();
            HideBackground();
            SetDefaultColors();
        }

        private void SetupHitArea()
        {
            if (hitArea == null)
            {
                hitArea = gameObject.AddComponent<Image>();
            }
            hitArea.color = Color.clear;
        }

        public void SetLabel(string text)
        {
            label.text = text;
        }

        public void SetLabelFontSize(int fontSize)
        {
            label.fontSize = fontSize;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActive)
            {
                ShowBackground();
                SetActiveColors();
                AnimateLabelHover();
            }
            UIManager.PlaySound(UISound.Hover);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isActive)
            {
                HideBackground();
                SetDefaultColors();
                AnimateLabelDefault();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateLabelPress();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isActive)
            {
                AnimateLabelDefault();
            }
            else
            {
                AnimateLabelHover();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void SetActive(bool active)
        {
            isActive = active;
            if (active)
            {
                ShowBackground();
                SetActiveColors();
                AnimateLabelDefault();
            }
            else
            {
                HideBackground();
                SetDefaultColors();
                AnimateLabelDefault();
            }
        }

        public void AnimateEntrance(float duration, float delay, Vector3? positionToStart = null)
        {
            Vector3 startPosition = positionToStart ?? originalPosition;
            rectTransform.anchoredPosition = startPosition - new Vector3(rectTransform.rect.width, 0, 0);
            LeanTween.moveX(rectTransform, startPosition.x, duration).setDelay(delay).setEaseOutQuint();
        }

        private void SetDefaultColors()
        {
            label.color = ColorPalette.Black;
            icon.color = ColorPalette.Black;
        }

        private void SetActiveColors()
        {
            label.color = ColorPalette.White;
            icon.color = ColorPalette.White;
        }

        private void ShowBackground()
        {
            LeanTween.cancel(background.gameObject);
            LeanTween.move(background, backgroundOriginalPosition, 0.2f).setEaseOutQuint();
            SetBackgroundColor();
        }

        private void HideBackground()
        {
            LeanTween.cancel(background.gameObject);
            LeanTween.moveX(background, -background.rect.width * 1.25f, 0.2f).setEaseInQuint();
        }

        private void SetBackgroundColor()
        {
            Color bgColor = GetVariantColor();
            background.GetComponent<Image>().color = bgColor;
        }

        private Color GetVariantColor()
        {
            switch (variant)
            {
                case ButtonVariant.Blue:
                    return ColorPalette.Blue;
                case ButtonVariant.Orange:
                    return ColorPalette.Orange;
                case ButtonVariant.Green:
                    return ColorPalette.Green;
                case ButtonVariant.Red:
                    return ColorPalette.Red;
                case ButtonVariant.Purple:
                    return ColorPalette.Purple;
                default:
                    return ColorPalette.Primary;
            }
        }

        public void SetVariant(ButtonVariant newVariant)
        {
            variant = newVariant;
            if (isActive)
            {
                SetBackgroundColor();
            }
        }

        private void AnimateLabelHover()
        {
            LeanTween.cancel(labelRect);
            LeanTween.moveX(labelRect, labelOriginalPosition.x + hoverOffset, 0.2f).setEaseOutQuint();
        }

        private void AnimateLabelPress()
        {
            LeanTween.cancel(labelRect);
            LeanTween.moveX(labelRect, labelOriginalPosition.x + pressOffset, 0.1f).setEaseOutQuint();
        }

        private void AnimateLabelDefault()
        {
            LeanTween.cancel(labelRect);
            LeanTween.moveX(labelRect, labelOriginalPosition.x, 0.2f).setEaseOutQuint();
        }
    }
}