# Titan
Inherits from [Character](../Object/Character.md)
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../Static/String.md)|False|Titans's name.|
|Guild|[String](../Static/String.md)|False|Titans's guild.|
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
|NapePosition|[Vector3](../Static/Vector3.md)|False|The titan's nape position.|
|IsCrawler|bool|False|Is titan a crawler.|
|DetectRange|float|False|(AI) titan's detect range.|
|FocusRange|float|False|(AI) titan's focus range.|
|FocusTime|float|False|(AI) titan's focus time.|
|FarAttackCooldown|float|False|(AI) Titan's cooldown after performing a ranged attack.|
|AttackWait|float|False|(AI) Titan's wait time between being in range and performing an attack.|
|CanRun|bool|False|(AI) Titan can run or only walk.|
|AttackSpeedMultiplier|float|False|Titan's attack animation speed.|
|UsePathfinding|bool|False|Determines whether the (AI) titan uses pathfinding. (Smart Movement in titan settings)|
|HeadMount|[Transform](../Object/Transform.md)|False|Titan's head transform.|
|NeckMount|[Transform](../Object/Transform.md)|False|Titan's neck transform.|
|Player|[Player](../Object/Player.md)|False|Player who owns this character.|
|IsAI|bool|False|Is this character AI?|
|ViewID|int|False|Network view ID of the character.|
|IsMine|bool|False|Is this character mine?|
|IsMainCharacter|bool|False||
|Transform|[Transform](../Object/Transform.md)|False|Unity transform of the character.|
|Position|[Vector3](../Static/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../Static/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../Static/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../Static/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../Static/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../Static/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../Static/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|False|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../Static/Vector3.md)|False|The character's target direction.|
|Team|[String](../Static/String.md)|False|Team character belongs to.|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|[String](../Static/String.md)|False|Character's current playing animation.|
|Grounded|bool|False|Character's grounded status.|
## Methods
|Function|Returns|Description|
|---|---|---|
|MoveTo(position : [Vector3](../Static/Vector3.md), range : float, ignoreEnemies : bool)|none|Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.|
|Target(enemyObj : Object, focus : float)|none|Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time|
|Idle(time : float)|none|Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.|
|Wander()|none|Causes the (AI) titan to cancel any move commands and begin wandering randomly.|
|Blind()|none|Causes the titan to enter the blind state.|
|Cripple(time : float)|none|Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.|
|GetKilled(killer : [String](../Static/String.md))|none|Kills the character. Callable by non-owners.|
|GetDamaged(killer : [String](../Static/String.md), damage : int)|none|Damages the character and kills it if its health reaches 0. Callable by non-owners.|
|Emote(emote : [String](../Static/String.md))|none|Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.|
|PlayAnimation(animation : [String](../Static/String.md), fade : float = 0.1)|none|Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.|
|GetAnimationLength(animation : [String](../Static/String.md))|float|Gets the length of animation.|
|PlaySound(sound : [String](../Static/String.md))|none|Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.|
|StopSound(sound : [String](../Static/String.md))|none|Stops the sound.|
|LookAt(position : [Vector3](../Static/Vector3.md))|none|Rotates the character such that it is looking towards a world position.|
|AddForce(force : [Vector3](../Static/Vector3.md), mode : [String](../Static/String.md) = Acceleration)|none|Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.|
|Reveal(delay : float)|none|Reveaal the titan for a set number of seconds.|
|AddOutline(color : [Color](../Static/Color.md) = , mode : [String](../Static/String.md) = OutlineAll)|none|Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor|
|RemoveOutline()|none|Removes the outline effect from the character.|
