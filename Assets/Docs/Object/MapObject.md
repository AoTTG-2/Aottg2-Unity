# MapObject
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Static|bool|False|Object does not move|
|Position|[Vector3](../Static/Vector3.md)|False|The position of the object|
|LocalPosition|[Vector3](../Static/Vector3.md)|False|The local position of the object|
|Rotation|[Vector3](../Static/Vector3.md)|False|The rotation of the object|
|LocalRotation|[Vector3](../Static/Vector3.md)|False|The local rotation of the object|
|QuaternionRotation|[Quaternion](../Static/Quaternion.md)|False|The rotation of the object as a quaternion|
|QuaternionLocalRotation|[Quaternion](../Static/Quaternion.md)|False|The local rotation of the object as a quaternion|
|Forward|[Vector3](../Static/Vector3.md)|False|The forward direction of the object|
|Up|[Vector3](../Static/Vector3.md)|False|The up direction of the object|
|Right|[Vector3](../Static/Vector3.md)|False|The right direction of the object|
|Scale|[Vector3](../Static/Vector3.md)|False|The scale of the object|
|Name|[String](../Static/String.md)|False|The name of the object|
|Parent|Object|False|The parent of the object|
|Active|bool|False|Whether the object is active|
|Transform|[Transform](../Object/Transform.md)|False|The transform of the object|
|HasRenderer|bool|False|Whether the object has a renderer|
|Color|[Color](../Static/Color.md)|False|The color of the object|
|TextureTilingX|float|False|The x tiling of the object's texture|
|TextureTilingY|float|False|The y tiling of the object's texture|
|TextureOffsetX|float|False|The x offset of the object's texture|
|TextureOffsetY|float|False|The y offset of the object's texture|
|ID|int|False|The ID of the object|
|Tag|[String](../Static/String.md)|False|The tag of the object|
|Layer|int|False|The layer of the object|
## Methods
|Function|Returns|Description|
|---|---|---|
|AddComponent(name : [String](../Static/String.md))|CustomLogicComponentInstance|Add a component to the object|
|RemoveComponent(name : [String](../Static/String.md))|none|Remove a component from the object|
|GetComponent(name : [String](../Static/String.md))|CustomLogicComponentInstance|Get a component from the object|
|SetComponentEnabled(name : [String](../Static/String.md), enabled : bool)|none|Set whether a component is enabled|
|SetComponentsEnabled(enabled : bool)|none|Set whether all components are enabled|
|AddSphereCollider(collideMode : [String](../Static/String.md), collideWith : [String](../Static/String.md), center : [Vector3](../Static/Vector3.md), radius : float)|none|Add a sphere collider to the object|
|AddBoxCollider(collideMode : [String](../Static/String.md), collideWith : [String](../Static/String.md), center : [Vector3](../Static/Vector3.md) = , size : [Vector3](../Static/Vector3.md) = )|none|Add a box collider to the object|
|AddSphereTarget(team : [String](../Static/String.md), center : [Vector3](../Static/Vector3.md), radius : float)|[MapTargetable](../Object/MapTargetable.md)|Add a sphere target to the object|
|AddBoxTarget(team : [String](../Static/String.md), center : [Vector3](../Static/Vector3.md), size : [Vector3](../Static/Vector3.md))|[MapTargetable](../Object/MapTargetable.md)|Add a box target to the object|
|GetChild(name : [String](../Static/String.md))|[MapObject](../Object/MapObject.md)|Get a child object by name|
|GetChildren()|[List](../Object/List.md)|Get all child objects|
|GetTransform(name : [String](../Static/String.md))|[Transform](../Object/Transform.md)|Get a child transform by name|
|SetColorAll(color : [Color](../Static/Color.md))|none|Set the color of all renderers on the object|
|InBounds(position : [Vector3](../Static/Vector3.md))|bool|Check if a position is within the object's bounds|
|GetBoundsAverageCenter()|[Vector3](../Static/Vector3.md)|Get the bounds average center|
|GetBoundsCenter()|[Vector3](../Static/Vector3.md)|Get the bounds center|
|GetBoundsSize()|[Vector3](../Static/Vector3.md)|Get the bounds size|
|GetBoundsMin()|[Vector3](../Static/Vector3.md)|Get the bounds min|
|GetBoundsMax()|[Vector3](../Static/Vector3.md)|Get the bounds max|
|GetBoundsExtents()|[Vector3](../Static/Vector3.md)|Get the bounds extents|
|GetCorners()|[List](../Object/List.md)|Get the corners of the bounds|
|AddBuiltinComponent(parameter0 : Object = , parameter1 : Object = , parameter2 : Object = , parameter3 : Object = , parameter4 : Object = )|none|[OBSELETE] Add builtin component|
|ReadBuiltinComponent(name : [String](../Static/String.md), param : [String](../Static/String.md))|Object|[OBSELETE] Read a builtin component|
|UpdateBuiltinComponent(parameter0 : Object = , parameter1 : Object = , parameter2 : Object = , parameter3 : Object = , parameter4 : Object = )|none|[OBSELETE] Update a builtin component|
