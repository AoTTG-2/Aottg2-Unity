using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "ScrollView", Description = "ScrollView UI element that provides scrollable content. Note: Most methods return self to allow method chaining.")]
    partial class CustomLogicScrollViewBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly ScrollView _scrollView;

        public CustomLogicScrollViewBuiltin(ScrollView scrollView) : base(scrollView)
        {
            _scrollView = scrollView;
        }

        [CLProperty("The current scroll offset.")]
        public CustomLogicVector2Builtin ScrollOffset
        {
            get => new CustomLogicVector2Builtin(_scrollView.scrollOffset);
            set => _scrollView.scrollOffset = value;
        }

        [CLProperty("Controls the scrolling speed when using the scroll wheel.")]
        public float ScrollDecelerationRate
        {
            get => _scrollView.scrollDecelerationRate;
            set => _scrollView.scrollDecelerationRate = value;
        }

        [CLProperty("Controls the sensitivity/speed of mouse wheel scrolling.")]
        public float MouseWheelScrollSize
        {
            get => _scrollView.mouseWheelScrollSize;
            set => _scrollView.mouseWheelScrollSize = value;
        }

        [CLProperty("Enable or disable horizontal scrolling.")]
        public bool HorizontalScrollEnabled
        {
            get => _scrollView.mode == ScrollViewMode.Horizontal || _scrollView.mode == ScrollViewMode.VerticalAndHorizontal;
            set
            {
                if (value)
                {
                    // Enable horizontal scrolling
                    if (_scrollView.mode == ScrollViewMode.Vertical)
                        _scrollView.mode = ScrollViewMode.VerticalAndHorizontal;
                    else if (_scrollView.mode == ScrollViewMode.Horizontal || _scrollView.horizontalScrollerVisibility == ScrollerVisibility.Hidden)
                    {
                        _scrollView.mode = ScrollViewMode.Horizontal;
                    }
                    // Restore scroller visibility
                    _scrollView.horizontalScrollerVisibility = ScrollerVisibility.Auto;
                }
                else
                {
                    // Disable horizontal scrolling  
                    _scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
                    if (_scrollView.mode == ScrollViewMode.VerticalAndHorizontal)
                        _scrollView.mode = ScrollViewMode.Vertical;
                }
            }
        }

        [CLProperty("Enable or disable vertical scrolling.")]
        public bool VerticalScrollEnabled
        {
            get => _scrollView.mode == ScrollViewMode.Vertical || _scrollView.mode == ScrollViewMode.VerticalAndHorizontal;
            set
            {
                if (value)
                {
                    // Enable vertical scrolling
                    if (_scrollView.mode == ScrollViewMode.Horizontal)
                        _scrollView.mode = ScrollViewMode.VerticalAndHorizontal;
                    else if (_scrollView.mode == ScrollViewMode.Vertical || _scrollView.verticalScrollerVisibility == ScrollerVisibility.Hidden)
                    {
                        _scrollView.mode = ScrollViewMode.Vertical;
                    }
                    // Restore scroller visibility
                    _scrollView.verticalScrollerVisibility = ScrollerVisibility.Auto;
                }
                else
                {
                    // Disable vertical scrolling
                    _scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
                    if (_scrollView.mode == ScrollViewMode.VerticalAndHorizontal)
                        _scrollView.mode = ScrollViewMode.Horizontal;
                }
            }
        }

        [CLMethod("The behavior to use when scrolling reaches limits of the content.")]
        public CustomLogicScrollViewBuiltin Elasticity(
            [CLParam("Acceptable values are: `Clamped`, `Elastic`, and `Unrestricted`")]
            string value)
        {
            _scrollView.touchScrollBehavior = value switch
            {
                "Clamped" => ScrollView.TouchScrollBehavior.Clamped,
                "Elastic" => ScrollView.TouchScrollBehavior.Elastic,
                "Unrestricted" => ScrollView.TouchScrollBehavior.Unrestricted,
                _ => throw new Exception("Unknown elasticity value")
            };
            return this;
        }

        [CLMethod("Controls the rate at which scrolling movement slows after a user scrolling action.")]
        public CustomLogicScrollViewBuiltin SetScrollDecelerationRate(
            [CLParam("The deceleration rate (0-1, where 1 is fastest deceleration).")]
            float rate)
        {
            _scrollView.scrollDecelerationRate = Mathf.Clamp01(rate);
            return this;
        }

        [CLMethod("Set the scroll offset.")]
        public CustomLogicScrollViewBuiltin SetScrollOffset(
            [CLParam("The scroll offset vector (x, y).")]
            CustomLogicVector2Builtin offset)
        {
            _scrollView.scrollOffset = offset;
            return this;
        }

        [CLMethod("Scroll to the top of the content.")]
        public void ScrollToTop()
        {
            _scrollView.scrollOffset = new Vector2(_scrollView.scrollOffset.x, 0);
        }

        [CLMethod("Scroll to the bottom of the content.")]
        public void ScrollToBottom()
        {
            _scrollView.ScrollTo(_scrollView.contentContainer);
        }
    }
}
