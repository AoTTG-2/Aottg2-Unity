# Collider
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|AttachedArticulationBody|[Transform](../objects/Transform.md)|False||
|ContactOffset|float|False||
|Enabled|bool|False||
|ExludeLayers|int|False||
|includeLayers|int|False||
|IsTrigger|bool|False||
|Center|[Vector3](../objects/Vector3.md)|False||
|ProvidesContacts|bool|False||
|MaterialName|[String](../static/String.md)|False||
|SharedMaterialName|[String](../static/String.md)|False||
|Transform|[Transform](../objects/Transform.md)|False||
|GameObjectTransform|[Transform](../objects/Transform.md)|False||
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
<td>ClosestPoint(position : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td></td>
</tr>
<tr>
<td>ClosestPointOnBounds(position : [Vector3](../objects/Vector3.md))</td>
<td>[Vector3](../objects/Vector3.md)</td>
<td></td>
</tr>
<tr>
<td>Raycast(start : [Vector3](../objects/Vector3.md),end : [Vector3](../objects/Vector3.md),maxDistance : float,collideWith : [String](../static/String.md))</td>
<td>[LineCastHitResult](../objects/LineCastHitResult.md)</td>
<td></td>
</tr>
</tbody>
</table>
