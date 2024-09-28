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

            SetupPopupBackground();
            UpdateUIScale();
        }

        private void SetupPopupBackground()
        {
            Canvas mainCanvas = GisketchUIManager.Instance.MainCanvas;

            // Add background
            GameObject bgObject = new GameObject("PopupBackground");
            bgObject.transform.SetParent(mainCanvas.transform, false);
            background = bgObject.AddComponent<PopupBackground>();
            background.gameObject.SetActive(false);
        }

        public T GetOrCreatePopup<T>() where T : BasePopup
        {
            string popupName = typeof(T).Name;

            if (!_popups.TryGetValue(popupName, out BasePopup popup))
            {
                popup = gameObject.AddComponent<T>();
                popup.Setup(GisketchUIManager.Instance.MainCanvas.transform);
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
            GisketchUIManager.Instance.UpdateUIScale(scaleFactor);
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