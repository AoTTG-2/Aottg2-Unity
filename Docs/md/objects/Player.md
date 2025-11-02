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
<pre class="language-typescript"><code class="lang-typescript">function GetCustomProperty(property: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Get a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCustomProperty(property: string, value: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Sets a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearKDR()</code></pre>
> Clears kills, deaths, highestdamage, and totaldamage properties.
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
