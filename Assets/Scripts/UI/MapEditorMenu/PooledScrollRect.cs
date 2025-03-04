using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    class PooledScrollRect : ScrollRect
    {
        public void Copy(ScrollRect rect)
        {
            this.content = rect.content;
            this.horizontal = rect.horizontal;
            this.vertical = rect.vertical;
            this.movementType = rect.movementType;
            this.elasticity = rect.elasticity;
            this.inertia = rect.inertia;
            this.decelerationRate = rect.decelerationRate;
            this.scrollSensitivity = rect.scrollSensitivity;
            this.viewport = rect.viewport;
            this.horizontalScrollbar = rect.horizontalScrollbar;
            this.verticalScrollbar = rect.verticalScrollbar;
            this.horizontalScrollbarVisibility = rect.horizontalScrollbarVisibility;
            this.verticalScrollbarVisibility = rect.verticalScrollbarVisibility;
            this.horizontalScrollbarSpacing = rect.horizontalScrollbarSpacing;
            this.verticalScrollbarSpacing = rect.verticalScrollbarSpacing;
        }

        protected override void Start()
        {
            base.Start();
            if (verticalScrollbar != null)
            {
                verticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            // verticalNormalizedPosition = 0f;
        }

        public override void OnDrag(PointerEventData eventData) { }

        public override void OnEndDrag(PointerEventData eventData) { }

        public override void OnInitializePotentialDrag(PointerEventData eventData) { }

        private void OnScrollbarValueChanged(float value)
        {
            if (onValueChanged != null)
                onValueChanged.Invoke(new Vector2(0, value));
        }
    }
}
