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
<td>AddComponent(name : [String](../static/String.md))</td>
<td>CustomLogicComponentInstance</td>
<td>Add a component to the object</td>
</tr>
<tr>
<td>RemoveComponent(name : [String](../static/String.md))</td>
<td>none</td>
<td>Remove a component from the object</td>
</tr>
<tr>
<td>GetComponent(name : [String](../static/String.md))</td>
<td>CustomLogicComponentInstance</td>
<td>Get a component from the object</td>
</tr>
<tr>
<td>SetComponentEnabled(name : [String](../static/String.md),enabled : bool)</td>
<td>none</td>
<td>Set whether a component is enabled</td>
</tr>
<tr>
<td>SetComponentsEnabled(enabled : bool)</td>
<td>none</td>
<td>Set whether all components are enabled</td>
</tr>
<tr>
<td>AddSphereCollider(collideMode : [String](../static/String.md),collideWith : [String](../static/String.md),center : [Vector3](../objects/Vector3.md),radius : float)</td>
<td>none</td>
<td>Add a sphere collider to the object</td>
</tr>
<tr>
<td>AddBoxCollider(collideMode : [String](../static/String.md),collideWith : [String](../static/String.md),center : [Vector3](../objects/Vector3.md) = ,size : [Vector3](../objects/Vector3.md) = )</td>
<td>none</td>
<td>Add a box collider to the object</td>
</tr>
<tr>
<td>AddSphereTarget(team : [String](../static/String.md),center : [Vector3](../objects/Vector3.md),radius : float)</td>
<td>[MapTargetable](../objects/MapTargetable.md)</td>
<td>Add a sphere target to the object</td>
</tr>
<tr>
<td>AddBoxTarget(team : [String](../static/String.md),center : [Vector3](../objects/Vector3.md),size : [Vector3](../objects/Vector3.md))</td>
<td>[MapTargetable](../objects/MapTargetable.md)</td>
<td>Add a box target to the object</td>
</tr>
<tr>
<td>GetChild(name : [String](../static/String.md))</td>
<td>[MapObject](../objects/MapObject.md)</td>
<td>Get a child object by name</td>
</tr>
<tr>
<td>GetChildren()</td>
<td>[List](../objects/List.md)</td>
<td>Get all child objects</td>
</tr>
<tr>
<td>GetTransform(name : [String](../static/String.md))</td>
<td>[Transform](../objects/Transform.md)</td>
<td>Get a child transform by name</td>
</tr>
<tr>
<td>SetColorAll(color : [Color](../objects/Color.md))</td>
<td>none</td>
<td>Set the color of all renderers on the object</td>
</tr>
<tr>
<td>InBounds(position : [Vector3](../objects/Vector3.md))</td>
<td>bool</td>
<td>Check if a position is within the object's bounds</td>
</tr>
<tr>
<td>GetBoundsAverageCenter()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds average center</td>
</tr>
<tr>
<td>GetBoundsCenter()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds center</td>
</tr>
<tr>
<td>GetBoundsSize()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds size</td>
</tr>
<tr>
<td>GetBoundsMin()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds min</td>
</tr>
<tr>
<td>GetBoundsMax()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds max</td>
</tr>
<tr>
<td>GetBoundsExtents()</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td>Get the bounds extents</td>
</tr>
<tr>
<td>GetCorners()</td>
<td>[List](../objects/List.md)</td>
<td>Get the corners of the bounds</td>
</tr>
<tr>
<td>AddBuiltinComponent(parameter0 : Object = ,parameter1 : Object = ,parameter2 : Object = ,parameter3 : Object = ,parameter4 : Object = )</td>
<td>none</td>
<td>[OBSELETE] Add builtin component</td>
</tr>
<tr>
<td>ReadBuiltinComponent(name : [String](../static/String.md),param : [String](../static/String.md))</td>
<td>Object</td>
<td>[OBSELETE] Read a builtin component</td>
</tr>
<tr>
<td>UpdateBuiltinComponent(parameter0 : Object = ,parameter1 : Object = ,parameter2 : Object = ,parameter3 : Object = ,parameter4 : Object = )</td>
<td>none</td>
<td>[OBSELETE] Update a builtin component</td>
</tr>
</tbody>
</table>
