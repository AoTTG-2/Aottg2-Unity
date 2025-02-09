# Set
Inherits from object
## Initialization
```csharp
example = Set()
example = Set((CustomLogicListBuiltin))
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the set|
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
<td>Clear()</td>
<td>none</td>
<td>Clear all set elements</td>
</tr>
<tr>
<td>Contains(value : Object)</td>
<td>bool</td>
<td>Check if the set contains the specified element</td>
</tr>
<tr>
<td>Add(value : Object)</td>
<td>none</td>
<td>Add an element to the set</td>
</tr>
<tr>
<td>Remove(value : Object)</td>
<td>none</td>
<td>Remove the element from the set</td>
</tr>
<tr>
<td>Union(set : [Set](../objects/Set.md))</td>
<td>none</td>
<td>Union with another set</td>
</tr>
<tr>
<td>Intersect(set : [Set](../objects/Set.md))</td>
<td>none</td>
<td>Intersect with another set</td>
</tr>
<tr>
<td>Difference(set : [Set](../objects/Set.md))</td>
<td>none</td>
<td>Difference with another set</td>
</tr>
<tr>
<td>IsSubsetOf(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set is a subset of another set</td>
</tr>
<tr>
<td>IsSupersetOf(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set is a superset of another set</td>
</tr>
<tr>
<td>IsProperSubsetOf(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set is a proper subset of another set</td>
</tr>
<tr>
<td>IsProperSupersetOf(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set is a proper superset of another set</td>
</tr>
<tr>
<td>Overlaps(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set overlaps with another set</td>
</tr>
<tr>
<td>SetEquals(set : [Set](../objects/Set.md))</td>
<td>bool</td>
<td>Check if the set has the same elements as another set</td>
</tr>
<tr>
<td>ToList()</td>
<td>[List](../objects/List.md)</td>
<td>Convert the set to a list</td>
</tr>
</tbody>
</table>
