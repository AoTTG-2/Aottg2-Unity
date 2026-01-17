using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Base class for all UI elements. Note: Most methods return self to allow method chaining.
    /// </summary>
    [CLType(Name = "VisualElement", Abstract = true)]
    partial class CustomLogicVisualElementBuiltin : BuiltinClassInstance
    {
        private readonly VisualElement _visualElement;

        private TextShadow _textShadow = new();
        private readonly Dictionary<string,EventCallback<GeometryChangedEvent>> _onResize = new();

        public CustomLogicVisualElementBuiltin(VisualElement visualElement)
        {
            _visualElement = visualElement;
        }

        /// <summary>
        /// The number of child elements in this visual element.
        /// </summary>
        [CLProperty]
        public int ChildCount => _visualElement.childCount;

        /// <summary>
        /// Add a child element.
        /// </summary>
        /// <param name="visualElement">The visual element to add as a child.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Add(CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Add(visualElement._visualElement);
            return this;
        }

        /// <summary>
        /// Remove a child element.
        /// </summary>
        /// <param name="visualElement">The visual element to remove.</param>
        [CLMethod]
        public void Remove(CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Remove(visualElement._visualElement);
        }

        /// <summary>
        /// Removes this element from its parent hierarchy.
        /// </summary>
        [CLMethod]
        public void RemoveFromHierarchy()
        {
            _visualElement.RemoveFromHierarchy();
        }

        /// <summary>
        /// Remove all child elements.
        /// </summary>
        [CLMethod]
        public void Clear()
        {
            _visualElement.Clear();
        }

        /// <summary>
        /// Get child element at index.
        /// </summary>
        /// <param name="index">The index of the child element to get.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin GetElementAt(int index)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.ElementAt(index));
        }

        /// <summary>
        /// Query child element by name.
        /// </summary>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The first child element with the matching name.</returns>
        [CLMethod]
        public CustomLogicVisualElementBuiltin QueryByName(string name)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.Q(name));
        }

        /// <summary>
        /// Query child element by class name.
        /// </summary>
        /// <param name="className">The class name of the element to find.</param>
        /// <returns>The first child element with the matching class name.</returns>
        [CLMethod]
        public CustomLogicVisualElementBuiltin QueryByClassName(string className)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.Q(className: className));
        }

        #region Events

        /// <summary>
        /// Register a callback for mouse enter event. Mouse enter event is fired when the mouse pointer enters an element or one of its children.
        /// </summary>
        /// <param name="method">The method to call when the mouse enters the element.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin RegisterMouseEnterEventCallback(UserMethod method)
        {
            _visualElement.RegisterCallback<MouseEnterEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        /// <summary>
        /// Register a callback for mouse leave event. Mouse leave event is fired when the mouse pointer exits an element and all its children.
        /// </summary>
        /// <param name="method">The method to call when the mouse leaves the element.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin RegisterMouseLeaveEventCallback(UserMethod method)
        {
            _visualElement.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        /// <summary>
        /// Register a callback for click event.
        /// </summary>
        /// <param name="method">The method to call when the element is clicked.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin RegisterClickEventCallback(UserMethod method)
        {
            _visualElement.RegisterCallback<ClickEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        /// <summary>
        /// Register a callback for focus in event. Focus in event is fired immediately before an element gains focus.
        /// </summary>
        /// <param name="method">The method to call when the element gains focus.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin RegisterFocusInEventCallback(UserMethod method)
        {
            _visualElement.RegisterCallback<FocusInEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        /// <summary>
        /// Register a callback for focus out event. Focus out event is fired immediately before an element loses focus.
        /// </summary>
        /// <param name="method">The method to call when the element loses focus.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin RegisterFocusOutEventCallback(UserMethod method)
        {
            _visualElement.RegisterCallback<FocusOutEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        #endregion

        #region Display

        /// <summary>
        /// Set the opacity of the element.
        /// </summary>
        /// <param name="value">A value between 0 and 100.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Opacity(float value)
        {
            _visualElement.style.opacity = Mathf.Clamp01(value / 100f);
            return this;
        }

        /// <summary>
        /// Set the element to be active or inactive.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Active(bool value = true)
        {
            _visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return this;
        }

        /// <summary>
        /// Set the element to be visible or hidden.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Visible(bool value = true)
        {
            _visualElement.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
            return this;
        }

        /// <summary>
        /// Set the transition duration of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TransitionDuration(float value)
        {
            StyleList<TimeValue> transitionDuration = new List<TimeValue> { new TimeValue(value, TimeUnit.Millisecond) };
            _visualElement.style.transitionDuration = transitionDuration;
            return this;
        }

        #endregion

        #region Position

        /// <summary>
        /// Set the element to be absolute or relative positioned.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Absolute(bool value = true)
        {
            _visualElement.style.position = value ? Position.Absolute : Position.Relative;
            return this;
        }

        /// <summary>
        /// Set the left position of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Left(float value, bool percentage = false)
        {
            _visualElement.style.left = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the top position of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Top(float value, bool percentage = false)
        {
            _visualElement.style.top = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the right position of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Right(float value, bool percentage = false)
        {
            _visualElement.style.right = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the bottom position of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Bottom(float value, bool percentage = false)
        {
            _visualElement.style.bottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Flex

        /// <summary>
        /// Set the flex shrink value.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexShrink(float value)
        {
            _visualElement.style.flexShrink = value;
            return this;
        }

        /// <summary>
        /// Set the flex grow value.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexGrow(float value)
        {
            _visualElement.style.flexGrow = value;
            return this;
        }

        // TODO: Enum
        /// <summary>
        /// Set the flex direction.
        /// </summary>
        /// <param name="value">Acceptable values are: `Row`, `Column`, `RowReverse`, and `ColumnReverse`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin FlexDirection(string value)
        {
            _visualElement.style.flexDirection = value switch
            {
                "Row" => UnityEngine.UIElements.FlexDirection.Row,
                "Column" => UnityEngine.UIElements.FlexDirection.Column,
                "RowReverse" => UnityEngine.UIElements.FlexDirection.RowReverse,
                "ColumnReverse" => UnityEngine.UIElements.FlexDirection.ColumnReverse,
                _ => throw new System.Exception("Unknown flex direction value")
            };
            return this;
        }

        #endregion

        #region Align

        // TODO: Enum
        /// <summary>
        /// Set the align items property.
        /// </summary>
        /// <param name="value">Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin AlignItems(string value)
        {
            _visualElement.style.alignItems = value switch
            {
                "Auto" => Align.Auto,
                "FlexStart" => Align.FlexStart,
                "Center" => Align.Center,
                "FlexEnd" => Align.FlexEnd,
                "Stretch" => Align.Stretch,
                _ => throw new System.Exception("Unknown align value")
            };
            return this;
        }

        // TODO: Enum
        /// <summary>
        /// Set the justify content property.
        /// </summary>
        /// <param name="value">Acceptable values are: `FlexStart`, `Center`, `FlexEnd`, `SpaceBetween`, `SpaceAround`, and `SpaceEvenly`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin JustifyContent(string value)
        {
            _visualElement.style.justifyContent = value switch
            {
                "FlexStart" => Justify.FlexStart,
                "Center" => Justify.Center,
                "FlexEnd" => Justify.FlexEnd,
                "SpaceBetween" => Justify.SpaceBetween,
                "SpaceAround" => Justify.SpaceAround,
                "SpaceEvenly" => Justify.SpaceEvenly,
                _ => throw new System.Exception("Unknown justify value")
            };
            return this;
        }

        // TODO: Enum
        /// <summary>
        /// Set the align self property.
        /// </summary>
        /// <param name="value">Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin AlignSelf(string value)
        {
            _visualElement.style.alignSelf = value switch
            {
                "Auto" => Align.Auto,
                "FlexStart" => Align.FlexStart,
                "Center" => Align.Center,
                "FlexEnd" => Align.FlexEnd,
                "Stretch" => Align.Stretch,
                _ => throw new System.Exception("Unknown align value")
            };
            return this;
        }

        #endregion

        #region Size

        /// <summary>
        /// Set the width of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Width(float value, bool percentage = false)
        {
            _visualElement.style.width = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the height of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Height(float value, bool percentage = false)
        {
            _visualElement.style.height = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the height of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mode">Determines the direction of the aspect ratio.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin AspectRatio(float value, string mode = "Width")
        {
            if (_onResize.TryGetValue("AspectRatio", out var evt))
                _visualElement.UnregisterCallback(evt);

            if (value == 0)
                return this;

            if (mode != "Height" && mode != "Width") // need to convert to enum
               throw new System.Exception("Unknown aspect ratio mode");

            _visualElement.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                if (mode == "Height")
                {
                    float width = evt.newRect.width;
                    _visualElement.style.height = width / value;
                }
                else if (mode == "Width")
                {
                    float height = evt.newRect.height;
                    _visualElement.style.width = height / value;
                }
            });
            return this;
        }

        #endregion

        #region Margin

        /// <summary>
        /// Set the margin of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Margin(float value, bool percentage = false)
        {
            return MarginLeft(value, percentage)
                .MarginTop(value, percentage)
                .MarginRight(value, percentage)
                .MarginBottom(value, percentage);
        }

        /// <summary>
        /// Set the left margin of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginLeft(float value, bool percentage = false)
        {
            _visualElement.style.marginLeft = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the top margin of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginTop(float value, bool percentage = false)
        {
            _visualElement.style.marginTop = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the right margin of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginRight(float value, bool percentage = false)
        {
            _visualElement.style.marginRight = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the bottom margin of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin MarginBottom(float value, bool percentage = false)
        {
            _visualElement.style.marginBottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Padding

        /// <summary>
        /// Set the padding of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Padding(float value, bool percentage = false)
        {
            return PaddingLeft(value, percentage)
                .PaddingTop(value, percentage)
                .PaddingRight(value, percentage)
                .PaddingBottom(value, percentage);
        }

        /// <summary>
        /// Set the left padding of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingLeft(float value, bool percentage = false)
        {
            _visualElement.style.paddingLeft = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the top padding of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingTop(float value, bool percentage = false)
        {
            _visualElement.style.paddingTop = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the right padding of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingRight(float value, bool percentage = false)
        {
            _visualElement.style.paddingRight = GetLength(value, percentage);
            return this;
        }

        /// <summary>
        /// Set the bottom padding of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin PaddingBottom(float value, bool percentage = false)
        {
            _visualElement.style.paddingBottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Text

        /// <summary>
        /// Set the font style of the element.
        /// </summary>
        /// <param name="value">Acceptable values are: `Normal`, `Bold`, `Italic`, and `BoldAndItalic`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin FontStyle(string value)
        {
            _visualElement.style.unityFontStyleAndWeight = value switch
            {
                "Normal" => UnityEngine.FontStyle.Normal,
                "Bold" => UnityEngine.FontStyle.Bold,
                "Italic" => UnityEngine.FontStyle.Italic,
                "BoldAndItalic" => UnityEngine.FontStyle.BoldAndItalic,
                _ => throw new System.Exception("Unknown font style value")
            };
            return this;
        }

        /// <summary>
        /// Set the font size of the element.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        /// <param name="scaleMode">Determines the container dimension used for percentage calculations.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin FontSize(float value, bool percentage = false, string scaleMode = "Height")
        {
            if (_onResize.TryGetValue("FontScale", out var evt))
                _visualElement.UnregisterCallback(evt);

            if (scaleMode != "Height" && scaleMode != "Width") // need to convert to enum
               throw new System.Exception("Unknown font scale mode");

            if (percentage)
            {
                _visualElement.RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    float percent = value / 100f;
                    float scale = scaleMode switch
                    {
                        "Height" => evt.newRect.height,
                        "Width" => evt.newRect.width,
                        _ => 1
                    };

                    _visualElement.style.fontSize = scale * percent;
                });
            }
            else
            {
                _visualElement.style.fontSize = new Length(value);
            }
            return this;
        }

        /// <summary>
        /// Set the text color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin Color(CustomLogicColorBuiltin color)
        {
            _visualElement.style.color = color.Value.ToColor();
            return this;
        }

        // TODO: Enum
        /// <summary>
        /// Set the text alignment of the element.
        /// </summary>
        /// <param name="value">Valid values are: `UpperLeft`, `UpperCenter`, `UpperRight`, `MiddleLeft`, `MiddleCenter`, `MiddleRight`, `LowerLeft`, `LowerCenter`, `LowerRight`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextAlign(string value)
        {
            _visualElement.style.unityTextAlign = value switch
            {
                "UpperLeft" => TextAnchor.UpperLeft,
                "UpperCenter" => TextAnchor.UpperCenter,
                "UpperRight" => TextAnchor.UpperRight,
                "MiddleLeft" => TextAnchor.MiddleLeft,
                "MiddleCenter" => TextAnchor.MiddleCenter,
                "MiddleRight" => TextAnchor.MiddleRight,
                "LowerLeft" => TextAnchor.LowerLeft,
                "LowerCenter" => TextAnchor.LowerCenter,
                "LowerRight" => TextAnchor.LowerRight,
                _ => throw new System.Exception("Unknown text align value")
            };
            return this;
        }

        /// <summary>
        /// Set whether the text should wrap or not.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextWrap(bool value = true)
        {
            _visualElement.style.whiteSpace = value switch
            {
                true => WhiteSpace.Normal,
                false => WhiteSpace.NoWrap
            };
            return this;
        }

        /// <summary>
        /// Set the text overflow behavior.
        /// </summary>
        /// <param name="value">Acceptable values are: `Clip`, `Ellipsis`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextOverflow(string value)
        {
            _visualElement.style.textOverflow = value switch
            {
                "Clip" => UnityEngine.UIElements.TextOverflow.Clip,
                "Ellipsis" => UnityEngine.UIElements.TextOverflow.Ellipsis,
                _ => throw new System.Exception("Unknown text overflow value")
            };
            return this;
        }

        /// <summary>
        /// Set the text outline width.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextOutlineWidth(float value)
        {
            _visualElement.style.unityTextOutlineWidth = value;
            return this;
        }

        /// <summary>
        /// Set the text outline color.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextOutlineColor(CustomLogicColorBuiltin value)
        {
            _visualElement.style.unityTextOutlineColor = value.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Set the text shadow color.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextShadowColor(CustomLogicColorBuiltin value)
        {
            _textShadow.color = value.Value.ToColor();
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        /// <summary>
        /// Set the text shadow offset.
        /// </summary>
        /// <param name="horizontal">Horizontal offset.</param>
        /// <param name="vertical">Vertical offset.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextShadowOffset(float horizontal, float vertical)
        {
            _textShadow.offset = new Vector2(horizontal, vertical);
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        /// <summary>
        /// Set the text shadow horizontal offset.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextShadowHorizontalOffset(float value)
        {
            _textShadow.offset.x = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        /// <summary>
        /// Set the text shadow vertical offset.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextShadowVerticalOffset(float value)
        {
            _textShadow.offset.y = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        /// <summary>
        /// Set the text shadow blur radius.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextShadowBlurRadius(float value)
        {
            _textShadow.blurRadius = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        /// <summary>
        /// Set the text letter spacing.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextLetterSpacing(float value)
        {
            _visualElement.style.letterSpacing = value;
            return this;
        }

        /// <summary>
        /// Set the text word spacing.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextWordSpacing(float value)
        {
            _visualElement.style.wordSpacing = value;
            return this;
        }

        /// <summary>
        /// Set the text paragraph spacing.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TextParagraphSpacing(float value)
        {
            _visualElement.style.unityParagraphSpacing = value;
            return this;
        }

        #endregion

        #region Background

        /// <summary>
        /// Set the background color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BackgroundColor(CustomLogicColorBuiltin color)
        {
            _visualElement.style.backgroundColor = color.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Set the background image of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin SetBackgroundImage(CustomLogicImageBuiltin image)
        {
            // Apply the image to this element
            if (image != null)
            {
                var texture = image.GetTexture();
                if (texture != null)
                {
                    _visualElement.style.backgroundImage = new StyleBackground(texture);
                }
            }
            return this;
        }

        #endregion

        #region Border Color

        /// <summary>
        /// Set the border color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColor(CustomLogicColorBuiltin color)
        {
            return BorderColorLeft(color)
                .BorderColorTop(color)
                .BorderColorRight(color)
                .BorderColorBottom(color);
        }

        /// <summary>
        /// Set the left border color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorLeft(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderLeftColor = color.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Set the top border color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorTop(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderTopColor = color.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Set the right border color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorRight(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderRightColor = color.Value.ToColor();
            return this;
        }

        /// <summary>
        /// Set the bottom border color of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderColorBottom(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderBottomColor = color.Value.ToColor();
            return this;
        }

        #endregion

        #region Border Width

        /// <summary>
        /// Set the border width of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidth(float value)
        {
            return BorderWidthLeft(value)
                .BorderWidthTop(value)
                .BorderWidthRight(value)
                .BorderWidthBottom(value);
        }

        /// <summary>
        /// Set the left border width of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthLeft(float value)
        {
            _visualElement.style.borderLeftWidth = value;
            return this;
        }

        /// <summary>
        /// Set the top border width of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthTop(float value)
        {
            _visualElement.style.borderTopWidth = value;
            return this;
        }

        /// <summary>
        /// Set the right border width of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthRight(float value)
        {
            _visualElement.style.borderRightWidth = value;
            return this;
        }

        /// <summary>
        /// Set the bottom border width of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderWidthBottom(float value)
        {
            _visualElement.style.borderBottomWidth = value;
            return this;
        }

        #endregion

        #region Border Radius

        /// <summary>
        /// Set the border radius of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadius(float value)
        {
            return BorderRadiusTopLeft(value)
                .BorderRadiusTopRight(value)
                .BorderRadiusBottomLeft(value)
                .BorderRadiusBottomRight(value);
        }

        /// <summary>
        /// Set the top-left border radius of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusTopLeft(float value)
        {
            _visualElement.style.borderTopLeftRadius = value;
            return this;
        }

        /// <summary>
        /// Set the top-right border radius of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusTopRight(float value)
        {
            _visualElement.style.borderTopRightRadius = value;
            return this;
        }

        /// <summary>
        /// Set the bottom-left border radius of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomLeft(float value)
        {
            _visualElement.style.borderBottomLeftRadius = value;
            return this;
        }

        /// <summary>
        /// Set the bottom-right border radius of the element.
        /// </summary>
        [CLMethod]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomRight(float value)
        {
            _visualElement.style.borderBottomRightRadius = value;
            return this;
        }

        #endregion

        #region Overflow

        /// <summary>
        /// Set the overflow behavior on the X axis.
        /// </summary>
        /// <param name="value">Acceptable values are: `Visible` and `Hidden`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin OverflowX(string value)
        {
            _visualElement.style.overflow = value switch
            {
                "Visible" => Overflow.Visible,
                "Hidden" => Overflow.Hidden,
                _ => throw new System.Exception("Unknown overflow value")
            };
            return this;
        }

        /// <summary>
        /// Set the overflow behavior on the Y axis.
        /// </summary>
        /// <param name="value">Acceptable values are: `Visible` and `Hidden`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin OverflowY(string value)
        {
            _visualElement.style.overflow = value switch
            {
                "Visible" => Overflow.Visible,
                "Hidden" => Overflow.Hidden,
                _ => throw new System.Exception("Unknown overflow value")
            };
            return this;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Set the origin of the element.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin SetTransformOrigin(float x, float y, bool percentage = false)
        {
            _visualElement.style.transformOrigin = new TransformOrigin(
                GetLength(x, percentage),
                GetLength(y, percentage),
                0);
            return this;
        }

        /// <summary>
        /// Translate the element.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="percentage">If true, the `value` will be treated as percentage value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TransformTranslate(float x, float y, bool percentage = false)
        {
            _visualElement.style.translate = new Translate(
                GetLength(x, percentage),
                GetLength(y, percentage),
                0);
            return this;
        }

        /// <summary>
        /// Scale the element.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TransformScale(float x, float y)
        {
            _visualElement.style.scale = new Scale(new Vector2(x, y));
            return this;
        }

        // TODO: Enum
        /// <summary>
        /// Rotate the element.
        /// </summary>
        /// <param name="angle">The angle of rotation.</param>
        /// <param name="angleUnit">The unit of the angle. Valid values are: `Degree`, `Gradian`, `Radian`, and `Turn`.</param>
        [CLMethod]
        public CustomLogicVisualElementBuiltin TransformRotate(float angle, string angleUnit = "Degree")
        {
            AngleUnit unit = angleUnit switch
            {
                "Degree" => AngleUnit.Degree,
                "Gradian" => AngleUnit.Gradian,
                "Radian" => AngleUnit.Radian,
                "Turn" => AngleUnit.Turn,
                _ => throw new Exception("Unknown angle unit")
            };
            _visualElement.style.rotate = new Rotate(new Angle(angle, unit));
            return this;
        }

        #endregion

        private static Length GetLength(float value, bool percentage)
        {
            return new Length(value, percentage ? LengthUnit.Percent : LengthUnit.Pixel);
        }
    }
}
