# Set
Inherits from [Object](../objects/Object.md)
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
<pre class="language-typescript"><code class="lang-typescript">function Clear() -> null</code></pre>
> Clear all set elements
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> bool</code></pre>
> Check if the set contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Add an element to the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Remove the element from the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Union(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> null</code></pre>
> Union with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Intersect(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> null</code></pre>
> Intersect with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Difference(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> null</code></pre>
> Difference with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSubsetOf(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
> Check if the set is a subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSupersetOf(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
> Check if the set is a superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSubsetOf(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
> Check if the set is a proper subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSupersetOf(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
> Check if the set is a proper superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Overlaps(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
> Check if the set overlaps with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function SetEquals(set: <a data-footnote-ref href="#user-content-fn-28">Set</a>) -> bool</code></pre>
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
[^15]: [Map](../static/Map.md)
[^16]: [MapObject](../objects/MapObject.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [Math](../static/Math.md)
[^19]: [Network](../static/Network.md)
[^20]: [NetworkView](../objects/NetworkView.md)
[^21]: [PersistentData](../static/PersistentData.md)
[^22]: [Physics](../static/Physics.md)
[^23]: [Player](../objects/Player.md)
[^24]: [Quaternion](../objects/Quaternion.md)
[^25]: [Random](../objects/Random.md)
[^26]: [Range](../objects/Range.md)
[^27]: [RoomData](../static/RoomData.md)
[^28]: [Set](../objects/Set.md)
[^29]: [Shifter](../objects/Shifter.md)
[^30]: [String](../static/String.md)
[^31]: [Time](../static/Time.md)
[^32]: [Titan](../objects/Titan.md)
[^33]: [Transform](../objects/Transform.md)
[^34]: [UI](../static/UI.md)
[^35]: [Vector2](../objects/Vector2.md)
[^36]: [Vector3](../objects/Vector3.md)
[^37]: [Object](../objects/Object.md)
[^38]: [Component](../objects/Component.md)
