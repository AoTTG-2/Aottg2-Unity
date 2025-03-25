# Camera
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsManual|bool|False|Is camera in manual mode.|
|Position|[Vector3](../objects/Vector3.md)|False|Position of the camera.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Rotation of the camera.|
|Velocity|[Vector3](../objects/Vector3.md)|False|Velocity of the camera.|
|FOV|float|False|Field of view of the camera.|
|CameraMode|[String](../static/String.md)|False|Current camera mode.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward vector of the camera.|
|Right|[Vector3](../objects/Vector3.md)|False|Right vector of the camera.|
|Up|[Vector3](../objects/Vector3.md)|False|Up vector of the camera.|
|FollowDistance|float|False|Distance from the camera to the character.|
## Static Methods
#### function <mark style="color:yellow;">SetManual</mark>(manual: <mark style="color:blue;">bool</mark>) -> <mark style="color:blue;">void</mark>
> Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.

#### function <mark style="color:yellow;">SetPosition</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets camera position.

#### function <mark style="color:yellow;">SetRotation</mark>(rotation: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets camera rotation.

#### function <mark style="color:yellow;">SetVelocity</mark>(velocity: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets camera velocity.

#### function <mark style="color:yellow;">LookAt</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the camera forward direction such that it is looking at a world position.

#### function <mark style="color:yellow;">SetFOV</mark>(fov: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">void</mark>
> Sets the camera field of view. Use 0 to use the default field of view.

#### function <mark style="color:yellow;">SetCameraMode</mark>(mode: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Forces the player to use a certain camera mode, taking priority over their camera setting. Accepted values are TPS, Original, FPS.

#### function <mark style="color:yellow;">ResetDistance</mark>() -> <mark style="color:blue;">void</mark>
> Resets the follow distance to player's settings.

#### function <mark style="color:yellow;">ResetCameraMode</mark>() -> <mark style="color:blue;">void</mark>
> Resets the camera mode to player's settings.


---

