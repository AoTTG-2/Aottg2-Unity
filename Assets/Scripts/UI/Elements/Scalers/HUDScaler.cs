using System;
using UnityEngine;
using Settings;
using UnityEngine.UI;

namespace UI
{
    class HUDScaler: IgnoreScaler
    {
        public override void ApplyScale()
        {
            base.ApplyScale();
            float scale = SettingsManager.UISettings.HUDScale.Value * 0.5f;
            RectTransform rect = GetComponent<RectTransform>();
            Vector3 currentScale = rect.localScale;
            rect.localScale = new Vector2(currentScale.x * scale, currentScale.y * scale);
        }
    }
}
