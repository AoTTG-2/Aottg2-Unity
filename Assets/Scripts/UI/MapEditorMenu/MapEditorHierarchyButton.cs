using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;
using Map;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using log4net.Core;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;

namespace UI
{
    class MapEditorHierarchyButton : Button
    {
        public GameObject Highlight;
        public GameObject HighlightTop;
        public GameObject HighlightBottom;
        public Text Name;
        public LayoutElement TextLayoutElement;
        public LayoutElement LayoutElement;
        public HorizontalLayoutGroup LayoutGroup;
        public RectTransform RectTransform;
        public int BoundID = -1;

        private GameObject ArrowClosed;
        private GameObject ArrowExpanded;

        private UnityAction _onMouseOver;
        private UnityAction _onMouseDown;

        public void Setup(
            float Width,
            UnityAction onButtonClick,
            UnityAction onExpand,
            UnityAction onCollapse
            )
        {

            #region Styling
            Highlight = transform.Find("Highlight").gameObject;
            Highlight.SetActive(false);
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

            Name.text = obj.ScriptObject.Name;
            BoundID = obj.ScriptObject.Id;
            SetNesting(obj.Level);
            SetExpanded(obj.Expanded, obj.HasChildren);
            SetHighlight(isSelected);
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

        public void ContextHighlight()
        {
            HighlightTop.SetActive(false);
            HighlightBottom.SetActive(false);
            Highlight.SetActive(false);

            // Shift all highlight UI elements by the level


            Vector2 percent = GetPercentCovered();
            if (percent.y >= 0.9f)
            {
                HighlightTop.SetActive(true);
            }
            else if (percent.y <= 0.1f)
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
            Vector2 sizeDelta = rectTransform.sizeDelta / 2;
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

            // clamp mouse position to bounds
            localMousePosition.x = Mathf.Clamp(localMousePosition.x, 0, sizeDelta.x);
            localMousePosition.y = Mathf.Clamp(localMousePosition.y, 0, sizeDelta.y);

            // calculate percentage covered
            float percentX = localMousePosition.x / sizeDelta.x;
            float percentY = localMousePosition.y / sizeDelta.y;

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
