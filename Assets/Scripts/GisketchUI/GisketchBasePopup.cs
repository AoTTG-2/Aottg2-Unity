using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public abstract class GisketchBasePopup : GisketchUIElement
    {
        protected GisketchPanelContentView contentView;

        protected virtual string Title => "Popup";

        public virtual void Setup(Transform parent)
        {
            contentView = GisketchElementFactory.CreatePanel(parent, Title, ContentLayoutType.List);
            contentView.gameObject.SetActive(false); // Start hidden
            SetupContent();
        }

        protected abstract void SetupContent();

        protected virtual void ClearContent()
        {
            contentView.ClearContent();
        }

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
            LeanTween.delayedCall(duration, () => contentView.gameObject.SetActive(false));
        }
    }
}