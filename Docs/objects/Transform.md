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
<td>GetTransform(name : [String](../static/String.md))</td>
<td>[Transform](../objects/Transform.md)</td>
<td>Gets the transform of the specified child.</td>
</tr>
<tr>
<td>GetTransforms()</td>
<td>[List](../objects/List.md)</td>
<td>Gets all child transforms.</td>
</tr>
<tr>
<td>PlayAnimation(anim : [String](../static/String.md),fade : float = 0.1)</td>
<td>none</td>
<td>Plays the specified animation.</td>
</tr>
<tr>
<td>GetAnimationLength(anim : [String](../static/String.md))</td>
<td>float</td>
<td>Gets the length of the specified animation.</td>
</tr>
<tr>
<td>PlaySound()</td>
<td>none</td>
<td>Plays the sound.</td>
</tr>
<tr>
<td>StopSound()</td>
<td>none</td>
<td>Stops the sound.</td>
</tr>
<tr>
<td>ToggleParticle(enabled : bool)</td>
<td>none</td>
<td>Toggles the particle system.</td>
</tr>
<tr>
<td>InverseTransformDirection(direction : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.</td>
</tr>
<tr>
<td>InverseTransformPoint(point : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Transforms position from world space to local space.</td>
</tr>
<tr>
<td>TransformDirection(direction : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Transforms direction from local space to world space.</td>
</tr>
<tr>
<td>TransformPoint(point : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Transforms position from local space to world space.</td>
</tr>
<tr>
<td>Rotate(rotation : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).</td>
</tr>
<tr>
<td>RotateAround(point : [Vector3](../objects/Vector3.md),axis : [Vector3](../objects/Vector3.md),angle : float)</td>
<td>none</td>
<td>Rotates the transform about axis passing through point in world coordinates by angle degrees.</td>
</tr>
<tr>
<td>LookAt(target : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Rotates the transform so the forward vector points at worldPosition.</td>
</tr>
<tr>
<td>SetRenderersEnabled(enabled : bool)</td>
<td>none</td>
<td>Sets the enabled state of all child renderers.</td>
</tr>
</tbody>
</table>
