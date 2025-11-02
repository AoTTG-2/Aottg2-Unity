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
<pre class="language-typescript"><code class="lang-typescript">function Get(key: <a data-footnote-ref href="#user-content-fn-57">Object</a>, defaultValue: <a data-footnote-ref href="#user-content-fn-57">Object</a> = null) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Gets a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to get
> - `defaultValue`: The value to return if the key is not found
> 
> **Returns**: The value associated with the key, or the default value if the key is not found
<pre class="language-typescript"><code class="lang-typescript">function Set(key: <a data-footnote-ref href="#user-content-fn-57">Object</a>, value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Sets a value in the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to set
> - `value`: The value to set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(key: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Removes a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to remove
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(key: <a data-footnote-ref href="#user-content-fn-57">Object</a>) -> bool</code></pre>
> Checks if the dictionary contains a key
> 
> **Parameters**:
> - `key`: The key to check
> 
> **Returns**: True if the dictionary contains the key, false otherwise

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
