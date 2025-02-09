using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    class HoldableButton : EventTrigger
    {
        public float ClickSpeed = 12.0f; // clicks per second
        public float HoldDelay = 0.5f; // delay before starting repeated clicks

        private bool isHeld = false;
        private float holdTime = 0f;
        private float nextClickTime = 0f;

        public event Action OnClick;

        void Update()
        {
            if (isHeld)
            {
                holdTime += Time.deltaTime;
                if (holdTime >= HoldDelay)
                {
                    if (Time.time >= nextClickTime)
                    {
                        OnClick?.Invoke();
                        nextClickTime = Time.time + 1f / ClickSpeed;
                    }
                }
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            isHeld = true;
            holdTime = 0f;
            nextClickTime = Time.time + HoldDelay;
            OnClick?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isHeld = false;
        }
    }
}
