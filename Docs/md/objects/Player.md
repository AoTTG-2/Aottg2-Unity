# Player
Inherits from [Object](../objects/Object.md)

Represents a network player. Only master client or player may modify fields.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Character|[Character](../objects/Character.md)|True|Player's current character, if alive.|
|Connected|bool|True|Player is still connected to the room.|
|ID|int|True|Player unique ID.|
|Name|string|True|Player name.|
|Guild|string|True|Player guild.|
|Team|string|True|Player's chosen team ("None", "Blue", "Red", "Titan", "Human"). Note that this may be different from the character's final team (Character.Team field) if the character's team field is modified.|
|Status|string|True|Player's spawn status ("Alive", "Dead", "Spectating").|
|CharacterType|string|True|Player's chosen character ("Human", "Titan", "Shifter")|
|Loadout|string|True|Player's chosen loadout ("Blades", "AHSS", "APG", "Thunderspears").|
|Kills|int|False|Player's kills.|
|Deaths|int|False|Player's deaths.|
|HighestDamage|int|False|Player's highest damage.|
|TotalDamage|int|False|Player's total damage.|
|Ping|int|True|The player's connection ping.|
|SpectateID|int|True|The player's spectating ID. If not spectating anyone, returns -1.|
|SpawnPoint|[Vector3](../objects/Vector3.md)|False|Player's respawn point. Is initially null and can be set back to null, at which point map spawn points are used.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetCustomProperty(property: string) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Get a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
> **Parameters**:
> - `property`: The property key to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCustomProperty(property: string, value: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Sets a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
> **Parameters**:
> - `property`: The property key to set.
> - `value`: The value to set (must be a primitive type).
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearKDR()</code></pre>
> Clears kills, deaths, highestdamage, and totaldamage properties.
> 

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
