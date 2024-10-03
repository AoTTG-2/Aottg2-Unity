using DentedPixel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GisketchUI
{
    public class SettingsListItem : UIElement, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Text labelText;
        [SerializeField] private RectTransform container;
        [SerializeField] private Image hoverImage;

        public string CategoryName { get; private set; }

        private const float HoverOpacity = 0.05f;
        private const float NormalOpacity = 0f;
        private const float AnimationDuration = 0.05f;

        public void Initialize(string label, string categoryName = "")
        {
            labelText.text = label;
            CategoryName = categoryName;

            // Set initial opacity
            SetImageOpacity(NormalOpacity);
        }

        public void SetContent(GameObject content)
        {
            content.transform.SetParent(container, false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.alpha(hoverImage.rectTransform, HoverOpacity, AnimationDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.alpha(hoverImage.rectTransform, NormalOpacity, AnimationDuration);
        }

        public override void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            // Add show animation if needed
        }

        public override void Hide(float duration = 0.3f)
        {
            // Add hide animation if needed
            gameObject.SetActive(false);
        }

        private void SetImageOpacity(float opacity)
        {
            Color color = hoverImage.color;
            color.a = opacity;
            hoverImage.color = color;
        }
    }
}