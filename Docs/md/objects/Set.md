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
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> bool</code></pre>
> Check if the set contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Add an element to the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Remove the element from the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Union(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>)</code></pre>
> Union with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Intersect(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>)</code></pre>
> Intersect with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Difference(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>)</code></pre>
> Difference with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSubsetOf(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set is a subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSupersetOf(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set is a superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSubsetOf(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set is a proper subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSupersetOf(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set is a proper superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Overlaps(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set overlaps with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function SetEquals(set: <a data-footnote-ref href="#user-content-fn-29">Set</a>) -> bool</code></pre>
> Check if the set has the same elements as another set
> 
<pre class="language-typescript"><code class="lang-typescript">function ToList() -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
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
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
