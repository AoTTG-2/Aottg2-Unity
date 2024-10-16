using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class MultiTextLabel : MonoBehaviour
    {
        // Suggested method for avoiding garbage collection: https://docs.unity3d.com/Manual/performance-garbage-collection-best-practices.html
        Text[] texts;

        public void Setup(Transform parent, ElementStyle style, FontStyle fontStyle, TextAnchor anchor, float fontSize, int numberOfLabels, bool richText=false)
        {
            // create a horizontal group of labels
            texts = new Text[numberOfLabels];
            for (int i = 0; i < numberOfLabels; i++)
            {
                texts[i] = ElementFactory.CreateDefaultLabel(transform, style, string.Empty, fontStyle, anchor).GetComponent<Text>();
                texts[i].supportRichText = richText;
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (gameObject.activeSelf != enabled)
                gameObject.SetActive(enabled);
        }

        public bool GetEnabled()
        {
            return gameObject.activeSelf;
        }

        public void SetElementEnabled(int index, bool enabled)
        {
            if (index < 0 || index >= texts.Length)
                return;
            if (texts[index].gameObject.activeSelf != enabled)
                texts[index].gameObject.SetActive(enabled);
        }

        public bool GetElementEnabled(int index)
        {
            if (index < 0 || index >= texts.Length)
                return false;
            return texts[index].gameObject.activeSelf;
        }

        public void SetValue(int index, string value)
        {
            if (index < 0 || index >= texts.Length)
                return;
            texts[index].text = value;
        }

        public void ChangeTextColor(int index, Color color)
        {
            if (index < 0 || index >= texts.Length)
                return;
            texts[index].color = color;
        }

        public string GetValue(int index)
        {
            if (index < 0 || index >= texts.Length)
                return string.Empty;
            return texts[index].text;
        }
    }
}
