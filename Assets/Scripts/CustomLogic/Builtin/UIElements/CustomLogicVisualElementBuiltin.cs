using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "VisualElement", Abstract = true, Description = "Base class for all UI elements. Note: Most methods return self to allow method chaining.")]
    partial class CustomLogicVisualElementBuiltin : BuiltinClassInstance
    {
        private readonly VisualElement _visualElement;

        private TextShadow _textShadow = new();

        public CustomLogicVisualElementBuiltin(VisualElement visualElement)
        {
            _visualElement = visualElement;
        }

        [CLProperty("The number of child elements in this visual element.")]
        public int ChildCount => _visualElement.childCount;

        [CLMethod("Add a child element.")]
        public CustomLogicVisualElementBuiltin Add(
            [CLParam("The visual element to add as a child.")]
            CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Add(visualElement._visualElement);
            return this;
        }

        [CLMethod("Remove a child element.")]
        public void Remove(
            [CLParam("The visual element to remove.")]
            CustomLogicVisualElementBuiltin visualElement)
        {
            _visualElement.Remove(visualElement._visualElement);
        }

        [CLMethod("Removes this element from its parent hierarchy.")]
        public void RemoveFromHierarchy()
        {
            _visualElement.RemoveFromHierarchy();
        }

        [CLMethod("Remove all child elements.")]
        public void Clear()
        {
            _visualElement.Clear();
        }

        [CLMethod("Get child element at index.")]
        public CustomLogicVisualElementBuiltin GetElementAt(
            [CLParam("The index of the child element to get.")]
            int index)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.ElementAt(index));
        }

        [CLMethod(Description = "Query child element by name. Returns: The first child element with the matching name.")]
        public CustomLogicVisualElementBuiltin QueryByName(
            [CLParam("The name of the element to find.")]
            string name)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.Q(name));
        }

        [CLMethod(Description = "Query child element by class name. Returns: The first child element with the matching class name.")]
        public CustomLogicVisualElementBuiltin QueryByClassName(
            [CLParam("The class name of the element to find.")]
            string className)
        {
            return new CustomLogicVisualElementBuiltin(_visualElement.Q(className: className));
        }

        #region Events

        [CLMethod(Description = "Register a callback for mouse enter event. Mouse enter event is fired when the mouse pointer enters an element or one of its children")]
        public CustomLogicVisualElementBuiltin RegisterMouseEnterEventCallback(
            [CLParam("The method to call when the mouse enters the element.")]
            UserMethod method)
        {
            _visualElement.RegisterCallback<MouseEnterEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        [CLMethod(Description = "Register a callback for mouse leave event. Mouse leave event is fired when the mouse pointer exits an element and all its children")]
        public CustomLogicVisualElementBuiltin RegisterMouseLeaveEventCallback(
            [CLParam("The method to call when the mouse leaves the element.")]
            UserMethod method)
        {
            _visualElement.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        [CLMethod(Description = "Register a callback for click event")]
        public CustomLogicVisualElementBuiltin RegisterClickEventCallback(
            [CLParam("The method to call when the element is clicked.")]
            UserMethod method)
        {
            _visualElement.RegisterCallback<ClickEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        [CLMethod(Description = "Register a callback for focus in event. Focus in event is fired immediately before an element gains focus")]
        public CustomLogicVisualElementBuiltin RegisterFocusInEventCallback(
            [CLParam("The method to call when the element gains focus.")]
            UserMethod method)
        {
            _visualElement.RegisterCallback<FocusInEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        [CLMethod(Description = "Register a callback for focus out event. Focus out event is fired immediately before an element loses focus")]
        public CustomLogicVisualElementBuiltin RegisterFocusOutEventCallback(
            [CLParam("The method to call when the element loses focus.")]
            UserMethod method)
        {
            _visualElement.RegisterCallback<FocusOutEvent>(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { });
            });
            return this;
        }

        #endregion

        #region Display

        [CLMethod(Description = "Set the opacity of the element")]
        public CustomLogicVisualElementBuiltin Opacity(
            [CLParam("a value between 0 and 100")]
            float value)
        {
            _visualElement.style.opacity = Mathf.Clamp01(value / 100f);
            return this;
        }

        [CLMethod(Description = "Set the element to be active or inactive")]
        public CustomLogicVisualElementBuiltin Active(bool value = true)
        {
            _visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return this;
        }

        [CLMethod(Description = "Set the element to be visible or hidden")]
        public CustomLogicVisualElementBuiltin Visible(bool value = true)
        {
            _visualElement.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
            return this;
        }

        [CLMethod(Description = "Set the transition duration of the element")]
        public CustomLogicVisualElementBuiltin TransitionDuration(float value)
        {
            StyleList<TimeValue> transitionDuration = new List<TimeValue> { new TimeValue(value, TimeUnit.Millisecond) };
            _visualElement.style.transitionDuration = transitionDuration;
            return this;
        }

        #endregion

        #region Position

        [CLMethod(Description = "Set the element to be absolute or relative positioned")]
        public CustomLogicVisualElementBuiltin Absolute(bool value = true)
        {
            _visualElement.style.position = value ? Position.Absolute : Position.Relative;
            return this;
        }

        [CLMethod(Description = "Set the left position of the element")]
        public CustomLogicVisualElementBuiltin Left(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.left = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the top position of the element")]
        public CustomLogicVisualElementBuiltin Top(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.top = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the right position of the element")]
        public CustomLogicVisualElementBuiltin Right(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.right = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the bottom position of the element")]
        public CustomLogicVisualElementBuiltin Bottom(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.bottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Flex

        [CLMethod(Description = "Set the flex shrink value")]
        public CustomLogicVisualElementBuiltin FlexShrink(float value)
        {
            _visualElement.style.flexShrink = value;
            return this;
        }

        [CLMethod(Description = "Set the flex grow value")]
        public CustomLogicVisualElementBuiltin FlexGrow(float value)
        {
            _visualElement.style.flexGrow = value;
            return this;
        }

        [CLMethod(Description = "Set the flex direction")]
        public CustomLogicVisualElementBuiltin FlexDirection(
            [CLParam("Acceptable values are: `Row`, `Column`, `RowReverse`, and `ColumnReverse`")]
            string value)
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

        [CLMethod(Description = "Set the align items property")]
        public CustomLogicVisualElementBuiltin AlignItems(
            [CLParam("Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`")]
            string value)
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

        [CLMethod(Description = "Set the justify content property")]
        public CustomLogicVisualElementBuiltin JustifyContent(
            [CLParam("Acceptable values are: `FlexStart`, `Center`, `FlexEnd`, `SpaceBetween`, `SpaceAround`, and `SpaceEvenly`")]
            string value)
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

        [CLMethod(Description = "Set the align self property")]
        public CustomLogicVisualElementBuiltin AlignSelf(
            [CLParam("Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`")]
            string value)
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

        [CLMethod(Description = "Set the width of the element")]
        public CustomLogicVisualElementBuiltin Width(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.width = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the height of the element")]
        public CustomLogicVisualElementBuiltin Height(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.height = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Margin

        [CLMethod(Description = "Set the margin of the element")]
        public CustomLogicVisualElementBuiltin Margin(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            return MarginLeft(value, percentage)
                .MarginTop(value, percentage)
                .MarginRight(value, percentage)
                .MarginBottom(value, percentage);
        }

        [CLMethod(Description = "Set the left margin of the element")]
        public CustomLogicVisualElementBuiltin MarginLeft(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.marginLeft = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the top margin of the element")]
        public CustomLogicVisualElementBuiltin MarginTop(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.marginTop = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the right margin of the element")]
        public CustomLogicVisualElementBuiltin MarginRight(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.marginRight = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the bottom margin of the element")]
        public CustomLogicVisualElementBuiltin MarginBottom(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.marginBottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Padding

        [CLMethod(Description = "Set the padding of the element")]
        public CustomLogicVisualElementBuiltin Padding(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            return PaddingLeft(value, percentage)
                .PaddingTop(value, percentage)
                .PaddingRight(value, percentage)
                .PaddingBottom(value, percentage);
        }

        [CLMethod(Description = "Set the left padding of the element")]
        public CustomLogicVisualElementBuiltin PaddingLeft(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.paddingLeft = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the top padding of the element")]
        public CustomLogicVisualElementBuiltin PaddingTop(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.paddingTop = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the right padding of the element")]
        public CustomLogicVisualElementBuiltin PaddingRight(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.paddingRight = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the bottom padding of the element")]
        public CustomLogicVisualElementBuiltin PaddingBottom(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.paddingBottom = GetLength(value, percentage);
            return this;
        }

        #endregion

        #region Text

        [CLMethod(Description = "Set the font style of the element")]
        public CustomLogicVisualElementBuiltin FontStyle(
            [CLParam("Acceptable values are: `Normal`, `Bold`, `Italic`, and `BoldAndItalic`")]
            string value)
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

        [CLMethod(Description = "Set the font size of the element")]
        public CustomLogicVisualElementBuiltin FontSize(
            [CLParam("the value")]
            float value,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.fontSize = GetLength(value, percentage);
            return this;
        }

        [CLMethod(Description = "Set the text color of the element")]
        public CustomLogicVisualElementBuiltin Color(CustomLogicColorBuiltin color)
        {
            _visualElement.style.color = color.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the text alignment of the element")]
        public CustomLogicVisualElementBuiltin TextAlign(
            [CLParam("Valid values are: `UpperLeft`, `UpperCenter`, `UpperRight`, `MiddleLeft`, `MiddleCenter`, `MiddleRight`, `LowerLeft`, `LowerCenter`, `LowerRight`")]
            string value)
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

        [CLMethod(Description = "Set whether the text should wrap or not")]
        public CustomLogicVisualElementBuiltin TextWrap(bool value = true)
        {
            _visualElement.style.whiteSpace = value switch
            {
                true => WhiteSpace.Normal,
                false => WhiteSpace.NoWrap
            };
            return this;
        }

        [CLMethod(Description = "Set the text overflow behavior")]
        public CustomLogicVisualElementBuiltin TextOverflow(
            [CLParam("Acceptable values are: `Clip`, `Ellipsis`")]
            string value)
        {
            _visualElement.style.textOverflow = value switch
            {
                "Clip" => UnityEngine.UIElements.TextOverflow.Clip,
                "Ellipsis" => UnityEngine.UIElements.TextOverflow.Ellipsis,
                _ => throw new System.Exception("Unknown text overflow value")
            };
            return this;
        }

        [CLMethod(Description = "Set the text outline width")]
        public CustomLogicVisualElementBuiltin TextOutlineWidth(float value)
        {
            _visualElement.style.unityTextOutlineWidth = value;
            return this;
        }

        [CLMethod(Description = "Set the text outline color")]
        public CustomLogicVisualElementBuiltin TextOutlineColor(CustomLogicColorBuiltin value)
        {
            _visualElement.style.unityTextOutlineColor = value.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the text shadow color")]
        public CustomLogicVisualElementBuiltin TextShadowColor(CustomLogicColorBuiltin value)
        {
            _textShadow.color = value.Value.ToColor();
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        [CLMethod(Description = "Set the text shadow offset")]
        public CustomLogicVisualElementBuiltin TextShadowOffset(
            [CLParam("horizontal offset")]
            float horizontal,
            [CLParam("vertical offset")]
            float vertical)
        {
            _textShadow.offset = new Vector2(horizontal, vertical);
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        [CLMethod(Description = "Set the text shadow horizontal offset")]
        public CustomLogicVisualElementBuiltin TextShadowHorizontalOffset(float value)
        {
            _textShadow.offset.x = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        [CLMethod(Description = "Set the text shadow vertical offset")]
        public CustomLogicVisualElementBuiltin TextShadowVerticalOffset(float value)
        {
            _textShadow.offset.y = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        [CLMethod(Description = "Set the text shadow blur radius")]
        public CustomLogicVisualElementBuiltin TextShadowBlurRadius(float value)
        {
            _textShadow.blurRadius = value;
            _visualElement.style.textShadow = _textShadow;
            return this;
        }

        [CLMethod(Description = "Set the text letter spacing")]
        public CustomLogicVisualElementBuiltin TextLetterSpacing(float value)
        {
            _visualElement.style.letterSpacing = value;
            return this;
        }

        [CLMethod(Description = "Set the text word spacing")]
        public CustomLogicVisualElementBuiltin TextWordSpacing(float value)
        {
            _visualElement.style.wordSpacing = value;
            return this;
        }

        [CLMethod(Description = "Set the text paragraph spacing")]
        public CustomLogicVisualElementBuiltin TextParagraphSpacing(float value)
        {
            _visualElement.style.unityParagraphSpacing = value;
            return this;
        }

        #endregion

        #region Background

        [CLMethod(Description = "Set the background color of the element")]
        public CustomLogicVisualElementBuiltin BackgroundColor(CustomLogicColorBuiltin color)
        {
            _visualElement.style.backgroundColor = color.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the background image of the element")]
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

        [CLMethod(Description = "Set the border color of the element")]
        public CustomLogicVisualElementBuiltin BorderColor(CustomLogicColorBuiltin color)
        {
            return BorderColorLeft(color)
                .BorderColorTop(color)
                .BorderColorRight(color)
                .BorderColorBottom(color);
        }

        [CLMethod(Description = "Set the left border color of the element")]
        public CustomLogicVisualElementBuiltin BorderColorLeft(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderLeftColor = color.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the top border color of the element")]
        public CustomLogicVisualElementBuiltin BorderColorTop(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderTopColor = color.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the right border color of the element")]
        public CustomLogicVisualElementBuiltin BorderColorRight(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderRightColor = color.Value.ToColor();
            return this;
        }

        [CLMethod(Description = "Set the bottom border color of the element")]
        public CustomLogicVisualElementBuiltin BorderColorBottom(CustomLogicColorBuiltin color)
        {
            _visualElement.style.borderBottomColor = color.Value.ToColor();
            return this;
        }

        #endregion

        #region Border Width

        [CLMethod(Description = "Set the border width of the element")]
        public CustomLogicVisualElementBuiltin BorderWidth(float value)
        {
            return BorderWidthLeft(value)
                .BorderWidthTop(value)
                .BorderWidthRight(value)
                .BorderWidthBottom(value);
        }

        [CLMethod(Description = "Set the left border width of the element")]
        public CustomLogicVisualElementBuiltin BorderWidthLeft(float value)
        {
            _visualElement.style.borderLeftWidth = value;
            return this;
        }

        [CLMethod(Description = "Set the top border width of the element")]
        public CustomLogicVisualElementBuiltin BorderWidthTop(float value)
        {
            _visualElement.style.borderTopWidth = value;
            return this;
        }

        [CLMethod(Description = "Set the right border width of the element")]
        public CustomLogicVisualElementBuiltin BorderWidthRight(float value)
        {
            _visualElement.style.borderRightWidth = value;
            return this;
        }

        [CLMethod(Description = "Set the bottom border width of the element")]
        public CustomLogicVisualElementBuiltin BorderWidthBottom(float value)
        {
            _visualElement.style.borderBottomWidth = value;
            return this;
        }

        #endregion

        #region Border Radius

        [CLMethod(Description = "Set the border radius of the element")]
        public CustomLogicVisualElementBuiltin BorderRadius(float value)
        {
            return BorderRadiusTopLeft(value)
                .BorderRadiusTopRight(value)
                .BorderRadiusBottomLeft(value)
                .BorderRadiusBottomRight(value);
        }

        [CLMethod(Description = "Set the top-left border radius of the element")]
        public CustomLogicVisualElementBuiltin BorderRadiusTopLeft(float value)
        {
            _visualElement.style.borderTopLeftRadius = value;
            return this;
        }

        [CLMethod(Description = "Set the top-right border radius of the element")]
        public CustomLogicVisualElementBuiltin BorderRadiusTopRight(float value)
        {
            _visualElement.style.borderTopRightRadius = value;
            return this;
        }

        [CLMethod(Description = "Set the bottom-left border radius of the element")]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomLeft(float value)
        {
            _visualElement.style.borderBottomLeftRadius = value;
            return this;
        }

        [CLMethod(Description = "Set the bottom-right border radius of the element")]
        public CustomLogicVisualElementBuiltin BorderRadiusBottomRight(float value)
        {
            _visualElement.style.borderBottomRightRadius = value;
            return this;
        }

        #endregion

        #region Overflow

        [CLMethod(Description = "Set the overflow behavior on the X axis")]
        public CustomLogicVisualElementBuiltin OverflowX(
            [CLParam("Acceptable values are: `Visible` and `Hidden`")]
            string value)
        {
            _visualElement.style.overflow = value switch
            {
                "Visible" => Overflow.Visible,
                "Hidden" => Overflow.Hidden,
                _ => throw new System.Exception("Unknown overflow value")
            };
            return this;
        }

        [CLMethod(Description = "Set the overflow behavior on the Y axis")]
        public CustomLogicVisualElementBuiltin OverflowY(
            [CLParam("Acceptable values are: `Visible` and `Hidden`")]
            string value)
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

        [CLMethod(Description = "Set the origin of the element")]
        public CustomLogicVisualElementBuiltin SetTransformOrigin(
            [CLParam("x value")]
            float x,
            [CLParam("y value")]
            float y,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.transformOrigin = new TransformOrigin(
                GetLength(x, percentage),
                GetLength(y, percentage),
                0);
            return this;
        }

        [CLMethod(Description = "Translate the element")]
        public CustomLogicVisualElementBuiltin TransformTranslate(
            [CLParam("x value")]
            float x,
            [CLParam("y value")]
            float y,
            [CLParam("if true, the `value` will be treated as percentage value")]
            bool percentage = false)
        {
            _visualElement.style.translate = new Translate(
                GetLength(x, percentage),
                GetLength(y, percentage),
                0);
            return this;
        }

        [CLMethod(Description = "Scale the element")]
        public CustomLogicVisualElementBuiltin TransformScale(
            [CLParam("x value")]
            float x,
            [CLParam("y value")]
            float y)
        {
            _visualElement.style.scale = new Scale(new Vector2(x, y));
            return this;
        }

        [CLMethod(Description = "Rotate the element")]
        public CustomLogicVisualElementBuiltin TransformRotate(
            [CLParam("the angle of rotation")]
            float angle,
            [CLParam("the unit of the angle. Valid values are: `Degree`, `Gradian`, `Radian`, and `Turn`")]
            string angleUnit = "Degree")
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
