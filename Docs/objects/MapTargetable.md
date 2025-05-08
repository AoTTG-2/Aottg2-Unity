# MapTargetable
Inherits from Object

## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

> MapTargetable object returned from MapObject.AddTarget method.             Creating a map targetable is similar to adding a collider to the MapObject,             except this collider can be targeted by AI such as titans.             Map targetables that are on a different team than the AI will be targeted by the titan,             and will trigger the OnGetHit callback on the attached MapObject.
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Team|string|False|The team of the targetable|
|Position|[Vector3](../objects/Vector3.md)|True|The position of the targetable|
|Enabled|bool|False|Is the targetable enabled|
