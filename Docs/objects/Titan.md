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
<td>MoveTo(position : [Vector3](../objects/Vector3.md),range : float,ignoreEnemies : bool)</td>
<td>none</td>
<td>Causes the (AI) titan to move towards a position and stopping when within specified range. If ignoreEnemies is true, will not engage enemies along the way.</td>
</tr>
<tr>
<td>Target(enemyObj : Object,focus : float)</td>
<td>none</td>
<td>Causes the (AI) titan to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time</td>
</tr>
<tr>
<td>Idle(time : float)</td>
<td>none</td>
<td>Causes the (AI) titan to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.</td>
</tr>
<tr>
<td>Wander()</td>
<td>none</td>
<td>Causes the (AI) titan to cancel any move commands and begin wandering randomly.</td>
</tr>
<tr>
<td>Blind()</td>
<td>none</td>
<td>Causes the titan to enter the blind state.</td>
</tr>
<tr>
<td>Cripple(time : float)</td>
<td>none</td>
<td>Causes the titan to enter the cripple state for time seconds. Using 0 will use the default cripple time.</td>
</tr>
<tr>
<td>GetKilled(killer : [String](../static/String.md))</td>
<td>none</td>
<td>Kills the character. Callable by non-owners.</td>
</tr>
<tr>
<td>GetDamaged(killer : [String](../static/String.md),damage : int)</td>
<td>none</td>
<td>Damages the character and kills it if its health reaches 0. Callable by non-owners.</td>
</tr>
<tr>
<td>Emote(emote : [String](../static/String.md))</td>
<td>none</td>
<td>Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.</td>
</tr>
<tr>
<td>PlayAnimation(animation : [String](../static/String.md),fade : float = 0.1)</td>
<td>none</td>
<td>Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.</td>
</tr>
<tr>
<td>GetAnimationLength(animation : [String](../static/String.md))</td>
<td>float</td>
<td>Gets the length of animation.</td>
</tr>
<tr>
<td>PlaySound(sound : [String](../static/String.md))</td>
<td>none</td>
<td>Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.</td>
</tr>
<tr>
<td>StopSound(sound : [String](../static/String.md))</td>
<td>none</td>
<td>Stops the sound.</td>
</tr>
<tr>
<td>LookAt(position : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Rotates the character such that it is looking towards a world position.</td>
</tr>
<tr>
<td>AddForce(force : [Vector3](../objects/Vector3.md),mode : [String](../static/String.md) = Acceleration)</td>
<td>none</td>
<td>Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.</td>
</tr>
<tr>
<td>Reveal(delay : float)</td>
<td>none</td>
<td>Reveaal the titan for a set number of seconds.</td>
</tr>
<tr>
<td>AddOutline(color : [Color](../objects/Color.md) = ,mode : [String](../static/String.md) = OutlineAll)</td>
<td>none</td>
<td>Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor</td>
</tr>
<tr>
<td>RemoveOutline()</td>
<td>none</td>
<td>Removes the outline effect from the character.</td>
</tr>
</tbody>
</table>
