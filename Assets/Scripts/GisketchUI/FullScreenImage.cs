using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    [RequireComponent(typeof(Image))]
    public class FullScreenImage : MonoBehaviour
    {
        private RectTransform rectTransform;
        private CanvasScaler canvasScaler;
        private Vector2 lastScreenSize;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();

            if (canvasScaler == null)
            {
                Debug.LogError("FullScreenImage: CanvasScaler not found in parent hierarchy. Make sure this component is child of a Canvas with CanvasScaler.");
                return;
            }

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            Image image = GetComponent<Image>();
            image.raycastTarget = false;
            image.type = Image.Type.Sliced;
            image.fillCenter = true;

            // Initialize lastScreenSize
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            if (lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height)
            {
                UpdateImageSize();
                lastScreenSize = new Vector2(Screen.width, Screen.height);
            }
        }

        private void UpdateImageSize()
        {
            rectTransform.sizeDelta = Vector2.zero;
        }

        public void ForceUpdateSize()
        {
            UpdateImageSize();
        }
    }
}