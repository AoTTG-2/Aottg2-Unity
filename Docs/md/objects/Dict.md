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
|Keys|[List](../objects/List.md)<K>|True|Keys in the dictionary|
|Values|[List](../objects/List.md)<V>|True|Values in the dictionary|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clears the dictionary
> 
<pre class="language-typescript"><code class="lang-typescript">function Get(key: <a data-footnote-ref href="#user-content-fn-59">Object</a>, defaultValue: <a data-footnote-ref href="#user-content-fn-59">Object</a> = null) -> <a data-footnote-ref href="#user-content-fn-59">Object</a><V></code></pre>
> Gets a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to get
> - `defaultValue`: The value to return if the key is not found
> 
> **Returns**: The value associated with the key, or the default value if the key is not found
<pre class="language-typescript"><code class="lang-typescript">function Set(key: <a data-footnote-ref href="#user-content-fn-59">Object</a>, value: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Sets a value in the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to set
> - `value`: The value to set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(key: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Removes a value from the dictionary
> 
> **Parameters**:
> - `key`: The key of the value to remove
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(key: <a data-footnote-ref href="#user-content-fn-59">Object</a>) -> bool</code></pre>
> Checks if the dictionary contains a key
> 
> **Parameters**:
> - `key`: The key to check
> 
> **Returns**: True if the dictionary contains the key, false otherwise

[^0]: [Color](../objects/Color.md)
[^1]: [Dict](../objects/Dict.md)
[^2]: [LightBuiltin](../static/LightBuiltin.md)
[^3]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^4]: [List](../objects/List.md)
[^5]: [Quaternion](../objects/Quaternion.md)
[^6]: [Range](../objects/Range.md)
[^7]: [Set](../objects/Set.md)
[^8]: [Vector2](../objects/Vector2.md)
[^9]: [Vector3](../objects/Vector3.md)
[^10]: [Animation](../objects/Animation.md)
[^11]: [Animator](../objects/Animator.md)
[^12]: [AudioSource](../objects/AudioSource.md)
[^13]: [Collider](../objects/Collider.md)
[^14]: [Collision](../objects/Collision.md)
[^15]: [LineRenderer](../objects/LineRenderer.md)
[^16]: [LodBuiltin](../static/LodBuiltin.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^21]: [Character](../objects/Character.md)
[^22]: [Human](../objects/Human.md)
[^23]: [MapObject](../objects/MapObject.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [Player](../objects/Player.md)
[^26]: [Prefab](../objects/Prefab.md)
[^27]: [Shifter](../objects/Shifter.md)
[^28]: [Titan](../objects/Titan.md)
[^29]: [Transform](../objects/Transform.md)
[^30]: [WallColossal](../objects/WallColossal.md)
[^31]: [Camera](../static/Camera.md)
[^32]: [Cutscene](../static/Cutscene.md)
[^33]: [Game](../static/Game.md)
[^34]: [Input](../static/Input.md)
[^35]: [Locale](../static/Locale.md)
[^36]: [Map](../static/Map.md)
[^37]: [Network](../static/Network.md)
[^38]: [PersistentData](../static/PersistentData.md)
[^39]: [Physics](../static/Physics.md)
[^40]: [RoomData](../static/RoomData.md)
[^41]: [Time](../static/Time.md)
[^42]: [Button](../objects/Button.md)
[^43]: [Dropdown](../objects/Dropdown.md)
[^44]: [Icon](../objects/Icon.md)
[^45]: [Image](../objects/Image.md)
[^46]: [Label](../objects/Label.md)
[^47]: [ProgressBar](../objects/ProgressBar.md)
[^48]: [ScrollView](../objects/ScrollView.md)
[^49]: [Slider](../objects/Slider.md)
[^50]: [TextField](../objects/TextField.md)
[^51]: [Toggle](../objects/Toggle.md)
[^52]: [UI](../static/UI.md)
[^53]: [VisualElement](../objects/VisualElement.md)
[^54]: [Convert](../static/Convert.md)
[^55]: [Json](../static/Json.md)
[^56]: [Math](../static/Math.md)
[^57]: [Random](../objects/Random.md)
[^58]: [String](../static/String.md)
[^59]: [Object](../objects/Object.md)
[^60]: [Component](../objects/Component.md)
