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
<pre class="language-typescript"><code class="lang-typescript">function Debug(message: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Print a debug statement to the console
> 
<pre class="language-typescript"><code class="lang-typescript">function Print(message: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Print a message to the chat
> 
<pre class="language-typescript"><code class="lang-typescript">function PrintAll(message: <a data-footnote-ref href="#user-content-fn-57">Object</a>)</code></pre>
> Print a message to all players
> 
<pre class="language-typescript"><code class="lang-typescript">function GetGeneralSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Get a general setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTitanSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Get a titan setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMiscSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Get a misc setting
> 
<pre class="language-typescript"><code class="lang-typescript">function End(delay: float)</code></pre>
> End the game
> 
<pre class="language-typescript"><code class="lang-typescript">function FindCharacterByViewID(viewID: int) -> <a data-footnote-ref href="#user-content-fn-4">Character</a></code></pre>
> Find a character by view ID
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitan(type: string) -> <a data-footnote-ref href="#user-content-fn-42">Titan</a></code></pre>
> Spawn a titan
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitanAt(type: string, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-42">Titan</a></code></pre>
> Spawn a titan at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitans(type: string, count: int) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Spawn titans
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAsync(type: string, count: int)</code></pre>
> Spawn titans asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAt(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Spawn titans at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAtAsync(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn titans at a position asynchronously
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifter(type: string) -> <a data-footnote-ref href="#user-content-fn-39">Shifter</a></code></pre>
> Spawn a shifter
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifterAt(type: string, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-39">Shifter</a></code></pre>
> Spawn a shifter at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectile(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, liveTime: float, team: string, extraParam: <a data-footnote-ref href="#user-content-fn-57">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-57">Object</a> = null)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectileWithOwner(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, liveTime: float, owner: <a data-footnote-ref href="#user-content-fn-4">Character</a>, extraParam: <a data-footnote-ref href="#user-content-fn-57">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-57">Object</a> = null)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function SpawnEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, scale: float, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-7">Color</a> = null, tsKillSound: string = null)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayer(player: <a data-footnote-ref href="#user-content-fn-31">Player</a>, force: bool)</code></pre>
> Spawn a player
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAll(force: bool)</code></pre>
> Spawn a player for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAt(player: <a data-footnote-ref href="#user-content-fn-31">Player</a>, force: bool, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAtAll(force: bool, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position for all players
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPlaylist(playlist: string)</code></pre>
> Set the music playlist
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSong(song: string)</code></pre>
> Set the music song
> 
<pre class="language-typescript"><code class="lang-typescript">function DrawRay(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, dir: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, color: <a data-footnote-ref href="#user-content-fn-7">Color</a>, duration: float)</code></pre>
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
