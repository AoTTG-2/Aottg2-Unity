# Vector3
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
`__Copy__`, `+`, `-`, `*`, `/`, `==`, `__Hash__`
### Initialization
```csharp
Vector3() // Default constructor, Initializes the Vector3 to (0, 0, 0).
Vector3(xyz: float) // Initializes the Vector3 to (xyz, xyz, xyz).
Vector3(x: float, y: float) // Initializes the Vector3 to (x, y, 0).
Vector3(x: float, y: float, z: float) // Initializes the Vector3 to (x, y, z).
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False||
|Y|float|False||
|Z|float|False||
|Normalized|[Vector3](../objects/Vector3.md)|True||
|Magnitude|float|True||
|SqrMagnitude|float|True||


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector3](../objects/Vector3.md)|True||
|One|[Vector3](../objects/Vector3.md)|True||
|Up|[Vector3](../objects/Vector3.md)|True||
|Down|[Vector3](../objects/Vector3.md)|True||
|Left|[Vector3](../objects/Vector3.md)|True||
|Right|[Vector3](../objects/Vector3.md)|True||
|Forward|[Vector3](../objects/Vector3.md)|True||
|Back|[Vector3](../objects/Vector3.md)|True||
|NegativeInfinity|[Vector3](../objects/Vector3.md)|True||
|PositiveInfinity|[Vector3](../objects/Vector3.md)|True||


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Set(x: float, y: float, z: float)</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function GetRotationDirection(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Scale(scale: <a data-footnote-ref href="#user-content-fn-57">Object</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use multiply operator instead
{% endhint %}

> Returns the Vector3 multiplied by scale.
> 
> **Parameters**:
> - `scale`: float | Vector3
> 
<pre class="language-typescript"><code class="lang-typescript">function Multiply(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use multiply operator instead
{% endhint %}

> Returns the multiplication of two Vector3s.
> 
> **Parameters**:
> - `a`: Vector3
> - `b`: Vector3
> 
> **Returns**: Vector3
<pre class="language-typescript"><code class="lang-typescript">function Divide(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use divide operator instead
{% endhint %}

> Returns the division of two Vector3s.
> 
> **Parameters**:
> - `a`: Vector3
> - `b`: Vector3
> 
> **Returns**: Vector3

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Angle(from: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function ClampMagnitude(vector: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, maxLength: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Cross(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Distance(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Dot(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, maxDistanceDelta: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Normalize(value: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function OrthoNormalize(normal: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, tangent: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Makes vectors normalized and orthogonal to each other.
Normalizes normal. Normalizes tangent and makes sure it is orthogonal to normal (that is, angle between them is 90 degrees).
Honestly just go look up the unity docs for this one idk.
This one uses references so the Vectors will be modified in place.
> 
<pre class="language-typescript"><code class="lang-typescript">function Project(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function ProjectOnPlane(vector: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, plane: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Reflect(inDirection: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, inNormal: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(current: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, maxRadiansDelta: float, maxMagnitudeDelta: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SignedAngle(from: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, smoothTime: float, maxSpeed: float) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Smoothly damps current towards target with currentVelocity as state. smoothTime and maxSpeed control how aggressive the transition is.
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
