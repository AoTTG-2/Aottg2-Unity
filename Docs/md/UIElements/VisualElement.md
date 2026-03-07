# VisualElement
Inherits from [Object](../objects/Object.md)

Base class for all UI elements. Note: Most methods return self to allow method chaining.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ChildCount|int|True|The number of child elements in this visual element.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Add(visualElement: <a data-footnote-ref href="#user-content-fn-113">VisualElement</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Add a child element.
> 
> **Parameters**:
> - `visualElement`: The visual element to add as a child.
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(visualElement: <a data-footnote-ref href="#user-content-fn-113">VisualElement</a>)</code></pre>
> Remove a child element.
> 
> **Parameters**:
> - `visualElement`: The visual element to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveFromHierarchy()</code></pre>
> Removes this element from its parent hierarchy.
> 
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Remove all child elements.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetElementAt(index: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Get child element at index.
> 
> **Parameters**:
> - `index`: The index of the child element to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function QueryByName(name: string) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Query child element by name.
> 
> **Parameters**:
> - `name`: The name of the element to find.
> 
> **Returns**: The first child element with the matching name.
<pre class="language-typescript"><code class="lang-typescript">function QueryByClassName(className: string) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Query child element by class name.
> 
> **Parameters**:
> - `className`: The class name of the element to find.
> 
> **Returns**: The first child element with the matching class name.
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseEnterEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Register a callback for mouse enter event. Mouse enter event is fired when the mouse pointer enters an element or one of its children.
> 
> **Parameters**:
> - `method`: The method to call when the mouse enters the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseLeaveEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Register a callback for mouse leave event. Mouse leave event is fired when the mouse pointer exits an element and all its children.
> 
> **Parameters**:
> - `method`: The method to call when the mouse leaves the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterClickEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Register a callback for click event.
> 
> **Parameters**:
> - `method`: The method to call when the element is clicked.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusInEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Register a callback for focus in event. Focus in event is fired immediately before an element gains focus.
> 
> **Parameters**:
> - `method`: The method to call when the element gains focus.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusOutEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Register a callback for focus out event. Focus out event is fired immediately before an element loses focus.
> 
> **Parameters**:
> - `method`: The method to call when the element loses focus.
> 
<pre class="language-typescript"><code class="lang-typescript">function Opacity(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the opacity of the element.
> 
> **Parameters**:
> - `value`: A value between 0 and 100.
> 
<pre class="language-typescript"><code class="lang-typescript">function Active(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the element to be active or inactive.
> 
<pre class="language-typescript"><code class="lang-typescript">function Visible(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the element to be visible or hidden.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransitionDuration(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the transition duration of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function Absolute(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the element to be absolute or relative positioned.
> 
<pre class="language-typescript"><code class="lang-typescript">function Left(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the left position of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Top(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top position of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Right(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the right position of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Bottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom position of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexShrink(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the flex shrink value.
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexGrow(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the flex grow value.
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexDirection(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the flex direction.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Row`, `Column`, `RowReverse`, and `ColumnReverse`. Refer to [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignItems(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the align items property.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`. Refer to [AlignEnum](../Enums/AlignEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function JustifyContent(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the justify content property.
> 
> **Parameters**:
> - `value`: Acceptable values are: `FlexStart`, `Center`, `FlexEnd`, `SpaceBetween`, `SpaceAround`, and `SpaceEvenly`. Refer to [JustifyEnum](../Enums/JustifyEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignSelf(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the align self property.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`. Refer to [AlignEnum](../Enums/AlignEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function Width(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the width of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Height(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the height of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function AspectRatio(value: float, mode: int = 0) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the height of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `mode`: Determines the direction of the aspect ratio. Acceptable values are: `Width` and `Height` Refer to [AspectRatioEnum](../Enums/AspectRatioEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function Margin(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the margin of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the left margin of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top margin of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the right margin of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom margin of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Padding(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the padding of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the left padding of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top padding of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the right padding of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom padding of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function FontStyle(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the font style of the element.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Normal`, `Bold`, `Italic`, and `BoldAndItalic`. Refer to [FontStyleEnum](../Enums/FontStyleEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function FontSize(value: float, percentage: bool = False, scaleMode: int = 0) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the font size of the element.
> 
> **Parameters**:
> - `value`: The value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> - `scaleMode`: Determines the container dimension used for percentage calculations. Acceptable values are: `Width`, `Height`. Refer to [FontScaleModeEnum](../Enums/FontScaleModeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function Color(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextAlign(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text alignment of the element.
> 
> **Parameters**:
> - `value`: Valid values are: `UpperLeft`, `UpperCenter`, `UpperRight`, `MiddleLeft`, `MiddleCenter`, `MiddleRight`, `LowerLeft`, `LowerCenter`, `LowerRight`. Refer to [TextAlignEnum](../Enums/TextAlignEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function TextWrap(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set whether the text should wrap or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOverflow(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text overflow behavior.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Clip`, `Ellipsis`. Refer to [TextOverflowEnum](../Enums/TextOverflowEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOutlineWidth(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text outline width.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOutlineColor(value: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text outline color.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowColor(value: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text shadow color.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowOffset(horizontal: float, vertical: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text shadow offset.
> 
> **Parameters**:
> - `horizontal`: Horizontal offset.
> - `vertical`: Vertical offset.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowHorizontalOffset(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text shadow horizontal offset.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowVerticalOffset(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text shadow vertical offset.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowBlurRadius(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text shadow blur radius.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextLetterSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text letter spacing.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextWordSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text word spacing.
> 
<pre class="language-typescript"><code class="lang-typescript">function TextParagraphSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the text paragraph spacing.
> 
<pre class="language-typescript"><code class="lang-typescript">function BackgroundColor(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the background color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetBackgroundImage(image: <a data-footnote-ref href="#user-content-fn-105">Image</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the background image of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColor(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the border color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorLeft(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the left border color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorTop(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top border color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorRight(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the right border color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorBottom(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom border color of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidth(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the border width of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the left border width of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthTop(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top border width of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthRight(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the right border width of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthBottom(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom border width of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadius(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the border radius of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top-left border radius of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopRight(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the top-right border radius of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom-left border radius of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomRight(value: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the bottom-right border radius of the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowX(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the overflow behavior on the X axis.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`. Refer to [OverflowEnum](../Enums/OverflowEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowY(value: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the overflow behavior on the Y axis.
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`. Refer to [OverflowEnum](../Enums/OverflowEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SetTransformOrigin(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Set the origin of the element.
> 
> **Parameters**:
> - `x`: X value.
> - `y`: Y value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformTranslate(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Translate the element.
> 
> **Parameters**:
> - `x`: X value.
> - `y`: Y value.
> - `percentage`: If true, the `value` will be treated as percentage value.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformScale(x: float, y: float) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Scale the element.
> 
> **Parameters**:
> - `x`: X value.
> - `y`: Y value.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformRotate(angle: float, angleUnit: int) -> <a data-footnote-ref href="#user-content-fn-113">VisualElement</a></code></pre>
> Rotate the element.
> 
> **Parameters**:
> - `angle`: The angle of rotation.
> - `angleUnit`: The unit of the angle. Valid values are: `Degree`, `Gradian`, `Radian`, and `Turn`. Refer to [AngleUnitEnum](../Enums/AngleUnitEnum.md)
> 

[^0]: [Color](../Collections/Color.md)
[^1]: [Dict](../Collections/Dict.md)
[^2]: [LightBuiltin](../Collections/LightBuiltin.md)
[^3]: [LineCastHitResult](../Collections/LineCastHitResult.md)
[^4]: [List](../Collections/List.md)
[^5]: [Quaternion](../Collections/Quaternion.md)
[^6]: [Range](../Collections/Range.md)
[^7]: [Set](../Collections/Set.md)
[^8]: [Vector2](../Collections/Vector2.md)
[^9]: [Vector3](../Collections/Vector3.md)
[^10]: [Animation](../Component/Animation.md)
[^11]: [Animator](../Component/Animator.md)
[^12]: [AudioSource](../Component/AudioSource.md)
[^13]: [Collider](../Component/Collider.md)
[^14]: [Collision](../Component/Collision.md)
[^15]: [LineRenderer](../Component/LineRenderer.md)
[^16]: [LodBuiltin](../Component/LodBuiltin.md)
[^17]: [MapTargetable](../Component/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../Component/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../Component/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)
[^21]: [Character](../Entities/Character.md)
[^22]: [Human](../Entities/Human.md)
[^23]: [MapObject](../Entities/MapObject.md)
[^24]: [NetworkView](../Entities/NetworkView.md)
[^25]: [Player](../Entities/Player.md)
[^26]: [Prefab](../Entities/Prefab.md)
[^27]: [Shifter](../Entities/Shifter.md)
[^28]: [Titan](../Entities/Titan.md)
[^29]: [Transform](../Entities/Transform.md)
[^30]: [WallColossal](../Entities/WallColossal.md)
[^31]: [AlignEnum](../Enums/AlignEnum.md)
[^32]: [AngleUnitEnum](../Enums/AngleUnitEnum.md)
[^33]: [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md)
[^34]: [AspectRatioEnum](../Enums/AspectRatioEnum.md)
[^35]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^36]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^37]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^38]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^39]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^40]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^41]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^42]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^43]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^44]: [FontScaleModeEnum](../Enums/FontScaleModeEnum.md)
[^45]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^46]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^47]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^48]: [HandStateEnum](../Enums/HandStateEnum.md)
[^49]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^50]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^51]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^52]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^53]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^54]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^55]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^56]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^57]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^58]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^59]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^60]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^61]: [JustifyEnum](../Enums/JustifyEnum.md)
[^62]: [LanguageEnum](../Enums/LanguageEnum.md)
[^63]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^64]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^65]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^66]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^67]: [OverflowEnum](../Enums/OverflowEnum.md)
[^68]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^69]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^70]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^71]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^72]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^73]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^74]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^75]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^76]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^77]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^78]: [SpecialEnum](../Enums/SpecialEnum.md)
[^79]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^80]: [StunStateEnum](../Enums/StunStateEnum.md)
[^81]: [TeamEnum](../Enums/TeamEnum.md)
[^82]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^83]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^84]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^85]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^86]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^87]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^88]: [UILabelEnum](../Enums/UILabelEnum.md)
[^89]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^90]: [WeaponEnum](../Enums/WeaponEnum.md)
[^91]: [Camera](../Game/Camera.md)
[^92]: [Cutscene](../Game/Cutscene.md)
[^93]: [Game](../Game/Game.md)
[^94]: [Input](../Game/Input.md)
[^95]: [Locale](../Game/Locale.md)
[^96]: [Map](../Game/Map.md)
[^97]: [Network](../Game/Network.md)
[^98]: [PersistentData](../Game/PersistentData.md)
[^99]: [Physics](../Game/Physics.md)
[^100]: [RoomData](../Game/RoomData.md)
[^101]: [Time](../Game/Time.md)
[^102]: [Button](../UIElements/Button.md)
[^103]: [Dropdown](../UIElements/Dropdown.md)
[^104]: [Icon](../UIElements/Icon.md)
[^105]: [Image](../UIElements/Image.md)
[^106]: [Label](../UIElements/Label.md)
[^107]: [ProgressBar](../UIElements/ProgressBar.md)
[^108]: [ScrollView](../UIElements/ScrollView.md)
[^109]: [Slider](../UIElements/Slider.md)
[^110]: [TextField](../UIElements/TextField.md)
[^111]: [Toggle](../UIElements/Toggle.md)
[^112]: [UI](../UIElements/UI.md)
[^113]: [VisualElement](../UIElements/VisualElement.md)
[^114]: [Convert](../Utility/Convert.md)
[^115]: [Json](../Utility/Json.md)
[^116]: [Math](../Utility/Math.md)
[^117]: [Random](../Utility/Random.md)
[^118]: [String](../Utility/String.md)
[^119]: [Object](../objects/Object.md)
[^120]: [Component](../objects/Component.md)
