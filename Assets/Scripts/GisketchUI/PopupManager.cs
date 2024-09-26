using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GisketchUI
{
    public class PopupManager : MonoBehaviour
    {
        private static PopupManager _instance;
        public static PopupManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<PopupManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("PopupManager");
                        _instance = go.AddComponent<PopupManager>();
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, GisketchBasePopup> _popups = new Dictionary<string, GisketchBasePopup>();
        private Canvas _popupCanvas;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            SetupPopupCanvas();
        }

        private void SetupPopupCanvas()
        {
            GameObject canvasObject = new GameObject("PopupCanvas");
            canvasObject.transform.SetParent(transform);
            _popupCanvas = canvasObject.AddComponent<Canvas>();
            _popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _popupCanvas.sortingOrder = 100; // Ensure it's on top

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            canvasObject.AddComponent<GraphicRaycaster>();
        }

        public T GetOrCreatePopup<T>() where T : GisketchBasePopup
        {
            string popupName = typeof(T).Name;

            if (!_popups.TryGetValue(popupName, out GisketchBasePopup popup))
            {
                popup = gameObject.AddComponent<T>();
                popup.Setup(_popupCanvas.transform);
                _popups[popupName] = popup;
            }

            return (T)popup;
        }

        public void ShowPopup<T>() where T : GisketchBasePopup
        {
            GetOrCreatePopup<T>().Show();
        }

        public void HidePopup<T>() where T : GisketchBasePopup
        {
            if (_popups.TryGetValue(typeof(T).Name, out GisketchBasePopup popup))
            {
                popup.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var popup in _popups.Values)
            {
                popup.Hide();
            }
        }
    }
}