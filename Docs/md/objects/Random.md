# Random
Inherits from [Object](../objects/Object.md)

Random can be initialized as a class with an int given as the seed value.
Note that this is optional, and you can reference Random directly as a static class.

### Example
```csharp
# Use random methods directly
r = Random.RandomInt(0, 100);

# Or create an instance of Random with a seed
generator = Random(123);

# Use it
a = generator.RandomInt(0, 100);

# Seed allows repeatable random values
generator2 = Random(123);
b = generator2.RandomInt(0, 100);
compared = a == b;    # Always True
```
### Initialization
```csharp
Random()
Random(seed: int)
```

### Methods
<pre class="language-typescript"><code class="lang-typescript">function RandomInt(min: int, max: int) -> int</code></pre>
> Generates a random integer between the specified range.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomFloat(min: float, max: float) -> float</code></pre>
> Generates a random float between the specified range.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomBool() -> bool</code></pre>
> Returns random boolean.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomVector3(a: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Generates a random Vector3 between the specified ranges.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomDirection(flat: bool = False) -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Generates a random normalized direction vector. If flat is true, the y component will be zero.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomSign() -> int</code></pre>
> Generates a random sign, either 1 or -1.
> 
<pre class="language-typescript"><code class="lang-typescript">function PerlinNoise(x: float, y: float) -> float</code></pre>
> Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)
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
