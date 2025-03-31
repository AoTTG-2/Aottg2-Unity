# LineCastHitResult
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsCharacter|bool|True|true if the linecast hit a character|
|IsMapObject|bool|True|true if the linecast hit a map object|
|Distance|float|True|The distance to the hit point|
|Point|[Vector3](../objects/Vector3.md)|True|The point in world space where the linecast hit|
|Normal|[Vector3](../objects/Vector3.md)|True|The normal of the surface the linecast hit|
|Collider|[Collider](../objects/Collider.md)|True|The collider that was hit|
