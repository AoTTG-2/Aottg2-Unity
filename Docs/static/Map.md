# Map
Inherits from object
## Methods
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>FindAllMapObjects()</td>
<td>[List](../objects/List.md)</td>
<td>Find all map objects</td>
</tr>
<tr>
<td>FindMapObjectByName(objectName : [String](../static/String.md))</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Find a map object by name</td>
</tr>
<tr>
<td>FindMapObjectsByName(objectName : [String](../static/String.md))</td>
<td>[List](../objects/List.md)</td>
<td>Find all map objects by name</td>
</tr>
<tr>
<td>FindMapObjectByComponent(className : [String](../static/String.md))</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Find all map objects by component</td>
</tr>
<tr>
<td>FindMapObjectsByComponent(className : [String](../static/String.md))</td>
<td>[List](../objects/List.md)</td>
<td>Find all map objects by component</td>
</tr>
<tr>
<td>FindMapObjectByID(id : int)</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Find a map object by ID</td>
</tr>
<tr>
<td>FindMapObjectByTag(tag : [String](../static/String.md))</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Find a map object by tag</td>
</tr>
<tr>
<td>FindMapObjectsByTag(tag : [String](../static/String.md))</td>
<td>[List](../objects/List.md)</td>
<td>Find all map objects by tag</td>
</tr>
<tr>
<td>CreateMapObjectRaw(prefab : [String](../static/String.md))</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Create a new map object</td>
</tr>
<tr>
<td>DestroyMapObject(mapObject : [MapObject](../objects/MapObject.md),includeChildren : bool)</td>
<td>none</td>
<td>Destroy a map object</td>
</tr>
<tr>
<td>CopyMapObject(mapObject : [MapObject](../objects/MapObject.md),includeChildren : bool)</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Copy a map object</td>
</tr>
<tr>
<td>DestroyMapTargetable(targetable : [MapTargetable](../objects/MapTargetable.md))</td>
<td>none</td>
<td>Destroy a map targetable</td>
</tr>
<tr>
<td>UpdateNavMesh()</td>
<td>none</td>
<td>Update the nav mesh</td>
</tr>
<tr>
<td>UpdateNavMeshAsync()</td>
<td>none</td>
<td>Update the nav mesh asynchronously</td>
</tr>
</tbody>
</table>
