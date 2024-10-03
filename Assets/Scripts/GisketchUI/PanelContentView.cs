using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DentedPixel;

namespace GisketchUI
{
    public enum ContentLayoutType
    {
        List,
        // Grid,
        // RowList
    }

    public class PanelContentView : UIPanel
    {
        [SerializeField] private Text headerLabel;
        [SerializeField] private RectTransform listContent;
        // [SerializeField] private RectTransform gridContent;
        // [SerializeField] private RectTransform rowListContent;

        private ContentLayoutType currentLayoutType;
        private RectTransform currentContent;

        private List<UIElement> contentElements = new List<UIElement>();

        public void Initialize(string headerText, ContentLayoutType layoutType)
        {
            headerLabel.text = headerText;
            SetContentLayoutType(layoutType);
        }

        public void SetContentLayoutType(ContentLayoutType layoutType)
        {
            currentLayoutType = layoutType;
            listContent.gameObject.SetActive(false);
            // gridContent.gameObject.SetActive(false);
            // rowListContent.gameObject.SetActive(false);

            switch (layoutType)
            {
                case ContentLayoutType.List:
                    currentContent = listContent;
                    break;
                    // case ContentLayoutType.Grid:
                    //     currentContent = gridContent;
                    //     break;
                    // case ContentLayoutType.RowList:
                    //     currentContent = rowListContent;
                    //     break;
            }

            currentContent.gameObject.SetActive(true);
        }

        public void AddElement(UIElement element)
        {
            element.transform.SetParent(currentContent, false);
            contentElements.Add(element);
            AnimateElementEntry(element);
        }

        private void AnimateElementEntry(UIElement element)
        {
            element.transform.localScale = Vector3.zero;
            LeanTween.scale(element.gameObject, Vector3.one, 0.3f).setEaseOutBack();
        }

        public override void Show(float duration = 0.3f)
        {
            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(currentContent);

            base.Show(duration);
            AnimateContentElements(true, duration);

            // Force another layout rebuild after animation
            LeanTween.delayedCall(duration, () =>
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(currentContent);
            });
        }

        public override void Hide(float duration = 0.3f)
        {
            AnimateContentElements(false, duration);
            LeanTween.scale(rectTransform, Vector3.zero, duration).setEaseInBack()
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    ResetContentElements();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(currentContent);
                });
        }


        private void AnimateContentElements(bool show, float duration)
        {
            float delay = 0f;
            float delayIncrement = duration / (contentElements.Count + 1);

            if (show)
            {
                // Activate all elements immediately
                foreach (var element in contentElements)
                {
                    element.gameObject.SetActive(true);
                    element.transform.localScale = Vector3.zero;
                }

                // Then animate them
                foreach (var element in contentElements)
                {
                    LeanTween.scale(element.gameObject, Vector3.one, duration)
                        .setEaseOutBack()
                        .setDelay(delay);
                    delay += delayIncrement;
                }
            }

        }

        private void ResetContentElements()
        {
            foreach (var element in contentElements)
            {
                element.transform.localScale = Vector3.one;
                element.gameObject.SetActive(false);
            }
        }

        public void ClearContent()
        {
            foreach (var element in contentElements)
            {
                Destroy(element.gameObject);
            }
            contentElements.Clear();
        }
    }
}