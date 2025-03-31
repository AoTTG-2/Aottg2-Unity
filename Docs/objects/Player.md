# Player
Inherits from object
## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Character|[Character](../objects/Character.md)|True|Player's current character, if alive.|
|Connected|bool|True|Player is still connected to the room.|
|ID|int|True|Player unique ID.|
|Name|string|True|Player name.|
|Guild|string|True|Player guild.|
|Team|string|True|Player's chosen team ("None", "Blue", "Red", "Titan", "Human"). Note that this may be different from the character's final team (Character.Team field) if the character's team field is modified.|
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
## Methods
#### function <mark style="color:yellow;">GetCustomProperty</mark>(property: <mark style="color:blue;">string</mark>) 薔 <mark style="color:blue;">Object</mark>
> Get a custom property at given key. Must be a primitive type. This is synced to all clients.

#### function <mark style="color:yellow;">SetCustomProperty</mark>(property: <mark style="color:blue;">string</mark>, value: <mark style="color:blue;">Object</mark>) 薔 <mark style="color:blue;">null</mark>
> Sets a custom property at given key. Must be a primitive type. This is synced to all clients.

#### function <mark style="color:yellow;">ClearKDR</mark>() 薔 <mark style="color:blue;">null</mark>
> Clears kills, deaths, highestdamage, and totaldamage properties.


---

