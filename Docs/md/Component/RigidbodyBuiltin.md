# RigidbodyBuiltin

Represents a Rigidbody component that enables physics simulation for game objects.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Owner|BuiltinClassInstance|True|The MapObject this rigidbody is attached to.|
|Position|[Vector3](../Collections/Vector3.md)|False|The position of the Rigidbody in world space. This is the same as the position of the GameObject it is attached to.|
|Rotation|[Quaternion](../Collections/Quaternion.md)|False|The rotation of the Rigidbody in world space. This is the same as the rotation of the GameObject it is attached to.|
|Velocity|[Vector3](../Collections/Vector3.md)|False|The velocity of the Rigidbody in world space.|
|AngularVelocity|[Vector3](../Collections/Vector3.md)|False|The angular velocity of the Rigidbody in world space.|
|AngularDrag|float|False|The angular damping of the Rigidbody. This is a multiplier applied to the angular velocity every frame, reducing it over time.|
|Mass|float|False|The Mass of the Rigidbody.|
|UseGravity|bool|False|Whether or not the Rigidbody use gravity.|
|Gravity|[Vector3](../Collections/Vector3.md)|False|The force of gravity applied to the Rigidbody. If null, the Rigidbody will use Unity's default gravity settings and will enable gravity. If Vector3 is provided, it will apply that as a custom gravity force using ConstantForce and disable gravity.|
|FreezeXPosition|bool|False|If the x movement axis is frozen.|
|FreezeYPosition|bool|False|If the y movement axis is frozen.|
|FreezeZPosition|bool|False|If the z movement axis is frozen.|
|FreezeXRotation|bool|False|If the x rotation axis is frozen.|
|FreezeYRotation|bool|False|If the y rotation axis is frozen.|
|FreezeZRotation|bool|False|If the z rotation axis is frozen.|
|FreezeAllRotations|bool|False|Freeze all rotations.|
|FreezeAllPositions|bool|False|Freeze all positions. This will also freeze all rotations.|
|IsKinematic|bool|False|If the Rigidbody is kinematic. Kinematic bodies are not affected by forces and can only be moved manually.|
|Interpolate|bool|False|Interpolation mode of the Rigidbody. If true, it will interpolate between frames.|
|CenterOfMass|[Vector3](../Collections/Vector3.md)|False|The center of mass of the Rigidbody in local space.|
|CollisionDetectionMode|int|False|The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved. Refer to [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)|
|DetectCollisions|bool|False|If the Rigidbody detects collisions. If false, it will not collide with other colliders.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: int, atPoint: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody.
> 
> **Parameters**:
> - `force`: The force vector to apply.
> - `forceMode`: The force mode. Refer to [ForceModeEnum](../Enums/ForceModeEnum.md)
> - `atPoint`: Optional. If provided, applies force at this world position instead of center of mass.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorque(torque: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: int)</code></pre>
> Apply a torque to the Rigidbody.
> 
> **Parameters**:
> - `torque`: The torque vector to apply.
> - `forceMode`: The force mode. Refer to [ForceModeEnum](../Enums/ForceModeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function AddExplosionForce(explosionForce: float, explosionPosition: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, explosionRadius: float, upwardsModifier: float = 0, forceMode: int = 5)</code></pre>
> Apply an explosion force to the Rigidbody.
> 
> **Parameters**:
> - `explosionForce`: The force of the explosion.
> - `explosionPosition`: The center position of the explosion.
> - `explosionRadius`: The radius of the explosion.
> - `upwardsModifier`: Adjustment to the apparent position of the explosion to make it seem to lift objects (default: 0.0).
> - `forceMode`: The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).
> 
<pre class="language-typescript"><code class="lang-typescript">function Move(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.
> 
> **Parameters**:
> - `position`: The new position.
> - `rotation`: The new rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function MovePosition(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.
> 
> **Parameters**:
> - `position`: The target position.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveRotation(rotation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.
> 
> **Parameters**:
> - `rotation`: The target rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function ResetCenterOfMass()</code></pre>
> Reset the center of mass of the Rigidbody to the default value (0, 0, 0).
This will also reset the inertia tensor.
> 
<pre class="language-typescript"><code class="lang-typescript">function PublishTransform()</code></pre>
> Publish the current position and rotation of the Rigidbody to the MapObject.
This will update the MapObject's transform to match the Rigidbody's transform.
> 
<pre class="language-typescript"><code class="lang-typescript">function SweepTest(direction: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, distance: float) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Checks if the rigidbody would collide with anything, returns a LineCastHitResult object.
> 
> **Parameters**:
> - `direction`: The direction to sweep in.
> - `distance`: The distance to sweep.
> 
> **Returns**: A LineCastHitResult if collision detected, null otherwise.

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
