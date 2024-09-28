using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
    public class IntroPanelAnimator : MonoBehaviour
    {
        public float panelSlideDuration = 1.5f;
        public float buttonsSlideDuration = 1f;

        private RectTransform panelRect;
        private RectTransform buttonsContainer;
        private float buttonsSlideOffset;

        private void Awake()
        {
            panelRect = GetComponent<RectTransform>();
            buttonsContainer = GetComponentInChildren<VerticalLayoutGroup>().GetComponent<RectTransform>();

            buttonsSlideOffset = Screen.width / 2.5f;
        }

        public void StartAnimation()
        {
            StartCoroutine(AnimatePanelAndButtons());
        }

        private IEnumerator AnimatePanelAndButtons()
        {
            // Set initial positions
            panelRect.anchoredPosition = new Vector2(-panelRect.rect.width, panelRect.anchoredPosition.y);
            Vector2 buttonsStartPos = buttonsContainer.anchoredPosition;
            buttonsContainer.anchoredPosition = new Vector2(buttonsStartPos.x - buttonsSlideOffset, buttonsStartPos.y);

            float elapsedTime = 0f;
            bool buttonAnimationStarted = false;

            while (elapsedTime < panelSlideDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / panelSlideDuration;
                t = EaseOutCubic(t);

                // Animate panel
                panelRect.anchoredPosition = Vector2.Lerp(
                    new Vector2(-panelRect.rect.width, panelRect.anchoredPosition.y),
                    Vector2.zero,
                    t
                );

                // Start button animation when panel is halfway
                if (!buttonAnimationStarted && t >= 0.5f)
                {
                    buttonAnimationStarted = true;
                    StartCoroutine(AnimateButtons(buttonsStartPos));
                }

                yield return null;
            }

            panelRect.anchoredPosition = Vector2.zero;

            // Ensure button animation completes
            if (buttonAnimationStarted)
            {
                yield return new WaitForSeconds(buttonsSlideDuration * 0.5f);
            }
        }

        private IEnumerator AnimateButtons(Vector2 endPos)
        {
            Vector2 startPos = buttonsContainer.anchoredPosition;
            float elapsedTime = 0f;

            while (elapsedTime < buttonsSlideDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / buttonsSlideDuration;
                t = EaseOutCubic(t);

                buttonsContainer.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            buttonsContainer.anchoredPosition = endPos;
        }

        private float EaseOutCubic(float t)
        {
            return 1 - Mathf.Pow(1 - t, 3);
        }
    }
}