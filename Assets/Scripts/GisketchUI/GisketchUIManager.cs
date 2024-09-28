using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public class GisketchUIManager : MonoBehaviour
    {
        private static GisketchUIManager _instance;
        public static GisketchUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GisketchUIManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("UIManager");
                        _instance = go.AddComponent<GisketchUIManager>();
                    }
                }
                return _instance;
            }
        }

        public Canvas MainCanvas { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            SetupMainCanvas();
        }

        private void SetupMainCanvas()
        {
            GameObject canvasObject = new GameObject("GisketchUICanvas");
            canvasObject.transform.SetParent(transform);
            MainCanvas = canvasObject.AddComponent<Canvas>();
            MainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            MainCanvas.sortingOrder = 100; // Ensure it's on top

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            canvasObject.AddComponent<GraphicRaycaster>();
        }

        public Canvas CreateNonScalingCanvas(string name)
        {
            GameObject canvasObject = new GameObject(name);
            canvasObject.transform.SetParent(transform);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 101;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            canvasObject.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        public void UpdateUIScale(float scaleFactor)
        {
            CanvasScaler scaler = MainCanvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.referenceResolution = new Vector2(1920 * scaleFactor, 1080 * scaleFactor);
            }
        }
    }
}