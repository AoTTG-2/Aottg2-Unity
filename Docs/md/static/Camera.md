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
<pre class="language-typescript"><code class="lang-typescript">function SetPosition(position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Sets camera position.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetRotation(rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Sets camera rotation.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetVelocity(velocity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
> Sets camera velocity.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function SetCameraLocked(locked: bool)</code></pre>
> Locks or unlocks the camera to prevent or allow camera movement.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCursorVisible(visible: bool)</code></pre>
> Sets the visibility of the cursor.
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
