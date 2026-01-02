# VisualElement
Inherits from [Object](../objects/Object.md)

Base class for all UI elements. Note: Most methods return self to allow method chaining.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ChildCount|int|True|The number of child elements in this visual element.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Add(visualElement: <a data-footnote-ref href="#user-content-fn-53">VisualElement</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Add a child element.
> 
> **Parameters**:
> - `visualElement`: The visual element to add as a child.
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(visualElement: <a data-footnote-ref href="#user-content-fn-53">VisualElement</a>)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function GetElementAt(index: int) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Get child element at index.
> 
> **Parameters**:
> - `index`: The index of the child element to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function QueryByName(name: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Query child element by name. Returns: The first child element with the matching name.
> 
> **Parameters**:
> - `name`: The name of the element to find.
> 
<pre class="language-typescript"><code class="lang-typescript">function QueryByClassName(className: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Query child element by class name. Returns: The first child element with the matching class name.
> 
> **Parameters**:
> - `className`: The class name of the element to find.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseEnterEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Register a callback for mouse enter event. Mouse enter event is fired when the mouse pointer enters an element or one of its children
> 
> **Parameters**:
> - `method`: The method to call when the mouse enters the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseLeaveEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Register a callback for mouse leave event. Mouse leave event is fired when the mouse pointer exits an element and all its children
> 
> **Parameters**:
> - `method`: The method to call when the mouse leaves the element.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterClickEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Register a callback for click event
> 
> **Parameters**:
> - `method`: The method to call when the element is clicked.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusInEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Register a callback for focus in event. Focus in event is fired immediately before an element gains focus
> 
> **Parameters**:
> - `method`: The method to call when the element gains focus.
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusOutEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Register a callback for focus out event. Focus out event is fired immediately before an element loses focus
> 
> **Parameters**:
> - `method`: The method to call when the element loses focus.
> 
<pre class="language-typescript"><code class="lang-typescript">function Opacity(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the opacity of the element
> 
> **Parameters**:
> - `value`: a value between 0 and 100
> 
<pre class="language-typescript"><code class="lang-typescript">function Active(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the element to be active or inactive
> 
<pre class="language-typescript"><code class="lang-typescript">function Visible(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the element to be visible or hidden
> 
<pre class="language-typescript"><code class="lang-typescript">function TransitionDuration(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the transition duration of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function Absolute(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the element to be absolute or relative positioned
> 
<pre class="language-typescript"><code class="lang-typescript">function Left(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the left position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Top(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Right(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the right position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Bottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexShrink(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the flex shrink value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexGrow(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the flex grow value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexDirection(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the flex direction
> 
> **Parameters**:
> - `value`: Acceptable values are: `Row`, `Column`, `RowReverse`, and `ColumnReverse`
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignItems(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the align items property
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`
> 
<pre class="language-typescript"><code class="lang-typescript">function JustifyContent(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the justify content property
> 
> **Parameters**:
> - `value`: Acceptable values are: `FlexStart`, `Center`, `FlexEnd`, `SpaceBetween`, `SpaceAround`, and `SpaceEvenly`
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignSelf(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the align self property
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`
> 
<pre class="language-typescript"><code class="lang-typescript">function Width(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the width of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Height(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the height of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Margin(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the left margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the right margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Padding(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the left padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the right padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function FontStyle(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the font style of the element
> 
> **Parameters**:
> - `value`: Acceptable values are: `Normal`, `Bold`, `Italic`, and `BoldAndItalic`
> 
<pre class="language-typescript"><code class="lang-typescript">function FontSize(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the font size of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Color(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function TextAlign(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text alignment of the element
> 
> **Parameters**:
> - `value`: Valid values are: `UpperLeft`, `UpperCenter`, `UpperRight`, `MiddleLeft`, `MiddleCenter`, `MiddleRight`, `LowerLeft`, `LowerCenter`, `LowerRight`
> 
<pre class="language-typescript"><code class="lang-typescript">function TextWrap(value: bool = True) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set whether the text should wrap or not
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOverflow(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text overflow behavior
> 
> **Parameters**:
> - `value`: Acceptable values are: `Clip`, `Ellipsis`
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOutlineWidth(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text outline width
> 
<pre class="language-typescript"><code class="lang-typescript">function TextOutlineColor(value: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text outline color
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowColor(value: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text shadow color
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowOffset(horizontal: float, vertical: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text shadow offset
> 
> **Parameters**:
> - `horizontal`: horizontal offset
> - `vertical`: vertical offset
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowHorizontalOffset(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text shadow horizontal offset
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowVerticalOffset(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text shadow vertical offset
> 
<pre class="language-typescript"><code class="lang-typescript">function TextShadowBlurRadius(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text shadow blur radius
> 
<pre class="language-typescript"><code class="lang-typescript">function TextLetterSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text letter spacing
> 
<pre class="language-typescript"><code class="lang-typescript">function TextWordSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text word spacing
> 
<pre class="language-typescript"><code class="lang-typescript">function TextParagraphSpacing(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the text paragraph spacing
> 
<pre class="language-typescript"><code class="lang-typescript">function BackgroundColor(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the background color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function SetBackgroundImage(image: <a data-footnote-ref href="#user-content-fn-45">Image</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the background image of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColor(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorLeft(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the left border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorTop(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorRight(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the right border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorBottom(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidth(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the left border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthTop(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthRight(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the right border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthBottom(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadius(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top-left border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopRight(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the top-right border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom-left border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomRight(value: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the bottom-right border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowX(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the overflow behavior on the X axis
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowY(value: string) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the overflow behavior on the Y axis
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`
> 
<pre class="language-typescript"><code class="lang-typescript">function SetTransformOrigin(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Set the origin of the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformTranslate(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Translate the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformScale(x: float, y: float) -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Scale the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformRotate(angle: float, angleUnit: string = "Degree") -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Rotate the element
> 
> **Parameters**:
> - `angle`: the angle of rotation
> - `angleUnit`: the unit of the angle. Valid values are: `Degree`, `Gradian`, `Radian`, and `Turn`
> 

[^0]: [Color](../objects/Color.md)
[^1]: [Dict](../objects/Dict.md)
[^2]: [LightBuiltin](../static/LightBuiltin.md)
[^3]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^4]: [List](../objects/List.md)
[^5]: [Quaternion](../objects/Quaternion.md)
[^6]: [Range](../objects/Range.md)
[^7]: [Set](../objects/Set.md)
[^8]: [Vector2](../objects/Vector2.md)
[^9]: [Vector3](../objects/Vector3.md)
[^10]: [Animation](../objects/Animation.md)
[^11]: [Animator](../objects/Animator.md)
[^12]: [AudioSource](../objects/AudioSource.md)
[^13]: [Collider](../objects/Collider.md)
[^14]: [Collision](../objects/Collision.md)
[^15]: [LineRenderer](../objects/LineRenderer.md)
[^16]: [LodBuiltin](../static/LodBuiltin.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^21]: [Character](../objects/Character.md)
[^22]: [Human](../objects/Human.md)
[^23]: [MapObject](../objects/MapObject.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [Player](../objects/Player.md)
[^26]: [Prefab](../objects/Prefab.md)
[^27]: [Shifter](../objects/Shifter.md)
[^28]: [Titan](../objects/Titan.md)
[^29]: [Transform](../objects/Transform.md)
[^30]: [WallColossal](../objects/WallColossal.md)
[^31]: [Camera](../static/Camera.md)
[^32]: [Cutscene](../static/Cutscene.md)
[^33]: [Game](../static/Game.md)
[^34]: [Input](../static/Input.md)
[^35]: [Locale](../static/Locale.md)
[^36]: [Map](../static/Map.md)
[^37]: [Network](../static/Network.md)
[^38]: [PersistentData](../static/PersistentData.md)
[^39]: [Physics](../static/Physics.md)
[^40]: [RoomData](../static/RoomData.md)
[^41]: [Time](../static/Time.md)
[^42]: [Button](../objects/Button.md)
[^43]: [Dropdown](../objects/Dropdown.md)
[^44]: [Icon](../objects/Icon.md)
[^45]: [Image](../objects/Image.md)
[^46]: [Label](../objects/Label.md)
[^47]: [ProgressBar](../objects/ProgressBar.md)
[^48]: [ScrollView](../objects/ScrollView.md)
[^49]: [Slider](../objects/Slider.md)
[^50]: [TextField](../objects/TextField.md)
[^51]: [Toggle](../objects/Toggle.md)
[^52]: [UI](../static/UI.md)
[^53]: [VisualElement](../objects/VisualElement.md)
[^54]: [Convert](../static/Convert.md)
[^55]: [Json](../static/Json.md)
[^56]: [Math](../static/Math.md)
[^57]: [Random](../objects/Random.md)
[^58]: [String](../static/String.md)
[^59]: [Object](../objects/Object.md)
[^60]: [Component](../objects/Component.md)
