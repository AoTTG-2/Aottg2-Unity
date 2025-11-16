# Color
Inherits from [Object](../objects/Object.md)

Represents a color. Every component is in the range [0, 255].

### Remarks
Implements `__Copy__` which means that this class will act like a struct.

Overloads operators: 
`==`, `__Hash__`, `__Copy__`, `__Str__`, `+`, `-`, `*`, `/`
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
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-7">Color</a>, b: <a data-footnote-ref href="#user-content-fn-7">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-7">Color</a></code></pre>
> Linearly interpolates between colors `a` and `b` by `t`
> 
> **Parameters**:
> - `a`: Color to interpolate from
> - `b`: Color to interpolate to
> - `t`: Interpolation factor. 0 = `a`, 1 = `b`
> 
> **Returns**: A new color between `a` and `b`
<pre class="language-typescript"><code class="lang-typescript">function Gradient(a: <a data-footnote-ref href="#user-content-fn-7">Color</a>, b: <a data-footnote-ref href="#user-content-fn-7">Color</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-7">Color</a></code></pre>
> Creates a gradient color from two colors
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
