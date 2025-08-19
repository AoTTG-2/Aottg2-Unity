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
