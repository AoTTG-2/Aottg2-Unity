# Game
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|IsEnding|bool|False|Is the game ending?|
|EndTimeLeft|float|False|Time left until the game ends|
|Titans|[List](../objects/List.md)|False|List of all titans|
|AITitans|[List](../objects/List.md)|False|List of all AI titans|
|PlayerTitans|[List](../objects/List.md)|False|List of all player titans|
|Shifters|[List](../objects/List.md)|False|List of all shifters|
|AIShifters|[List](../objects/List.md)|False|List of all AI shifters|
|PlayerShifters|[List](../objects/List.md)|False|List of all player shifters|
|Humans|[List](../objects/List.md)|False|List of all humans|
|AIHumans|[List](../objects/List.md)|False|List of all AI humans|
|PlayerHumans|[List](../objects/List.md)|False|List of all player humans|
|Loadouts|[List](../objects/List.md)|False|List of all loadouts|
|DefaultShowKillScore|bool|False|Is the kill score shown by default?|
|DefaultHideKillScore|bool|False|Is the kill feed shown by default?|
|DefaultAddKillScore|bool|False|Is the kill score added by default?|
|ShowScoreboardLoadout|bool|False|Is the loadout shown in the scoreboard?|
|ShowScoreboardStatus|bool|False|Is the status shown in the scoreboard?|
|ForcedCharacterType|[String](../static/String.md)|False|Forced character type|
|ForcedLoadout|[String](../static/String.md)|False|Forced loadout|
## Methods
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>Debug(message : Object)</td>
<td>none</td>
<td>Print a debug statement to the console</td>
</tr>
<tr>
<td>Print(message : Object)</td>
<td>none</td>
<td>Print a message to the chat</td>
</tr>
<tr>
<td>PrintAll(message : Object)</td>
<td>none</td>
<td>Print a message to all players</td>
</tr>
<tr>
<td>GetGeneralSetting(settingName : [String](../static/String.md))</td>
<td>Object</td>
<td>Get a general setting</td>
</tr>
<tr>
<td>GetTitanSetting(settingName : [String](../static/String.md))</td>
<td>Object</td>
<td>Get a titan setting</td>
</tr>
<tr>
<td>GetMiscSetting(settingName : [String](../static/String.md))</td>
<td>Object</td>
<td>Get a misc setting</td>
</tr>
<tr>
<td>End(delay : float)</td>
<td>none</td>
<td>End the game</td>
</tr>
<tr>
<td>FindCharacterByViewID(viewID : int)</td>
<td>[Character](../objects/Character.md)</td>
<td>Find a character by view ID</td>
</tr>
<tr>
<td>SpawnTitan(type : [String](../static/String.md))</td>
<td>[Titan](../objects/Titan.md)</td>
<td>Spawn a titan</td>
</tr>
<tr>
<td>SpawnTitanAt(type : [String](../static/String.md),position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>[Titan](../objects/Titan.md)</td>
<td>Spawn a titan at a position</td>
</tr>
<tr>
<td>SpawnTitans(type : [String](../static/String.md),count : int)</td>
<td>[List](../objects/List.md)</td>
<td>Spawn titans</td>
</tr>
<tr>
<td>SpawnTitansAsync(type : [String](../static/String.md),count : int)</td>
<td>none</td>
<td>Spawn titans asynchronously</td>
</tr>
<tr>
<td>SpawnTitansAt(type : [String](../static/String.md),count : int,position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>[List](../objects/List.md)</td>
<td>Spawn titans at a position</td>
</tr>
<tr>
<td>SpawnTitansAtAsync(type : [String](../static/String.md),count : int,position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>none</td>
<td>Spawn titans at a position asynchronously</td>
</tr>
<tr>
<td>SpawnShifter(type : [String](../static/String.md))</td>
<td>[Shifter](../objects/Shifter.md)</td>
<td>Spawn a shifter</td>
</tr>
<tr>
<td>SpawnShifterAt(type : [String](../static/String.md),position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>[Shifter](../objects/Shifter.md)</td>
<td>Spawn a shifter at a position</td>
</tr>
<tr>
<td>SpawnProjectile(parameters : Object[])</td>
<td>none</td>
<td>Spawn a projectile</td>
</tr>
<tr>
<td>SpawnProjectileWithOwner(parameters : Object[])</td>
<td>none</td>
<td>Spawn a projectile with an owner</td>
</tr>
<tr>
<td>SpawnEffect(parameters : Object[])</td>
<td>none</td>
<td>Spawn an effect</td>
</tr>
<tr>
<td>SpawnPlayer(player : [Player](../objects/Player.md),force : bool)</td>
<td>none</td>
<td>Spawn a player</td>
</tr>
<tr>
<td>SpawnPlayerAll(force : bool)</td>
<td>none</td>
<td>Spawn a player for all players</td>
</tr>
<tr>
<td>SpawnPlayerAt(player : [Player](../objects/Player.md),force : bool,position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>none</td>
<td>Spawn a player at a position</td>
</tr>
<tr>
<td>SpawnPlayerAtAll(force : bool,position : [Vector3](../objects/Vector3.md),rotationY : float = 0)</td>
<td>none</td>
<td>Spawn a player at a position for all players</td>
</tr>
<tr>
<td>SetPlaylist(playlist : [String](../static/String.md))</td>
<td>none</td>
<td>Set the music playlist</td>
</tr>
<tr>
<td>SetSong(song : [String](../static/String.md))</td>
<td>none</td>
<td>Set the music song</td>
</tr>
<tr>
<td>DrawRay(start : [Vector3](../objects/Vector3.md),dir : [Vector3](../objects/Vector3.md),color : [Color](../objects/Color.md),duration : float)</td>
<td>none</td>
<td>Draw a ray</td>
</tr>
<tr>
<td>ShowKillScore(damage : int)</td>
<td>none</td>
<td>Show the kill score</td>
</tr>
<tr>
<td>ShowKillFeed(killer : [String](../static/String.md),victim : [String](../static/String.md),score : int,weapon : [String](../static/String.md))</td>
<td>none</td>
<td>Show the kill feed</td>
</tr>
<tr>
<td>ShowKillFeedAll(killer : [String](../static/String.md),victim : [String](../static/String.md),score : int,weapon : [String](../static/String.md))</td>
<td>none</td>
<td>Show the kill feed for all players</td>
</tr>
</tbody>
</table>
