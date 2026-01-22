# Transform
Inherits from [Object](../objects/Object.md)

Represents a transform.

### Remarks
Overloads operators: 
`==`, `__Hash__`
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Position|[Vector3](../Collections/Vector3.md)|False|Gets or sets the position of the transform.|
|LocalPosition|[Vector3](../Collections/Vector3.md)|False|Gets or sets the local position of the transform.|
|Rotation|[Vector3](../Collections/Vector3.md)|False|Gets or sets the rotation of the transform.|
|LocalRotation|[Vector3](../Collections/Vector3.md)|False|Gets or sets the local rotation of the transform.|
|QuaternionRotation|[Quaternion](../Collections/Quaternion.md)|False|Gets or sets the quaternion rotation of the transform.|
|QuaternionLocalRotation|[Quaternion](../Collections/Quaternion.md)|False|Gets or sets the quaternion local rotation of the transform.|
|Scale|[Vector3](../Collections/Vector3.md)|False|Gets or sets the scale of the transform.|
|Forward|[Vector3](../Collections/Vector3.md)|False|Gets the forward vector of the transform.|
|Up|[Vector3](../Collections/Vector3.md)|False|Gets the up vector of the transform.|
|Right|[Vector3](../Collections/Vector3.md)|False|Gets the right vector of the transform.|
|Parent|[Transform](../Entities/Transform.md)|False|Sets the parent of the transform.|
|Name|string|True|Gets the name of the transform.|
|Layer|int|False|Gets the Physics Layer of the transform.|
|Animation|[Animation](../Component/Animation.md)|True|The Animation attached to this transform, returns null if there is none.|
|Animator|[Animator](../Component/Animator.md)|True|The Animator attached to this transform, returns null if there is none.|
|AudioSource|[AudioSource](../Component/AudioSource.md)|True|The AudioSource attached to this transform, returns null if there is none.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-29">Transform</a></code></pre>
> Gets the transform of the specified child.
> 
> **Parameters**:
> - `name`: The name of the child transform to find.
> 
> **Returns**: The child transform if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetTransforms() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-29">Transform</a>></code></pre>
> Gets all child transforms.
> 
> **Returns**: A list of all child transforms.
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingAnimation(anim: string) -> bool</code></pre>
> Checks if the given animation is playing.
> 
> **Parameters**:
> - `anim`: The name of the animation to check. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: True if the animation is playing, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(anim: string, fade: float = 0.1)</code></pre>
> Plays the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation to play. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> - `fade`: The fade time in seconds for cross-fading (default: 0.1).
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(anim: string) -> float</code></pre>
> Gets the length of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: The length of the animation in seconds, or -1 if not found.
<pre class="language-typescript"><code class="lang-typescript">function PlaySound()</code></pre>
> Plays the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound()</code></pre>
> Stops the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToggleParticle(enabled: bool)</code></pre>
> Toggles the particle system.
> 
> **Parameters**:
> - `enabled`: Whether to enable or disable the particle emission.
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformDirection(direction: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Transforms a direction from world space to local space.
> 
> **Parameters**:
> - `direction`: The direction vector in world space.
> 
> **Returns**: The direction in local space.
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformPoint(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Transforms a position from world space to local space.
> 
> **Parameters**:
> - `point`: The point in world space.
> 
> **Returns**: The position in local space.
<pre class="language-typescript"><code class="lang-typescript">function TransformDirection(direction: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Transforms a direction from local space to world space.
> 
> **Parameters**:
> - `direction`: The direction vector in local space.
> 
> **Returns**: The direction in world space.
<pre class="language-typescript"><code class="lang-typescript">function TransformPoint(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Transforms a position from local space to world space.
> 
> **Parameters**:
> - `point`: The point in local space.
> 
> **Returns**: The position in world space.
<pre class="language-typescript"><code class="lang-typescript">function Rotate(rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Rotates the transform by the given rotation in euler angles.
> 
> **Parameters**:
> - `rotation`: The rotation in euler angles to apply.
> 
<pre class="language-typescript"><code class="lang-typescript">function RotateAround(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, angle: float)</code></pre>
> Rotates the transform around a point by the given angle.
> 
> **Parameters**:
> - `point`: The point to rotate around.
> - `axis`: The axis to rotate around.
> - `angle`: The angle in degrees to rotate.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(target: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Rotates the transform to look at the target position.
> 
> **Parameters**:
> - `target`: The world position to look at.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetRenderersEnabled(enabled: bool)</code></pre>
> Sets the enabled state of all child renderers.
> 
> **Parameters**:
> - `enabled`: Whether to enable or disable the renderers.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetColliders(recursive: bool = False) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-13">Collider</a>></code></pre>
> Gets colliders of the transform.
> 
> **Parameters**:
> - `recursive`: If true, includes colliders from all children recursively (default: false).
> 
> **Returns**: A list of colliders.

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
