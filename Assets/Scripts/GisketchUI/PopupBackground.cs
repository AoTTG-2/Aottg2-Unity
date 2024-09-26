using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public class PopupBackground : MonoBehaviour
    {
        private Image backgroundImage;
        private RectTransform rectTransform;
        private UnityEngine.UI.Button closeButton;

        private void Awake()
        {
            backgroundImage = gameObject.AddComponent<Image>();
            backgroundImage.color = new Color(0, 0, 0, 0); // Start fully transparent
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;

            // Add button to close popup when background is clicked
            closeButton = gameObject.AddComponent<UnityEngine.UI.Button>();
            closeButton.onClick.AddListener(() => PopupManager.Instance.OnBackgroundClicked());

            // Set the button's transition to none to prevent visual feedback
            closeButton.transition = Selectable.Transition.None;
        }

        public void Show(float duration)
        {
            gameObject.SetActive(true);
            closeButton.interactable = false; // Disable button during fade-in
            backgroundImage.raycastTarget = true;
            LeanTween.color(rectTransform, new Color(0, 0, 0, 0.5f), duration)
                .setOnComplete(() => closeButton.interactable = true); // Enable button after fade-in
        }

        public void Hide(float duration)
        {
            closeButton.interactable = false; // Disable button during fade-out
            backgroundImage.raycastTarget = false;
            LeanTween.color(rectTransform, new Color(0, 0, 0, 0), duration)
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    closeButton.interactable = true; // Re-enable button for next use
                });
        }
    }
}