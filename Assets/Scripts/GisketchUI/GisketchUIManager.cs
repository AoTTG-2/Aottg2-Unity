using UnityEngine;
using UnityEngine.UI;
using Utility;
using Settings;
using System.Collections;

namespace GisketchUI
{
    public class GisketchUIManager : MonoBehaviour
    {
        private static GisketchUIManager _instance;
        public static GisketchUIManager Instance => _instance;

        public Canvas MainCanvas { get; private set; }

        public static void Init()
        {
            if (_instance == null)
            {
                _instance = SingletonFactory.CreateSingleton(_instance);
                _instance.SetupMainCanvas();
                SidePanelManager.Init();
                PopupManager.Init();
                _instance.UpdateUIScale();
            }
        }

        private void SetupMainCanvas()
        {
            GameObject canvasObject = new GameObject("GisketchUICanvas");
            canvasObject.transform.SetParent(transform);
            MainCanvas = canvasObject.AddComponent<Canvas>();
            MainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            MainCanvas.sortingOrder = 50;

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
            canvas.sortingOrder = 25;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            canvasObject.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        public void UpdateUIScale()
        {
            StartCoroutine(WaitAndApplyScale());
        }

        private IEnumerator WaitAndApplyScale()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            ApplyUIScale();
        }

        private void ApplyUIScale()
        {
            float scaleFactor = 1f / SettingsManager.UISettings.UIMasterScale.Value;
            CanvasScaler scaler = MainCanvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.referenceResolution = new Vector2(1920 * scaleFactor, 1080 * scaleFactor);
            }
        }
    }
}