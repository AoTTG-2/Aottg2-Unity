using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Utility;
using UI;

namespace GisketchUI
{
    public class PopupManager : MonoBehaviour
    {
        private static PopupManager _instance;
        public static PopupManager Instance => _instance;

        private Dictionary<string, BasePopup> _popups = new Dictionary<string, BasePopup>();
        private PopupBackground background;
        private int _activePopupCount = 0;

        public static void Init()
        {
            if (_instance == null)
            {
                _instance = SingletonFactory.CreateSingleton(_instance);
                _instance.Initialize();
            }
        }

        private void Initialize()
        {
            SetupPopupBackground();
        }

        private void SetupPopupBackground()
        {
            Canvas mainCanvas = GisketchUIManager.Instance.MainCanvas;

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
                UIManager.PlaySound(UISound.Forward);
            }

            T popup = GetOrCreatePopup<T>();

            background.transform.SetSiblingIndex(popup.transform.GetSiblingIndex() - 1);
            background.Show(0.15f);

            popup.Show();
        }

        public void HidePopup<T>() where T : BasePopup
        {
            if (_popups.TryGetValue(typeof(T).Name, out BasePopup popup))
            {
                UIManager.PlaySound(UISound.Back);
                popup.Hide();
            }
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