using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class PerkButton : Button
    {
        private string _tooltipMessage;
        private float _offset;

        private void Awake()
        {
            transition = Transition.ColorTint;
            targetGraphic = transform.Find("Image").GetComponent<Image>();
        }

        public virtual void Setup(string tooltipMessage, ElementStyle style, float offset)
        {
            _tooltipMessage = tooltipMessage;
            _offset = offset;
            colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
            var nav = navigation;
            nav.mode = Navigation.Mode.None;
            navigation = nav;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (UIManager.CurrentMenu == null)
                return;
            TooltipPopup popup = UIManager.CurrentMenu.TooltipPopup;
            if (state == SelectionState.Pressed || state == SelectionState.Highlighted)
            {
                popup.Show(_tooltipMessage, this, _offset);

            }
            else if (popup.Caller == this)
            {
                UIManager.CurrentMenu.TooltipPopup.Hide();
            }
        }
    }
}
