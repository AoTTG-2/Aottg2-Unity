# ScrollView
Inherits from [VisualElement](../objects/VisualElement.md)

ScrollView UI element that provides scrollable content.

Note: Most methods return self to allow method chaining

### Initialization
```csharp
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ScrollOffset|[Vector2](../objects/Vector2.md)|False|The current scroll offset|
|ScrollDecelerationRate|float|False|Controls the scrolling speed when using the scroll wheel|
|MouseWheelScrollSize|float|False|Controls the sensitivity/speed of mouse wheel scrolling|
|HorizontalScrollEnabled|bool|False|Enable or disable horizontal scrolling|
|VerticalScrollEnabled|bool|False|Enable or disable vertical scrolling|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Elasticity(value: string) -> <a data-footnote-ref href="#user-content-fn-52">ScrollView</a></code></pre>
> The behavior to use when scrolling reaches limits of the content
> 
> **Parameters**:
> - `value`: Acceptable values are: `Clamped`, `Elastic`, and `Unrestricted`
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScrollDecelerationRate(rate: float) -> <a data-footnote-ref href="#user-content-fn-52">ScrollView</a></code></pre>
> Controls the rate at which scrolling movement slows after a user scrolling action
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScrollOffset(offset: <a data-footnote-ref href="#user-content-fn-45">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-52">ScrollView</a></code></pre>
> Set the scroll offset
> 
<pre class="language-typescript"><code class="lang-typescript">function ScrollToTop()</code></pre>
> Scroll to the top of the content
> 
<pre class="language-typescript"><code class="lang-typescript">function ScrollToBottom()</code></pre>
> Scroll to the bottom of the content
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
