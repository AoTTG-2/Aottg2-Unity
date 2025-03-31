# MapObject
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Static|bool|True|Object does not move|
|Position|[Vector3](../objects/Vector3.md)|False|The position of the object|
|LocalPosition|[Vector3](../objects/Vector3.md)|False|The local position of the object|
|Rotation|[Vector3](../objects/Vector3.md)|False|The rotation of the object|
|LocalRotation|[Vector3](../objects/Vector3.md)|False|The local rotation of the object|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|The rotation of the object as a quaternion|
|QuaternionLocalRotation|[Quaternion](../objects/Quaternion.md)|False|The local rotation of the object as a quaternion|
|Forward|[Vector3](../objects/Vector3.md)|False|The forward direction of the object|
|Up|[Vector3](../objects/Vector3.md)|False|The up direction of the object|
|Right|[Vector3](../objects/Vector3.md)|False|The right direction of the object|
|Scale|[Vector3](../objects/Vector3.md)|False|The scale of the object|
|Name|string|True|The name of the object|
|Parent|Object|False|The parent of the object|
|Active|bool|False|Whether the object is active|
|Transform|[Transform](../objects/Transform.md)|True|The transform of the object|
|HasRenderer|bool|True|Whether the object has a renderer|
|Color|[Color](../objects/Color.md)|False|The color of the object|
|TextureTilingX|float|False|The x tiling of the object's texture|
|TextureTilingY|float|False|The y tiling of the object's texture|
|TextureOffsetX|float|False|The x offset of the object's texture|
|TextureOffsetY|float|False|The y offset of the object's texture|
|ID|int|True|The ID of the object|
|Tag|string|False|The tag of the object|
|Layer|int|False|The layer of the object|
## Methods
#### function <span style="color:yellow;">AddComponent</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">Component</span>
> Add a component to the object

#### function <span style="color:yellow;">RemoveComponent</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Remove a component from the object

#### function <span style="color:yellow;">GetComponent</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">Component</span>
> Get a component from the object

#### function <span style="color:yellow;">SetComponentEnabled</span>(name: <span style="color:blue;">string</span>, enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Set whether a component is enabled

#### function <span style="color:yellow;">SetComponentsEnabled</span>(enabled: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Set whether all components are enabled

#### function <span style="color:yellow;">AddSphereCollider</span>(collideMode: <span style="color:blue;">string</span>, collideWith: <span style="color:blue;">string</span>, center: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, radius: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Add a sphere collider to the object

#### function <span style="color:yellow;">AddBoxCollider</span>(collideMode: <span style="color:blue;">string</span>, collideWith: <span style="color:blue;">string</span>, center: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span> = <span style="color:blue;">null</span>, size: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span> = <span style="color:blue;">null</span>) → <span style="color:blue;">null</span>
> Add a box collider to the object

#### function <span style="color:yellow;">AddSphereTarget</span>(team: <span style="color:blue;">string</span>, center: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, radius: <span style="color:blue;">float</span>) → <span style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</span>
> Add a sphere target to the object

#### function <span style="color:yellow;">AddBoxTarget</span>(team: <span style="color:blue;">string</span>, center: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, size: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</span>
> Add a box target to the object

#### function <span style="color:yellow;">GetChild</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Get a child object by name

#### function <span style="color:yellow;">GetChildren</span>() → <span style="color:blue;">[List](../objects/List.md)</span>
> Get all child objects

#### function <span style="color:yellow;">GetTransform</span>(name: <span style="color:blue;">string</span>) → <span style="color:blue;">[Transform](../objects/Transform.md)</span>
> Get a child transform by name

#### function <span style="color:yellow;">SetColorAll</span>(color: <span style="color:blue;">[Color](../objects/Color.md)</span>) → <span style="color:blue;">null</span>
> Set the color of all renderers on the object

#### function <span style="color:yellow;">InBounds</span>(position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">bool</span>
> Check if a position is within the object's bounds

#### function <span style="color:yellow;">GetBoundsAverageCenter</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds average center

#### function <span style="color:yellow;">GetBoundsCenter</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds center

#### function <span style="color:yellow;">GetBoundsSize</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds size

#### function <span style="color:yellow;">GetBoundsMin</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds min

#### function <span style="color:yellow;">GetBoundsMax</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds max

#### function <span style="color:yellow;">GetBoundsExtents</span>() → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Get the bounds extents

#### function <span style="color:yellow;">GetCorners</span>() → <span style="color:blue;">[List](../objects/List.md)</span>
> Get the corners of the bounds

#### function <span style="color:yellow;">AddBuiltinComponent</span>(componentName: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter1: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter2: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter3: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter4: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>) → <span style="color:blue;">null</span>
> Add builtin component

#### function <span style="color:yellow;">HasTag</span>(tag: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Whether or not the object has the given tag

#### function <span style="color:yellow;">ReadBuiltinComponent</span>(name: <span style="color:blue;">string</span>, param: <span style="color:blue;">string</span>) → <span style="color:blue;">Object</span>
> Read a builtin component

#### function <span style="color:yellow;">UpdateBuiltinComponent</span>(componentName: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter1: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter2: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter3: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>, parameter4: <span style="color:blue;">Object</span> = <span style="color:blue;">null</span>) → <span style="color:blue;">null</span>
> Update a builtin component


---

