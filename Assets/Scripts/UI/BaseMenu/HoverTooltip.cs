using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Message;
        public float Offset = 40f;

        private TooltipPopup popup;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (UIManager.CurrentMenu.TooltipPopup && Message != string.Empty)
            {
                popup = UIManager.CurrentMenu.TooltipPopup;
                popup.Show(Message, this, Offset);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (popup && popup.Caller == this)
                popup.Hide();
            popup = null;
        }
    }
}
