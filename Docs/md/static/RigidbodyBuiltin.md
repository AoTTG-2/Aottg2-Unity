# RigidbodyBuiltin
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
|CollisionDetectionMode|string|False|The collision detection mode of the Rigidbody. This determines how collisions are detected and resolved.|
|DetectCollisions|bool|False|If the Rigidbody detects collisions. If false, it will not collide with other colliders.|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|ForceModeAcceleration|int|True|ForceMode.Acceleration|
|ForceModeForce|int|True|ForceMode.Force|
|ForceModeImpulse|int|True|ForceMode.Impulse|
|ForceModeVelocityChange|int|True|ForceMode.VelocityChange|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, forceMode: string = "Acceleration", atPoint: <a data-footnote-ref href="#user-content-fn-46">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody - legacy version, please use optimized if possible.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForceOptimized(force: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, forceMode: int = 5, atPoint: <a data-footnote-ref href="#user-content-fn-46">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorque(torque: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, forceMode: string)</code></pre>
> Apply a torque to the Rigidbody - legacy version, please use optimized if possible.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorqueOptimized(torque: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, forceMode: int = 5)</code></pre>
> Apply a torque to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddExplosionForce(explosionForce: float, explosionPosition: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, explosionRadius: float, upwardsModifier: float = 0, forceMode: int = 5)</code></pre>
> Apply an explosion force to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function Move(position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.
> 
<pre class="language-typescript"><code class="lang-typescript">function MovePosition(position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveRotation(rotation: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function ResetCenterOfMass()</code></pre>
> Reset the center of mass of the Rigidbody to the default value (0, 0, 0). This will also reset the inertia tensor.
> 
<pre class="language-typescript"><code class="lang-typescript">function PublishTransform()</code></pre>
> Publish the current position and rotation of the Rigidbody to the MapObject. This will update the MapObject's transform to match the Rigidbody's transform.
> 
<pre class="language-typescript"><code class="lang-typescript">function SweepTest(direction: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, distance: float) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Checks if the rigidbody would collide with anything, returns a LineCastHitResult object.
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
