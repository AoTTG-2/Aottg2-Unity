# Vector3
Inherits from [Object](../objects/Object.md)

Represents a 3D vector with X, Y, and Z components. Supports mathematical operations and implements copy semantics.

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
|X|float|False|The X component of the vector.|
|Y|float|False|The Y component of the vector.|
|Z|float|False|The Z component of the vector.|
|Normalized|[Vector3](../Collections/Vector3.md)|True|Returns a normalized copy of this vector (magnitude of 1).|
|Magnitude|float|True|Returns the length of this vector.|
|SqrMagnitude|float|True|Returns the squared length of this vector (faster than Magnitude).|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(0, 0, 0).|
|One|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(1, 1, 1).|
|Up|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(0, 1, 0).|
|Down|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(0, -1, 0).|
|Left|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(-1, 0, 0).|
|Right|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(1, 0, 0).|
|Forward|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(0, 0, 1).|
|Back|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(0, 0, -1).|
|NegativeInfinity|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).|
|PositiveInfinity|[Vector3](../Collections/Vector3.md)|True|Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Set(x: float, y: float, z: float)</code></pre>
> Sets the X, Y, and Z components of the vector.
> 
> **Parameters**:
> - `x`: The X component.
> - `y`: The Y component.
> - `z`: The Z component.
> 
<pre class="language-typescript"><code class="lang-typescript">function Scale(scale: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use multiply operator instead
{% endhint %}

> Scales the vector by a float or Vector3.
> 
> **Parameters**:
> - `scale`: The scale value (float or Vector3).
> 
> **Returns**: A new scaled vector.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Angle(from: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
> Calculates the angle between two vectors.
> 
> **Parameters**:
> - `from`: The vector from which the angular difference is measured.
> - `to`: The vector to which the angular difference is measured.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClampMagnitude(vector: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxLength: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Clamps the magnitude of a vector to a maximum value.
> 
> **Parameters**:
> - `vector`: The vector to clamp.
> - `maxLength`: The maximum length of the vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Cross(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Calculates the cross product of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Distance(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
> Calculates the distance between two points.
> 
> **Parameters**:
> - `a`: The first point.
> - `b`: The second point.
> 
<pre class="language-typescript"><code class="lang-typescript">function Dot(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
> Calculates the dot product of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Linearly interpolates between two vectors.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Linearly interpolates between two vectors without clamping.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (not clamped).
> 
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a vector that is made from the largest components of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a vector that is made from the smallest components of two vectors.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxDistanceDelta: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Moves a point towards a target position.
> 
> **Parameters**:
> - `current`: The current position.
> - `target`: The target position.
> - `maxDistanceDelta`: The maximum distance to move.
> 
<pre class="language-typescript"><code class="lang-typescript">function Normalize(value: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a normalized copy of the vector.
> 
> **Parameters**:
> - `value`: The vector to normalize.
> 
<pre class="language-typescript"><code class="lang-typescript">function OrthoNormalize(normal: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, tangent: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Orthonormalizes two vectors (normalizes the normal vector and makes the tangent vector orthogonal to it).
> 
> **Parameters**:
> - `normal`: The normal vector (will be normalized).
> - `tangent`: The tangent vector (will be normalized and made orthogonal to normal).
> 
<pre class="language-typescript"><code class="lang-typescript">function Project(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Projects a vector onto another vector.
> 
> **Parameters**:
> - `a`: The vector to project.
> - `b`: The vector to project onto.
> 
<pre class="language-typescript"><code class="lang-typescript">function ProjectOnPlane(vector: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, plane: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Projects a vector onto a plane defined by a normal vector.
> 
> **Parameters**:
> - `vector`: The vector to project.
> - `plane`: The plane normal vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reflect(inDirection: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, inNormal: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Reflects a vector off a plane defined by a normal vector.
> 
> **Parameters**:
> - `inDirection`: The incoming direction vector.
> - `inNormal`: The normal vector of the surface.
> 
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, maxRadiansDelta: float, maxMagnitudeDelta: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Rotates a vector towards a target vector.
> 
> **Parameters**:
> - `current`: The current direction vector.
> - `target`: The target direction vector.
> - `maxRadiansDelta`: The maximum change in radians.
> - `maxMagnitudeDelta`: The maximum change in magnitude.
> 
<pre class="language-typescript"><code class="lang-typescript">function SignedAngle(from: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, to: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> float</code></pre>
> Calculates the signed angle between two vectors.
> 
> **Parameters**:
> - `from`: The vector from which the angular difference is measured.
> - `to`: The vector to which the angular difference is measured.
> - `axis`: The axis around which the rotation is measured.
> 
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Spherically interpolates between two vectors.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Spherically interpolates between two vectors without clamping.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (not clamped).
> 
<pre class="language-typescript"><code class="lang-typescript">function SmoothDamp(current: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, currentVelocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, smoothTime: float, maxSpeed: float) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Smoothly dampens a vector towards a target over time.
> 
> **Parameters**:
> - `current`: The current position.
> - `target`: The target position.
> - `currentVelocity`: The current velocity (modified by the function).
> - `smoothTime`: The time it takes to reach the target (approximately).
> - `maxSpeed`: The maximum speed.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetRotationDirection(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Gets the direction vector transformed by a rotation.
> 
> **Parameters**:
> - `a`: The reference rotation vector (e.g., forward direction).
> - `b`: The vector to transform relative to the reference.
> 
> **Returns**: A new direction vector.
<pre class="language-typescript"><code class="lang-typescript">function Multiply(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use multiply operator instead
{% endhint %}

> Multiplies two vectors component-wise.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
> **Returns**: A new vector with multiplied components.
<pre class="language-typescript"><code class="lang-typescript">function Divide(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Use divide operator instead
{% endhint %}

> Divides two vectors component-wise.
> 
> **Parameters**:
> - `a`: The first vector.
> - `b`: The second vector.
> 
> **Returns**: A new vector with divided components.

[^0]: [Color](../Collections/Color.md)
[^1]: [Dict](../Collections/Dict.md)
[^2]: [LightBuiltin](../Collections/LightBuiltin.md)
[^3]: [LineCastHitResult](../Collections/LineCastHitResult.md)
[^4]: [List](../Collections/List.md)
[^5]: [Quaternion](../Collections/Quaternion.md)
[^6]: [Range](../Collections/Range.md)
[^7]: [Set](../Collections/Set.md)
[^8]: [Vector2](../Collections/Vector2.md)
[^9]: [Vector3](../Collections/Vector3.md)
[^10]: [Animation](../Component/Animation.md)
[^11]: [Animator](../Component/Animator.md)
[^12]: [AudioSource](../Component/AudioSource.md)
[^13]: [Collider](../Component/Collider.md)
[^14]: [Collision](../Component/Collision.md)
[^15]: [LineRenderer](../Component/LineRenderer.md)
[^16]: [LodBuiltin](../Component/LodBuiltin.md)
[^17]: [MapTargetable](../Component/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../Component/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../Component/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)
[^21]: [Character](../Entities/Character.md)
[^22]: [Human](../Entities/Human.md)
[^23]: [MapObject](../Entities/MapObject.md)
[^24]: [NetworkView](../Entities/NetworkView.md)
[^25]: [Player](../Entities/Player.md)
[^26]: [Prefab](../Entities/Prefab.md)
[^27]: [Shifter](../Entities/Shifter.md)
[^28]: [Titan](../Entities/Titan.md)
[^29]: [Transform](../Entities/Transform.md)
[^30]: [WallColossal](../Entities/WallColossal.md)
[^31]: [AlignEnum](../Enums/AlignEnum.md)
[^32]: [AngleUnitEnum](../Enums/AngleUnitEnum.md)
[^33]: [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md)
[^34]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^35]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^36]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^37]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^38]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^39]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^40]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^41]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^42]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^43]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^44]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^45]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^46]: [HandStateEnum](../Enums/HandStateEnum.md)
[^47]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^48]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^49]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^50]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^51]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^52]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^53]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^54]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^55]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^56]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^57]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^58]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^59]: [JustifyEnum](../Enums/JustifyEnum.md)
[^60]: [LanguageEnum](../Enums/LanguageEnum.md)
[^61]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^62]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^63]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^64]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^65]: [OverflowEnum](../Enums/OverflowEnum.md)
[^66]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^67]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^68]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^69]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^70]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^71]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^72]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^73]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^74]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^75]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^76]: [SpecialEnum](../Enums/SpecialEnum.md)
[^77]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^78]: [TeamEnum](../Enums/TeamEnum.md)
[^79]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^80]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^81]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^82]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^83]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^84]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^85]: [UILabelEnum](../Enums/UILabelEnum.md)
[^86]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^87]: [WeaponEnum](../Enums/WeaponEnum.md)
[^88]: [Camera](../Game/Camera.md)
[^89]: [Cutscene](../Game/Cutscene.md)
[^90]: [Game](../Game/Game.md)
[^91]: [Input](../Game/Input.md)
[^92]: [Locale](../Game/Locale.md)
[^93]: [Map](../Game/Map.md)
[^94]: [Network](../Game/Network.md)
[^95]: [PersistentData](../Game/PersistentData.md)
[^96]: [Physics](../Game/Physics.md)
[^97]: [RoomData](../Game/RoomData.md)
[^98]: [Time](../Game/Time.md)
[^99]: [Button](../UIElements/Button.md)
[^100]: [Dropdown](../UIElements/Dropdown.md)
[^101]: [Icon](../UIElements/Icon.md)
[^102]: [Image](../UIElements/Image.md)
[^103]: [Label](../UIElements/Label.md)
[^104]: [ProgressBar](../UIElements/ProgressBar.md)
[^105]: [ScrollView](../UIElements/ScrollView.md)
[^106]: [Slider](../UIElements/Slider.md)
[^107]: [TextField](../UIElements/TextField.md)
[^108]: [Toggle](../UIElements/Toggle.md)
[^109]: [UI](../UIElements/UI.md)
[^110]: [VisualElement](../UIElements/VisualElement.md)
[^111]: [Convert](../Utility/Convert.md)
[^112]: [Json](../Utility/Json.md)
[^113]: [Math](../Utility/Math.md)
[^114]: [Random](../Utility/Random.md)
[^115]: [String](../Utility/String.md)
[^116]: [Object](../objects/Object.md)
[^117]: [Component](../objects/Component.md)
