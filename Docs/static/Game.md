# Game
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>
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
###### function <mark style="color:yellow;">Debug</mark>(message: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Print a debug statement to the console

###### function <mark style="color:yellow;">Print</mark>(message: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Print a message to the chat

###### function <mark style="color:yellow;">PrintAll</mark>(message: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">null</mark>
> Print a message to all players

###### function <mark style="color:yellow;">GetGeneralSetting</mark>(settingName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Object</mark>
> Get a general setting

###### function <mark style="color:yellow;">GetTitanSetting</mark>(settingName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Object</mark>
> Get a titan setting

###### function <mark style="color:yellow;">GetMiscSetting</mark>(settingName: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Object</mark>
> Get a misc setting

###### function <mark style="color:yellow;">End</mark>(delay: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">null</mark>
> End the game

###### function <mark style="color:yellow;">FindCharacterByViewID</mark>(viewID: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">[Character](../objects/Character.md)</mark>
> Find a character by view ID

###### function <mark style="color:yellow;">SpawnTitan</mark>(type: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[Titan](../objects/Titan.md)</mark>
> Spawn a titan

###### function <mark style="color:yellow;">SpawnTitanAt</mark>(type: <mark style="color:blue;">string</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">[Titan](../objects/Titan.md)</mark>
> Spawn a titan at a position

###### function <mark style="color:yellow;">SpawnTitans</mark>(type: <mark style="color:blue;">string</mark>, count: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Spawn titans

###### function <mark style="color:yellow;">SpawnTitansAsync</mark>(type: <mark style="color:blue;">string</mark>, count: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">null</mark>
> Spawn titans asynchronously

###### function <mark style="color:yellow;">SpawnTitansAt</mark>(type: <mark style="color:blue;">string</mark>, count: <mark style="color:blue;">int</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">[List](../objects/List.md)</mark>
> Spawn titans at a position

###### function <mark style="color:yellow;">SpawnTitansAtAsync</mark>(type: <mark style="color:blue;">string</mark>, count: <mark style="color:blue;">int</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">null</mark>
> Spawn titans at a position asynchronously

###### function <mark style="color:yellow;">SpawnShifter</mark>(type: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[Shifter](../objects/Shifter.md)</mark>
> Spawn a shifter

###### function <mark style="color:yellow;">SpawnShifterAt</mark>(type: <mark style="color:blue;">string</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">[Shifter](../objects/Shifter.md)</mark>
> Spawn a shifter at a position

###### function <mark style="color:yellow;">SpawnProjectile</mark>(parameters: <mark style="color:blue;">Object[]</mark>) → <mark style="color:blue;">null</mark>
> Spawn a projectile

###### function <mark style="color:yellow;">SpawnProjectileWithOwner</mark>(parameters: <mark style="color:blue;">Object[]</mark>) → <mark style="color:blue;">null</mark>
> Spawn a projectile with an owner

###### function <mark style="color:yellow;">SpawnEffect</mark>(parameters: <mark style="color:blue;">Object[]</mark>) → <mark style="color:blue;">null</mark>
> Spawn an effect

###### function <mark style="color:yellow;">SpawnPlayer</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, force: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Spawn a player

###### function <mark style="color:yellow;">SpawnPlayerAll</mark>(force: <mark style="color:blue;">bool</mark>) → <mark style="color:blue;">null</mark>
> Spawn a player for all players

###### function <mark style="color:yellow;">SpawnPlayerAt</mark>(player: <mark style="color:blue;">[Player](../objects/Player.md)</mark>, force: <mark style="color:blue;">bool</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">null</mark>
> Spawn a player at a position

###### function <mark style="color:yellow;">SpawnPlayerAtAll</mark>(force: <mark style="color:blue;">bool</mark>, position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, rotationY: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0</mark>) → <mark style="color:blue;">null</mark>
> Spawn a player at a position for all players

###### function <mark style="color:yellow;">SetPlaylist</mark>(playlist: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Set the music playlist

###### function <mark style="color:yellow;">SetSong</mark>(song: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Set the music song

###### function <mark style="color:yellow;">DrawRay</mark>(start: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, dir: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, color: <mark style="color:blue;">[Color](../objects/Color.md)</mark>, duration: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">null</mark>
> Draw a ray

###### function <mark style="color:yellow;">ShowKillScore</mark>(damage: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">null</mark>
> Show the kill score

###### function <mark style="color:yellow;">ShowKillFeed</mark>(killer: <mark style="color:blue;">string</mark>, victim: <mark style="color:blue;">string</mark>, score: <mark style="color:blue;">int</mark>, weapon: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Show the kill feed

###### function <mark style="color:yellow;">ShowKillFeedAll</mark>(killer: <mark style="color:blue;">string</mark>, victim: <mark style="color:blue;">string</mark>, score: <mark style="color:blue;">int</mark>, weapon: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">null</mark>
> Show the kill feed for all players


---

