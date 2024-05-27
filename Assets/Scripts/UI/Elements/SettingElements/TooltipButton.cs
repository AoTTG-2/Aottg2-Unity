using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class TooltipButton: Button
    {
        private string _tooltipMessage;

        private void Awake()
        {
            transition = Transition.ColorTint;
            targetGraphic = GetComponent<Graphic>();
        }

        public virtual void Setup(string tooltipMessage, ElementStyle style)
        {
            _tooltipMessage = tooltipMessage;
            colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (UIManager.CurrentMenu == null)
                return;
            TooltipPopup popup = UIManager.CurrentMenu.TooltipPopup;
            if (state == SelectionState.Pressed || state == SelectionState.Highlighted)
            {
                popup.Show(_tooltipMessage, this, 40f);
            }
            else if (state == SelectionState.Normal && popup.Caller == this)
            {
                UIManager.CurrentMenu.TooltipPopup.Hide();
            }
        }
    }
}
