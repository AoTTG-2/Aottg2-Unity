# Camera
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
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
#### function <span style="color:yellow;">SetManual</span>(manual: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.

#### function <span style="color:yellow;">SetPosition</span>(position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Sets camera position.

#### function <span style="color:yellow;">SetRotation</span>(rotation: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Sets camera rotation.

#### function <span style="color:yellow;">SetVelocity</span>(velocity: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Sets camera velocity.

#### function <span style="color:yellow;">LookAt</span>(position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Sets the camera forward direction such that it is looking at a world position.

#### function <span style="color:yellow;">SetFOV</span>(fov: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Sets the camera field of view. Use 0 to use the default field of view.

#### function <span style="color:yellow;">SetCameraMode</span>(mode: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Forces the player to use a certain camera mode, taking priority over their camera setting. Accepted values are TPS, Original, FPS.

#### function <span style="color:yellow;">ResetDistance</span>() → <span style="color:blue;">null</span>
> Resets the follow distance to player's settings.

#### function <span style="color:yellow;">ResetCameraMode</span>() → <span style="color:blue;">null</span>
> Resets the camera mode to player's settings.


---

