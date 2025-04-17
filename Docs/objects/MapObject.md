# MapObject
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

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
###### function <mark style="color:yellow;">AddComponent</mark>(name: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Component</mark>
> Add a component to the object

###### function <mark style="color:yellow;">RemoveComponent</mark>(name: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Remove a component from the object

###### function <mark style="color:yellow;">GetComponent</mark>(name: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Component</mark>
> Get a component from the object

###### function <mark style="color:yellow;">SetComponentEnabled</mark>(name: <mark style="color:blue;">string</mark>, enabled: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Set whether a component is enabled

###### function <mark style="color:yellow;">SetComponentsEnabled</mark>(enabled: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Set whether all components are enabled

###### function <mark style="color:yellow;">AddSphereCollider</mark>(collideMode: <mark style="color:blue;">string</mark>, collideWith: <mark style="color:blue;">string</mark>, center: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, radius: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">null</mark>
> Add a sphere collider to the object

###### function <mark style="color:yellow;">AddBoxCollider</mark>(collideMode: <mark style="color:blue;">string</mark>, collideWith: <mark style="color:blue;">string</mark>, center: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark> = <mark style="color:blue;">null</mark>, size: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark> = <mark style="color:blue;">null</mark>) → <mark style="color:blue;">null</mark>
> Add a box collider to the object

###### function <mark style="color:yellow;">AddSphereTarget</mark>(team: <mark style="color:blue;">string</mark>, center: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, radius: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</mark>
> Add a sphere target to the object

###### function <mark style="color:yellow;">AddBoxTarget</mark>(team: <mark style="color:blue;">string</mark>, center: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, size: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</mark>
> Add a box target to the object

###### function <mark style="color:yellow;">GetChild</mark>(name: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Get a child object by name

###### function <mark style="color:yellow;">GetChildren</mark>() → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Get all child objects

###### function <mark style="color:yellow;">GetTransform</mark>(name: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[Transform](../objects/Transform.md)</mark>
> Get a child transform by name

###### function <mark style="color:yellow;">SetColorAll</mark>(color: <mark style="color:blue;">[Color](../objects/Color.md)</mark>) → <mark style="color:blue;">null</mark>
> Set the color of all renderers on the object

###### function <mark style="color:yellow;">InBounds</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if a position is within the object's bounds

###### function <mark style="color:yellow;">GetBoundsAverageCenter</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds average center

###### function <mark style="color:yellow;">GetBoundsCenter</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds center

###### function <mark style="color:yellow;">GetBoundsSize</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds size

###### function <mark style="color:yellow;">GetBoundsMin</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds min

###### function <mark style="color:yellow;">GetBoundsMax</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds max

###### function <mark style="color:yellow;">GetBoundsExtents</mark>() → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Get the bounds extents

###### function <mark style="color:yellow;">GetCorners</mark>() → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Get the corners of the bounds

###### function <mark style="color:yellow;">AddBuiltinComponent</mark>(componentName: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter1: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter2: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter3: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter4: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>) → <mark style="color:blue;">null</mark>
> Add a builtin component to the object.             Components: Daylight, PointLight, Tag, Rigidbody, CustomPhysicsMaterial, NavMeshObstacle

###### function <mark style="color:yellow;">HasTag</mark>(tag: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">bool</mark>
> Whether or not the object has the given tag

###### function <mark style="color:yellow;">ReadBuiltinComponent</mark>(name: <mark style="color:blue;">string</mark>, param: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Object</mark>
> Read a builtin component

###### function <mark style="color:yellow;">UpdateBuiltinComponent</mark>(componentName: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter1: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter2: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter3: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>, parameter4: <mark style="color:blue;">Object</mark> = <mark style="color:blue;">null</mark>) → <mark style="color:blue;">null</mark>
> Update a builtin component


---

