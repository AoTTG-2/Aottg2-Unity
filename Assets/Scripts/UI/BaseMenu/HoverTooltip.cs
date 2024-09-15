using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Message;
        public float Offset = 40f;
        public TooltipPopup PopupOverride;

        private TooltipPopup popup;

        private TooltipPopup GetPopup() => PopupOverride ? PopupOverride : UIManager.CurrentMenu.TooltipPopup;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (Message != string.Empty && GetPopup())
            {
                popup = GetPopup();
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
