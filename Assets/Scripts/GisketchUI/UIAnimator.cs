using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public static class UIAnimator
    {
        public static void HoverAnimation(RectTransform rectTransform, Vector3 originalPosition, float offset, float duration)
        {
            Vector3 hoverPosition = originalPosition + new Vector3(offset, offset, 0);
            LeanTween.cancel(rectTransform);
            LeanTween.move(rectTransform, hoverPosition, duration).setEaseOutQuint();
        }

        public static void UnhoverAnimation(RectTransform rectTransform, Vector3 originalPosition, float duration)
        {
            LeanTween.cancel(rectTransform);
            LeanTween.move(rectTransform, originalPosition, duration).setEaseOutQuint();
        }

        public static void PressAnimation(RectTransform rectTransform, Vector3 originalPosition, float offset, float duration)
        {
            Vector3 pressPosition = originalPosition - new Vector3(offset, offset, 0);
            LeanTween.cancel(rectTransform);
            LeanTween.move(rectTransform, pressPosition, duration).setEaseOutQuint();
        }

        public static void ReleaseAnimation(RectTransform rectTransform, Vector3 originalPosition, float hoverOffset, float duration)
        {
            Vector3 hoverPosition = originalPosition + new Vector3(hoverOffset, hoverOffset, 0);
            LeanTween.cancel(rectTransform);
            LeanTween.move(rectTransform, hoverPosition, duration).setEaseOutQuint();
        }

        public static void ChangeButtonState(Image baseImage, Image outlineImage, Text labelText, Sprite newSprite, Color outlineColor, Color textColor)
        {
            baseImage.sprite = newSprite;
            outlineImage.color = outlineColor;
            labelText.color = textColor;
        }
    }
}