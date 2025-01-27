using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI
{
    public class ChatScrollRect : ScrollRect 
    {
        private bool isMouseOver = false;

        protected override void Start()
        {
            base.Start();
            // Initially hide scrollbar handle and set visibility mode
            if (verticalScrollbar != null)
            {
                verticalScrollbar.handleRect.GetComponent<Image>().enabled = false;
                verticalScrollbarVisibility = ScrollbarVisibility.AutoHideAndExpandViewport;
            }
        }

        public void OnMouseEnter()
        {
            isMouseOver = true;
            if (verticalScrollbar != null && content.sizeDelta.y > viewport.rect.height)
            {
                verticalScrollbar.handleRect.GetComponent<Image>().enabled = true;
            }
        }

        public void OnMouseExit()
        {
            isMouseOver = false;
            if (verticalScrollbar != null)
            {
                verticalScrollbar.handleRect.GetComponent<Image>().enabled = false;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData) 
        {
            // Only allow dragging if it started on the scrollbar
            if (verticalScrollbar != null && eventData.pointerCurrentRaycast.gameObject == verticalScrollbar.gameObject)
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData) 
        {
            // Only process drag if it started on the scrollbar
            if (verticalScrollbar != null && eventData.pointerCurrentRaycast.gameObject == verticalScrollbar.gameObject)
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData) 
        {
            // Only process end drag if it started on the scrollbar
            if (verticalScrollbar != null && eventData.pointerCurrentRaycast.gameObject == verticalScrollbar.gameObject)
            {
                base.OnEndDrag(eventData);
            }
        }

        public override void OnScroll(PointerEventData data)
        {
            // Disable scroll wheel by not calling base
        }
    }
} 