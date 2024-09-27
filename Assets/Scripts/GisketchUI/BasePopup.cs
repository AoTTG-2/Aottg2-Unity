using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public abstract class BasePopup : UIElement
    {
        protected PanelContentView contentView;

        protected virtual string Title => "Popup";
        protected virtual float Width => 480f;

        public virtual void Setup(Transform parent)
        {
            contentView = ElementFactory.CreatePanel(parent, Title, ContentLayoutType.List);
            contentView.gameObject.SetActive(false); // Start hidden

            RectTransform rectTransform = contentView.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
            }

            // Add a GraphicRaycaster to the content view if it doesn't have one
            if (contentView.GetComponent<GraphicRaycaster>() == null)
            {
                contentView.gameObject.AddComponent<GraphicRaycaster>();
            }

            SetupContent();
        }

        protected abstract void SetupContent();

        protected virtual void ClearContent()
        {
            contentView.ClearContent();
        }

        // public virtual void ApplyScale(float scaleFactor)
        // {
        //     if (contentView != null)
        //     {
        //         RectTransform rectTransform = contentView.GetComponent<RectTransform>();
        //         if (rectTransform != null)
        //         {
        //             rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width * scaleFactor);
        //         }
        //     }
        // }

        public override void Show(float duration = 0.3f)
        {
            contentView.gameObject.SetActive(true);
            ClearContent(); // Clear existing content
            SetupContent(); // Set up new content
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentView.rectTransform);
            contentView.Show(duration);
        }

        public override void Hide(float duration = 0.3f)
        {
            contentView.Hide(duration);
            LeanTween.delayedCall(duration, () =>
            {
                contentView.gameObject.SetActive(false);
                PopupManager.Instance.OnPopupHidden(this);
            });
        }
    }
}