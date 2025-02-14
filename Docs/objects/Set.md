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
##### void Clear()
- **Description:** Clear all set elements
##### bool Contains(Object value)
- **Description:** Check if the set contains the specified element
##### void Add(Object value)
- **Description:** Add an element to the set
##### void Remove(Object value)
- **Description:** Remove the element from the set
##### void Union([Set](../objects/Set.md) set)
- **Description:** Union with another set
##### void Intersect([Set](../objects/Set.md) set)
- **Description:** Intersect with another set
##### void Difference([Set](../objects/Set.md) set)
- **Description:** Difference with another set
##### bool IsSubsetOf([Set](../objects/Set.md) set)
- **Description:** Check if the set is a subset of another set
##### bool IsSupersetOf([Set](../objects/Set.md) set)
- **Description:** Check if the set is a superset of another set
##### bool IsProperSubsetOf([Set](../objects/Set.md) set)
- **Description:** Check if the set is a proper subset of another set
##### bool IsProperSupersetOf([Set](../objects/Set.md) set)
- **Description:** Check if the set is a proper superset of another set
##### bool Overlaps([Set](../objects/Set.md) set)
- **Description:** Check if the set overlaps with another set
##### bool SetEquals([Set](../objects/Set.md) set)
- **Description:** Check if the set has the same elements as another set
##### [List](../objects/List.md) ToList()
- **Description:** Convert the set to a list

---

