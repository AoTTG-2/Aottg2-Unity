# Titan
Inherits from [Character](../objects/Character.md)
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../static/String.md)|False|Titans's name.|
|Guild|[String](../static/String.md)|False|Titans's guild.|
|Size|float|False|Titan's size.|
|RunSpeedBase|float|False|Titan's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.|
|WalkSpeedBase|float|False|Titan's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.|
|WalkSpeedPerLevel|float|False|Titan's walk speed added per size.|
|RunSpeedPerLevel|float|False|Titan's run speed added per size.|
|TurnSpeed|float|False|Titan's turn animation speed.|
|RotateSpeed|float|False|Titan's rotate speed.|
|JumpForce|float|False|Titan's jump force.|
|ActionPause|float|False|Titan's pause delay after performing an action.|
|AttackPause|float|False|Titan's pause delay after performing an attack.|
|TurnPause|float|False|Titan's pause delay after performing a turn.|
|Stamina|float|False|PT stamina.|
|MaxStamina|float|False|PT max stamina.|
|NapePosition|[Vector3](../objects/Vector3.md)|False|The titan's nape position.|
|IsCrawler|bool|False|Is titan a crawler.|
|DetectRange|float|False|(AI) titan's detect range.|
|FocusRange|float|False|(AI) titan's focus range.|
|FocusTime|float|False|(AI) titan's focus time.|
|FarAttackCooldown|float|False|(AI) Titan's cooldown after performing a ranged attack.|
|AttackWait|float|False|(AI) Titan's wait time between being in range and performing an attack.|
|CanRun|bool|False|(AI) Titan can run or only walk.|
|AttackSpeedMultiplier|float|False|Titan's attack animation speed.|
|UsePathfinding|bool|False|Determines whether the (AI) titan uses pathfinding. (Smart Movement in titan settings)|
|HeadMount|[Transform](../objects/Transform.md)|False|Titan's head transform.|
|NeckMount|[Transform](../objects/Transform.md)|False|Titan's neck transform.|
|Player|[Player](../objects/Player.md)|False|Player who owns this character.|
|IsAI|bool|False|Is this character AI?|
|ViewID|int|False|Network view ID of the character.|
|IsMine|bool|False|Is this character mine?|
|IsMainCharacter|bool|False||
|Transform|[Transform](../objects/Transform.md)|False|Unity transform of the character.|
|Position|[Vector3](../objects/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../objects/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../objects/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../objects/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|False|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../objects/Vector3.md)|False|The character's target direction.|
|Team|[String](../static/String.md)|False|Team character belongs to.|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|[String](../static/String.md)|False|Character's current playing animation.|
|Grounded|bool|False|Character's grounded status.|
## Methods
#### void MoveTo([Vector3](../objects/Vector3.md) position, float range, bool ignoreEnemies)
- **Description:** Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.

---

#### void Target(Object enemyObj, float focus)
- **Description:** Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time

---

#### void Idle(float time)
- **Description:** Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.

---

#### void Wander()
- **Description:** Causes the (AI) titan to cancel any move commands and begin wandering randomly.

---

#### void Blind()
- **Description:** Causes the titan to enter the blind state.

---

#### void Cripple(float time)
- **Description:** Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.

---

#### void GetKilled([String](../static/String.md) killer)
- **Description:** Kills the character. Callable by non-owners.

---

#### void GetDamaged([String](../static/String.md) killer, int damage)
- **Description:** Damages the character and kills it if its health reaches 0. Callable by non-owners.

---

#### void Emote([String](../static/String.md) emote)
- **Description:** Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.

---

#### void PlayAnimation([String](../static/String.md) animation, float fade = 0.1)
- **Description:** Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

---

#### float GetAnimationLength([String](../static/String.md) animation)
- **Description:** Gets the length of animation.

---

#### void PlaySound([String](../static/String.md) sound)
- **Description:** Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.

---

#### void StopSound([String](../static/String.md) sound)
- **Description:** Stops the sound.

---

#### void LookAt([Vector3](../objects/Vector3.md) position)
- **Description:** Rotates the character such that it is looking towards a world position.

---

#### void AddForce([Vector3](../objects/Vector3.md) force, [String](../static/String.md) mode = Acceleration)
- **Description:** Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.

---

#### void Reveal(float delay)
- **Description:** Reveaal the titan for a set number of seconds.

---

#### void AddOutline([Color](../objects/Color.md) color = null, [String](../static/String.md) mode = OutlineAll)
- **Description:** Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor

---

#### void RemoveOutline()
- **Description:** Removes the outline effect from the character.
