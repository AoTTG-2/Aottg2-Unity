# List
Inherits from object
## Initialization
```csharp
# List()
example = List()

# List(CustomLogicSetBuiltin)
example = List(CustomLogicSetBuiltin)
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|
## Methods
#### function <span style="color:yellow;">Clear</span>() → <span style="color:blue;">null</span>
> Clear all list elements

#### function <span style="color:yellow;">Get</span>(index: <span style="color:blue;">int</span>) → <span style="color:blue;">Object</span>
> Get the element at the specified index

#### function <span style="color:yellow;">Set</span>(index: <span style="color:blue;">int</span>, value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Set the element at the specified index

#### function <span style="color:yellow;">Add</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Add an element to the end of the list

#### function <span style="color:yellow;">InsertAt</span>(index: <span style="color:blue;">int</span>, value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Insert an element at the specified index

#### function <span style="color:yellow;">RemoveAt</span>(index: <span style="color:blue;">int</span>) → <span style="color:blue;">null</span>
> Remove the element at the specified index

#### function <span style="color:yellow;">Remove</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Remove the first occurrence of the specified element

#### function <span style="color:yellow;">Contains</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">bool</span>
> Check if the list contains the specified element

#### function <span style="color:yellow;">Sort</span>() → <span style="color:blue;">null</span>
> Sort the list

#### function <span style="color:yellow;">SortCustom</span>(method: <span style="color:blue;">function</span>) → <span style="color:blue;">null</span>
> Sort the list using a custom method, expects a method with the signature int method(a,b)

#### function <span style="color:yellow;">Filter</span>(method: <span style="color:blue;">function</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Filter the list using a custom method, expects a method with the signature bool method(element)

#### function <span style="color:yellow;">Map</span>(method: <span style="color:blue;">function</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Map the list using a custom method, expects a method with the signature object method(element)

#### function <span style="color:yellow;">Reduce</span>(method: <span style="color:blue;">function</span>, initialValue: <span style="color:blue;">Object</span>) → <span style="color:blue;">Object</span>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element)

#### function <span style="color:yellow;">Randomize</span>() → <span style="color:blue;">null</span>
> Randomize the list

#### function <span style="color:yellow;">ToSet</span>() → <span style="color:blue;">[Set](../objects/Set.md)</span>
> Convert the list to a set


---

