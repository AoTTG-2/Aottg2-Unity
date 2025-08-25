# Camera
Inherits from [Object](../objects/Object.md)

References the main game camera.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsManual|bool|True|Is camera in manual mode.|
|Position|[Vector3](../objects/Vector3.md)|True|Position of the camera.|
|Rotation|[Vector3](../objects/Vector3.md)|True|Rotation of the camera.|
|Velocity|[Vector3](../objects/Vector3.md)|True|Velocity of the camera.|
|FOV|float|True|Field of view of the camera.|
|CameraMode|string|True|Current camera mode. TPS, Original, FPS.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward vector of the camera.|
|Right|[Vector3](../objects/Vector3.md)|False|Right vector of the camera.|
|Up|[Vector3](../objects/Vector3.md)|False|Up vector of the camera.|
|FollowDistance|float|False|Distance from the camera to the character.|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetManual(manual: bool)</code></pre>
> Sets the camera manual mode.
If true, camera will only be controlled by custom logic.
If false, camera will follow the spawned or spectated player and read input.
> 
> **Parameters**:
> - `manual`: True to enable manual mode, false to disable.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPosition(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Sets camera position.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetRotation(rotation: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Sets camera rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetVelocity(velocity: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Sets camera velocity.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>)</code></pre>
> Sets the camera forward direction such that it is looking at a world position.
> 
> **Parameters**:
> - `position`: The world position to look at
> 
<pre class="language-typescript"><code class="lang-typescript">function SetFOV(fov: float)</code></pre>
> Sets the camera field of view.
> 
> **Parameters**:
> - `fov`: The new field of view. Use 0 to use the default field of view.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCameraMode(mode: string)</code></pre>
> Forces the player to use a certain camera mode,
taking priority over their camera setting.
> 
> **Parameters**:
> - `mode`: The camera mode. Accepted values are TPS, Original, FPS.
> 
<pre class="language-typescript"><code class="lang-typescript">function ResetDistance()</code></pre>
> Resets the follow distance to player's settings.
> 
<pre class="language-typescript"><code class="lang-typescript">function ResetCameraMode()</code></pre>
> Resets the camera mode to player's settings.
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
