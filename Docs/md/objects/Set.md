# Set
Inherits from [Object](../objects/Object.md)

Collection of unique elements

### Initialization
```csharp
Set()
Set(parameterValues: Object)
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the set|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clear all set elements
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>) -> bool</code></pre>
> Check if the set contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Add an element to the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Remove the element from the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Union(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>)</code></pre>
> Union with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Intersect(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>)</code></pre>
> Intersect with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Difference(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>)</code></pre>
> Difference with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSubsetOf(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set is a subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSupersetOf(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set is a superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSubsetOf(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set is a proper subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSupersetOf(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set is a proper superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Overlaps(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set overlaps with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function SetEquals(set: <a data-footnote-ref href="#user-content-fn-35">Set</a>) -> bool</code></pre>
> Check if the set has the same elements as another set
> 
<pre class="language-typescript"><code class="lang-typescript">function ToList() -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Convert the set to a list
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
