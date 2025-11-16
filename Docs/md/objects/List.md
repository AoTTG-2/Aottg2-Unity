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
<pre class="language-typescript"><code class="lang-typescript">function Get(index: int) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Get the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(index: int, value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Set the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Add an element to the end of the list
> 
<pre class="language-typescript"><code class="lang-typescript">function InsertAt(index: int, value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Insert an element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveAt(index: int)</code></pre>
> Remove the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Remove the first occurrence of the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-57">Object</a>) -> bool</code></pre>
> Check if the list contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Sort()</code></pre>
> Sort the list
> 
<pre class="language-typescript"><code class="lang-typescript">function SortCustom(method: function)</code></pre>
> Sort the list using a custom method, expects a method with the signature int method(a,b)
> 
<pre class="language-typescript"><code class="lang-typescript">function Filter(method: function) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Filter the list using a custom method, expects a method with the signature bool method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Map(method: function) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Map the list using a custom method, expects a method with the signature object method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Reduce(method: function, initialValue: <a data-footnote-ref href="#user-content-fn-57">Object</a>) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Randomize() -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Returns a randomized version of the list.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToSet() -> <a data-footnote-ref href="#user-content-fn-38">Set</a></code></pre>
> Convert the list to a set
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
