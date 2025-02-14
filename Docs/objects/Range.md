# Range
## Initialization
```csharp
example = Range((Object[]))
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|
## Methods
##### void Clear()
- **Description:** Clear all list elements
##### Object Get(int index)
- **Description:** Get the element at the specified index
##### void Set(int index, Object value)
- **Description:** Set the element at the specified index
##### void Add(Object value)
- **Description:** Add an element to the end of the list
##### void InsertAt(int index, Object value)
- **Description:** Insert an element at the specified index
##### void RemoveAt(int index)
- **Description:** Remove the element at the specified index
##### void Remove(Object value)
- **Description:** Remove the first occurrence of the specified element
##### bool Contains(Object value)
- **Description:** Check if the list contains the specified element
##### void Sort()
- **Description:** Sort the list
##### void SortCustom(UserMethod method)
- **Description:** Sort the list using a custom method, expects a method with the signature int method(a,b)
##### [List](../objects/List.md) Filter(UserMethod method)
- **Description:** Filter the list using a custom method, expects a method with the signature bool method(element)
##### [List](../objects/List.md) Map(UserMethod method)
- **Description:** Map the list using a custom method, expects a method with the signature object method(element)
##### Object Reduce(UserMethod method, Object initialValue)
- **Description:** Reduce the list using a custom method, expects a method with the signature object method(acc, element)
##### void Randomize()
- **Description:** Randomize the list
##### [Set](../objects/Set.md) ToSet()
- **Description:** Convert the list to a set

---

