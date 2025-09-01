# RigidbodyBuiltin
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|OwnerMapObjectBuiltin|[MapObject](../objects/MapObject.md)|True|The MapObject this rigidbody is attached to.|
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
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, forceMode: string = "Acceleration", atPoint: <a data-footnote-ref href="#user-content-fn-43">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody - legacy version, please use optimized if possible.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForceOptimized(force: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, forceMode: int = 5, atPoint: <a data-footnote-ref href="#user-content-fn-43">Vector3</a> = null)</code></pre>
> Apply a force to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorque(torque: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, forceMode: string)</code></pre>
> Apply a torque to the Rigidbody - legacy version, please use optimized if possible.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddTorqueOptimized(torque: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, forceMode: int = 5)</code></pre>
> Apply a torque to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddExplosionForce(explosionForce: float, explosionPosition: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, explosionRadius: float, upwardsModifier: float = 0, forceMode: int = 5)</code></pre>
> Apply an explosion force to the Rigidbody.
> 
<pre class="language-typescript"><code class="lang-typescript">function Move(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new position. This will not apply any forces, it will just set the position directly.
> 
<pre class="language-typescript"><code class="lang-typescript">function MovePosition(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Move the Rigidbody to a new position. This will apply forces to move the Rigidbody to the new position.
> 
<pre class="language-typescript"><code class="lang-typescript">function MoveRotation(rotation: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>)</code></pre>
> Move the Rigidbody to a new rotation. This will apply forces to rotate the Rigidbody to the new rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function ResetCenterOfMass()</code></pre>
> Reset the center of mass of the Rigidbody to the default value (0, 0, 0). This will also reset the inertia tensor.
> 
<pre class="language-typescript"><code class="lang-typescript">function PublishTransform()</code></pre>
> Publish the current position and rotation of the Rigidbody to the MapObject. This will update the MapObject's transform to match the Rigidbody's transform.
> 

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
