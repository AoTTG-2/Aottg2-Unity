# Range

Inherits from List. Allows you to create lists of integers for convenient iteration, particularly in for loops.

### Example
```csharp
for (a in Range(10))
Game.Print(a);

for (a in Range(1, 10))
Game.Print(a);

for (a in Range(1, 10, 2))
Game.Print(a);
```
### Initialization
```csharp
Range(parameterValues: Object) // The constructor for the Range builtin class has multiple overloads.
Range(n) creates a range from 0 to n-1.
Range(a, n) creates a range from a to n-1.
Range(a, n, step) creates a range from a to n-1 with the specified step.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear() -> null</code></pre>
> Clear all list elements
> 
<pre class="language-typescript"><code class="lang-typescript">function Get(index: int) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Get the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(index: int, value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Set the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Add an element to the end of the list
> 
<pre class="language-typescript"><code class="lang-typescript">function InsertAt(index: int, value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Insert an element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveAt(index: int) -> null</code></pre>
> Remove the element at the specified index
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Remove the first occurrence of the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> bool</code></pre>
> Check if the list contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Sort() -> null</code></pre>
> Sort the list
> 
<pre class="language-typescript"><code class="lang-typescript">function SortCustom(method: function) -> null</code></pre>
> Sort the list using a custom method, expects a method with the signature int method(a,b)
> 
<pre class="language-typescript"><code class="lang-typescript">function Filter(method: function) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Filter the list using a custom method, expects a method with the signature bool method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Map(method: function) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Map the list using a custom method, expects a method with the signature object method(element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Reduce(method: function, initialValue: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element)
> 
<pre class="language-typescript"><code class="lang-typescript">function Randomize() -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Returns a randomized version of the list.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToSet() -> <a data-footnote-ref href="#user-content-fn-28">Set</a></code></pre>
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
