# List
Inherits from object
## Initialization
```csharp
example = List()
example = List((CustomLogicSetBuiltin))
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|
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
<td>Clear all list elements</td>
</tr>
<tr>
<td>Get(index : int)</td>
<td>Object</td>
<td>Get the element at the specified index</td>
</tr>
<tr>
<td>Set(index : int,value : Object)</td>
<td>none</td>
<td>Set the element at the specified index</td>
</tr>
<tr>
<td>Add(value : Object)</td>
<td>none</td>
<td>Add an element to the end of the list</td>
</tr>
<tr>
<td>InsertAt(index : int,value : Object)</td>
<td>none</td>
<td>Insert an element at the specified index</td>
</tr>
<tr>
<td>RemoveAt(index : int)</td>
<td>none</td>
<td>Remove the element at the specified index</td>
</tr>
<tr>
<td>Remove(value : Object)</td>
<td>none</td>
<td>Remove the first occurrence of the specified element</td>
</tr>
<tr>
<td>Contains(value : Object)</td>
<td>bool</td>
<td>Check if the list contains the specified element</td>
</tr>
<tr>
<td>Sort()</td>
<td>none</td>
<td>Sort the list</td>
</tr>
<tr>
<td>SortCustom(method : UserMethod)</td>
<td>none</td>
<td>Sort the list using a custom method, expects a method with the signature int method(a,b)</td>
</tr>
<tr>
<td>Filter(method : UserMethod)</td>
<td>[List](../objects/List.md)</td>
<td>Filter the list using a custom method, expects a method with the signature bool method(element)</td>
</tr>
<tr>
<td>Map(method : UserMethod)</td>
<td>[List](../objects/List.md)</td>
<td>Map the list using a custom method, expects a method with the signature object method(element)</td>
</tr>
<tr>
<td>Reduce(method : UserMethod,initialValue : Object)</td>
<td>Object</td>
<td>Reduce the list using a custom method, expects a method with the signature object method(acc, element)</td>
</tr>
<tr>
<td>Randomize()</td>
<td>none</td>
<td>Randomize the list</td>
</tr>
<tr>
<td>ToSet()</td>
<td>[Set](../objects/Set.md)</td>
<td>Convert the list to a set</td>
</tr>
</tbody>
</table>
