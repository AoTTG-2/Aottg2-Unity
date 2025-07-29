# LineRenderer
Inherits from [Object](../objects/Object.md)
### Initialization
```csharp
LineRenderer(parameterValues: Object) // Default constructor is parameterless.
lr = LineRenderer();
lr.StartWidth = 1;
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
<pre class="language-typescript"><code class="lang-typescript">function GetPosition(index: int) -> <a data-footnote-ref href="#user-content-fn-36">Vector3</a></code></pre>
> Get the position of a point in the line renderer
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPosition(index: int, position: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> null</code></pre>
> Set the position of a point in the line renderer
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function CreateLineRenderer() -> <a data-footnote-ref href="#user-content-fn-13">LineRenderer</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Create a new instance with LineRenderer() instead.
{% endhint %}

> Create a new LineRenderer
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Map](../static/Map.md)
[^16]: [MapObject](../objects/MapObject.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [Math](../static/Math.md)
[^19]: [Network](../static/Network.md)
[^20]: [NetworkView](../objects/NetworkView.md)
[^21]: [PersistentData](../static/PersistentData.md)
[^22]: [Physics](../static/Physics.md)
[^23]: [Player](../objects/Player.md)
[^24]: [Quaternion](../objects/Quaternion.md)
[^25]: [Random](../objects/Random.md)
[^26]: [Range](../objects/Range.md)
[^27]: [RoomData](../static/RoomData.md)
[^28]: [Set](../objects/Set.md)
[^29]: [Shifter](../objects/Shifter.md)
[^30]: [String](../static/String.md)
[^31]: [Time](../static/Time.md)
[^32]: [Titan](../objects/Titan.md)
[^33]: [Transform](../objects/Transform.md)
[^34]: [UI](../static/UI.md)
[^35]: [Vector2](../objects/Vector2.md)
[^36]: [Vector3](../objects/Vector3.md)
[^37]: [Object](../objects/Object.md)
[^38]: [Component](../objects/Component.md)
