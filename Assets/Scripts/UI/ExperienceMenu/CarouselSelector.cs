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
        private GameObject _selectedButton;
        private ScrollRect _scrollRect;
        private CanvasGroup _leftFade;
        private CanvasGroup _rightFade;
        private static Texture2D _fadeTexture;
        private const float FadeWidth = 300f;
        private const float FadeSpeed = 4f;

        public void Setup(Transform parent)
        {
            _content = this.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

            var scrollViewTransform = this.transform.Find("Scroll View");
            _scrollRect = scrollViewTransform.GetComponent<ScrollRect>();

            // Ensure the Viewport stretches to fill the Scroll View
            var viewportRT = scrollViewTransform.Find("Viewport").GetComponent<RectTransform>();
            viewportRT.anchorMin = Vector2.zero;
            viewportRT.anchorMax = Vector2.one;
            viewportRT.sizeDelta = Vector2.zero;
            viewportRT.anchoredPosition = Vector2.zero;

            // Ensure the Content stretches vertically to fill the Viewport
            _content.anchorMin = new Vector2(0f, 0f);
            _content.anchorMax = new Vector2(0f, 1f);
            _content.sizeDelta = new Vector2(_content.sizeDelta.x, 0f);
            _content.anchoredPosition = Vector2.zero;

            // Hide the scrollbar — scrolling still works via drag / mouse-wheel
            if (_scrollRect.horizontalScrollbar != null)
            {
                _scrollRect.horizontalScrollbar.gameObject.SetActive(false);
                _scrollRect.horizontalScrollbar = null;
            }
            if (_scrollRect.verticalScrollbar != null)
            {
                _scrollRect.verticalScrollbar.gameObject.SetActive(false);
                _scrollRect.verticalScrollbar = null;
            }

            // Build edge-fade overlays
            EnsureFadeTexture();
            Color bgColor = GetPanelBackgroundColor();
            _leftFade = CreateFadeOverlay("LeftFade", bgColor, false);
            _rightFade = CreateFadeOverlay("RightFade", bgColor, true);

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

                var capturedBtn = btn;
                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SetSelected(capturedBtn);
                    onSelected(option);
                });
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
            _selectedButton = null;
            this.gameObject.SetActive(false);
        }

        private void SetSelected(GameObject btn)
        {
            if (_selectedButton != null)
            {
                var prevText = _selectedButton.transform.Find("Text").GetComponent<Text>();
                prevText.fontStyle = FontStyle.Normal;
                prevText.color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }
            _selectedButton = btn;
            var text = btn.transform.Find("Text").GetComponent<Text>();
            text.fontStyle = FontStyle.Bold;
            text.color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor") + new Color(0.15f, 0.15f, 0.15f, 0f);
        }

        private void LateUpdate()
        {
            if (_scrollRect == null || _leftFade == null || _rightFade == null || _content == null)
                return;

            float contentWidth = _content.rect.width;
            float viewportWidth = _scrollRect.viewport != null ? _scrollRect.viewport.rect.width : 0f;

            float leftTarget = 0f;
            float rightTarget = 0f;

            if (contentWidth > viewportWidth && viewportWidth > 0f)
            {
                float pos = Mathf.Clamp01(_scrollRect.horizontalNormalizedPosition);
                leftTarget = Mathf.Clamp01(pos / 0.05f);
                rightTarget = Mathf.Clamp01((1f - pos) / 0.05f);
            }

            float step = FadeSpeed * Time.unscaledDeltaTime;
            _leftFade.alpha = Mathf.MoveTowards(_leftFade.alpha, leftTarget, step);
            _rightFade.alpha = Mathf.MoveTowards(_rightFade.alpha, rightTarget, step);
        }

        private Color GetPanelBackgroundColor()
        {
            Transform t = this.transform.parent;
            while (t != null)
            {
                var img = t.GetComponent<Image>();
                if (img != null && img.color.a > 0.5f)
                    return img.color;
                t = t.parent;
            }
            return new Color(0.082f, 0.082f, 0.082f, 1f);
        }

        private static void EnsureFadeTexture()
        {
            if (_fadeTexture != null) return;
            _fadeTexture = new Texture2D(32, 1, TextureFormat.RGBA32, false);
            for (int i = 0; i < 32; i++)
            {
                float a = 1f - (i / 31f);
                _fadeTexture.SetPixel(i, 0, new Color(1f, 1f, 1f, a));
            }
            _fadeTexture.Apply();
            _fadeTexture.wrapMode = TextureWrapMode.Clamp;
            _fadeTexture.filterMode = FilterMode.Bilinear;
        }

        private CanvasGroup CreateFadeOverlay(string name, Color tint, bool rightSide)
        {
            // Parent to the Scroll View so the overlays sit on top of the viewport
            // without interfering with any layout group on the CarouselSelector root.
            var scrollViewTransform = this.transform.Find("Scroll View");
            var go = new GameObject(name, typeof(RectTransform), typeof(RawImage), typeof(CanvasGroup));
            go.transform.SetParent(scrollViewTransform, false);
            go.transform.SetAsLastSibling();

            // Exclude from any layout calculations
            var layoutElem = go.AddComponent<LayoutElement>();
            layoutElem.ignoreLayout = true;

            var rt = go.GetComponent<RectTransform>();
            if (rightSide)
            {
                rt.anchorMin = new Vector2(1f, 0f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.pivot = new Vector2(1f, 0.5f);
            }
            else
            {
                rt.anchorMin = new Vector2(0f, 0f);
                rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 0.5f);
            }
            rt.sizeDelta = new Vector2(FadeWidth, 0f);
            rt.anchoredPosition = Vector2.zero;

            var raw = go.GetComponent<RawImage>();
            raw.texture = _fadeTexture;
            raw.color = tint;
            raw.raycastTarget = false;
            if (rightSide)
                raw.uvRect = new Rect(1f, 0f, -1f, 1f);

            var cg = go.GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;

            return cg;
        }
    }
}
