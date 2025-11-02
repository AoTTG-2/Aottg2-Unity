# LineRenderer
Inherits from [Object](../objects/Object.md)

Represents a LineRenderer

### Initialization
```csharp
LineRenderer() // Default constructor, Creates a black line with a width of 1
LineRenderer(color: Color, width: float) // Creates a line with the given color and width
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|StartWidth|float|False|The width of the line at the start|
|EndWidth|float|False|The width of the line at the end|
|LineColor|[Color](../objects/Color.md)|False|The color of the line|
|PositionCount|int|False|The number of points in the line|
|Enabled|bool|False|Is the line renderer enabled|
|Loop|bool|False|Is the line renderer a loop|
|NumCornerVertices|int|False|The number of corner vertices|
|NumCapVertices|int|False|The number of end cap vertices|
|Alignment|string|False|The alignment of the line renderer|
|TextureMode|string|False|The texture mode of the line renderer|
|UseWorldSpace|bool|False|Is the line renderer in world space|
|ShadowCastingMode|string|False|Does the line renderer cast shadows|
|ReceiveShadows|bool|False|Does the line renderer receive shadows|
|ColorGradient|[List](../objects/List.md)|False|The gradient of the line renderer|
|AlphaGradient|[List](../objects/List.md)|False|The alpha gradient of the line renderer|
|WidthCurve|[List](../objects/List.md)|False|The width curve of the line renderer|
|WidthMultiplier|float|False|The width multiplier of the line renderer|
|ColorGradientMode|string|False|The color gradient mode of the line renderer|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Destroy()</code></pre>
> Remove the line renderer (can also be done by removing all references to this object)
> 
<pre class="language-typescript"><code class="lang-typescript">function GetPosition(index: int) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Get the position of a point in the line renderer
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPosition(index: int, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Set the position of a point in the line renderer
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function CreateLineRenderer() -> <a data-footnote-ref href="#user-content-fn-17">LineRenderer</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Create a new instance with LineRenderer() instead.
{% endhint %}

> Create a new LineRenderer
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
