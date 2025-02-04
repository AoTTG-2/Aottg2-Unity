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
|Function|Returns|Description|
|---|---|---|
|Clear()|none|Clear all set elements|
|Contains(value : Object)|bool|Check if the set contains the specified element|
|Add(value : Object)|none|Add an element to the set|
|Remove(value : Object)|none|Remove the element from the set|
|Union(set : [Set](../Object/Set.md))|none|Union with another set|
|Intersect(set : [Set](../Object/Set.md))|none|Intersect with another set|
|Difference(set : [Set](../Object/Set.md))|none|Difference with another set|
|IsSubsetOf(set : [Set](../Object/Set.md))|bool|Check if the set is a subset of another set|
|IsSupersetOf(set : [Set](../Object/Set.md))|bool|Check if the set is a superset of another set|
|IsProperSubsetOf(set : [Set](../Object/Set.md))|bool|Check if the set is a proper subset of another set|
|IsProperSupersetOf(set : [Set](../Object/Set.md))|bool|Check if the set is a proper superset of another set|
|Overlaps(set : [Set](../Object/Set.md))|bool|Check if the set overlaps with another set|
|SetEquals(set : [Set](../Object/Set.md))|bool|Check if the set has the same elements as another set|
|ToList()|[List](../Object/List.md)|Convert the set to a list|
