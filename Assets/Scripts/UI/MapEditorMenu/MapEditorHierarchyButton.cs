using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;
using Map;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UI
{
    class MapEditorHirarchyButton: Button
    {
        private UnityAction _onButtonRelease;

        public void Setup(UnityAction onButtonClick, UnityAction onButtonRelease)
        {
            onClick.AddListener(onButtonClick);
            _onButtonRelease = onButtonRelease;
            transition = Transition.None;
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _onButtonRelease.Invoke();
        }
    }
}
