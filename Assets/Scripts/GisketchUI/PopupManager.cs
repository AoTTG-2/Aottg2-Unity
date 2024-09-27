using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Settings;
using System.Collections;

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

        private Dictionary<string, BasePopup> _popups = new Dictionary<string, BasePopup>();
        private Canvas _popupCanvas;
        private PopupBackground background;
        private int _activePopupCount = 0;

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
            UpdateUIScale();
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
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            canvasObject.AddComponent<GraphicRaycaster>();

            // Add background
            GameObject bgObject = new GameObject("PopupBackground");
            bgObject.transform.SetParent(_popupCanvas.transform, false);
            background = bgObject.AddComponent<PopupBackground>();
            background.gameObject.SetActive(false);
        }

        public T GetOrCreatePopup<T>() where T : BasePopup
        {
            string popupName = typeof(T).Name;

            if (!_popups.TryGetValue(popupName, out BasePopup popup))
            {
                popup = gameObject.AddComponent<T>();
                popup.Setup(_popupCanvas.transform);
                _popups[popupName] = popup;
            }

            return (T)popup;
        }

        public void ShowPopup<T>() where T : BasePopup
        {
            _activePopupCount++;
            if (_activePopupCount == 1)
            {
                background.Show(0.15f);
            }
            GetOrCreatePopup<T>().Show();
        }

        public void HidePopup<T>() where T : BasePopup
        {
            if (_popups.TryGetValue(typeof(T).Name, out BasePopup popup))
            {
                popup.Hide();
            }
        }

        private void ApplyUIScale()
        {
            float scaleFactor = 1f / SettingsManager.UISettings.UIMasterScale.Value;

            // Apply scale to the popup canvas
            if (_popupCanvas != null)
            {
                CanvasScaler scaler = _popupCanvas.GetComponent<CanvasScaler>();
                if (scaler != null)
                {
                    scaler.referenceResolution = new Vector2(1920 * scaleFactor, 1080 * scaleFactor);
                }
            }

            // // Apply scale to all existing popups
            // foreach (var popup in _popups.Values)
            // {
            //     popup.ApplyScale(scaleFactor);
            // }
        }

        // Call this method when you want to update the UI scale
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

        public void OnPopupHidden(BasePopup popup)
        {
            _activePopupCount--;
            if (_activePopupCount <= 0)
            {
                _activePopupCount = 0;
                background.Hide(0.15f);
            }
        }

        public void HideAllPopups()
        {
            background.Hide(0.15f);
            foreach (var popup in _popups.Values)
            {
                popup.Hide();
            }
            _activePopupCount = 0;
        }

        public void OnBackgroundClicked()
        {
            HideAllPopups();
        }
    }
}