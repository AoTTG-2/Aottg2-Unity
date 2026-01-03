# RigidbodyBuiltin

Represents a Rigidbody component that enables physics simulation for game objects.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Owner|BuiltinClassInstance|True|The MapObject this rigidbody is attached to.|
|Position|[Vector3](../objects/Vector3.md)|False|The position of the Rigidbody in world space. This is the same as the position of the GameObject it is attached to.|
|Rotation|[Quaternion](../objects/Quaternion.md)|False|The rotation of the Rigidbody in world space. This is the same as the rotation of the GameObject it is attached to.|
|Velocity|[Vector3](../objects/Vector3.md)|False|The velocity of the Rigidbody in world space.|
|AngularVelocity|[Vector3](../objects/Vector3.md)|False|The angular velocity of the Rigidbody in world space.|
|AngularDrag|float|False|The angular damping of the Rigidbody. This is a multiplier applied to the angular velocity every frame, reducing it over time.|
|Mass|float|False|The Mass of the Rigidbody|
|UseGravity|bool|False|Whether or not the Rigidbody use gravity.|
|Gravity|[Vector3](../objects/Vector3.md)|False|The force of gravity applied to the Rigidbody. If null, the Rigidbody will use Unity's default gravity settings and will enable gravity. If Vector3 is provided, it will apply that as a custom gravity force using ConstantForce and disable gravity.|
|FreezeXPosition|bool|False|If the x movement axis is frozen|
|FreezeYPosition|bool|False|If the y movement axis is frozen|
|FreezeZPosition|bool|False|If the z movement axis is frozen|
|FreezeXRotation|bool|False|If the x rotation axis is frozen|
|FreezeYRotation|bool|False|If the y rotation axis is frozen|
|FreezeZRotation|bool|False|If the z rotation axis is frozen|
|FreezeAllRotations|bool|False|Freeze all rotations|
|FreezeAllPositions|bool|False|Freeze all positions. This will also freeze all rotations.|
|IsKinematic|bool|False|If the Rigidbody is kinematic. Kinematic bodies are not affected by forces and can only be moved manually.|
|Interpolate|bool|False|Interpolation mode of the Rigidbody. If true, it will interpolate between frames.|
|CenterOfMass|[Vector3](../objects/Vector3.md)|False|The center of mass of the Rigidbody in local space.|
|CollisionDetectionMode|string|False|The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved. Refer to [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)|
|DetectCollisions|bool|False|If the Rigidbody detects collisions. If false, it will not collide with other colliders.|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ForceModeAcceleration|int|True|ForceMode.Acceleration|
|ForceModeForce|int|True|ForceMode.Force|
|ForceModeImpulse|int|True|ForceMode.Impulse|
|ForceModeVelocityChange|int|True|ForceMode.VelocityChange|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: string = "Acceleration", atPoint: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody - legacy version, please use optimized if possible.
> 
> **Parameters**:
> - `force`: The force vector to apply.
> - `forceMode`: The force mode. Refer to [ForceModeEnum](../static/ForceModeEnum.md)
> - `atPoint`: Optional. If provided, applies force at this world position instead of center of mass.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForceOptimized(force: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: int = 5, atPoint: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody.
> 
> **Parameters**:
> - `force`: The force vector to apply.
> - `forceMode`: The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).
> - `atPoint`: Optional. If provided, applies force at this world position instead of center of mass.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorque(torque: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: string)</code></pre>
> Apply a torque to the Rigidbody - legacy version, please use optimized if possible.
> 
> **Parameters**:
> - `torque`: The torque vector to apply.
> - `forceMode`: The force mode. Refer to [ForceModeEnum](../static/ForceModeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorqueOptimized(torque: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, forceMode: int = 5)</code></pre>
> Apply a torque to the Rigidbody.
> 
> **Parameters**:
> - `torque`: The torque vector to apply.
> - `forceMode`: The force mode as integer (use ForceModeAcceleration, ForceModeForce, ForceModeImpulse, or ForceModeVelocityChange constants, default: 5).
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
> Reset the center of mass of the Rigidbody to the default value (0, 0, 0). This will also reset the inertia tensor.
> 
<pre class="language-typescript"><code class="lang-typescript">function PublishTransform()</code></pre>
> Publish the current position and rotation of the Rigidbody to the MapObject. This will update the MapObject's transform to match the Rigidbody's transform.
> 
<pre class="language-typescript"><code class="lang-typescript">function SweepTest(direction: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, distance: float) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Checks if the rigidbody would collide with anything, returns a LineCastHitResult object.
> 
> **Parameters**:
> - `direction`: The direction to sweep in.
> - `distance`: The distance to sweep.
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
