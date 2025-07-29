# Color
Inherits from [Object](../objects/Object.md)

Represents a color. Every component is in the range [0, 255].

### Remarks
Implements `__Copy__` which means that this class will act like a struct.

Overloads operators: 
- `==`
- `__Hash__`
- `__Copy__`
- `__Str__`
- `+`
- `-`
- `*`
- `/`
### Example
```csharp
Game.Print(color.ToHexString()) // Prints the color in hex format
```
### Initialization
```csharp
Color() // Default constructor, creates a white color.
Color(hexString: string) // Creates a color from a hex string
Color(r: int, g: int, b: int) // Creates a color from RGB
Color(r: int, g: int, b: int, a: int) // Creates a color from RGBA
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|R|int|False|Red component of the color|
|G|int|False|Green component of the color|
|B|int|False|Blue component of the color|
|A|int|False|Alpha component of the color|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function ToHexString() -> string</code></pre>
> Converts the color to a hex string
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-4">Color</a>, b: <a data-footnote-ref href="#user-content-fn-4">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-4">Color</a></code></pre>
> Linearly interpolates between colors `a` and `b` by `t`
> 
> **Parameters**:
> - `a`: Color to interpolate from
> - `b`: Color to interpolate to
> - `t`: Interpolation factor. 0 = `a`, 1 = `b`
> 
> **Returns**: A new color between `a` and `b`
<pre class="language-typescript"><code class="lang-typescript">function Gradient(a: <a data-footnote-ref href="#user-content-fn-4">Color</a>, b: <a data-footnote-ref href="#user-content-fn-4">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-4">Color</a></code></pre>
> Creates a gradient color from two colors
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
