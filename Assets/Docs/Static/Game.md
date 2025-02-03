# Game
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|False|Is the game ending?|
|EndTimeLeft|float|False|Time left until the game ends|
|Titans|[List](../Object/List.md)|False|List of all titans|
|AITitans|[List](../Object/List.md)|False|List of all AI titans|
|PlayerTitans|[List](../Object/List.md)|False|List of all player titans|
|Shifters|[List](../Object/List.md)|False|List of all shifters|
|AIShifters|[List](../Object/List.md)|False|List of all AI shifters|
|PlayerShifters|[List](../Object/List.md)|False|List of all player shifters|
|Humans|[List](../Object/List.md)|False|List of all humans|
|AIHumans|[List](../Object/List.md)|False|List of all AI humans|
|PlayerHumans|[List](../Object/List.md)|False|List of all player humans|
|Loadouts|[List](../Object/List.md)|False|List of all loadouts|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|[String](../Static/String.md)|False|Forced character type|
|ForcedLoadout|[String](../Static/String.md)|False|Forced loadout|
## Methods
|Function|Returns|Description|
|---|---|---|
|Debug(message : Object)|none|Print a debug statement to the console|
|Print(message : Object)|none|Print a message to the chat|
|PrintAll(message : Object)|none|Print a message to all players|
|GetGeneralSetting(settingName : [String](../Static/String.md))|Object|Get a general setting|
|GetTitanSetting(settingName : [String](../Static/String.md))|Object|Get a titan setting|
|GetMiscSetting(settingName : [String](../Static/String.md))|Object|Get a misc setting|
|End(delay : float)|none|End the game|
|FindCharacterByViewID(viewID : int)|[Character](../Object/Character.md)|Find a character by view ID|
|SpawnTitan(type : [String](../Static/String.md))|[Titan](../Object/Titan.md)|Spawn a titan|
|SpawnTitanAt(type : [String](../Static/String.md), position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|[Titan](../Object/Titan.md)|Spawn a titan at a position|
|SpawnTitans(type : [String](../Static/String.md), count : int)|[List](../Object/List.md)|Spawn titans|
|SpawnTitansAsync(type : [String](../Static/String.md), count : int)|none|Spawn titans asynchronously|
|SpawnTitansAt(type : [String](../Static/String.md), count : int, position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|[List](../Object/List.md)|Spawn titans at a position|
|SpawnTitansAtAsync(type : [String](../Static/String.md), count : int, position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|none|Spawn titans at a position asynchronously|
|SpawnShifter(type : [String](../Static/String.md))|[Shifter](../Object/Shifter.md)|Spawn a shifter|
|SpawnShifterAt(type : [String](../Static/String.md), position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|[Shifter](../Object/Shifter.md)|Spawn a shifter at a position|
|SpawnProjectile(parameters : Object[])|none|Spawn a projectile|
|SpawnProjectileWithOwner(parameters : Object[])|none|Spawn a projectile with an owner|
|SpawnEffect(parameters : Object[])|none|Spawn an effect|
|SpawnPlayer(player : [Player](../Object/Player.md), force : bool)|none|Spawn a player|
|SpawnPlayerAll(force : bool)|none|Spawn a player for all players|
|SpawnPlayerAt(player : [Player](../Object/Player.md), force : bool, position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|none|Spawn a player at a position|
|SpawnPlayerAtAll(force : bool, position : [Vector3](../Static/Vector3.md), rotationY : float = 0)|none|Spawn a player at a position for all players|
|SetPlaylist(playlist : [String](../Static/String.md))|none|Set the music playlist|
|SetSong(song : [String](../Static/String.md))|none|Set the music song|
|DrawRay(start : [Vector3](../Static/Vector3.md), dir : [Vector3](../Static/Vector3.md), color : [Color](../Static/Color.md), duration : float)|none|Draw a ray|
|ShowKillScore(damage : int)|none|Show the kill score|
|ShowKillFeed(killer : [String](../Static/String.md), victim : [String](../Static/String.md), score : int, weapon : [String](../Static/String.md))|none|Show the kill feed|
|ShowKillFeedAll(killer : [String](../Static/String.md), victim : [String](../Static/String.md), score : int, weapon : [String](../Static/String.md))|none|Show the kill feed for all players|
