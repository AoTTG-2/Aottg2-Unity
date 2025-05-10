# Map
Inherits from Object

<mark style="color:red;">This class is static and cannot be instantiated.</mark>

> Finding, creating, and destroying map objects.
## Methods
###### function <mark style="color:yellow;">FindAllMapObjects</mark>() → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Find all map objects

###### function <mark style="color:yellow;">FindMapObjectByName</mark>(objectName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Find a map object by name

###### function <mark style="color:yellow;">FindMapObjectsByName</mark>(objectName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Find all map objects by name

###### function <mark style="color:yellow;">FindMapObjectByComponent</mark>(className: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Find all map objects by component

###### function <mark style="color:yellow;">FindMapObjectsByComponent</mark>(className: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Find all map objects by component

###### function <mark style="color:yellow;">FindMapObjectByID</mark>(id: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Find a map object by ID

###### function <mark style="color:yellow;">FindMapObjectByTag</mark>(tag: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Find a map object by tag

###### function <mark style="color:yellow;">FindMapObjectsByTag</mark>(tag: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Find all map objects by tag

###### function <mark style="color:yellow;">CreateMapObjectRaw</mark>(prefab: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Create a new map object

###### function <mark style="color:yellow;">DestroyMapObject</mark>(mapObject: <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>, includeChildren: <mark style="color:blue;">bool</mark>)
> Destroy a map object

###### function <mark style="color:yellow;">CopyMapObject</mark>(mapObject: <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>, includeChildren: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">[MapObject](../objects/MapObject.md)</mark>
> Copy a map object

###### function <mark style="color:yellow;">DestroyMapTargetable</mark>(targetable: <mark style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</mark>)
> Destroy a map targetable

###### function <mark style="color:yellow;">UpdateNavMesh</mark>()
> Update the nav mesh

###### function <mark style="color:yellow;">UpdateNavMeshAsync</mark>()
> Update the nav mesh asynchronously


---

