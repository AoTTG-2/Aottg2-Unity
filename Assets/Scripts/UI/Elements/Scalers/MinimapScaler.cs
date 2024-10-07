using System;
using UnityEngine;
using Settings;
using UnityEngine.UI;

namespace UI
{
    class MinimapScaler: IgnoreScaler
    {
        public override void ApplyScale()
        {
            base.ApplyScale();
            float scale = SettingsManager.UISettings.MinimapScale.Value;
            RectTransform rect = GetComponent<RectTransform>();
            Vector3 currentScale = rect.localScale;
            rect.localScale = new Vector2(currentScale.x * scale, currentScale.y * scale);
        }
    }
}
