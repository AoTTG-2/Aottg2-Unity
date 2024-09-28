using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GisketchUI
{
    public class ActionButton : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEngine.UI.Button button;
        public Text labelText;
        public RectTransform baseRectTransform;
        public RectTransform shadowRectTransform;
        public RectTransform rectTransform;
        public Image baseImage;
        public Image outlineImage;


        [SerializeField] private ButtonVariant variant = ButtonVariant.Neutral;
        [SerializeField] private float hoverOffset = 10f;
        [SerializeField] private float pressOffset = 5f;
        [SerializeField] private float animationDuration = 0.1f;

        private Vector3 originalBasePosition;
        private Sprite lightSprite;
        private Sprite darkSprite;
        private Color originalTextColor;
        private Color originalOutlineColor;
        private Color variantColor;

        public enum ButtonVariant
        {
            Neutral,
            Primary,
            Secondary,
            Tertiary,
            Blue,
            Orange,
            Green,
            Red,
            Purple
        }

        private void Awake()
        {
            button = GetComponent<UnityEngine.UI.Button>();
            labelText = GetComponentInChildren<Text>();

            Transform baseTransform = transform.Find("Base");
            Transform shadowTransform = transform.Find("Shadow");

            if (baseTransform == null || shadowTransform == null)
            {
                Debug.LogError("GisketchButton: Base or Shadow child not found!");
                return;
            }

            baseRectTransform = baseTransform.GetComponent<RectTransform>();
            shadowRectTransform = shadowTransform.GetComponent<RectTransform>();
            baseImage = baseTransform.Find("Texture").GetComponent<Image>();
            outlineImage = baseTransform.Find("Outline").GetComponent<Image>();

            if (button == null || labelText == null || baseRectTransform == null ||
                shadowRectTransform == null || baseImage == null || outlineImage == null)
            {
                Debug.LogError("GisketchButton: One or more required components not found!");
                return;
            }

            originalBasePosition = baseRectTransform.anchoredPosition3D;
            originalTextColor = labelText.color;
            originalOutlineColor = outlineImage.color;

            lightSprite = Resources.Load<Sprite>("GisketchUI/Texture/Light");
            darkSprite = Resources.Load<Sprite>("GisketchUI/Texture/Dark");

            if (lightSprite == null || darkSprite == null)
            {
                Debug.LogError("GisketchButton: Light or Dark sprite not found in Resources!");
            }

            SetButtonVariant(variant);
        }

        public void SetButtonVariant(ButtonVariant newVariant)
        {
            variant = newVariant;
            variantColor = GetColorForVariant(variant);
        }

        private Color GetColorForVariant(ButtonVariant variant)
        {
            switch (variant)
            {
                case ButtonVariant.Neutral: return Color.white;
                case ButtonVariant.Primary: return ColorPalette.Primary;
                case ButtonVariant.Secondary: return ColorPalette.Secondary;
                case ButtonVariant.Tertiary: return ColorPalette.Tertiary;
                case ButtonVariant.Blue: return ColorPalette.Blue;
                case ButtonVariant.Orange: return ColorPalette.Orange;
                case ButtonVariant.Green: return ColorPalette.Green;
                case ButtonVariant.Red: return ColorPalette.Red;
                case ButtonVariant.Purple: return ColorPalette.Purple;
                default: return ColorPalette.Primary;
            }
        }

        public void SetLabel(string label)
        {
            if (labelText != null)
            {
                labelText.text = label;
            }
            else
            {
                Debug.LogWarning("Label Text component is missing on GisketchButton");
            }
        }

        public void AddListener(UnityAction onClick)
        {
            if (button != null)
            {
                button.onClick.AddListener(onClick);
            }
            else
            {
                Debug.LogError("Button component is missing on GisketchButton");
            }
        }

        public override void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            if (rectTransform != null)
                LeanTween.scale(rectTransform, Vector3.one, duration).setEaseOutBack();
        }

        public override void Hide(float duration = 0.3f)
        {
            if (rectTransform != null)
            {
                LeanTween.scale(rectTransform, Vector3.zero, duration).setEaseInBack()
                    .setOnComplete(() => gameObject.SetActive(false));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIAnimator.HoverAnimation(baseRectTransform, originalBasePosition, hoverOffset, animationDuration);
            UIAnimator.ChangeButtonState(baseImage, outlineImage, labelText, darkSprite, variantColor, Color.white);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIAnimator.UnhoverAnimation(baseRectTransform, originalBasePosition, animationDuration);
            UIAnimator.ChangeButtonState(baseImage, outlineImage, labelText, lightSprite, originalOutlineColor, originalTextColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UIAnimator.PressAnimation(baseRectTransform, originalBasePosition, pressOffset, animationDuration);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UIAnimator.ReleaseAnimation(baseRectTransform, originalBasePosition, hoverOffset, animationDuration);
        }
    }
}