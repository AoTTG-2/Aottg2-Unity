using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using GameManagers;
using System.Collections;
using System.Linq;
using SimpleJSONFixed;

namespace UI
{
    class CreditsMenu : BaseMenu
    {
        public float scrollSpeed = 60f;
        public float fastScrollMultiplier = 5f;

        private RectTransform _contentTransform;
        private VerticalLayoutGroup _layoutGroup;
        private Font _categoryFont;
        private float _tipDisplayTime = 5f;
        private float _tipTimer;

        private List<Color> _categoryColors;
        private int _currentColorIndex = 0;
        private Sprite _brushSprite;
        private Text _tipText;

        public override void Setup()
        {
            base.Setup();
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: "DefaultPanel");

            // Set up background
            GameObject bgObject = new GameObject("Background", typeof(RectTransform), typeof(Image), typeof(AspectRatioFitter));
            bgObject.transform.SetParent(transform, false);
            Image bgImage = bgObject.GetComponent<Image>();
            bgImage.sprite = Resources.Load<Sprite>("UI/Backgrounds/DarkBackgroundTextured");
            bgImage.type = Image.Type.Sliced;
            bgImage.preserveAspect = true;

            RectTransform bgRect = bgObject.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            AspectRatioFitter aspectFitter = bgObject.GetComponent<AspectRatioFitter>();
            aspectFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            aspectFitter.aspectRatio = bgImage.sprite.rect.width / bgImage.sprite.rect.height;

            // Create back button
            var button = ElementFactory.CreateDefaultButton(transform, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.SetAnchor(button, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));

            // Create content container
            GameObject contentContainer = new GameObject("CreditsContent", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            _contentTransform = contentContainer.GetComponent<RectTransform>();
            _layoutGroup = contentContainer.GetComponent<VerticalLayoutGroup>();
            ContentSizeFitter contentSizeFitter = contentContainer.GetComponent<ContentSizeFitter>();
            _contentTransform.SetParent(transform, false);
            _contentTransform.anchorMin = new Vector2(0.5f, 1);
            _contentTransform.anchorMax = new Vector2(0.5f, 1);
            _contentTransform.anchoredPosition = Vector2.zero;
            _contentTransform.sizeDelta = new Vector2(800, 0);

            // Configure layout
            _layoutGroup.childAlignment = TextAnchor.UpperCenter;
            _layoutGroup.childControlHeight = true;
            _layoutGroup.childForceExpandHeight = false;
            _layoutGroup.spacing = 10f;

            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Load resources
            _categoryFont = Resources.Load<Font>("UI/Fonts/Intensa Fuente");
            _brushSprite = Resources.Load<Sprite>("UI/Sprites/Elements/Brush");

            // Initialize category colors
            _categoryColors = new List<Color>
            {
                ColorUtility.TryParseHtmlString("#2065a0", out Color c1) ? c1 : Color.white,
                ColorUtility.TryParseHtmlString("#ba661f", out Color c2) ? c2 : Color.white,
                ColorUtility.TryParseHtmlString("#29888a", out Color c3) ? c3 : Color.white,
                ColorUtility.TryParseHtmlString("#813d52", out Color c4) ? c4 : Color.white,
                ColorUtility.TryParseHtmlString("#614c90", out Color c5) ? c5 : Color.white
            };

            CreateTip();
            PopulateCredits();
        }

        private void CreateTip()
        {
            ElementStyle tipStyle = new ElementStyle();
            GameObject tipObject = ElementFactory.CreateDefaultLabel(transform, tipStyle, "Hold LMB or Space to fast forward, and RMB to go backwards");
            _tipText = tipObject.GetComponent<Text>();
            _tipText.fontSize = 20;
            _tipText.alignment = TextAnchor.MiddleCenter;

            RectTransform tipRect = tipObject.GetComponent<RectTransform>();
            tipRect.anchorMin = new Vector2(0.5f, 1);
            tipRect.anchorMax = new Vector2(0.5f, 1);
            tipRect.anchoredPosition = new Vector2(0, -50);
            tipRect.sizeDelta = new Vector2(600, 50);

            _tipTimer = _tipDisplayTime;
        }

        private void PopulateCredits()
        {
            if (MiscInfo.Credits == null)
            {
                CreateTextElement("Error loading data.", false);
                return;
            }

            foreach (JSONNode node in MiscInfo.Credits)
            {
                CreateCategoryElement(node["Category"].Value);

                List<string> names = new List<string>();
                foreach (JSONNode name in node["Names"])
                {
                    if (!names.Contains(name.Value))
                        names.Add(name.Value);
                }
                names.Sort();
                CreateTextElement(string.Join("\n", names), false);

                CreateSpacerElement(20f);
            }

            CreateTextElement("\nBased on the original game created by Feng Li and Jiang Li.", false);

            // Force layout update and position content
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentTransform);

            float contentHeight = _layoutGroup.preferredHeight;
            _contentTransform.anchoredPosition = new Vector2(0, (-contentHeight / 2) - 300);
        }

        private void CreateCategoryElement(string content)
        {
            GameObject container = new GameObject("CategoryContainer", typeof(RectTransform), typeof(LayoutElement));
            container.transform.SetParent(_contentTransform, false);
            RectTransform containerRect = container.GetComponent<RectTransform>();
            LayoutElement layoutElement = container.GetComponent<LayoutElement>();

            // Create background brush image
            GameObject bgObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgObject.transform.SetParent(container.transform, false);
            Image bgImage = bgObject.GetComponent<Image>();
            bgImage.sprite = _brushSprite;
            bgImage.color = GetNextCategoryColor();
            RectTransform bgRect = bgObject.GetComponent<RectTransform>();

            // Create text
            GameObject textObj = ElementFactory.CreateDefaultLabel(container.transform, new ElementStyle(), content);
            Text textComponent = textObj.GetComponent<Text>();
            textComponent.font = _categoryFont;
            textComponent.fontSize = 32;
            textComponent.fontStyle = FontStyle.Bold;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.color = Color.white;
            RectTransform textRect = textComponent.rectTransform;

            // Set up layout
            containerRect.anchorMin = new Vector2(0, 1);
            containerRect.anchorMax = new Vector2(1, 1);
            containerRect.anchoredPosition = Vector2.zero;

            LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);

            // Calculate sizes
            Vector2 textSize = textRect.sizeDelta;
            Vector2 padding = new Vector2(40, 20);
            Vector2 totalSize = textSize + padding;

            float brushWidthReduction = 10f;
            float brushHeightReduction = 15f;
            Vector2 brushSize = new Vector2(totalSize.x - brushWidthReduction, totalSize.y - brushHeightReduction);

            // Apply sizes
            containerRect.sizeDelta = new Vector2(0, totalSize.y);
            bgRect.sizeDelta = brushSize;
            textRect.sizeDelta = totalSize;

            // Center elements
            bgRect.anchorMin = bgRect.anchorMax = new Vector2(0.5f, 0.5f);
            bgRect.anchoredPosition = new Vector2(0, -5f);

            textRect.anchorMin = textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;

            bgObject.transform.SetAsFirstSibling();

            layoutElement.minHeight = totalSize.y;
            layoutElement.preferredHeight = totalSize.y;
        }

        private void CreateSpacerElement(float height)
        {
            GameObject spacer = new GameObject("Spacer", typeof(RectTransform), typeof(LayoutElement));
            spacer.transform.SetParent(_contentTransform, false);
            LayoutElement layoutElement = spacer.GetComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;
        }

        private void CreateTextElement(string content, bool isCategory)
        {
            ElementStyle style = new ElementStyle();

            if (isCategory)
            {
                GameObject container = new GameObject("CategoryContainer", typeof(RectTransform));
                container.transform.SetParent(_contentTransform, false);
                RectTransform containerRect = container.GetComponent<RectTransform>();

                GameObject bgObject = new GameObject("Background", typeof(RectTransform), typeof(Image));
                bgObject.transform.SetParent(container.transform, false);
                Image bgImage = bgObject.GetComponent<Image>();
                bgImage.sprite = _brushSprite;
                bgImage.color = GetNextCategoryColor();
                RectTransform bgRect = bgObject.GetComponent<RectTransform>();

                GameObject textObj = ElementFactory.CreateDefaultLabel(container.transform, style, content);
                Text textComponent = textObj.GetComponent<Text>();
                textComponent.font = _categoryFont;
                textComponent.fontSize = 32;
                textComponent.fontStyle = FontStyle.Bold;
                textComponent.alignment = TextAnchor.MiddleCenter;
                textComponent.color = Color.white;
                RectTransform textRect = textComponent.rectTransform;

                containerRect.anchorMin = new Vector2(0.5f, 1);
                containerRect.anchorMax = new Vector2(0.5f, 1);
                containerRect.anchoredPosition = Vector2.zero;

                LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);

                Vector2 textSize = textRect.sizeDelta;
                Vector2 padding = new Vector2(40, 10);
                Vector2 totalSize = textSize + padding;

                float brushWidthReduction = 25f;
                float brushHeightReduction = 35f;
                Vector2 brushSize = new Vector2(totalSize.x - brushWidthReduction, totalSize.y - brushHeightReduction);

                containerRect.sizeDelta = totalSize;
                bgRect.sizeDelta = brushSize;
                textRect.sizeDelta = totalSize;

                bgRect.anchorMin = bgRect.anchorMax = new Vector2(0.5f, 0.5f);
                bgRect.anchoredPosition = new Vector2(0, -15f);

                textRect.anchorMin = textRect.anchorMax = new Vector2(0.5f, 0.5f);
                textRect.anchoredPosition = Vector2.zero;

                bgObject.transform.SetAsFirstSibling();
            }
            else
            {
                string[] lines = content.Split('\n');
                if (lines.Length <= 8)
                {
                    // Create single-column text
                    GameObject textObj = ElementFactory.CreateDefaultLabel(_contentTransform, style, content);
                    Text textComponent = textObj.GetComponent<Text>();
                    textComponent.fontSize = 18;
                    textComponent.alignment = TextAnchor.UpperCenter;
                }
                else
                {
                    // Create two-column layout for longer content
                    var sortedLines = lines.OrderByDescending(l => l.Length).ToList();

                    List<string> leftColumn = new List<string>();
                    List<string> rightColumn = new List<string>();
                    int leftCharCount = 0, rightCharCount = 0;

                    // Distribute lines between columns
                    for (int i = 0; i < sortedLines.Count; i++)
                    {
                        if (leftCharCount <= rightCharCount)
                        {
                            leftColumn.Add(sortedLines[i]);
                            leftCharCount += sortedLines[i].Length;
                        }
                        else
                        {
                            rightColumn.Add(sortedLines[i]);
                            rightCharCount += sortedLines[i].Length;
                        }
                    }

                    string leftColumnArranged = ArrangeColumn(leftColumn);
                    string rightColumnArranged = ArrangeColumn(rightColumn);

                    // Create container for two columns
                    GameObject container = new GameObject("TwoColumnContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
                    container.transform.SetParent(_contentTransform, false);
                    HorizontalLayoutGroup hlg = container.GetComponent<HorizontalLayoutGroup>();
                    hlg.childAlignment = TextAnchor.UpperCenter;
                    hlg.childControlWidth = true;
                    hlg.childForceExpandWidth = false;
                    hlg.spacing = 60f;
                    hlg.childControlHeight = false;

                    CreateColumnText(container.transform, leftColumnArranged, style, TextAnchor.UpperLeft);
                    CreateColumnText(container.transform, rightColumnArranged, style, TextAnchor.UpperRight);
                }
            }
        }

        private string ArrangeColumn(List<string> column)
        {
            int midPoint = column.Count / 2;
            List<string> topHalf = column.Take(midPoint).ToList();
            List<string> bottomHalf = column.Skip(midPoint).Reverse().ToList();

            List<string> arranged = new List<string>();
            arranged.AddRange(topHalf);
            arranged.AddRange(bottomHalf);

            return string.Join("\n", arranged);
        }

        private void CreateColumnText(Transform parent, string content, ElementStyle style, TextAnchor alignment)
        {
            GameObject textObj = ElementFactory.CreateDefaultLabel(parent, style, content);
            Text textComponent = textObj.GetComponent<Text>();
            textComponent.fontSize = 18;
            textComponent.alignment = alignment;

            RectTransform rectTransform = textObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(360f, 0);
        }

        private Color GetNextCategoryColor()
        {
            Color color = _categoryColors[_currentColorIndex];
            _currentColorIndex = (_currentColorIndex + 1) % _categoryColors.Count;
            return color;
        }

        private void Update()
        {
            float currentScrollSpeed = scrollSpeed;

            // Handle input for scrolling speed
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                currentScrollSpeed *= fastScrollMultiplier;
            }
            else if (Input.GetMouseButton(1))
            {
                currentScrollSpeed *= -fastScrollMultiplier;
            }

            // Scroll the content
            Vector3 position = _contentTransform.localPosition;
            position.y += currentScrollSpeed * Time.deltaTime;
            _contentTransform.localPosition = position;

            // Reset position when scrolling is complete
            if (_contentTransform.localPosition.y >= _layoutGroup.preferredHeight)
            {
                _contentTransform.localPosition = new Vector3(0, -_layoutGroup.preferredHeight, 0);
            }
            else if (_contentTransform.localPosition.y <= -_layoutGroup.preferredHeight)
            {
                _contentTransform.localPosition = Vector3.zero;
            }

            // Handle tip fade out
            if (_tipTimer > 0)
            {
                _tipTimer -= Time.deltaTime;
                if (_tipTimer <= 0)
                {
                    StartCoroutine(FadeOutTip());
                }
            }
        }

        private IEnumerator FadeOutTip()
        {
            float fadeTime = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
                _tipText.color = new Color(_tipText.color.r, _tipText.color.g, _tipText.color.b, alpha);
                yield return null;
            }

            _tipText.gameObject.SetActive(false);
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                SceneLoader.LoadScene(SceneName.MainMenu);
        }
    }
}