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
|Function|Returns|Description|
|---|---|---|
|Clear()|none|Clear all list elements|
|Get(index : int)|Object|Get the element at the specified index|
|Set(index : int, value : Object)|none|Set the element at the specified index|
|Add(value : Object)|none|Add an element to the end of the list|
|InsertAt(index : int, value : Object)|none|Insert an element at the specified index|
|RemoveAt(index : int)|none|Remove the element at the specified index|
|Remove(value : Object)|none|Remove the first occurrence of the specified element|
|Contains(value : Object)|bool|Check if the list contains the specified element|
|Sort()|none|Sort the list|
|SortCustom(method : UserMethod)|none|Sort the list using a custom method, expects a method with the signature int method(a,b)|
|Filter(method : UserMethod)|[List](../Object/List.md)|Filter the list using a custom method, expects a method with the signature bool method(element)|
|Map(method : UserMethod)|[List](../Object/List.md)|Map the list using a custom method, expects a method with the signature object method(element)|
|Reduce(method : UserMethod, initialValue : Object)|Object|Reduce the list using a custom method, expects a method with the signature object method(acc, element)|
|Randomize()|none|Randomize the list|
|ToSet()|[Set](../Object/Set.md)|Convert the list to a set|
