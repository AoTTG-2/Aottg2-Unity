# Map
Inherits from object
## Methods
|Function|Returns|Description|
|---|---|---|
|FindAllMapObjects()|[List](../Object/List.md)|Find all map objects|
|FindMapObjectByName(objectName : [String](../Static/String.md))|[MapObject](../Object/MapObject.md)|Find a map object by name|
|FindMapObjectsByName(objectName : [String](../Static/String.md))|[List](../Object/List.md)|Find all map objects by name|
|FindMapObjectByComponent(className : [String](../Static/String.md))|[MapObject](../Object/MapObject.md)|Find all map objects by component|
|FindMapObjectsByComponent(className : [String](../Static/String.md))|[List](../Object/List.md)|Find all map objects by component|
|FindMapObjectByID(id : int)|[MapObject](../Object/MapObject.md)|Find a map object by ID|
|FindMapObjectByTag(tag : [String](../Static/String.md))|[MapObject](../Object/MapObject.md)|Find a map object by tag|
|FindMapObjectsByTag(tag : [String](../Static/String.md))|[List](../Object/List.md)|Find all map objects by tag|
|CreateMapObjectRaw(prefab : [String](../Static/String.md))|[MapObject](../Object/MapObject.md)|Create a new map object|
|DestroyMapObject(mapObject : [MapObject](../Object/MapObject.md), includeChildren : bool)|none|Destroy a map object|
|CopyMapObject(mapObject : [MapObject](../Object/MapObject.md), includeChildren : bool)|[MapObject](../Object/MapObject.md)|Copy a map object|
|DestroyMapTargetable(targetable : [MapTargetable](../Object/MapTargetable.md))|none|Destroy a map targetable|
|UpdateNavMesh()|none|Update the nav mesh|
|UpdateNavMeshAsync()|none|Update the nav mesh asynchronously|
