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
