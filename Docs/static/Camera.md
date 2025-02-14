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
##### void SetManual(bool manual)
- **Description:** Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.
##### void SetPosition([Vector3](../objects/Vector3.md) position)
- **Description:** Sets camera position.
##### void SetRotation([Vector3](../objects/Vector3.md) rotation)
- **Description:** Sets camera rotation.
##### void SetVelocity([Vector3](../objects/Vector3.md) velocity)
- **Description:** Sets camera velocity.
##### void LookAt([Vector3](../objects/Vector3.md) position)
- **Description:** Sets the camera forward direction such that it is looking at a world position.
##### void SetFOV(float fov)
- **Description:** Sets the camera field of view. Use 0 to use the default field of view.
##### void SetCameraMode([String](../static/String.md) mode)
- **Description:** Forces the player to use a certain camera mode, taking priority over their camera setting. Accepted values are TPS, Original, FPS.
##### void ResetDistance()
- **Description:** Resets the follow distance to player's settings.
##### void ResetCameraMode()
- **Description:** Resets the camera mode to player's settings.

---

