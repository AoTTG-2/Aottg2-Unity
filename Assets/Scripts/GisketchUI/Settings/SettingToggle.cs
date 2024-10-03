using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DentedPixel;

namespace GisketchUI
{
    public class SettingToggle : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform baseRect;
        [SerializeField] private RectTransform maskRect;
        [SerializeField] private RectTransform buttonRect;
        public Button toggleButton;

        private bool isOn = false;

        private const float ButtonOffPosition = -10f;
        private const float ButtonOnPosition = 10f;
        private const float MaskOffPosition = -45f;
        private const float MaskOnPosition = 0f;
        private const float AnimationDuration = 0.15f;

        public void Initialize(bool defaultValue = false)
        {
            if (baseRect == null) baseRect = transform.Find("Base").GetComponent<RectTransform>();
            if (maskRect == null) maskRect = transform.Find("Base/Mask").GetComponent<RectTransform>();
            if (buttonRect == null) buttonRect = transform.Find("Button").GetComponent<RectTransform>();
            if (toggleButton == null) toggleButton = buttonRect.GetComponent<Button>();

            // Set the initial color of the mask
            Image maskImage = maskRect.GetComponent<Image>();
            if (maskImage != null)
            {
                maskImage.color = ColorPalette.Red;
            }

            SetValue(defaultValue, false);
            toggleButton.onClick.AddListener(Toggle);
        }

        public void Toggle()
        {
            SetValue(!isOn);
        }

        public bool GetValue()
        {
            return isOn;
        }

        public void SetValue(bool value, bool animate = true)
        {
            isOn = value;
            float targetButtonX = isOn ? ButtonOnPosition : ButtonOffPosition;
            float targetMaskX = isOn ? MaskOnPosition : MaskOffPosition;

            if (animate)
            {
                LeanTween.moveX(buttonRect, targetButtonX, AnimationDuration).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.moveX(maskRect, targetMaskX, AnimationDuration).setEase(LeanTweenType.easeInOutQuad);
            }
            else
            {
                buttonRect.anchoredPosition = new Vector2(targetButtonX, buttonRect.anchoredPosition.y);
                maskRect.anchoredPosition = new Vector2(targetMaskX, maskRect.anchoredPosition.y);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.scale(buttonRect, Vector3.one * 1.1f, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.scale(buttonRect, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LeanTween.scale(buttonRect, Vector3.one * 0.95f, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            LeanTween.scale(buttonRect, Vector3.one * 1.1f, 0.1f).setEase(LeanTweenType.easeOutQuad);
        }
    }
}