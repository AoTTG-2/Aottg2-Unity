# List
Inherits from [Object](../objects/Object.md)

Ordered collection of objects.

### Example
```csharp
values = List(1,2,1,4,115);

# Common generic list operations Map, Filter, Reduce, and Sort are supported.
# These methods take in a defined method with an expected signature and return type.

# Filter list to only unique values using set conversion.
uniques = values.ToSet().ToList();

# Accumulate values in list using reduce.
sum = values.Reduce(self.Sum2, 0);  # returns 123

# Filter list using predicate method.
filteredList = values.Filter(self.Filter);  # returns List(115)

# Transform list using mapping method.
newList = values.Map(self.TransformData);   # returns List(2,4,2,8,230)

function Sum2(a, b)
{
    return a + b;
}

function Filter(a)
{
    return a > 20;
}

function TransformData(a)
{
    return a * 2;
}
```
### Initialization
```csharp
List()
List(parameterValues: Object)
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clear all list elements
> 
<pre class="language-typescript"><code class="lang-typescript">function Get(index: int) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Get the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(index: int, value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Set the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Add an element to the end of the list
> 
<pre class="language-typescript"><code class="lang-typescript">function InsertAt(index: int, value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Insert an element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveAt(index: int)</code></pre>
> Remove the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Remove the first occurrence of the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>) -> bool</code></pre>
> Check if the list contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Sort()</code></pre>
> Sort the list
> 
<pre class="language-typescript"><code class="lang-typescript">function SortCustom(method: function)</code></pre>
> Sort the list using a custom method, expects a method with the signature int method(a,b)
> 
<pre class="language-typescript"><code class="lang-typescript">function Filter(method: function) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Filter the list using a custom method, expects a method with the signature bool method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Map(method: function) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Map the list using a custom method, expects a method with the signature object method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Reduce(method: function, initialValue: <a data-footnote-ref href="#user-content-fn-45">Object</a>) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Randomize() -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Returns a randomized version of the list.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToSet() -> <a data-footnote-ref href="#user-content-fn-35">Set</a></code></pre>
> Convert the list to a set
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
