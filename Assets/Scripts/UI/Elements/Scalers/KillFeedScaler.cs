using System;
using UnityEngine;
using Settings;
using UnityEngine.UI;

namespace UI
{
    class KillFeedScaler : IgnoreScaler
    {
        public override void ApplyScale()
        {
            base.ApplyScale();
            float mainScale = SettingsManager.UISettings.KillFeedScale.Value;
            RectTransform rect = GetComponent<RectTransform>();
            Scale = rect.localScale.x * mainScale;
            rect.localScale = new Vector2(Scale, Scale);
        }
    }
}