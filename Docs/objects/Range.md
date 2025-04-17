# Range
## Initialization
```csharp
# Range(Object[])
example = Range(Object[])
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|
## Methods
###### function <mark style="color:yellow;">Clear</mark>() → <mark style="color:blue;">null</mark>
> Clear all list elements

###### function <mark style="color:yellow;">Get</mark>(index: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">Object</mark>
> Get the element at the specified index

###### function <mark style="color:yellow;">Set</mark>(index: <mark style="color:blue;">int</mark>, value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Set the element at the specified index

###### function <mark style="color:yellow;">Add</mark>(value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Add an element to the end of the list

###### function <mark style="color:yellow;">InsertAt</mark>(index: <mark style="color:blue;">int</mark>, value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Insert an element at the specified index

###### function <mark style="color:yellow;">RemoveAt</mark>(index: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">null</mark>
> Remove the element at the specified index

###### function <mark style="color:yellow;">Remove</mark>(value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Remove the first occurrence of the specified element

###### function <mark style="color:yellow;">Contains</mark>(value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">bool</mark>
> Check if the list contains the specified element

###### function <mark style="color:yellow;">Sort</mark>() → <mark style="color:blue;">null</mark>
> Sort the list

###### function <mark style="color:yellow;">SortCustom</mark>(method: <mark style="color:blue;">function</mark>) → <mark style="color:blue;">null</mark>
> Sort the list using a custom method, expects a method with the signature int method(a,b)

###### function <mark style="color:yellow;">Filter</mark>(method: <mark style="color:blue;">function</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Filter the list using a custom method, expects a method with the signature bool method(element)

###### function <mark style="color:yellow;">Map</mark>(method: <mark style="color:blue;">function</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Map the list using a custom method, expects a method with the signature object method(element)

###### function <mark style="color:yellow;">Reduce</mark>(method: <mark style="color:blue;">function</mark>, initialValue: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">Object</mark>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element)

###### function <mark style="color:yellow;">Randomize</mark>() → <mark style="color:blue;">null</mark>
> Randomize the list

###### function <mark style="color:yellow;">ToSet</mark>() → <mark style="color:blue;">[Set](../objects/Set.md)</mark>
> Convert the list to a set


---

