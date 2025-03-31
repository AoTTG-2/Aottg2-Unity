# Transform
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
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
#### function <span style="color:yellow;">GetTransform</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">[Transform](../objects/Transform.md)</span>
> Gets the transform of the specified child.

#### function <span style="color:yellow;">GetTransforms</span>() → <span style="color:blue;">[List](../objects/List.md)</span>
> Gets all child transforms.

#### function <span style="color:yellow;">PlayAnimation</span>(anim: <span style="color:blue;">string</span>, fade: <span style="color:blue;">float</span> = <span style="color:blue;">0.1</span>) → <span style="color:blue;">null</span>
> Plays the specified animation.

#### function <span style="color:yellow;">GetAnimationLength</span>(anim: <span style="color:blue;">string</span>) → <span style="color:blue;">float</span>
> Gets the length of the specified animation.

#### function <span style="color:yellow;">PlaySound</span>() → <span style="color:blue;">null</span>
> Plays the sound.

#### function <span style="color:yellow;">StopSound</span>() → <span style="color:blue;">null</span>
> Stops the sound.

#### function <span style="color:yellow;">ToggleParticle</span>(enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Toggles the particle system.

#### function <span style="color:yellow;">InverseTransformDirection</span>(direction: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.

#### function <span style="color:yellow;">InverseTransformPoint</span>(point: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Transforms position from world space to local space.

#### function <span style="color:yellow;">TransformDirection</span>(direction: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Transforms direction from local space to world space.

#### function <span style="color:yellow;">TransformPoint</span>(point: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Transforms position from local space to world space.

#### function <span style="color:yellow;">Rotate</span>(rotation: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).

#### function <span style="color:yellow;">RotateAround</span>(point: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, axis: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, angle: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Rotates the transform about axis passing through point in world coordinates by angle degrees.

#### function <span style="color:yellow;">LookAt</span>(target: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">null</span>
> Rotates the transform so the forward vector points at worldPosition.

#### function <span style="color:yellow;">SetRenderersEnabled</span>(enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Sets the enabled state of all child renderers.

#### function <span style="color:yellow;">\_\_Eq\_\_</span>(self: <span style="color:blue;">Object</span>, other: <span style="color:blue;">Object</span>) → <span style="color:blue;">bool</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>

#### function <span style="color:yellow;">\_\_Hash\_\_</span>() → <span style="color:blue;">int</span>
> <span style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</span>


---

