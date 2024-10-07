using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UI
{
    class VerticalLineScaler: BaseScaler
    {
        public override void ApplyScale()
        {
            float scale = UIManager.CurrentCanvasScale;
            RectTransform rect = GetComponent<RectTransform>();
            float width = 1f;
            if (width * scale < 1f)
                width = 1f / scale;
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        }
    }
}
