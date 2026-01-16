# Vector2
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
`__Copy__`, `+`, `-`, `*`, `/`, `==`, `__Hash__`
### Initialization
```csharp
Vector2() // Default constructor, initializes the Vector2 to (0, 0).
Vector2(xy: float) // Initializes the Vector2 to (xy, xy).
Vector2(x: float, y: float) // Initializes the Vector2 to (x, y).
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|The X component of the vector.|
|Y|float|False|The Y component of the vector.|
|Normalized|[Vector2](../objects/Vector2.md)|True|Returns a normalized copy of this vector (magnitude of 1).|
|Magnitude|float|True|Returns the length of this vector.|
|SqrMagnitude|float|True|Returns the squared length of this vector (faster than Magnitude).|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).|
|PositiveInfinity|[Vector2](../objects/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Set(x: float, y: float)</code></pre>
> Sets the X and Y components of the vector.
> 
> **Parameters**:
> - `x`: The X component.
> - `y`: The Y component.
> 
<pre class="language-typescript"><code class="lang-typescript">function Normalize()</code></pre>
> Normalizes the vector in place.
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Angle(from: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, to: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> float</code></pre>
> Calculates the angle between two vectors.
> 
> **Parameters**:
> - `from`: The vector from which the angular difference is measured.
> - `to`: The vector to which the angular difference is measured.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClampMagnitude(vector: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, maxLength: float) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Clamps the magnitude of a vector to a maximum value.
> 
> **Parameters**:
> - `vector`: The vector to clamp.
> - `maxLength`: The maximum length of the vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Distance(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> float</code></pre>
> Calculates the distance between two points.
> 
> **Parameters**:
> - `a`: The first point.
> - `b`: The second point.
> 
<pre class="language-typescript"><code class="lang-typescript">function Dot(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> float</code></pre>
> Calculates the dot product of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Linearly interpolates between two vectors.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Linearly interpolates between two vectors without clamping.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (not clamped).
> 
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Returns a vector that is made from the largest components of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, b: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Returns a vector that is made from the smallest components of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, target: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, maxDistanceDelta: float) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Moves a point towards a target position.
> 
> **Parameters**:
> - `current`: The current position.
> - `target`: The target position.
> - `maxDistanceDelta`: The maximum distance to move.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reflect(inDirection: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, inNormal: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Reflects a vector off a plane defined by a normal vector.
> 
> **Parameters**:
> - `inDirection`: The incoming direction vector.
> - `inNormal`: The normal vector of the surface.
> 
<pre class="language-typescript"><code class="lang-typescript">function SignedAngle(from: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, to: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>) -> float</code></pre>
> Calculates the signed angle between two vectors.
> 
> **Parameters**:
> - `from`: The vector from which the angular difference is measured.
> - `to`: The vector to which the angular difference is measured.
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, target: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-8">Vector2</a>, smoothTime: float, maxSpeed: float) -> <a data-footnote-ref href="#user-content-fn-8">Vector2</a></code></pre>
> Smoothly dampens a vector towards a target over time.
> 
> **Parameters**:
> - `current`: The current position.
> - `target`: The target position.
> - `currentVelocity`: The current velocity (modified by the function).
> - `smoothTime`: The time it takes to reach the target (approximately).
> - `maxSpeed`: The maximum speed.
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
[^31]: [CharacterTypeEnum](../static/CharacterTypeEnum.md)
[^32]: [CollideModeEnum](../static/CollideModeEnum.md)
[^33]: [CollideWithEnum](../static/CollideWithEnum.md)
[^34]: [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)
[^35]: [EffectNameEnum](../static/EffectNameEnum.md)
[^36]: [ForceModeEnum](../static/ForceModeEnum.md)
[^37]: [HandStateEnum](../static/HandStateEnum.md)
[^38]: [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
[^39]: [InputCategoryEnum](../static/InputCategoryEnum.md)
[^40]: [LanguageEnum](../static/LanguageEnum.md)
[^41]: [LoadoutEnum](../static/LoadoutEnum.md)
[^42]: [OutlineModeEnum](../static/OutlineModeEnum.md)
[^43]: [PhysicMaterialCombineEnum](../static/PhysicMaterialCombineEnum.md)
[^44]: [PlayerStatusEnum](../static/PlayerStatusEnum.md)
[^45]: [ProjectileNameEnum](../static/ProjectileNameEnum.md)
[^46]: [ScaleModeEnum](../static/ScaleModeEnum.md)
[^47]: [ShifterTypeEnum](../static/ShifterTypeEnum.md)
[^48]: [SliderDirectionEnum](../static/SliderDirectionEnum.md)
[^49]: [SteamStateEnum](../static/SteamStateEnum.md)
[^50]: [TeamEnum](../static/TeamEnum.md)
[^51]: [TitanTypeEnum](../static/TitanTypeEnum.md)
[^52]: [TSKillSoundEnum](../static/TSKillSoundEnum.md)
[^53]: [WeaponEnum](../static/WeaponEnum.md)
[^54]: [Camera](../static/Camera.md)
[^55]: [Cutscene](../static/Cutscene.md)
[^56]: [Game](../static/Game.md)
[^57]: [Input](../static/Input.md)
[^58]: [Locale](../static/Locale.md)
[^59]: [Map](../static/Map.md)
[^60]: [Network](../static/Network.md)
[^61]: [PersistentData](../static/PersistentData.md)
[^62]: [Physics](../static/Physics.md)
[^63]: [RoomData](../static/RoomData.md)
[^64]: [Time](../static/Time.md)
[^65]: [Button](../objects/Button.md)
[^66]: [Dropdown](../objects/Dropdown.md)
[^67]: [Icon](../objects/Icon.md)
[^68]: [Image](../objects/Image.md)
[^69]: [Label](../objects/Label.md)
[^70]: [ProgressBar](../objects/ProgressBar.md)
[^71]: [ScrollView](../objects/ScrollView.md)
[^72]: [Slider](../objects/Slider.md)
[^73]: [TextField](../objects/TextField.md)
[^74]: [Toggle](../objects/Toggle.md)
[^75]: [UI](../static/UI.md)
[^76]: [VisualElement](../objects/VisualElement.md)
[^77]: [Convert](../static/Convert.md)
[^78]: [Json](../static/Json.md)
[^79]: [Math](../static/Math.md)
[^80]: [Random](../objects/Random.md)
[^81]: [String](../static/String.md)
[^82]: [Object](../objects/Object.md)
[^83]: [Component](../objects/Component.md)
