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
<pre class="language-typescript"><code class="lang-typescript">function RandomVector3(a: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Generates a random Vector3 between the specified ranges.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomDirection(flat: bool = False) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Generates a random normalized direction vector. If flat is true, the y component will be zero.
> 
<pre class="language-typescript"><code class="lang-typescript">function RandomSign() -> int</code></pre>
> Generates a random sign, either 1 or -1.
> 
<pre class="language-typescript"><code class="lang-typescript">function PerlinNoise(x: float, y: float) -> float</code></pre>
> Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)
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
