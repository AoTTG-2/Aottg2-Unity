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
#### void Debug(Object message)
- **Description:** Print a debug statement to the console

---

#### void Print(Object message)
- **Description:** Print a message to the chat

---

#### void PrintAll(Object message)
- **Description:** Print a message to all players

---

#### Object GetGeneralSetting([String](../static/String.md) settingName)
- **Description:** Get a general setting

---

#### Object GetTitanSetting([String](../static/String.md) settingName)
- **Description:** Get a titan setting

---

#### Object GetMiscSetting([String](../static/String.md) settingName)
- **Description:** Get a misc setting

---

#### void End(float delay)
- **Description:** End the game

---

#### [Character](../objects/Character.md) FindCharacterByViewID(int viewID)
- **Description:** Find a character by view ID

---

#### [Titan](../objects/Titan.md) SpawnTitan([String](../static/String.md) type)
- **Description:** Spawn a titan

---

#### [Titan](../objects/Titan.md) SpawnTitanAt([String](../static/String.md) type, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn a titan at a position

---

#### [List](../objects/List.md) SpawnTitans([String](../static/String.md) type, int count)
- **Description:** Spawn titans

---

#### void SpawnTitansAsync([String](../static/String.md) type, int count)
- **Description:** Spawn titans asynchronously

---

#### [List](../objects/List.md) SpawnTitansAt([String](../static/String.md) type, int count, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn titans at a position

---

#### void SpawnTitansAtAsync([String](../static/String.md) type, int count, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn titans at a position asynchronously

---

#### [Shifter](../objects/Shifter.md) SpawnShifter([String](../static/String.md) type)
- **Description:** Spawn a shifter

---

#### [Shifter](../objects/Shifter.md) SpawnShifterAt([String](../static/String.md) type, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn a shifter at a position

---

#### void SpawnProjectile(Object[] parameters)
- **Description:** Spawn a projectile

---

#### void SpawnProjectileWithOwner(Object[] parameters)
- **Description:** Spawn a projectile with an owner

---

#### void SpawnEffect(Object[] parameters)
- **Description:** Spawn an effect

---

#### void SpawnPlayer([Player](../objects/Player.md) player, bool force)
- **Description:** Spawn a player

---

#### void SpawnPlayerAll(bool force)
- **Description:** Spawn a player for all players

---

#### void SpawnPlayerAt([Player](../objects/Player.md) player, bool force, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn a player at a position

---

#### void SpawnPlayerAtAll(bool force, [Vector3](../objects/Vector3.md) position, float rotationY = 0)
- **Description:** Spawn a player at a position for all players

---

#### void SetPlaylist([String](../static/String.md) playlist)
- **Description:** Set the music playlist

---

#### void SetSong([String](../static/String.md) song)
- **Description:** Set the music song

---

#### void DrawRay([Vector3](../objects/Vector3.md) start, [Vector3](../objects/Vector3.md) dir, [Color](../objects/Color.md) color, float duration)
- **Description:** Draw a ray

---

#### void ShowKillScore(int damage)
- **Description:** Show the kill score

---

#### void ShowKillFeed([String](../static/String.md) killer, [String](../static/String.md) victim, int score, [String](../static/String.md) weapon)
- **Description:** Show the kill feed

---

#### void ShowKillFeedAll([String](../static/String.md) killer, [String](../static/String.md) victim, int score, [String](../static/String.md) weapon)
- **Description:** Show the kill feed for all players
