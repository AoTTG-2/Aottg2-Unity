# Transform
Inherits from object
## Initialization
```csharp
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Position|[Vector3](../Static/Vector3.md)|False|Gets or sets the position of the transform.|
|LocalPosition|[Vector3](../Static/Vector3.md)|False|Gets or sets the local position of the transform.|
|Rotation|[Vector3](../Static/Vector3.md)|False|Gets or sets the rotation of the transform.|
|LocalRotation|[Vector3](../Static/Vector3.md)|False|Gets or sets the local rotation of the transform.|
|QuaternionRotation|[Quaternion](../Static/Quaternion.md)|False|Gets or sets the quaternion rotation of the transform.|
|QuaternionLocalRotation|[Quaternion](../Static/Quaternion.md)|False|Gets or sets the quaternion local rotation of the transform.|
|Scale|[Vector3](../Static/Vector3.md)|False|Gets or sets the scale of the transform.|
|Forward|[Vector3](../Static/Vector3.md)|False|Gets the forward vector of the transform.|
|Up|[Vector3](../Static/Vector3.md)|False|Gets the up vector of the transform.|
|Right|[Vector3](../Static/Vector3.md)|False|Gets the right vector of the transform.|
## Methods
|Function|Returns|Description|
|---|---|---|
|GetTransform(name : [String](../Static/String.md))|[Transform](../Object/Transform.md)|Gets the transform of the specified child.|
|GetTransforms()|[List](../Object/List.md)|Gets all child transforms.|
|PlayAnimation(anim : [String](../Static/String.md), fade : float = 0.1)|none|Plays the specified animation.|
|GetAnimationLength(anim : [String](../Static/String.md))|float|Gets the length of the specified animation.|
|PlaySound()|none|Plays the sound.|
|StopSound()|none|Stops the sound.|
|ToggleParticle(enabled : bool)|none|Toggles the particle system.|
|InverseTransformDirection(direction : [Vector3](../Static/Vector3.md))|[Vector3](../Static/Vector3.md)|Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.|
|InverseTransformPoint(point : [Vector3](../Static/Vector3.md))|[Vector3](../Static/Vector3.md)|Transforms position from world space to local space.|
|TransformDirection(direction : [Vector3](../Static/Vector3.md))|[Vector3](../Static/Vector3.md)|Transforms direction from local space to world space.|
|TransformPoint(point : [Vector3](../Static/Vector3.md))|[Vector3](../Static/Vector3.md)|Transforms position from local space to world space.|
|Rotate(rotation : [Vector3](../Static/Vector3.md))|none|Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).|
|RotateAround(point : [Vector3](../Static/Vector3.md), axis : [Vector3](../Static/Vector3.md), angle : float)|none|Rotates the transform about axis passing through point in world coordinates by angle degrees.|
|LookAt(target : [Vector3](../Static/Vector3.md))|none|Rotates the transform so the forward vector points at worldPosition.|
|SetRenderersEnabled(enabled : bool)|none|Sets the enabled state of all child renderers.|
