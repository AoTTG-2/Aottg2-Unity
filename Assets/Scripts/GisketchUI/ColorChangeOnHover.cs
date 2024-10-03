using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DentedPixel;

namespace GisketchUI
{
    public class ColorChangeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum ComponentType
        {
            Image,
            Text
        }

        [Serializable]
        public enum PaletteColor
        {
            Blue,
            Orange,
            Green,
            Red,
            Purple,
            Primary,
            PrimaryLight,
            Secondary,
            Tertiary,
            White,
            Black
        }

        [SerializeField] private GameObject targetObject;
        [SerializeField] private ComponentType targetComponent = ComponentType.Image;
        [SerializeField] private PaletteColor hoverColorChoice = PaletteColor.PrimaryLight;
        [SerializeField] private float colorTransitionDuration = 0.05f;
        [SerializeField] private bool enableScaling = true;
        [SerializeField] private float scaleTransitionDuration = 0.05f;

        private Image imageComponent;
        private Text textComponent;
        private Color originalColor;
        private Vector3 originalScale;

        private void Awake()
        {
            if (targetObject == null)
            {
                targetObject = gameObject;
            }

            switch (targetComponent)
            {
                case ComponentType.Image:
                    imageComponent = targetObject.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        originalColor = imageComponent.color;
                    }
                    break;
                case ComponentType.Text:
                    textComponent = targetObject.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        originalColor = textComponent.color;
                    }
                    break;
            }

            originalScale = targetObject.transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTweenColor(GetColorFromPalette(hoverColorChoice));
            if (enableScaling)
            {
                LeanTween.scale(targetObject, originalScale * 1.1f, scaleTransitionDuration);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LeanTweenColor(GetColorFromPalette(hoverColorChoice));
            if (enableScaling)
            {
                LeanTween.scale(targetObject, originalScale * 0.9f, scaleTransitionDuration);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTweenColor(originalColor);
            if (enableScaling)
            {
                LeanTween.scale(targetObject, originalScale, scaleTransitionDuration);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            LeanTweenColor(originalColor);
            if (enableScaling)
            {
                LeanTween.scale(targetObject, originalScale, scaleTransitionDuration);
            }
        }

        private void LeanTweenColor(Color targetColor)
        {
            switch (targetComponent)
            {
                case ComponentType.Image:
                    if (imageComponent != null)
                    {
                        LeanTween.color(targetObject.GetComponent<RectTransform>(), targetColor, colorTransitionDuration);
                    }
                    break;
                case ComponentType.Text:
                    if (textComponent != null)
                    {
                        LeanTween.textColor(targetObject.GetComponent<RectTransform>(), targetColor, colorTransitionDuration);
                    }
                    break;
            }
        }

        private Color GetColorFromPalette(PaletteColor paletteColor)
        {
            switch (paletteColor)
            {
                case PaletteColor.Blue: return ColorPalette.Blue;
                case PaletteColor.Orange: return ColorPalette.Orange;
                case PaletteColor.Green: return ColorPalette.Green;
                case PaletteColor.Red: return ColorPalette.Red;
                case PaletteColor.Purple: return ColorPalette.Purple;
                case PaletteColor.Primary: return ColorPalette.Primary;
                case PaletteColor.PrimaryLight: return ColorPalette.PrimaryLight;
                case PaletteColor.Secondary: return ColorPalette.Secondary;
                case PaletteColor.Tertiary: return ColorPalette.Tertiary;
                case PaletteColor.White: return ColorPalette.White;
                case PaletteColor.Black: return ColorPalette.Black;
                default: return Color.white;
            }
        }

        // Reset to original color and scale when the script is disabled
        private void OnDisable()
        {
            LeanTweenColor(originalColor);
            if (enableScaling)
            {
                LeanTween.scale(targetObject, originalScale, scaleTransitionDuration);
            }
        }
    }
}