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

namespace UI
{
    class MapEditorHierarchyButton : Button
    {
        public GameObject Highlight;
        public Text Name;
        public LayoutElement TextLayoutElement;
        public LayoutElement LayoutElement;
        public HorizontalLayoutGroup LayoutGroup;
        public GameObject TopBar;
        public GameObject BottomBar;
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
            TopBar.SetActive(active);
            BottomBar.SetActive(!active);
        }

        public void HighlightBottomBorder(bool active)
        {
            BottomBar.SetActive(active);
            TopBar.SetActive(!active);
        }

        public void SetBarHighlight(bool active)
        {
            TopBar.SetActive(active);
            BottomBar.SetActive(active);
        }

        public void SetHighlight(bool highlight)
        {
            Highlight.SetActive(highlight);
        }
        
        public void SetNesting(int level)
        {
            LayoutGroup.padding = new RectOffset(10 + (level * 20), 0, 0, 0);
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
