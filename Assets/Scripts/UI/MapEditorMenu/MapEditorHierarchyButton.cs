using UnityEngine.UI;
using UnityEngine;
using Map;
using UnityEngine.Events;
using PlasticPipe.PlasticProtocol.Messages;

namespace UI
{
    class MapEditorHierarchyButton : Button
    {
        public GameObject Highlight;
        public GameObject HighlightDark;
        public GameObject HighlightTop;
        public GameObject HighlightBottom;
        public Text Name;
        public LayoutElement TextLayoutElement;
        public LayoutElement LayoutElement;
        public HorizontalLayoutGroup LayoutGroup;
        public RectTransform RectTransform;
        public int BoundID = -1;
        public float TopBorder = 0.65f;
        public float BottomBorder = 0.35f;

        private GameObject ArrowClosed;
        private GameObject ArrowExpanded;


        public void Setup(float Width,
            UnityAction onButtonClick,
            UnityAction onExpand,
            UnityAction onCollapse)
        {

            #region Styling
            Highlight = transform.Find("Highlight").gameObject;
            Highlight.SetActive(false);
            HighlightDark = transform.Find("HighlightDark").gameObject;
            HighlightDark.SetActive(false);
            HighlightTop = transform.Find("HighlightTop").gameObject;
            HighlightTop.SetActive(false);
            HighlightBottom = transform.Find("HighlightBottom").gameObject;
            HighlightBottom.SetActive(false);


            Name = transform.Find("Text").GetComponent<Text>();
            Name.text = string.Empty;
            Name.color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");

            TextLayoutElement = transform.Find("Text").GetComponent<LayoutElement>();
            TextLayoutElement.preferredWidth = Width;

            LayoutGroup = transform.GetComponent<HorizontalLayoutGroup>();
            LayoutGroup.padding = new RectOffset(10, 0, 0, 0);

            LayoutElement = transform.GetComponent<LayoutElement>();
            LayoutElement.minWidth = Width;
            LayoutElement.preferredWidth = Width;

            // Tree view buttons
            ArrowClosed = transform.Find("ArrowRightButton").gameObject;
            ArrowExpanded = transform.Find("ArrowDownButton").gameObject;
            ArrowClosed.SetActive(true);
            ArrowExpanded.SetActive(false);

            RectTransform = transform.GetComponent<RectTransform>();

            #endregion

            onClick.AddListener(onButtonClick);
            transition = Transition.None;

            Button arrowClosed = ArrowClosed.GetComponent<Button>();
            arrowClosed.onClick.AddListener(onExpand);
            Button arrowExpanded = ArrowExpanded.GetComponent<Button>();
            arrowExpanded.onClick.AddListener(onCollapse);

        }

        public bool IsCorrectlyBound(MapObject obj, bool isSelected)
        {
            return obj.ScriptObject.Id == BoundID && obj.Level == Level && ExpandStateValid(obj.Expanded, obj.HasChildren) && Highlight.activeSelf == isSelected;
        }

        public void Bind(MapObject obj = null, bool isSelected = false)
        {
            if (obj == null)
            {
                Name.text = string.Empty;
                BoundID = -1;
                SetNesting(0);
                SetExpanded(false, false);
                SetHighlight(false);
                return;
            }

            Name.text = obj.Name;
            BoundID = obj.ScriptObject.Id;
            SetNesting(obj.Level);
            SetExpanded(obj.Expanded, obj.HasChildren);
            SetHighlight(isSelected);
            SetDarkHighlight(false);
        }

        public void Bind(string name, int id, int level, bool isSelected)
        {
            Name.text = name;
            BoundID = id;
            SetNesting(level);
            SetHighlight(isSelected);
        }

        public void HighlightTopBorder(bool active)
        {
            HighlightTop.SetActive(active);
            HighlightBottom.SetActive(!active);
        }

        public void HighlightBottomBorder(bool active)
        {
            HighlightBottom.SetActive(active);
            HighlightTop.SetActive(!active);
        }

        public void SetBarHighlight(bool active)
        {
            HighlightTop.SetActive(active);
            HighlightBottom.SetActive(active);
        }

        public void SetDarkHighlight(bool active)
        {
            HighlightDark.SetActive(active);
            HighlightDark.SetActive(active);
        }

        public void ContextHighlight()
        {
            HighlightTop.SetActive(false);
            HighlightBottom.SetActive(false);
            Highlight.SetActive(false);

            Vector2 percent = GetPercentCovered();
            if (percent.y >= TopBorder)
            {
                HighlightTop.SetActive(true);
            }
            else if (percent.y <= BottomBorder)
            {
                HighlightBottom.SetActive(true);
            }
            else
            {
                Highlight.SetActive(true);
            }
        }

        public Vector2 GetPercentCovered()
        {
            RectTransform rectTransform = transform.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 mousePosition);
            mousePosition += rectTransform.sizeDelta / 2;
            mousePosition.x = Mathf.Clamp(mousePosition.x, 0, rectTransform.sizeDelta.x);
            mousePosition.y = Mathf.Clamp(mousePosition.y, 0, rectTransform.sizeDelta.y);

            float percentX = mousePosition.x / rectTransform.sizeDelta.x;
            float percentY = mousePosition.y / rectTransform.sizeDelta.y;

            return new Vector2(percentX, percentY);
        }

        public void ClearContextHighlight()
        {
            HighlightTop.SetActive(false);
            HighlightBottom.SetActive(false);
            Highlight.SetActive(false);
        }

        public void SetHighlight(bool highlight)
        {
            Highlight.SetActive(highlight);
        }
        
        public void SetNesting(int level)
        {
            LayoutGroup.padding = new RectOffset(10 + (level * 20), 0, 0, 0);

            // Calculate new size of the highlight objects
            float parentWidth = transform.GetComponent<RectTransform>().rect.width;
            float padding = LayoutGroup.padding.left + LayoutGroup.padding.right;
            float barWidth = parentWidth - padding;

            Highlight.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
            HighlightBottom.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
            HighlightTop.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
        }

        public float Level => (LayoutGroup.padding.left / 20) - 10;

        public void SetExpanded(bool isExpanded, bool hasChildren)
        {
            ArrowClosed.SetActive(!isExpanded && hasChildren);
            ArrowExpanded.SetActive(isExpanded && hasChildren);
        }

        public bool ExpandStateValid(bool isExpanded, bool hasChildren)
        {
            return ArrowClosed.activeSelf == !isExpanded && hasChildren && ArrowExpanded.activeSelf == isExpanded && hasChildren;
        }
    }
}
