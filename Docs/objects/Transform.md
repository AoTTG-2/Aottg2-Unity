# Transform
Inherits from object
## Initialization
```csharp
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Position|[Vector3](../objects/Vector3.md)|False|Gets or sets the position of the transform.|
|LocalPosition|[Vector3](../objects/Vector3.md)|False|Gets or sets the local position of the transform.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Gets or sets the rotation of the transform.|
|LocalRotation|[Vector3](../objects/Vector3.md)|False|Gets or sets the local rotation of the transform.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Gets or sets the quaternion rotation of the transform.|
|QuaternionLocalRotation|[Quaternion](../objects/Quaternion.md)|False|Gets or sets the quaternion local rotation of the transform.|
|Scale|[Vector3](../objects/Vector3.md)|False|Gets or sets the scale of the transform.|
|Forward|[Vector3](../objects/Vector3.md)|False|Gets the forward vector of the transform.|
|Up|[Vector3](../objects/Vector3.md)|False|Gets the up vector of the transform.|
|Right|[Vector3](../objects/Vector3.md)|False|Gets the right vector of the transform.|
## Methods
#### [Transform](../objects/Transform.md) GetTransform([String](../static/String.md) name)
- **Description:** Gets the transform of the specified child.

---

#### [List](../objects/List.md) GetTransforms()
- **Description:** Gets all child transforms.

---

#### void PlayAnimation([String](../static/String.md) anim, float fade = 0.1)
- **Description:** Plays the specified animation.

---

#### float GetAnimationLength([String](../static/String.md) anim)
- **Description:** Gets the length of the specified animation.

---

#### void PlaySound()
- **Description:** Plays the sound.

---

#### void StopSound()
- **Description:** Stops the sound.

---

#### void ToggleParticle(bool enabled)
- **Description:** Toggles the particle system.

---

#### [Vector3](../objects/Vector3.md) InverseTransformDirection([Vector3](../objects/Vector3.md) direction)
- **Description:** Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.

---

#### [Vector3](../objects/Vector3.md) InverseTransformPoint([Vector3](../objects/Vector3.md) point)
- **Description:** Transforms position from world space to local space.

---

#### [Vector3](../objects/Vector3.md) TransformDirection([Vector3](../objects/Vector3.md) direction)
- **Description:** Transforms direction from local space to world space.

---

#### [Vector3](../objects/Vector3.md) TransformPoint([Vector3](../objects/Vector3.md) point)
- **Description:** Transforms position from local space to world space.

---

#### void Rotate([Vector3](../objects/Vector3.md) rotation)
- **Description:** Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).

---

#### void RotateAround([Vector3](../objects/Vector3.md) point, [Vector3](../objects/Vector3.md) axis, float angle)
- **Description:** Rotates the transform about axis passing through point in world coordinates by angle degrees.

---

#### void LookAt([Vector3](../objects/Vector3.md) target)
- **Description:** Rotates the transform so the forward vector points at worldPosition.

---

#### void SetRenderersEnabled(bool enabled)
- **Description:** Sets the enabled state of all child renderers.
