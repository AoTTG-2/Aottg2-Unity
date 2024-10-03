using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DentedPixel;

namespace GisketchUI
{
    public class SettingsListView : UIPanel
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;

        private List<SettingsListItem> settingsItems = new List<SettingsListItem>();

        public RectTransform Content => content;

        public void Initialize()
        {
            if (scrollRect == null)
                scrollRect = GetComponentInChildren<ScrollRect>();

            if (content == null)
                content = scrollRect.transform.Find("Content") as RectTransform;

            if (content == null)
                Debug.LogError("Content transform not found in SettingsListView. Make sure there's a ScrollArea/Content in the hierarchy.");
        }

        public void AddSettingsItem(SettingsListItem item)
        {
            item.transform.SetParent(content, false);
            settingsItems.Add(item);
        }

        public void ClearSettingsItems()
        {
            foreach (var item in settingsItems)
            {
                Destroy(item.gameObject);
            }
            settingsItems.Clear();
        }

        public override void Show(float duration = 0.15f)
        {
            gameObject.SetActive(true);
            AnimateEntrance(duration);
        }

        public override void Hide(float duration = 0.15f)
        {
            AnimateExit(duration);
            LeanTween.delayedCall(duration, () =>
            {
                gameObject.SetActive(false);
            });
        }

        protected virtual void AnimateEntrance(float duration)
        {
            // Set initial state
            rectTransform.localScale = Vector3.one * 0.9f;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;

            // Animate scale
            LeanTween.scale(rectTransform, Vector3.one, duration).setEaseOutQuad();

            // Animate fade
            LeanTween.alphaCanvas(canvasGroup, 1f, duration).setEaseOutQuad();
        }

        protected virtual void AnimateExit(float duration)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // Animate scale
            LeanTween.scale(rectTransform, Vector3.one * 0.9f, duration).setEaseInQuad();

            // Animate fade
            LeanTween.alphaCanvas(canvasGroup, 0f, duration).setEaseInQuad();
        }

    }
}