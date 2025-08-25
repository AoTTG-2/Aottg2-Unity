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
|Team|string|True|Player's chosen team ("None", "Blue", "Red", "Titan", "Human").
Note that this may be different from the character's final team (Character.Team field) if the character's team field is modified.|
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
<pre class="language-typescript"><code class="lang-typescript">function GetCustomProperty(property: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Get a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCustomProperty(property: string, value: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Sets a custom property at given key. Must be a primitive type. This is synced to all clients.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearKDR()</code></pre>
> Clears kills, deaths, highestdamage, and totaldamage properties.
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
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
