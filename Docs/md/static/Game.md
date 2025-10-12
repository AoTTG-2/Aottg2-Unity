# Game
Inherits from [Object](../objects/Object.md)

Game functions such as spawning titans and managing game state.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|True|Is the game ending?|
|EndTimeLeft|float|True|Time left until the game ends|
|Titans|[List](../objects/List.md)|True|List of all titans|
|AITitans|[List](../objects/List.md)|True|List of all AI titans|
|PlayerTitans|[List](../objects/List.md)|True|List of all player titans|
|Shifters|[List](../objects/List.md)|True|List of all shifters|
|AIShifters|[List](../objects/List.md)|True|List of all AI shifters|
|PlayerShifters|[List](../objects/List.md)|True|List of all player shifters|
|Humans|[List](../objects/List.md)|True|List of all humans|
|AIHumans|[List](../objects/List.md)|True|List of all AI humans|
|PlayerHumans|[List](../objects/List.md)|True|List of all player humans|
|Loadouts|[List](../objects/List.md)|True|List of all loadouts|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|string|False|Forced character type|
|ForcedLoadout|string|False|Forced loadout|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Debug(message: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Print a debug statement to the console
> 
<pre class="language-typescript"><code class="lang-typescript">function Print(message: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Print a message to the chat
> 
<pre class="language-typescript"><code class="lang-typescript">function PrintAll(message: <a data-footnote-ref href="#user-content-fn-45">Object</a>)</code></pre>
> Print a message to all players
> 
<pre class="language-typescript"><code class="lang-typescript">function GetGeneralSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Get a general setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTitanSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Get a titan setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMiscSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Get a misc setting
> 
<pre class="language-typescript"><code class="lang-typescript">function End(delay: float)</code></pre>
> End the game
> 
<pre class="language-typescript"><code class="lang-typescript">function FindCharacterByViewID(viewID: int) -> <a data-footnote-ref href="#user-content-fn-1">Character</a></code></pre>
> Find a character by view ID
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitan(type: string) -> <a data-footnote-ref href="#user-content-fn-39">Titan</a></code></pre>
> Spawn a titan
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitanAt(type: string, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-39">Titan</a></code></pre>
> Spawn a titan at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitans(type: string, count: int) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Spawn titans
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAsync(type: string, count: int)</code></pre>
> Spawn titans asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAt(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Spawn titans at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAtAsync(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn titans at a position asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifter(type: string) -> <a data-footnote-ref href="#user-content-fn-36">Shifter</a></code></pre>
> Spawn a shifter
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifterAt(type: string, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-36">Shifter</a></code></pre>
> Spawn a shifter at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectile(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, liveTime: float, team: string, extraParam: <a data-footnote-ref href="#user-content-fn-45">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-45">Object</a> = null)</code></pre>
> Spawn a projectile.
Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Valid values are: "Thunderspear", "CannonBall", "Flare", "BladeThrow", "SmokeBomb", "Rock1"
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `velocity`: Spawn velocity
> - `gravity`: Spawn gravity
> - `liveTime`: Live time of the projectile
> - `team`: The team that the projectile belongs to
> - `extraParam`: Optional. Type depends on projectile:
>   - Thunderspear: float (explosion radius)
>   - Flare: Color (flare color)
>   - Rock1: float (rock size)
>   - Others: unused
> - `extraParam2`: Optional. Type depends on projectile:
>   - Thunderspear: Color (projectile color)
>   - Others: unused
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectileWithOwner(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, liveTime: float, owner: <a data-footnote-ref href="#user-content-fn-1">Character</a>, extraParam: <a data-footnote-ref href="#user-content-fn-45">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-45">Object</a> = null)</code></pre>
> Spawn a projectile with an owner.
Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Valid values are: "Thunderspear", "CannonBall", "Flare", "BladeThrow", "SmokeBomb", "Rock1"
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `velocity`: Spawn velocity
> - `gravity`: Spawn gravity
> - `liveTime`: Live time of the projectile
> - `owner`: The character that the projectile belongs to
> - `extraParam`: Optional. Type depends on projectile:
>   - Thunderspear: float (explosion radius)
>   - Flare: Color (flare color)
>   - Rock1: float (rock size)
>   - Others: unused
> - `extraParam2`: Optional. Type depends on projectile:
>   - Thunderspear: Color (projectile color)
>   - Others: unused
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, scale: float, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-4">Color</a> = null, tsKillSound: string = null)</code></pre>
> Spawns an effect.
> 
> **Parameters**:
> - `effectName`: Name of the effect. Effect names can be found [here](https://raw.githubusercontent.com/AoTTG-2/Aottg2-Unity/refs/heads/main/Assets/Scripts/Effects/EffectPrefabs.cs) (left-hand variable name)
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `scale`: Spawn scale
> - `tsExplodeColor`: Thunderspear explode color (Only valid when `effectName` is "ThunderspearExplode")
> - `tsKillSound`: Optional. Thunderspear explode sound (Only valid when `effectName` is "ThunderspearExplode"). Valid values are: "Kill", "Air", "Ground", "ArmorHit", "CloseShot", "MaxRangeShot"
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayer(player: <a data-footnote-ref href="#user-content-fn-28">Player</a>, force: bool)</code></pre>
> Spawn a player
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAll(force: bool)</code></pre>
> Spawn a player for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAt(player: <a data-footnote-ref href="#user-content-fn-28">Player</a>, force: bool, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAtAll(force: bool, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPlaylist(playlist: string)</code></pre>
> Set the music playlist
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSong(song: string)</code></pre>
> Set the music song
> 
<pre class="language-typescript"><code class="lang-typescript">function DrawRay(start: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, dir: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, color: <a data-footnote-ref href="#user-content-fn-4">Color</a>, duration: float)</code></pre>
> Draw a ray
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillScore(damage: int)</code></pre>
> Show the kill score
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeed(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeedAll(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed for all players
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
