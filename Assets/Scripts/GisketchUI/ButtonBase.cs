using UnityEngine;
using UnityEngine.EventSystems;

namespace GisketchUI
{
    public abstract class ButtonBase : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected bool isPressed = false;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!isPressed) OnHover();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!isPressed) OnExit();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            OnPress();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
            OnRelease();
        }

        protected virtual void OnHover() { }
        protected virtual void OnExit() { }
        protected virtual void OnPress() { }
        protected virtual void OnRelease() { }
    }
}