# Dict
Inherits from [Object](../objects/Object.md)

Collection of key-value pairs.
Keys must be unique.

### Initialization
```csharp
Dict() // Creates an empty dictionary
Dict(capacity: int) // Creates a dictionary with the specified capacity
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|Number of elements in the dictionary|
|Keys|[List](../objects/List.md)|True|Keys in the dictionary|
|Values|[List](../objects/List.md)|True|Values in the dictionary|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clears the dictionary
> 
<pre class="language-typescript"><code class="lang-typescript">function Get(key: <a data-footnote-ref href="#user-content-fn-38">Object</a>, defaultValue: <a data-footnote-ref href="#user-content-fn-38">Object</a> = null) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Gets a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to get
> - `defaultValue`: The value to return if the key is not found
> 
> **Returns**: The value associated with the key, or the default value if the key is not found
<pre class="language-typescript"><code class="lang-typescript">function Set(key: <a data-footnote-ref href="#user-content-fn-38">Object</a>, value: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Sets a value in the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to set
> - `value`: The value to set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(key: <a data-footnote-ref href="#user-content-fn-38">Object</a>)</code></pre>
> Removes a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to remove
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(key: <a data-footnote-ref href="#user-content-fn-38">Object</a>) -> bool</code></pre>
> Checks if the dictionary contains a key
> 
> **Parameters**:
> - `key`: The key to check
> 
> **Returns**: True if the dictionary contains the key, false otherwise

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
