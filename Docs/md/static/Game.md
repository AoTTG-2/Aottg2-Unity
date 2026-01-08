# Game
Inherits from [Object](../objects/Object.md)

Game functions such as spawning titans and managing game state.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|True|Is the game ending?|
|EndTimeLeft|float|True|Time left until the game ends|
|Titans|[List](../objects/List.md)<[Titan](../objects/Titan.md)>|True|List of all titans|
|AITitans|[List](../objects/List.md)<[Titan](../objects/Titan.md)>|True|List of all AI titans|
|PlayerTitans|[List](../objects/List.md)<[Titan](../objects/Titan.md)>|True|List of all player titans|
|Shifters|[List](../objects/List.md)<[Shifter](../objects/Shifter.md)>|True|List of all shifters|
|AIShifters|[List](../objects/List.md)<[Shifter](../objects/Shifter.md)>|True|List of all AI shifters|
|PlayerShifters|[List](../objects/List.md)<[Shifter](../objects/Shifter.md)>|True|List of all player shifters|
|Humans|[List](../objects/List.md)<[Human](../objects/Human.md)>|True|List of all humans|
|AIHumans|[List](../objects/List.md)<[Human](../objects/Human.md)>|True|List of all AI humans|
|PlayerHumans|[List](../objects/List.md)<[Human](../objects/Human.md)>|True|List of all player humans|
|Loadouts|[List](../objects/List.md)<string>|True|List of all loadouts|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|string|False|Forced character type. Refer to [CharacterTypeEnum](../static/CharacterTypeEnum.md)|
|ForcedLoadout|string|False|Forced loadout. Refer to [LoadoutEnum](../static/LoadoutEnum.md)|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Debug(message: <a data-footnote-ref href="#user-content-fn-82">Object</a>)</code></pre>
> Print a debug statement to the console
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function Print(message: <a data-footnote-ref href="#user-content-fn-82">Object</a>)</code></pre>
> Print a message to the chat
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function PrintAll(message: <a data-footnote-ref href="#user-content-fn-82">Object</a>)</code></pre>
> Print a message to all players
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetGeneralSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Get a general setting
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTitanSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Get a titan setting
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMiscSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Get a misc setting
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function End(delay: float)</code></pre>
> End the game
> 
> **Parameters**:
> - `delay`: The delay in seconds before ending the game.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindCharacterByViewID(viewID: int) -> <a data-footnote-ref href="#user-content-fn-21">Character</a></code></pre>
> Find a character by view ID
> 
> **Parameters**:
> - `viewID`: The Photon view ID of the character.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitan(type: string) -> <a data-footnote-ref href="#user-content-fn-28">Titan</a></code></pre>
> Spawn a titan
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitanAt(type: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-28">Titan</a></code></pre>
> Spawn a titan at a position
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitans(type: string, count: int) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-28">Titan</a>></code></pre>
> Spawn titans
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAsync(type: string, count: int)</code></pre>
> Spawn titans asynchronously
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAt(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-28">Titan</a>></code></pre>
> Spawn titans at a position
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAtAsync(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn titans at a position asynchronously
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../static/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifter(type: string) -> <a data-footnote-ref href="#user-content-fn-27">Shifter</a></code></pre>
> Spawn a shifter
> 
> **Parameters**:
> - `type`: The type of shifter to spawn. Refer to [ShifterTypeEnum](../static/ShifterTypeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifterAt(type: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-27">Shifter</a></code></pre>
> Spawn a shifter at a position
> 
> **Parameters**:
> - `type`: The type of shifter to spawn. Refer to [ShifterTypeEnum](../static/ShifterTypeEnum.md)
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectile(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, liveTime: float, team: string, extraParam: <a data-footnote-ref href="#user-content-fn-82">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-82">Object</a> = null)</code></pre>
> Spawn a projectile. Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Refer to [ProjectileNameEnum](../static/ProjectileNameEnum.md)
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `velocity`: Spawn velocity
> - `gravity`: Spawn gravity
> - `liveTime`: Live time of the projectile
> - `team`: The team that the projectile belongs to. Refer to [TeamEnum](../static/TeamEnum.md)
> - `extraParam`: Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused
> - `extraParam2`: Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectileWithOwner(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, liveTime: float, owner: <a data-footnote-ref href="#user-content-fn-21">Character</a>, extraParam: <a data-footnote-ref href="#user-content-fn-82">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-82">Object</a> = null)</code></pre>
> Spawn a projectile with an owner. Note: `extraParam` and `extraParam2` are optional. They may or may not be used depending on the value of `projectileName`
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Refer to [ProjectileNameEnum](../static/ProjectileNameEnum.md)
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `velocity`: Spawn velocity
> - `gravity`: Spawn gravity
> - `liveTime`: Live time of the projectile
> - `owner`: The character that the projectile belongs to
> - `extraParam`: Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused
> - `extraParam2`: Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, scale: float, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, tsKillSound: string = null)</code></pre>
> Spawn an effect
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [EffectNameEnum](../static/EffectNameEnum.md)
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `scale`: Spawn scale
> - `tsExplodeColor`: Thunderspear explode color (Only valid when effectName is "ThunderspearExplode")
> - `tsKillSound`: Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode"). Refer to [TSKillSoundEnum](../static/TSKillSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnUnscaledEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, tsKillSound: string = null)</code></pre>
> Spawn an unscaled effect
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [EffectNameEnum](../static/EffectNameEnum.md)
> - `position`: Spawn position
> - `rotation`: Spawn rotation
> - `tsExplodeColor`: Thunderspear explode color (Only valid when effectName is "ThunderspearExplode")
> - `tsKillSound`: Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode"). Refer to [TSKillSoundEnum](../static/TSKillSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayer(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>, force: bool)</code></pre>
> Spawn a player
> 
> **Parameters**:
> - `player`: The player to spawn.
> - `force`: If true, forces respawn even if the player is already alive.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAll(force: bool)</code></pre>
> Spawn a player for all players
> 
> **Parameters**:
> - `force`: If true, forces respawn even if players are already alive.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAt(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>, force: bool, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position
> 
> **Parameters**:
> - `player`: The player to spawn.
> - `force`: If true, forces respawn even if the player is already alive.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAtAll(force: bool, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position for all players
> 
> **Parameters**:
> - `force`: If true, forces respawn even if players are already alive.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPlaylist(playlist: string)</code></pre>
> Set the music playlist
> 
> **Parameters**:
> - `playlist`: The name of the playlist to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSong(song: string)</code></pre>
> Set the music song
> 
> **Parameters**:
> - `song`: The name of the song to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function DrawRay(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dir: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, color: <a data-footnote-ref href="#user-content-fn-0">Color</a>, duration: float)</code></pre>
> Draw a ray
> 
> **Parameters**:
> - `start`: The start position of the ray.
> - `dir`: The direction vector of the ray.
> - `color`: The color of the ray.
> - `duration`: The duration in seconds to display the ray.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillScore(damage: int)</code></pre>
> Show the kill score
> 
> **Parameters**:
> - `damage`: The damage value to display.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeed(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed
> 
> **Parameters**:
> - `killer`: The name of the killer.
> - `victim`: The name of the victim.
> - `score`: The score value.
> - `weapon`: The weapon name.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeedAll(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed for all players
> 
> **Parameters**:
> - `killer`: The name of the killer.
> - `victim`: The name of the victim.
> - `score`: The score value.
> - `weapon`: The weapon name.
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
[^31]: [CharacterTypeEnum](../static/CharacterTypeEnum.md)
[^32]: [CollideModeEnum](../static/CollideModeEnum.md)
[^33]: [CollideWithEnum](../static/CollideWithEnum.md)
[^34]: [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)
[^35]: [EffectNameEnum](../static/EffectNameEnum.md)
[^36]: [ForceModeEnum](../static/ForceModeEnum.md)
[^37]: [HandStateEnum](../static/HandStateEnum.md)
[^38]: [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
[^39]: [InputCategoryEnum](../static/InputCategoryEnum.md)
[^40]: [LanguageEnum](../static/LanguageEnum.md)
[^41]: [LoadoutEnum](../static/LoadoutEnum.md)
[^42]: [OutlineModeEnum](../static/OutlineModeEnum.md)
[^43]: [PhysicMaterialCombineEnum](../static/PhysicMaterialCombineEnum.md)
[^44]: [PlayerStatusEnum](../static/PlayerStatusEnum.md)
[^45]: [ProjectileNameEnum](../static/ProjectileNameEnum.md)
[^46]: [ScaleModeEnum](../static/ScaleModeEnum.md)
[^47]: [ShifterTypeEnum](../static/ShifterTypeEnum.md)
[^48]: [SliderDirectionEnum](../static/SliderDirectionEnum.md)
[^49]: [SteamStateEnum](../static/SteamStateEnum.md)
[^50]: [TeamEnum](../static/TeamEnum.md)
[^51]: [TitanTypeEnum](../static/TitanTypeEnum.md)
[^52]: [TSKillSoundEnum](../static/TSKillSoundEnum.md)
[^53]: [WeaponEnum](../static/WeaponEnum.md)
[^54]: [Camera](../static/Camera.md)
[^55]: [Cutscene](../static/Cutscene.md)
[^56]: [Game](../static/Game.md)
[^57]: [Input](../static/Input.md)
[^58]: [Locale](../static/Locale.md)
[^59]: [Map](../static/Map.md)
[^60]: [Network](../static/Network.md)
[^61]: [PersistentData](../static/PersistentData.md)
[^62]: [Physics](../static/Physics.md)
[^63]: [RoomData](../static/RoomData.md)
[^64]: [Time](../static/Time.md)
[^65]: [Button](../objects/Button.md)
[^66]: [Dropdown](../objects/Dropdown.md)
[^67]: [Icon](../objects/Icon.md)
[^68]: [Image](../objects/Image.md)
[^69]: [Label](../objects/Label.md)
[^70]: [ProgressBar](../objects/ProgressBar.md)
[^71]: [ScrollView](../objects/ScrollView.md)
[^72]: [Slider](../objects/Slider.md)
[^73]: [TextField](../objects/TextField.md)
[^74]: [Toggle](../objects/Toggle.md)
[^75]: [UI](../static/UI.md)
[^76]: [VisualElement](../objects/VisualElement.md)
[^77]: [Convert](../static/Convert.md)
[^78]: [Json](../static/Json.md)
[^79]: [Math](../static/Math.md)
[^80]: [Random](../objects/Random.md)
[^81]: [String](../static/String.md)
[^82]: [Object](../objects/Object.md)
[^83]: [Component](../objects/Component.md)
