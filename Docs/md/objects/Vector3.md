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
<pre class="language-typescript"><code class="lang-typescript">function GetRotationDirection(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Scale(scale: <a data-footnote-ref href="#user-content-fn-59">Object</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use multiply operator instead
{% endhint %}

> Returns the Vector3 multiplied by scale.
> 
> **Parameters**:
> - `scale`: float | Vector3
> 
<pre class="language-typescript"><code class="lang-typescript">function Multiply(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

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
<pre class="language-typescript"><code class="lang-typescript">function Divide(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

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
<pre class="language-typescript"><code class="lang-typescript">function Angle(from: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function ClampMagnitude(vector: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxLength: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Cross(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Distance(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Dot(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxDistanceDelta: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Normalize(value: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function OrthoNormalize(normal: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, tangent: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Makes vectors normalized and orthogonal to each other.
Normalizes normal. Normalizes tangent and makes sure it is orthogonal to normal (that is, angle between them is 90 degrees).
Honestly just go look up the unity docs for this one idk.
This one uses references so the Vectors will be modified in place.
> 
<pre class="language-typescript"><code class="lang-typescript">function Project(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function ProjectOnPlane(vector: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, plane: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Reflect(inDirection: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, inNormal: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxRadiansDelta: float, maxMagnitudeDelta: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SignedAngle(from: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, smoothTime: float, maxSpeed: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Smoothly damps current towards target with currentVelocity as state. smoothTime and maxSpeed control how aggressive the transition is.
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
