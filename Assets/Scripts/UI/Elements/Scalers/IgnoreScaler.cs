using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Settings;

namespace UI
{
    class IgnoreScaler: BaseScaler
    {
        public float Scale = 1f;

        public override void ApplyScale()
        {
            float scale = SettingsManager.UISettings.UIMasterScale.Value;
            RectTransform rect = GetComponent<RectTransform>();
            Scale = 1f / scale;
            rect.localScale = new Vector2(Scale, Scale);
        }
    }
}
