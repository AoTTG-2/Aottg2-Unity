# Game
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Fields
|Field|Type|Readonly|Description|
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
## Methods
#### function <span style="color:yellow;">Debug</span>(message: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Print a debug statement to the console

#### function <span style="color:yellow;">Print</span>(message: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Print a message to the chat

#### function <span style="color:yellow;">PrintAll</span>(message: <span style="color:blue;">Object</span>) → <span style="color:blue;">null</span>
> Print a message to all players

#### function <span style="color:yellow;">GetGeneralSetting</span>(settingName: <span style="color:blue;">string</span>) → <span style="color:blue;">Object</span>
> Get a general setting

#### function <span style="color:yellow;">GetTitanSetting</span>(settingName: <span style="color:blue;">string</span>) → <span style="color:blue;">Object</span>
> Get a titan setting

#### function <span style="color:yellow;">GetMiscSetting</span>(settingName: <span style="color:blue;">string</span>) → <span style="color:blue;">Object</span>
> Get a misc setting

#### function <span style="color:yellow;">End</span>(delay: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> End the game

#### function <span style="color:yellow;">FindCharacterByViewID</span>(viewID: <span style="color:blue;">int</span>) → <span style="color:blue;">[Character](../objects/Character.md)</span>
> Find a character by view ID

#### function <span style="color:yellow;">SpawnTitan</span>(type: <span style="color:blue;">string</span>) → <span style="color:blue;">[Titan](../objects/Titan.md)</span>
> Spawn a titan

#### function <span style="color:yellow;">SpawnTitanAt</span>(type: <span style="color:blue;">string</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">[Titan](../objects/Titan.md)</span>
> Spawn a titan at a position

#### function <span style="color:yellow;">SpawnTitans</span>(type: <span style="color:blue;">string</span>, count: <span style="color:blue;">int</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Spawn titans

#### function <span style="color:yellow;">SpawnTitansAsync</span>(type: <span style="color:blue;">string</span>, count: <span style="color:blue;">int</span>) → <span style="color:blue;">null</span>
> Spawn titans asynchronously

#### function <span style="color:yellow;">SpawnTitansAt</span>(type: <span style="color:blue;">string</span>, count: <span style="color:blue;">int</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">[List](../objects/List.md)</span>
> Spawn titans at a position

#### function <span style="color:yellow;">SpawnTitansAtAsync</span>(type: <span style="color:blue;">string</span>, count: <span style="color:blue;">int</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">null</span>
> Spawn titans at a position asynchronously

#### function <span style="color:yellow;">SpawnShifter</span>(type: <span style="color:blue;">string</span>) → <span style="color:blue;">[Shifter](../objects/Shifter.md)</span>
> Spawn a shifter

#### function <span style="color:yellow;">SpawnShifterAt</span>(type: <span style="color:blue;">string</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">[Shifter](../objects/Shifter.md)</span>
> Spawn a shifter at a position

#### function <span style="color:yellow;">SpawnProjectile</span>(parameters: <span style="color:blue;">Object[]</span>) → <span style="color:blue;">null</span>
> Spawn a projectile

#### function <span style="color:yellow;">SpawnProjectileWithOwner</span>(parameters: <span style="color:blue;">Object[]</span>) → <span style="color:blue;">null</span>
> Spawn a projectile with an owner

#### function <span style="color:yellow;">SpawnEffect</span>(parameters: <span style="color:blue;">Object[]</span>) → <span style="color:blue;">null</span>
> Spawn an effect

#### function <span style="color:yellow;">SpawnPlayer</span>(player: <span style="color:blue;">[Player](../objects/Player.md)</span>, force: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Spawn a player

#### function <span style="color:yellow;">SpawnPlayerAll</span>(force: <span style="color:blue;">bool</span>) → <span style="color:blue;">null</span>
> Spawn a player for all players

#### function <span style="color:yellow;">SpawnPlayerAt</span>(player: <span style="color:blue;">[Player](../objects/Player.md)</span>, force: <span style="color:blue;">bool</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">null</span>
> Spawn a player at a position

#### function <span style="color:yellow;">SpawnPlayerAtAll</span>(force: <span style="color:blue;">bool</span>, position: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, rotationY: <span style="color:blue;">float</span> = <span style="color:blue;">0</span>) → <span style="color:blue;">null</span>
> Spawn a player at a position for all players

#### function <span style="color:yellow;">SetPlaylist</span>(playlist: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Set the music playlist

#### function <span style="color:yellow;">SetSong</span>(song: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Set the music song

#### function <span style="color:yellow;">DrawRay</span>(start: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, dir: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, color: <span style="color:blue;">[Color](../objects/Color.md)</span>, duration: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Draw a ray

#### function <span style="color:yellow;">ShowKillScore</span>(damage: <span style="color:blue;">int</span>) → <span style="color:blue;">null</span>
> Show the kill score

#### function <span style="color:yellow;">ShowKillFeed</span>(killer: <span style="color:blue;">string</span>, victim: <span style="color:blue;">string</span>, score: <span style="color:blue;">int</span>, weapon: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Show the kill feed

#### function <span style="color:yellow;">ShowKillFeedAll</span>(killer: <span style="color:blue;">string</span>, victim: <span style="color:blue;">string</span>, score: <span style="color:blue;">int</span>, weapon: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Show the kill feed for all players


---

