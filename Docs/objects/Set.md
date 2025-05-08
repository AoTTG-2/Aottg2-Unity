# Set
Inherits from Object

## Initialization
```csharp
# Set()
example = Set()

# Set(Object[])
example = Set(Object[])
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the set|
## Methods
###### function <mark style="color:yellow;">Clear</mark>()
> Clear all set elements

###### function <mark style="color:yellow;">Contains</mark>(value: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set contains the specified element

###### function <mark style="color:yellow;">Add</mark>(value: <mark style="color:blue;">Object</mark>)
> Add an element to the set

###### function <mark style="color:yellow;">Remove</mark>(value: <mark style="color:blue;">Object</mark>)
> Remove the element from the set

###### function <mark style="color:yellow;">Union</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>)
> Union with another set

###### function <mark style="color:yellow;">Intersect</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>)
> Intersect with another set

###### function <mark style="color:yellow;">Difference</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>)
> Difference with another set

###### function <mark style="color:yellow;">IsSubsetOf</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set is a subset of another set

###### function <mark style="color:yellow;">IsSupersetOf</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set is a superset of another set

###### function <mark style="color:yellow;">IsProperSubsetOf</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set is a proper subset of another set

###### function <mark style="color:yellow;">IsProperSupersetOf</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set is a proper superset of another set

###### function <mark style="color:yellow;">Overlaps</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set overlaps with another set

###### function <mark style="color:yellow;">SetEquals</mark>(set: <mark style="color:blue;">[Set](../objects/Set.md)</mark>) → <mark style="color:blue;">bool</mark>
> Check if the set has the same elements as another set

###### function <mark style="color:yellow;">ToList</mark>() → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Convert the set to a list


---

