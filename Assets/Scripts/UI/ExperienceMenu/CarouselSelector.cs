using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class YourOptionData
    {
        public string ID;
        public string Name;
        public Texture2D Icon;

        public YourOptionData() { }

    }

    internal class CarouselSelector : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;

        private List<GameObject> _buttons = new List<GameObject>();

        public void Setup(Transform parent)
        {
            _content = this.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

            var rect = this.GetComponent<RectTransform>();
        }

        public void Populate(List<YourOptionData> options, Action<YourOptionData> onSelected)
        {
            ClearButtons();
            this.gameObject.SetActive(true);
            foreach (var option in options)
            {
                var btn = ElementFactory.InstantiateAndBind(_content.transform, "Prefabs/Misc/MapSelectObjectButton");
                var rect = btn.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);

                btn.GetComponent<Button>().onClick.AddListener(() => onSelected(option));
                if (option.Icon != null)
                {
                    rect.sizeDelta = new Vector2(256, 150);
                    btn.transform.Find("Icon").GetComponent<RawImage>().texture = option.Icon;
                }
                else
                {
                    rect.sizeDelta = new Vector2(256, 50);
                    btn.transform.Find("Icon").gameObject.SetActive(false);
                }

                btn.transform.Find("Text").GetComponent<Text>().text = option.Name;
                btn.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");

                _buttons.Add(btn);
            }
        }

        public void ClearButtons()
        {
            foreach (var btn in _buttons)
                Destroy(btn.gameObject);
            _buttons.Clear();
            this.gameObject.SetActive(false);
        }
    }
}
