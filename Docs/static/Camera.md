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
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>SetManual(manual : bool)</td>
<td>none</td>
<td>Sets the camera manual mode. If true, camera will only be controlled by custom logic. If false, camera will follow the spawned or spectated player and read input.</td>
</tr>
<tr>
<td>SetPosition(position : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Sets camera position.</td>
</tr>
<tr>
<td>SetRotation(rotation : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Sets camera rotation.</td>
</tr>
<tr>
<td>SetVelocity(velocity : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Sets camera velocity.</td>
</tr>
<tr>
<td>LookAt(position : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Sets the camera forward direction such that it is looking at a world position.</td>
</tr>
<tr>
<td>SetFOV(fov : float)</td>
<td>none</td>
<td>Sets the camera field of view. Use 0 to use the default field of view.</td>
</tr>
<tr>
<td>SetCameraMode(mode : [String](../static/String.md))</td>
<td>none</td>
<td>Forces the player to use a certain camera mode, taking priority over their camera setting. Accepted values are TPS, Original, FPS.</td>
</tr>
<tr>
<td>ResetDistance()</td>
<td>none</td>
<td>Resets the follow distance to player's settings.</td>
</tr>
<tr>
<td>ResetCameraMode()</td>
<td>none</td>
<td>Resets the camera mode to player's settings.</td>
</tr>
</tbody>
</table>
