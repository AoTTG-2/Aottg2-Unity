# Player
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Character|[Character](../Object/Character.md)|False|Player's current character, if alive.|
|Connected|bool|False|Player is still connected to the room.|
|ID|int|False|Player unique ID.|
|Name|[String](../Static/String.md)|False|Player name.|
|Guild|[String](../Static/String.md)|False|Player guild.|
|Team|[String](../Static/String.md)|False|Player's chosen team ("None", "Blue", "Red", "Titan", "Human").             Note that this may be different from the character's final team (Character.Team field) if the character's team field is modified.|
|Status|[String](../Static/String.md)|False|Player's spawn status ("Alive", "Dead", "Spectating").|
|CharacterType|[String](../Static/String.md)|False|Player's chosen character ("Human", "Titan", "Shifter")|
|Loadout|[String](../Static/String.md)|False|Player's chosen loadout ("Blades", "AHSS", "APG", "Thunderspears").|
|Kills|int|False|Player's kills.|
|Deaths|int|False|Player's deaths.|
|HighestDamage|int|False|Player's highest damage.|
|TotalDamage|int|False|Player's total damage.|
|Ping|int|False|The player's connection ping.|
|SpectateID|int|False|The player's spectating ID. If not spectating anyone, returns -1.|
|SpawnPoint|[Vector3](../Static/Vector3.md)|False|Player's respawn point. Is initially null and can be set back to null, at which point map spawn points are used.|
## Methods
|Function|Returns|Description|
|---|---|---|
|GetCustomProperty(property : [String](../Static/String.md))|Object|Get a custom property at given key. Must be a primitive type. This is synced to all clients.|
|SetCustomProperty(property : [String](../Static/String.md), value : Object)|none|Sets a custom property at given key. Must be a primitive type. This is synced to all clients.|
|ClearKDR()|none|Clears kills, deaths, highestdamage, and totaldamage properties.|
