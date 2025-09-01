# Vector2
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
- `__Copy__`
- `+`
- `-`
- `*`
- `/`
- `==`
- `__Hash__`
### Initialization
```csharp
Vector2()
Vector2(xy: float)
Vector2(x: float, y: float)
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the vector.|
|Y|float|False|Y component of the vector.|
|Normalized|[Vector2](../objects/Vector2.md)|True|Returns this vector with a magnitude of 1 (Read Only).|
|Magnitude|float|True|Returns the length of this vector (Read Only).|
|SqrMagnitude|float|True|Returns the squared length of this vector (Read Only).|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|
|PositiveInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Set(x: float, y: float)</code></pre>
> Set x and y components of an existing Vector2.
> 
<pre class="language-typescript"><code class="lang-typescript">function Normalize()</code></pre>
> Makes this vector have a magnitude of 1.
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Angle(from: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, to: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> float</code></pre>
> Gets the unsigned angle in degrees between from and to.
> 
> **Returns**: The unsigned angle in degrees between the two vectors.
<pre class="language-typescript"><code class="lang-typescript">function ClampMagnitude(vector: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, maxLength: float) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Returns a copy of vector with its magnitude clamped to maxLength.
> 
<pre class="language-typescript"><code class="lang-typescript">function Distance(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> float</code></pre>
> Returns the distance between a and b.
> 
<pre class="language-typescript"><code class="lang-typescript">function Dot(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> float</code></pre>
> Dot Product of two vectors.
> 
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Linearly interpolates between vectors a and b by t.
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Linearly interpolates between vectors a and b by t.
> 
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Returns a vector that is made from the largest components of two vectors.
> 
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Returns a vector that is made from the smallest components of two vectors.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, target: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, maxDistanceDelta: float) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Moves a point current towards target.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reflect(inDirection: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, inNormal: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>
> Reflects a vector off the vector defined by a normal.
> 
<pre class="language-typescript"><code class="lang-typescript">function SignedAngle(from: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, to: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>) -> float</code></pre>
> Gets the signed angle in degrees between from and to.
> 
> **Returns**: The signed angle in degrees between the two vectors.
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, target: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-36">Vector2</a>, smoothTime: float, maxSpeed: float) -> <a data-footnote-ref href="#user-content-fn-36">Vector2</a></code></pre>

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
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
