# Map
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Methods
#### function <span style="color:yellow;">FindAllMapObjects</span>() → <span style="color:blue;">[List](../objects/List.md)</span>
> Find all map objects

#### function <span style="color:yellow;">FindMapObjectByName</span>(objectName: <span style="color:blue;">string</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Find a map object by name

#### function <span style="color:yellow;">FindMapObjectsByName</span>(objectName: <span style="color:blue;">string</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Find all map objects by name

#### function <span style="color:yellow;">FindMapObjectByComponent</span>(className: <span style="color:blue;">string</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Find all map objects by component

#### function <span style="color:yellow;">FindMapObjectsByComponent</span>(className: <span style="color:blue;">string</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Find all map objects by component

#### function <span style="color:yellow;">FindMapObjectByID</span>(id: <span style="color:blue;">int</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Find a map object by ID

#### function <span style="color:yellow;">FindMapObjectByTag</span>(tag: <span style="color:blue;">string</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Find a map object by tag

#### function <span style="color:yellow;">FindMapObjectsByTag</span>(tag: <span style="color:blue;">string</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Find all map objects by tag

#### function <span style="color:yellow;">CreateMapObjectRaw</span>(prefab: <span style="color:blue;">string</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Create a new map object

#### function <span style="color:yellow;">DestroyMapObject</span>(mapObject: <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>, includeChildren: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Destroy a map object

#### function <span style="color:yellow;">CopyMapObject</span>(mapObject: <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>, includeChildren: <span style="color:blue;">bool</span>) → <span style="color:blue;">[MapObject](../objects/MapObject.md)</span>
> Copy a map object

#### function <span style="color:yellow;">DestroyMapTargetable</span>(targetable: <span style="color:blue;">[MapTargetable](../objects/MapTargetable.md)</span>) → <span style="color:blue;">null</span>
> Destroy a map targetable

#### function <span style="color:yellow;">UpdateNavMesh</span>() → <span style="color:blue;">null</span>
> Update the nav mesh

#### function <span style="color:yellow;">UpdateNavMeshAsync</span>() → <span style="color:blue;">null</span>
> Update the nav mesh asynchronously


---

