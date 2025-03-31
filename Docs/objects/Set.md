# Set
Inherits from object
## Initialization
```csharp
# Set()
example = Set()

# Set(CustomLogicListBuiltin)
example = Set(CustomLogicListBuiltin)
```
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the set|
## Methods
#### function <span style="color:yellow;">Clear</span>() → <span style="color:blue;">null</span>
> Clear all set elements

#### function <span style="color:yellow;">Contains</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">bool</span>
> Check if the set contains the specified element

#### function <span style="color:yellow;">Add</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Add an element to the set

#### function <span style="color:yellow;">Remove</span>(value: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Remove the element from the set

#### function <span style="color:yellow;">Union</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">null</span>
> Union with another set

#### function <span style="color:yellow;">Intersect</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">null</span>
> Intersect with another set

#### function <span style="color:yellow;">Difference</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">null</span>
> Difference with another set

#### function <span style="color:yellow;">IsSubsetOf</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set is a subset of another set

#### function <span style="color:yellow;">IsSupersetOf</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set is a superset of another set

#### function <span style="color:yellow;">IsProperSubsetOf</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set is a proper subset of another set

#### function <span style="color:yellow;">IsProperSupersetOf</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set is a proper superset of another set

#### function <span style="color:yellow;">Overlaps</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set overlaps with another set

#### function <span style="color:yellow;">SetEquals</span>(set: <span style="color:blue;">[Set](../objects/Set.md)</span>) → <span style="color:blue;">bool</span>
> Check if the set has the same elements as another set

#### function <span style="color:yellow;">ToList</span>() → <span style="color:blue;">[List](../objects/List.md)</span>
> Convert the set to a list


---

