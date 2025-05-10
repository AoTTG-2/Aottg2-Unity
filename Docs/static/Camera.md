# Camera
Inherits from Object

<mark style="color:red;">This class is static and cannot be instantiated.</mark>

## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsManual|bool|True|Is camera in manual mode.|
|Position|[Vector3](../objects/Vector3.md)|True|Position of the camera.|
|Rotation|[Vector3](../objects/Vector3.md)|True|Rotation of the camera.|
|Velocity|[Vector3](../objects/Vector3.md)|True|Velocity of the camera.|
|FOV|float|True|Field of view of the camera.|
|CameraMode|string|True|Current camera mode.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward vector of the camera.|
|Right|[Vector3](../objects/Vector3.md)|False|Right vector of the camera.|
|Up|[Vector3](../objects/Vector3.md)|False|Up vector of the camera.|
|FollowDistance|float|False|Distance from the camera to the character.|
## Static Methods
###### function <mark style="color:yellow;">SetManual</mark>(manual: <mark style="color:blue;">bool</mark>)
> Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.

###### function <mark style="color:yellow;">SetPosition</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Sets camera position.

###### function <mark style="color:yellow;">SetRotation</mark>(rotation: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Sets camera rotation.

###### function <mark style="color:yellow;">SetVelocity</mark>(velocity: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Sets camera velocity.

###### function <mark style="color:yellow;">LookAt</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Sets the camera forward direction such that it is looking at a world position.

###### function <mark style="color:yellow;">SetFOV</mark>(fov: <mark style="color:blue;">float</mark>)
> Sets the camera field of view. Use 0 to use the default field of view.

###### function <mark style="color:yellow;">SetCameraMode</mark>(mode: <mark style="color:blue;">string</mark>)
> Forces the player to use a certain camera mode, taking priority over their camera setting. Accepted values are TPS, Original, FPS.

###### function <mark style="color:yellow;">ResetDistance</mark>()
> Resets the follow distance to player's settings.

###### function <mark style="color:yellow;">ResetCameraMode</mark>()
> Resets the camera mode to player's settings.


---

