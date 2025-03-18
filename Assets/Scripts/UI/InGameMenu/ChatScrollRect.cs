using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI
{
    public class ChatScrollRect : ScrollRect 
    {
        private bool isMouseOver = false;
        private bool isDragging = false;
        private Image handleImage;
        private ChatPanel _chatPanel;
        private float lastScrollTime = 0f;
        private const float SCROLL_INTERACTION_TIMEOUT = 1.0f;

        protected override void Start()
        {
            base.Start();
            _chatPanel = GetComponentInParent<ChatPanel>();
            if (verticalScrollbar != null)
            {
                verticalScrollbarVisibility = ScrollbarVisibility.AutoHide;
                handleImage = verticalScrollbar.handleRect.GetComponent<Image>();
                verticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
                verticalScrollbar.gameObject.SetActive(true);
                if (handleImage != null)
                {
                    handleImage.enabled = true;
                }
            }
        }

        private void OnScrollbarValueChanged(float value)
        {
            if (onValueChanged != null)
                onValueChanged.Invoke(new Vector2(0, value));
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (_chatPanel != null && !_chatPanel.IsInteractingWithChatUI() && !isDragging && !isMouseOver && Time.time > lastScrollTime + SCROLL_INTERACTION_TIMEOUT)
            {
                verticalNormalizedPosition = 0f;
            }
            if (verticalScrollbar != null)
            {
                ScrollbarVisibility currentVisibility = verticalScrollbarVisibility;
                verticalScrollbarVisibility = ScrollbarVisibility.Permanent;
                verticalScrollbarVisibility = ScrollbarVisibility.AutoHide;
                if (isDragging)
                {
                    handleImage.enabled = true;
                }
            }
        }

        public void OnMouseEnter()
        {
            isMouseOver = true;
            UpdateHandleVisibility();
        }

        public void OnMouseExit()
        {
            isMouseOver = false;
            if (handleImage != null && !isDragging)
            {
                handleImage.enabled = false;
            }
        }

        private void UpdateHandleVisibility()
        {
            if (handleImage != null)
            {
                handleImage.enabled = isMouseOver || isDragging;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData) 
        {
            isDragging = true;
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData) 
        {
            base.OnEndDrag(eventData);
            isDragging = false;
            if (handleImage != null)
            {
                handleImage.enabled = isMouseOver;
            }
        }

        public override void OnDrag(PointerEventData eventData) 
        {
            if (isDragging)
            {
                if (handleImage != null)
                {
                    handleImage.enabled = true;
                }
                base.OnDrag(eventData);
            }
        }

        public override void OnScroll(PointerEventData data)
        {
            if (isMouseOver)
            {
                data.scrollDelta *= 1.5f;
                base.OnScroll(data);
                lastScrollTime = Time.time;
                if (_chatPanel != null)
                {
                    _chatPanel.UpdateChatInteractionState();
                }
                if (handleImage != null)
                {
                    handleImage.enabled = true;
                }
            }
        }
    }
} 