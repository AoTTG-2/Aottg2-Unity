# Quaternion
Inherits from [Object](../objects/Object.md)

Represents a quaternion.

### Remarks
Overloads operators: 
`__Copy__`, `*`, `==`, `__Hash__`
### Initialization
```csharp
Quaternion() // Default constructor, creates an identity quaternion (no rotation).
Quaternion(x: float, y: float, z: float, w: float) // Creates a new Quaternion from the given values.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|The X component of the quaternion.|
|Y|float|False|The Y component of the quaternion.|
|Z|float|False|The Z component of the quaternion.|
|W|float|False|The W component of the quaternion.|
|Euler|[Vector3](../Collections/Vector3.md)|False|Returns or sets the euler angle representation of the rotation.|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Identity|[Quaternion](../Collections/Quaternion.md)|True|The identity rotation (no rotation).|


### Methods

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Linearly interpolates between two rotations.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Linearly interpolates between two rotations without clamping.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (not clamped).
> 
> **Returns**: The interpolated quaternion.
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Spherically interpolates between two rotations.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
> **Returns**: The interpolated quaternion.
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Spherically interpolates between two rotations without clamping.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (not clamped).
> 
> **Returns**: The interpolated quaternion.
<pre class="language-typescript"><code class="lang-typescript">function FromEuler(euler: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation from euler angles.
> 
> **Parameters**:
> - `euler`: The euler angles in degrees (x, y, z).
> 
> **Returns**: A quaternion representing the rotation.
<pre class="language-typescript"><code class="lang-typescript">function LookRotation(forward: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, upwards: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation that looks in the specified direction.
> 
> **Parameters**:
> - `forward`: The forward direction vector.
> - `upwards`: Optional. The upwards direction vector (default: Vector3.up).
> 
> **Returns**: A quaternion representing the rotation.
<pre class="language-typescript"><code class="lang-typescript">function FromToRotation(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation from one direction to another.
> 
> **Parameters**:
> - `a`: The source direction vector.
> - `b`: The target direction vector.
> 
> **Returns**: A quaternion representing the rotation.
<pre class="language-typescript"><code class="lang-typescript">function Inverse(q: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Returns the inverse of a quaternion.
> 
> **Parameters**:
> - `q`: The quaternion to invert.
> 
> **Returns**: The inverse quaternion.
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(from: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, to: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, maxDegreesDelta: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Rotates a rotation towards a target rotation.
> 
> **Parameters**:
> - `from`: The current rotation.
> - `to`: The target rotation.
> - `maxDegreesDelta`: The maximum change in degrees.
> 
> **Returns**: The rotated quaternion.
<pre class="language-typescript"><code class="lang-typescript">function AngleAxis(angle: float, axis: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation that rotates around a specified axis by a specified angle.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> - `axis`: The axis to rotate around.
> 
> **Returns**: A quaternion representing the rotation.
<pre class="language-typescript"><code class="lang-typescript">function Angle(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> float</code></pre>
> Calculates the angle between two rotations.
> 
> **Parameters**:
> - `a`: The first rotation.
> - `b`: The second rotation.
> 
> **Returns**: The angle in degrees.

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
