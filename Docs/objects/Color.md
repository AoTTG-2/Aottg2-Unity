# Color
Inherits from [Object](./Object.md)

Represents a color.

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
Color() # Default constructor, creates a white color.
Color(hexString: string) # Creates a color from a hex string
Color(r: int, g: int, b: int) # Creates a color from RGB
Color(r: int, g: int, b: int, a: int) # Creates a color from RGBA
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

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-4">Color</a>, b: <a data-footnote-ref href="#user-content-fn-4">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-4">Color</a></code></pre>
> Linearly interpolates between colors `a` and `b` by `t`
<pre class="language-typescript"><code class="lang-typescript">function Gradient(a: <a data-footnote-ref href="#user-content-fn-4">Color</a>, b: <a data-footnote-ref href="#user-content-fn-4">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-4">Color</a></code></pre>
> Creates a gradient color from two colors

[^1]: [Camera](./Camera.md)
[^2]: [Character](./Character.md)
[^3]: [Collider](./Collider.md)
[^4]: [Collision](./Collision.md)
[^5]: [Color](./Color.md)
[^6]: [Convert](./Convert.md)
[^7]: [Cutscene](./Cutscene.md)
[^8]: [Dict](./Dict.md)
[^9]: [Game](./Game.md)
[^10]: [Human](./Human.md)
[^11]: [Input](./Input.md)
[^12]: [Json](./Json.md)
[^13]: [LineCastHitResult](./LineCastHitResult.md)
[^14]: [LineRenderer](./LineRenderer.md)
[^15]: [List](./List.md)
[^16]: [Map](./Map.md)
[^17]: [MapObject](./MapObject.md)
[^18]: [MapTargetable](./MapTargetable.md)
[^19]: [Math](./Math.md)
[^20]: [Network](./Network.md)
[^21]: [NetworkView](./NetworkView.md)
[^22]: [PersistentData](./PersistentData.md)
[^23]: [Physics](./Physics.md)
[^24]: [Player](./Player.md)
[^25]: [Quaternion](./Quaternion.md)
[^26]: [Random](./Random.md)
[^27]: [Range](./Range.md)
[^28]: [RoomData](./RoomData.md)
[^29]: [Set](./Set.md)
[^30]: [Shifter](./Shifter.md)
[^31]: [String](./String.md)
[^32]: [Time](./Time.md)
[^33]: [Titan](./Titan.md)
[^34]: [Transform](./Transform.md)
[^35]: [UI](./UI.md)
[^36]: [Vector2](./Vector2.md)
[^37]: [Vector3](./Vector3.md)
[^38]: [Object](./Object.md)
