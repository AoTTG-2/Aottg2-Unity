# Quaternion
Inherits from [Object](../objects/Object.md)

Represents a quaternion.

### Remarks
Overloads operators: 
`__Copy__`, `*`, `==`, `__Hash__`
### Initialization
```csharp
Quaternion()
Quaternion(x: float, y: float, z: float, w: float) // Creates a new Quaternion from the given values.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Y|float|False|Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|Z|float|False|Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.|
|W|float|False|W component of the Quaternion. Do not directly modify quaternions.|
|Euler|[Vector3](../objects/Vector3.md)|False|Returns or sets the euler angle representation of the rotation.|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Identity|[Quaternion](../objects/Quaternion.md)|True|The identity rotation (Read Only).|


### Methods

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Interpolates between a and b by t and normalizes the result afterwards.
> 
> **Returns**: A unit quaternion interpolated between quaternions a and b.
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.
> 
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Spherically linear interpolates between unit quaternions a and b by a ratio of t.
> 
> **Returns**: A unit quaternion spherically interpolated between quaternions a and b.
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Spherically linear interpolates between unit quaternions a and b by t.
> 
> **Returns**: A unit quaternion spherically interpolated between unit quaternions a and b.
<pre class="language-typescript"><code class="lang-typescript">function FromEuler(euler: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Returns the Quaternion rotation from the given euler angles.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookRotation(forward: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, upwards: <a data-footnote-ref href="#user-content-fn-43">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Creates a rotation with the specified forward and upwards directions.
> 
<pre class="language-typescript"><code class="lang-typescript">function FromToRotation(a: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Creates a rotation from fromDirection to toDirection.
> 
> **Returns**: A unit quaternion which rotates from fromDirection to toDirection.
<pre class="language-typescript"><code class="lang-typescript">function Inverse(q: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Returns the Inverse of rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(from: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, to: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, maxDegreesDelta: float) -> <a data-footnote-ref href="#user-content-fn-30">Quaternion</a></code></pre>
> Rotates a rotation from towards to.
> 
> **Returns**: A unit quaternion rotated towards to by an angular step of maxDegreesDelta.

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
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
