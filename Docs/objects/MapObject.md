# MapObject
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Static|bool|False|Object does not move|
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
|Name|[String](../static/String.md)|False|The name of the object|
|Parent|Object|False|The parent of the object|
|Active|bool|False|Whether the object is active|
|Transform|[Transform](../objects/Transform.md)|False|The transform of the object|
|HasRenderer|bool|False|Whether the object has a renderer|
|Color|[Color](../objects/Color.md)|False|The color of the object|
|TextureTilingX|float|False|The x tiling of the object's texture|
|TextureTilingY|float|False|The y tiling of the object's texture|
|TextureOffsetX|float|False|The x offset of the object's texture|
|TextureOffsetY|float|False|The y offset of the object's texture|
|ID|int|False|The ID of the object|
|Tag|[String](../static/String.md)|False|The tag of the object|
|Layer|int|False|The layer of the object|
## Methods
##### CustomLogicComponentInstance AddComponent([String](../static/String.md) name)
- **Description:** Add a component to the object
##### void RemoveComponent([String](../static/String.md) name)
- **Description:** Remove a component from the object
##### CustomLogicComponentInstance GetComponent([String](../static/String.md) name)
- **Description:** Get a component from the object
##### void SetComponentEnabled([String](../static/String.md) name, bool enabled)
- **Description:** Set whether a component is enabled
##### void SetComponentsEnabled(bool enabled)
- **Description:** Set whether all components are enabled
##### void AddSphereCollider([String](../static/String.md) collideMode, [String](../static/String.md) collideWith, [Vector3](../objects/Vector3.md) center, float radius)
- **Description:** Add a sphere collider to the object
##### void AddBoxCollider([String](../static/String.md) collideMode, [String](../static/String.md) collideWith, [Vector3](../objects/Vector3.md) center = null, [Vector3](../objects/Vector3.md) size = null)
- **Description:** Add a box collider to the object
##### [MapTargetable](../objects/MapTargetable.md) AddSphereTarget([String](../static/String.md) team, [Vector3](../objects/Vector3.md) center, float radius)
- **Description:** Add a sphere target to the object
##### [MapTargetable](../objects/MapTargetable.md) AddBoxTarget([String](../static/String.md) team, [Vector3](../objects/Vector3.md) center, [Vector3](../objects/Vector3.md) size)
- **Description:** Add a box target to the object
##### [MapObject](../objects/MapObject.md) GetChild([String](../static/String.md) name)
- **Description:** Get a child object by name
##### [List](../objects/List.md) GetChildren()
- **Description:** Get all child objects
##### [Transform](../objects/Transform.md) GetTransform([String](../static/String.md) name)
- **Description:** Get a child transform by name
##### void SetColorAll([Color](../objects/Color.md) color)
- **Description:** Set the color of all renderers on the object
##### bool InBounds([Vector3](../objects/Vector3.md) position)
- **Description:** Check if a position is within the object's bounds
##### [Vector3](../objects/Vector3.md) GetBoundsAverageCenter()
- **Description:** Get the bounds average center
##### [Vector3](../objects/Vector3.md) GetBoundsCenter()
- **Description:** Get the bounds center
##### [Vector3](../objects/Vector3.md) GetBoundsSize()
- **Description:** Get the bounds size
##### [Vector3](../objects/Vector3.md) GetBoundsMin()
- **Description:** Get the bounds min
##### [Vector3](../objects/Vector3.md) GetBoundsMax()
- **Description:** Get the bounds max
##### [Vector3](../objects/Vector3.md) GetBoundsExtents()
- **Description:** Get the bounds extents
##### [List](../objects/List.md) GetCorners()
- **Description:** Get the corners of the bounds
##### void AddBuiltinComponent(Object parameter0 = null, Object parameter1 = null, Object parameter2 = null, Object parameter3 = null, Object parameter4 = null)
- **Description:** [OBSELETE] Add builtin component
##### Object ReadBuiltinComponent([String](../static/String.md) name, [String](../static/String.md) param)
- **Description:** [OBSELETE] Read a builtin component
##### void UpdateBuiltinComponent(Object parameter0 = null, Object parameter1 = null, Object parameter2 = null, Object parameter3 = null, Object parameter4 = null)
- **Description:** [OBSELETE] Update a builtin component

---

