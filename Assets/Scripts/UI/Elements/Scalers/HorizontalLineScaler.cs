using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UI
{
    class HorizontalLineScaler: BaseScaler
    {
        public override void ApplyScale()
        {
            float scale = UIManager.CurrentCanvasScale;
            RectTransform rect = GetComponent<RectTransform>();
            float height = 1f;
            if (height * scale < 1f)
                height = 1f / scale;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        }
    }
}
