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
    class MapEditorHirarchyButton: Button
    {
        public GameObject Highlight;
        public Text Name;
        public LayoutElement TextLayoutElement;
        public LayoutElement LayoutElement;
        public HorizontalLayoutGroup LayoutGroup;
        public GameObject TopBar;
        public GameObject BottomBar;
        public int BoundID = -1;

        private UnityAction _onButtonRelease;
        private UnityAction _onButtonDown;
        private UnityAction _onMove;
        private GameObject ArrowClosed;
        private GameObject ArrowExpanded;

        public void Setup(
            float Width,
            UnityAction onButtonClick,
            UnityAction onExpand,
            UnityAction onCollapse)
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

            _onButtonRelease = onButtonRelease;
            _onButtonDown = onButtonDown;
            _onMove = onMove;

            Button arrowClosed = ArrowClosed.GetComponent<Button>();
            arrowClosed.onClick.AddListener(onExpand);
            Button arrowExpanded = ArrowExpanded.GetComponent<Button>();
            arrowExpanded.onClick.AddListener(onCollapse);

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

        public void SetExpanded(bool isExpanded, bool hasChildren)
        {
            ArrowClosed.SetActive(!isExpanded && hasChildren);
            ArrowExpanded.SetActive(isExpanded && hasChildren);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _onButtonRelease.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _onButtonDown.Invoke();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            Debug.Log(eventData.moveVector);
            _onMove.Invoke();
        }
    }
}
