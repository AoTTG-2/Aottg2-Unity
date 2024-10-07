using System;
using UnityEngine;
using Settings;
using UnityEngine.UI;

namespace UI
{
    class CrosshairScaler: IgnoreScaler
    {
        public override void ApplyScale()
        {
            base.ApplyScale();
            float mainScale = SettingsManager.UISettings.CrosshairScale.Value;
            RectTransform rect = GetComponent<RectTransform>();
            Vector3 currentScale = rect.localScale;
            rect.localScale = new Vector2(currentScale.x * mainScale, currentScale.y * mainScale);
            int fontSize = 16;
            float textScale = SettingsManager.UISettings.CrosshairTextScale.Value;
            if (textScale > 1f)
            {
                // fontSize = (int)(16 * textScale);
            }
            textScale = textScale / mainScale;
            transform.Find("DefaultLabel").GetComponent<Text>().fontSize = fontSize;
            transform.Find("DefaultLabel").GetComponent<RectTransform>().localScale = new Vector2(textScale, textScale);
        }
    }
}
