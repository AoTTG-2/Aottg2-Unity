using DentedPixel;
using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public static class UIAnimator
    {

        public enum SlideDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public static void SlideIn(RectTransform rectTransform, SlideDirection direction, float duration, float delay = 0f)
        {
            Vector2 startPosition = GetOffScreenPosition(rectTransform, direction);
            Vector2 endPosition = rectTransform.anchoredPosition;

            rectTransform.anchoredPosition = startPosition;
            LeanTween.value(rectTransform.gameObject, startPosition, endPosition, duration)
                .setDelay(delay)
                .setEaseOutQuint()
                .setOnUpdate((Vector2 pos) => rectTransform.anchoredPosition = pos);
        }

        public static void SlideOut(RectTransform rectTransform, SlideDirection direction, float duration, float delay = 0f)
        {
            Vector2 startPosition = rectTransform.anchoredPosition;
            Vector2 endPosition = GetOffScreenPosition(rectTransform, direction);

            LeanTween.value(rectTransform.gameObject, startPosition, endPosition, duration)
                .setDelay(delay)
                .setEaseInQuint()
                .setOnUpdate((Vector2 pos) => rectTransform.anchoredPosition = pos);
        }

        public static void SlideHorizontal(RectTransform rectTransform, float offset, float duration, bool isRight = true)
        {
            Vector2 startPosition = rectTransform.anchoredPosition;
            Vector2 endPosition = startPosition + new Vector2(isRight ? offset : -offset, 0);

            LeanTween.value(rectTransform.gameObject, startPosition, endPosition, duration)
                .setEaseOutQuint()
                .setOnUpdate((Vector2 pos) => rectTransform.anchoredPosition = pos);
        }

        public static void SnapSlide(RectTransform rectTransform, float targetX, float duration)
        {
            LeanTween.moveX(rectTransform, targetX, duration).setEaseOutExpo();
        }

        public static void FadeIn(Graphic graphic, float duration, float delay = 0f)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0f);
            LeanTween.value(graphic.gameObject, 0f, 1f, duration)
                .setDelay(delay)
                .setEaseOutQuint()
                .setOnUpdate((float alpha) => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha));
        }

        public static void ZoomImpact(RectTransform rectTransform, float duration, float startScale = 1.5f, float endScale = 1f)
        {
            rectTransform.localScale = Vector3.one * startScale;
            LeanTween.scale(rectTransform, Vector3.one * endScale, duration).setEaseOutQuint();
        }

        public static void BounceScale(RectTransform rectTransform, float duration, float delay, float bounceScale = 1.2f)
        {
            LeanTween.scale(rectTransform, Vector3.one * bounceScale, duration * 0.5f)
                .setDelay(delay)
                .setEaseOutQuad()
                .setLoopPingPong(1);
        }

        public static void TypewriterEntrance(Text text, string content, float duration, float delay = 0f)
        {
            text.text = "";
            LeanTween.value(text.gameObject, 0, content.Length, duration)
                .setDelay(delay)
                .setEaseOutQuad()
                .setOnUpdate((float val) =>
                {
                    int charCount = Mathf.FloorToInt(val);
                    text.text = content.Substring(0, charCount);
                });
        }

        private static Vector2 GetOffScreenPosition(RectTransform rectTransform, SlideDirection direction)
        {
            Vector2 position = rectTransform.anchoredPosition;
            switch (direction)
            {
                case SlideDirection.Left:
                    position.x -= rectTransform.rect.width;
                    break;
                case SlideDirection.Right:
                    position.x += rectTransform.rect.width;
                    break;
                case SlideDirection.Up:
                    position.y += rectTransform.rect.height;
                    break;
                case SlideDirection.Down:
                    position.y -= rectTransform.rect.height;
                    break;
            }
            return position;
        }

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

        public static void ShowSidePanelButtonBackground(RectTransform background)
        {
            background.anchoredPosition = new Vector2(-background.rect.width, 0);
            LeanTween.moveX(background, 0, 0.3f).setEaseOutQuint();
        }

        public static void HideSidePanelButtonBackground(RectTransform background)
        {
            LeanTween.moveX(background, -background.rect.width, 0.3f).setEaseInQuint();
        }

        public static void ChangeButtonState(Image baseImage, Image outlineImage, Text labelText, Sprite newSprite, Color outlineColor, Color textColor)
        {
            baseImage.sprite = newSprite;
            outlineImage.color = outlineColor;
            labelText.color = textColor;
        }
    }
}