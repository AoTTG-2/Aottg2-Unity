# VisualElement
Inherits from [Object](../objects/Object.md)

Base class for all UI elements

Note: Most methods return self to allow method chaining

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ChildCount|int|True|Number of child elements|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Add(visualElement: <a data-footnote-ref href="#user-content-fn-56">VisualElement</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Add a child element
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(visualElement: <a data-footnote-ref href="#user-content-fn-56">VisualElement</a>)</code></pre>
> Remove a child elemend
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveFromHierarchy()</code></pre>
> Removes this element from its parent hierarchy
> 
<pre class="language-typescript"><code class="lang-typescript">function GetElementAt(index: int) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Get child element at index
> 
<pre class="language-typescript"><code class="lang-typescript">function QueryByName(name: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Query child element by name
> 
> **Returns**: The first child element with the matching name
<pre class="language-typescript"><code class="lang-typescript">function QueryByClassName(className: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Query child element by class name
> 
> **Returns**: The first child element with the matching class name
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseEnterEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Register a callback for mouse enter event

Mouse enter event is fired when the mouse pointer enters an element or one of its children
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterMouseLeaveEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Register a callback for mouse leave event

Mouse leave event is fired when the mouse pointer exits an element and all its children
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterClickEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Register a callback for click event
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusInEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Register a callback for focus in event

Focus in event is fired immediately before an element gains focus
> 
<pre class="language-typescript"><code class="lang-typescript">function RegisterFocusOutEventCallback(method: function) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Register a callback for focus out event.

Focus out event is fired immediately before an element loses focus
> 
<pre class="language-typescript"><code class="lang-typescript">function Opacity(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the opacity of the element
> 
> **Parameters**:
> - `value`: a value between 0 and 100
> 
<pre class="language-typescript"><code class="lang-typescript">function Active(value: bool) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the element to be active or inactive
> 
<pre class="language-typescript"><code class="lang-typescript">function Visible(value: bool) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the element to be visible or hidden
> 
<pre class="language-typescript"><code class="lang-typescript">function TransitionDuration(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the element to be visible or hidden
> 
<pre class="language-typescript"><code class="lang-typescript">function Absolute(value: bool) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the element to be absolute or relative positioned
> 
<pre class="language-typescript"><code class="lang-typescript">function Left(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the left position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Top(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Right(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the right position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Bottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom position of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexShrink(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the flex shrink value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexGrow(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the flex grow value
> 
<pre class="language-typescript"><code class="lang-typescript">function FlexDirection(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the flex direction
> 
> **Parameters**:
> - `value`: Acceptable values are: `Row`, `Column`, `RowReverse`, and `ColumnReverse`
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignItems(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the align items property
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`
> 
<pre class="language-typescript"><code class="lang-typescript">function JustifyContent(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the justify content property
> 
> **Parameters**:
> - `value`: Acceptable values are: `FlexStart`, `Center`, `FlexEnd`, `SpaceBetween`, `SpaceAround`, and `SpaceEvenly`
> 
<pre class="language-typescript"><code class="lang-typescript">function AlignSelf(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the align self property
> 
> **Parameters**:
> - `value`: Acceptable values are: `Auto`, `FlexStart`, `Center`, `FlexEnd`, and `Stretch`
> 
<pre class="language-typescript"><code class="lang-typescript">function Width(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the width of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Height(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the height of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Margin(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the left margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the right margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function MarginBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom margin of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Padding(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingLeft(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the left padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingTop(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingRight(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the right padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function PaddingBottom(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom padding of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function FontStyle(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the font style of the element
> 
> **Parameters**:
> - `value`: Acceptable values are: `Normal`, `Bold`, `Italic`, and `BoldAndItalic`
> 
<pre class="language-typescript"><code class="lang-typescript">function FontSize(value: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the font size of the element
> 
> **Parameters**:
> - `value`: the value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function Color(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the text color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BackgroundColor(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the background color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColor(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorLeft(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the left border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorTop(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorRight(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the right border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderColorBottom(color: <a data-footnote-ref href="#user-content-fn-7">Color</a>) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom border color of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidth(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the left border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthTop(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthRight(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the right border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderWidthBottom(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom border width of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadius(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top-left border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusTopRight(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the top-right border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomLeft(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom-left border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function BorderRadiusBottomRight(value: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the bottom-right border radius of the element
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowX(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the overflow behavior on the X axis
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`
> 
<pre class="language-typescript"><code class="lang-typescript">function OverflowY(value: string) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the overflow behavior on the Y axis
> 
> **Parameters**:
> - `value`: Acceptable values are: `Visible` and `Hidden`
> 
<pre class="language-typescript"><code class="lang-typescript">function SetTransformOrigin(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Set the origin of the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformTranslate(x: float, y: float, percentage: bool = False) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Translate the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> - `percentage`: if true, the `value` will be treated as percentage value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformScale(x: float, y: float) -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Scale the element
> 
> **Parameters**:
> - `x`: x value
> - `y`: y value
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformRotate(angle: float, angleUnit: string = "Degree") -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Rotate the element
> 
> **Parameters**:
> - `angle`: the angle of rotation
> - `angleUnit`: the unit of the angle. Valid values are: `Degree`, `Gradian`, `Radian`, and `Turn`
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
