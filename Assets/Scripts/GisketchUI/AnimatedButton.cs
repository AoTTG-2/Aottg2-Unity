using DentedPixel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GisketchUI
{
    public class AnimatedButton : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float pressScale = 0.95f;
        [SerializeField] private float animationDuration = 0.1f;

        private RectTransform rectTransform;
        private Vector3 originalScale;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalScale = rectTransform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.scale(rectTransform, originalScale * hoverScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.scale(rectTransform, originalScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LeanTween.scale(rectTransform, originalScale * pressScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            LeanTween.scale(rectTransform, originalScale * hoverScale, animationDuration).setEase(LeanTweenType.easeOutQuad);
        }

        public void SetOnClickHandler(UnityEngine.Events.UnityAction action)
        {
            Button button = GetComponent<Button>();
            if (button == null)
            {
                button = gameObject.AddComponent<Button>();
            }
            button.onClick.AddListener(action);
        }
    }
}