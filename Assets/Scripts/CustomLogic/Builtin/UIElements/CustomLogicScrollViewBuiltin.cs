using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// ScrollView UI element that provides scrollable content. Note: Most methods return self to allow method chaining.
    /// </summary>
    [CLType(Name = "ScrollView")]
    partial class CustomLogicScrollViewBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly ScrollView _scrollView;

        public CustomLogicScrollViewBuiltin(ScrollView scrollView) : base(scrollView)
        {
            _scrollView = scrollView;
        }

        /// <summary>
        /// The current scroll offset.
        /// </summary>
        [CLProperty]
        public CustomLogicVector2Builtin ScrollOffset
        {
            get => new CustomLogicVector2Builtin(_scrollView.scrollOffset);
            set => _scrollView.scrollOffset = value;
        }

        /// <summary>
        /// Controls the scrolling speed when using the scroll wheel.
        /// </summary>
        [CLProperty]
        public float ScrollDecelerationRate
        {
            get => _scrollView.scrollDecelerationRate;
            set => _scrollView.scrollDecelerationRate = value;
        }

        /// <summary>
        /// Controls the sensitivity/speed of mouse wheel scrolling.
        /// </summary>
        [CLProperty]
        public float MouseWheelScrollSize
        {
            get => _scrollView.mouseWheelScrollSize;
            set => _scrollView.mouseWheelScrollSize = value;
        }

        /// <summary>
        /// Enable or disable horizontal scrolling.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Enable or disable vertical scrolling.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The behavior to use when scrolling reaches limits of the content.
        /// </summary>
        /// <param name="value">Acceptable values are: `Clamped`, `Elastic`, and `Unrestricted`.</param>
        [CLMethod]
        public CustomLogicScrollViewBuiltin Elasticity([CLParam(Enum = new Type[] { typeof(CustomLogicScrollElasticityEnum) })] int value)
        {
            if (!Enum.IsDefined(typeof(ScrollView.TouchScrollBehavior), value))
                throw new ArgumentException($"Unknown elasticity value: {value}");
            _scrollView.touchScrollBehavior = (ScrollView.TouchScrollBehavior)value;
            return this;
        }

        /// <summary>
        /// Controls the rate at which scrolling movement slows after a user scrolling action.
        /// </summary>
        /// <param name="rate">The deceleration rate (0-1, where 1 is fastest deceleration).</param>
        [CLMethod]
        public CustomLogicScrollViewBuiltin SetScrollDecelerationRate(float rate)
        {
            _scrollView.scrollDecelerationRate = Mathf.Clamp01(rate);
            return this;
        }

        /// <summary>
        /// Set the scroll offset.
        /// </summary>
        /// <param name="offset">The scroll offset vector (x, y).</param>
        [CLMethod]
        public CustomLogicScrollViewBuiltin SetScrollOffset(CustomLogicVector2Builtin offset)
        {
            _scrollView.scrollOffset = offset;
            return this;
        }

        /// <summary>
        /// Scroll to the top of the content.
        /// </summary>
        [CLMethod]
        public void ScrollToTop()
        {
            _scrollView.scrollOffset = new Vector2(_scrollView.scrollOffset.x, 0);
        }

        /// <summary>
        /// Scroll to the bottom of the content.
        /// </summary>
        [CLMethod]
        public void ScrollToBottom()
        {
            _scrollView.ScrollTo(_scrollView.contentContainer);
        }
    }
}
