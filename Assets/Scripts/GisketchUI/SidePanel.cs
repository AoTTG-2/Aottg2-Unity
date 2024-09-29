using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GisketchUI
{
    public abstract class SidePanel : UIElement
    {
        [SerializeField] protected RectTransform background;
        [SerializeField] protected RectTransform contentContainer;

        protected List<SidePanelButton> buttons = new List<SidePanelButton>();
        protected List<SidePanelButton> footerButtons = new List<SidePanelButton>();
        protected RectTransform panelRectTransform;

        public virtual void Initialize()
        {
            panelRectTransform = GetComponent<RectTransform>();
        }

        public override void Show(float duration = 0.5f)
        {
            gameObject.SetActive(true);
            ResetPosition();
            AnimateEntrance(duration);
        }

        public override void Hide(float duration = 0.5f)
        {
            AnimateExit(duration);
            LeanTween.delayedCall(duration, () =>
            {
                gameObject.SetActive(false);
                ResetPosition();
            });
        }

        protected virtual void AnimateEntrance(float duration)
        {
            // Animate the entire panel
            UIAnimator.SlideIn(panelRectTransform, UIAnimator.SlideDirection.Left, duration);
        }

        protected virtual void AnimateExit(float duration)
        {
            // Animate the entire panel
            UIAnimator.SlideOut(panelRectTransform, UIAnimator.SlideDirection.Left, duration);
        }

        protected void AddButton(SidePanelButton button)
        {
            buttons.Add(button);
        }

        protected void AddFooterButton(SidePanelButton button)
        {
            footerButtons.Add(button);
        }

        private void ResetPosition()
        {
            // Reset the position of the main panel
            panelRectTransform.anchoredPosition = Vector2.zero;
        }
    }
}