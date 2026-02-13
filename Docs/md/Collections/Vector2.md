# Vector2
Inherits from [Object](../objects/Object.md)

Represents a 2D vector with X and Y components. Supports mathematical operations and implements copy semantics.

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
|Normalized|[Vector2](../Collections/Vector2.md)|True|Returns a normalized copy of this vector (magnitude of 1).|
|Magnitude|float|True|Returns the length of this vector.|
|SqrMagnitude|float|True|Returns the squared length of this vector (faster than Magnitude).|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Zero|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(0, 0).|
|One|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(1, 1).|
|Up|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(0, 1).|
|Down|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(0, -1).|
|Left|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(-1, 0).|
|Right|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(1, 0).|
|NegativeInfinity|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).|
|PositiveInfinity|[Vector2](../Collections/Vector2.md)|True|Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).|


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
[^34]: [AspectRatioEnum](../Enums/AspectRatioEnum.md)
[^35]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^36]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^37]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^38]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^39]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^40]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^41]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^42]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^43]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^44]: [FontScaleModeEnum](../Enums/FontScaleModeEnum.md)
[^45]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^46]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^47]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^48]: [HandStateEnum](../Enums/HandStateEnum.md)
[^49]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^50]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^51]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^52]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^53]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^54]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^55]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^56]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^57]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^58]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^59]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^60]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^61]: [JustifyEnum](../Enums/JustifyEnum.md)
[^62]: [LanguageEnum](../Enums/LanguageEnum.md)
[^63]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^64]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^65]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^66]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^67]: [OverflowEnum](../Enums/OverflowEnum.md)
[^68]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^69]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^70]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^71]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^72]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^73]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^74]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^75]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^76]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^77]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^78]: [SpecialEnum](../Enums/SpecialEnum.md)
[^79]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^80]: [StunStateEnum](../Enums/StunStateEnum.md)
[^81]: [TeamEnum](../Enums/TeamEnum.md)
[^82]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^83]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^84]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^85]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^86]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^87]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^88]: [UILabelEnum](../Enums/UILabelEnum.md)
[^89]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^90]: [WeaponEnum](../Enums/WeaponEnum.md)
[^91]: [Camera](../Game/Camera.md)
[^92]: [Cutscene](../Game/Cutscene.md)
[^93]: [Game](../Game/Game.md)
[^94]: [Input](../Game/Input.md)
[^95]: [Locale](../Game/Locale.md)
[^96]: [Map](../Game/Map.md)
[^97]: [Network](../Game/Network.md)
[^98]: [PersistentData](../Game/PersistentData.md)
[^99]: [Physics](../Game/Physics.md)
[^100]: [RoomData](../Game/RoomData.md)
[^101]: [Time](../Game/Time.md)
[^102]: [Button](../UIElements/Button.md)
[^103]: [Dropdown](../UIElements/Dropdown.md)
[^104]: [Icon](../UIElements/Icon.md)
[^105]: [Image](../UIElements/Image.md)
[^106]: [Label](../UIElements/Label.md)
[^107]: [ProgressBar](../UIElements/ProgressBar.md)
[^108]: [ScrollView](../UIElements/ScrollView.md)
[^109]: [Slider](../UIElements/Slider.md)
[^110]: [TextField](../UIElements/TextField.md)
[^111]: [Toggle](../UIElements/Toggle.md)
[^112]: [UI](../UIElements/UI.md)
[^113]: [VisualElement](../UIElements/VisualElement.md)
[^114]: [Convert](../Utility/Convert.md)
[^115]: [Json](../Utility/Json.md)
[^116]: [Math](../Utility/Math.md)
[^117]: [Random](../Utility/Random.md)
[^118]: [String](../Utility/String.md)
[^119]: [Object](../objects/Object.md)
[^120]: [Component](../objects/Component.md)
