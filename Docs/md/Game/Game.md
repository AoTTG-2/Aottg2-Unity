# Game
Inherits from [Object](../objects/Object.md)

Game functions such as spawning titans and managing game state.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|True|Is the game ending?|
|EndTimeLeft|float|True|Time left until the game ends.|
|Titans|[List](../Collections/List.md)<[Titan](../Entities/Titan.md)>|True|List of all titans.|
|AITitans|[List](../Collections/List.md)<[Titan](../Entities/Titan.md)>|True|List of all AI titans.|
|PlayerTitans|[List](../Collections/List.md)<[Titan](../Entities/Titan.md)>|True|List of all player titans.|
|Shifters|[List](../Collections/List.md)<[Shifter](../Entities/Shifter.md)>|True|List of all shifters.|
|AIShifters|[List](../Collections/List.md)<[Shifter](../Entities/Shifter.md)>|True|List of all AI shifters.|
|PlayerShifters|[List](../Collections/List.md)<[Shifter](../Entities/Shifter.md)>|True|List of all player shifters.|
|Humans|[List](../Collections/List.md)<[Human](../Entities/Human.md)>|True|List of all humans.|
|AIHumans|[List](../Collections/List.md)<[Human](../Entities/Human.md)>|True|List of all AI humans.|
|PlayerHumans|[List](../Collections/List.md)<[Human](../Entities/Human.md)>|True|List of all player humans.|
|Loadouts|[List](../Collections/List.md)<string>|True|List of all loadouts.|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|string|False|Forced character type. Refer to [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)|
|ForcedLoadout|string|False|Forced loadout. Refer to [LoadoutEnum](../Enums/LoadoutEnum.md)|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Debug(message: <a data-footnote-ref href="#user-content-fn-116">Object</a>)</code></pre>
> Print a debug statement to the console.
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function Print(message: <a data-footnote-ref href="#user-content-fn-116">Object</a>)</code></pre>
> Print a message to the chat.
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function PrintAll(message: <a data-footnote-ref href="#user-content-fn-116">Object</a>)</code></pre>
> Print a message to all players.
> 
> **Parameters**:
> - `message`: The message to print.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetGeneralSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get a general setting.
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
> **Returns**: The setting value.
<pre class="language-typescript"><code class="lang-typescript">function GetTitanSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get a titan setting.
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
> **Returns**: The setting value.
<pre class="language-typescript"><code class="lang-typescript">function GetMiscSetting(settingName: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get a misc setting.
> 
> **Parameters**:
> - `settingName`: The name of the setting to get.
> 
> **Returns**: The setting value.
<pre class="language-typescript"><code class="lang-typescript">function End(delay: float)</code></pre>
> End the game.
> 
> **Parameters**:
> - `delay`: The delay in seconds before ending the game.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindCharacterByViewID(viewID: int) -> <a data-footnote-ref href="#user-content-fn-21">Character</a></code></pre>
> Find a character by view ID.
> 
> **Parameters**:
> - `viewID`: The Photon view ID of the character.
> 
> **Returns**: The character if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitan(type: string) -> <a data-footnote-ref href="#user-content-fn-28">Titan</a></code></pre>
> Spawn a titan.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> 
> **Returns**: The spawned titan, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitanAt(type: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-28">Titan</a></code></pre>
> Spawn a titan at a position.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
> **Returns**: The spawned titan, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitans(type: string, count: int) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-28">Titan</a>></code></pre>
> Spawn titans.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> 
> **Returns**: A list of spawned titans, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAsync(type: string, count: int)</code></pre>
> Spawn titans asynchronously.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAt(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-28">Titan</a>></code></pre>
> Spawn titans at a position.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
> **Returns**: A list of spawned titans, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnTitansAtAsync(type: string, count: int, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn titans at a position asynchronously.
> 
> **Parameters**:
> - `type`: The type of titan to spawn. Refer to [TitanTypeEnum](../Enums/TitanTypeEnum.md)
> - `count`: The number of titans to spawn.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifter(type: string) -> <a data-footnote-ref href="#user-content-fn-27">Shifter</a></code></pre>
> Spawn a shifter.
> 
> **Parameters**:
> - `type`: The type of shifter to spawn. Refer to [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
> 
> **Returns**: The spawned shifter, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnShifterAt(type: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0) -> <a data-footnote-ref href="#user-content-fn-27">Shifter</a></code></pre>
> Spawn a shifter at a position.
> 
> **Parameters**:
> - `type`: The type of shifter to spawn. Refer to [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
> **Returns**: The spawned shifter, or null if not master client.
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectile(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, liveTime: float, team: string, extraParam: <a data-footnote-ref href="#user-content-fn-116">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-116">Object</a> = null)</code></pre>
> Spawn a projectile. Note: `extraParam` and `extraParam2` are optional.
They may or may not be used depending on the value of `projectileName`.
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Refer to [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
> - `position`: Spawn position.
> - `rotation`: Spawn rotation.
> - `velocity`: Spawn velocity.
> - `gravity`: Spawn gravity.
> - `liveTime`: Live time of the projectile.
> - `team`: The team that the projectile belongs to. Refer to [TeamEnum](../Enums/TeamEnum.md)
> - `extraParam`: Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused.
> - `extraParam2`: Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnProjectileWithOwner(projectileName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, velocity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, gravity: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, liveTime: float, owner: <a data-footnote-ref href="#user-content-fn-21">Character</a>, extraParam: <a data-footnote-ref href="#user-content-fn-116">Object</a> = null, extraParam2: <a data-footnote-ref href="#user-content-fn-116">Object</a> = null)</code></pre>
> Spawn a projectile with an owner. Note: `extraParam` and `extraParam2` are optional.
They may or may not be used depending on the value of `projectileName`.
> 
> **Parameters**:
> - `projectileName`: Name of the projectile. Refer to [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
> - `position`: Spawn position.
> - `rotation`: Spawn rotation.
> - `velocity`: Spawn velocity.
> - `gravity`: Spawn gravity.
> - `liveTime`: Live time of the projectile.
> - `owner`: The character that the projectile belongs to.
> - `extraParam`: Optional. Type depends on projectile: Thunderspear: float (explosion radius), Flare: Color (flare color), Rock1: float (rock size), Others: unused.
> - `extraParam2`: Optional. Type depends on projectile: Thunderspear: Color (projectile color), Others: unused.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, scale: float, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, tsKillSound: string = null)</code></pre>
> Spawn an effect.
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [EffectNameEnum](../Enums/EffectNameEnum.md)
> - `position`: Spawn position.
> - `rotation`: Spawn rotation.
> - `scale`: Spawn scale.
> - `tsExplodeColor`: Thunderspear explode color (Only valid when effectName is "ThunderspearExplode").
> - `tsKillSound`: Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode"). Refer to [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnUnscaledEffect(effectName: string, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, tsExplodeColor: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, tsKillSound: string = null)</code></pre>
> Spawn an unscaled effect.
> 
> **Parameters**:
> - `effectName`: Name of the effect. Refer to [EffectNameEnum](../Enums/EffectNameEnum.md)
> - `position`: Spawn position.
> - `rotation`: Spawn rotation.
> - `tsExplodeColor`: Thunderspear explode color (Only valid when effectName is "ThunderspearExplode").
> - `tsKillSound`: Optional. Thunderspear explode sound (Only valid when effectName is "ThunderspearExplode"). Refer to [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayer(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>, force: bool)</code></pre>
> Spawn a player.
> 
> **Parameters**:
> - `player`: The player to spawn.
> - `force`: If true, forces respawn even if the player is already alive.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAll(force: bool)</code></pre>
> Spawn a player for all players.
> 
> **Parameters**:
> - `force`: If true, forces respawn even if players are already alive.
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAt(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>, force: bool, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position.
> 
> **Parameters**:
> - `player`: The player to spawn.
> - `force`: If true, forces respawn even if the player is already alive.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SpawnPlayerAtAll(force: bool, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationY: float = 0)</code></pre>
> Spawn a player at a position for all players.
> 
> **Parameters**:
> - `force`: If true, forces respawn even if players are already alive.
> - `position`: The spawn position.
> - `rotationY`: The Y rotation in degrees (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPlaylist(playlist: string)</code></pre>
> Set the music playlist.
> 
> **Parameters**:
> - `playlist`: The name of the playlist to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetSong(song: string)</code></pre>
> Set the music song.
> 
> **Parameters**:
> - `song`: The name of the song to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function DrawRay(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dir: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, color: <a data-footnote-ref href="#user-content-fn-0">Color</a>, duration: float)</code></pre>
> Draw a ray.
> 
> **Parameters**:
> - `start`: The start position of the ray.
> - `dir`: The direction vector of the ray.
> - `color`: The color of the ray.
> - `duration`: The duration in seconds to display the ray.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillScore(damage: int)</code></pre>
> Show the kill score.
> 
> **Parameters**:
> - `damage`: The damage value to display.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeed(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed.
> 
> **Parameters**:
> - `killer`: The name of the killer.
> - `victim`: The name of the victim.
> - `score`: The score value.
> - `weapon`: The weapon name.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowKillFeedAll(killer: string, victim: string, score: int, weapon: string)</code></pre>
> Show the kill feed for all players.
> 
> **Parameters**:
> - `killer`: The name of the killer.
> - `victim`: The name of the victim.
> - `score`: The score value.
> - `weapon`: The weapon name.
> 

[^0]: [Color](../Collections/Color.md)
[^1]: [Dict](../Collections/Dict.md)
[^2]: [LightBuiltin](../Collections/LightBuiltin.md)
[^3]: [LineCastHitResult](../Collections/LineCastHitResult.md)
[^4]: [List](../Collections/List.md)
[^5]: [Quaternion](../Collections/Quaternion.md)
[^6]: [Range](../Collections/Range.md)
[^7]: [Set](../Collections/Set.md)
[^8]: [Vector2](../Collections/Vector2.md)
[^9]: [Vector3](../Collections/Vector3.md)
[^10]: [Animation](../Component/Animation.md)
[^11]: [Animator](../Component/Animator.md)
[^12]: [AudioSource](../Component/AudioSource.md)
[^13]: [Collider](../Component/Collider.md)
[^14]: [Collision](../Component/Collision.md)
[^15]: [LineRenderer](../Component/LineRenderer.md)
[^16]: [LodBuiltin](../Component/LodBuiltin.md)
[^17]: [MapTargetable](../Component/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../Component/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../Component/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)
[^21]: [Character](../Entities/Character.md)
[^22]: [Human](../Entities/Human.md)
[^23]: [MapObject](../Entities/MapObject.md)
[^24]: [NetworkView](../Entities/NetworkView.md)
[^25]: [Player](../Entities/Player.md)
[^26]: [Prefab](../Entities/Prefab.md)
[^27]: [Shifter](../Entities/Shifter.md)
[^28]: [Titan](../Entities/Titan.md)
[^29]: [Transform](../Entities/Transform.md)
[^30]: [WallColossal](../Entities/WallColossal.md)
[^31]: [AlignEnum](../Enums/AlignEnum.md)
[^32]: [AngleUnitEnum](../Enums/AngleUnitEnum.md)
[^33]: [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md)
[^34]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^35]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^36]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^37]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^38]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^39]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^40]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^41]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^42]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^43]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^44]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^45]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^46]: [HandStateEnum](../Enums/HandStateEnum.md)
[^47]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^48]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^49]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^50]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^51]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^52]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^53]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^54]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^55]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^56]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^57]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^58]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^59]: [JustifyEnum](../Enums/JustifyEnum.md)
[^60]: [LanguageEnum](../Enums/LanguageEnum.md)
[^61]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^62]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^63]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^64]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^65]: [OverflowEnum](../Enums/OverflowEnum.md)
[^66]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^67]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^68]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^69]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^70]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^71]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^72]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^73]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^74]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^75]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^76]: [SpecialEnum](../Enums/SpecialEnum.md)
[^77]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^78]: [TeamEnum](../Enums/TeamEnum.md)
[^79]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^80]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^81]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^82]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^83]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^84]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^85]: [UILabelEnum](../Enums/UILabelEnum.md)
[^86]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^87]: [WeaponEnum](../Enums/WeaponEnum.md)
[^88]: [Camera](../Game/Camera.md)
[^89]: [Cutscene](../Game/Cutscene.md)
[^90]: [Game](../Game/Game.md)
[^91]: [Input](../Game/Input.md)
[^92]: [Locale](../Game/Locale.md)
[^93]: [Map](../Game/Map.md)
[^94]: [Network](../Game/Network.md)
[^95]: [PersistentData](../Game/PersistentData.md)
[^96]: [Physics](../Game/Physics.md)
[^97]: [RoomData](../Game/RoomData.md)
[^98]: [Time](../Game/Time.md)
[^99]: [Button](../UIElements/Button.md)
[^100]: [Dropdown](../UIElements/Dropdown.md)
[^101]: [Icon](../UIElements/Icon.md)
[^102]: [Image](../UIElements/Image.md)
[^103]: [Label](../UIElements/Label.md)
[^104]: [ProgressBar](../UIElements/ProgressBar.md)
[^105]: [ScrollView](../UIElements/ScrollView.md)
[^106]: [Slider](../UIElements/Slider.md)
[^107]: [TextField](../UIElements/TextField.md)
[^108]: [Toggle](../UIElements/Toggle.md)
[^109]: [UI](../UIElements/UI.md)
[^110]: [VisualElement](../UIElements/VisualElement.md)
[^111]: [Convert](../Utility/Convert.md)
[^112]: [Json](../Utility/Json.md)
[^113]: [Math](../Utility/Math.md)
[^114]: [Random](../Utility/Random.md)
[^115]: [String](../Utility/String.md)
[^116]: [Object](../objects/Object.md)
[^117]: [Component](../objects/Component.md)
