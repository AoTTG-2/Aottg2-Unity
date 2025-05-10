# Shifter
Inherits from [Character](../objects/Character.md)

## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|string|False|Shifter's name.|
|Guild|string|False|Shifter's guild.|
|Size|float|False|Shifter's size.|
|RunSpeedBase|float|False|Shifter's base run speed. Final run speed is RunSpeedBase + Size * RunSpeedPerLevel.|
|WalkSpeedBase|float|False|Shifter's base walk speed. Final walk speed is WalkSpeedBase + Size * WalkSpeedPerLevel.|
|WalkSpeedPerLevel|float|False|Shifter's walk speed added per level.|
|RunSpeedPerLevel|float|False|Shifter's run speed added per level.|
|TurnSpeed|float|False|Shifter's turn speed when running turn animation.|
|RotateSpeed|float|False|Shifter's rotate speed when rotating body.|
|JumpForce|float|False|Shifter's jump force when jumping.|
|ActionPause|float|False|Shifter's pause delay after performing an action.|
|AttackPause|float|False|Shifter's pause delay after performing an attack.|
|TurnPause|float|False|Shifter's pause delay after performing a turn.|
|DetectRange|float|False|(AI) shifter's detect range.|
|FocusRange|float|False|(AI) shifter's focus range.|
|FocusTime|float|False|(AI) shifter's focus time before switching targets.|
|FarAttackCooldown|float|True|(AI) Shifter's cooldown after performing a ranged attack.|
|AttackWait|float|True|(AI) Shifter's wait time between being in range to attack and performing the attack.|
|AttackSpeedMultiplier|float|False|Shifter's attack animation speed.|
|UsePathfinding|bool|False|(AI) Shifter uses pathfinding.|
|NapePosition|[Vector3](../objects/Vector3.md)|True|The shifter's nape position.|
|DeathAnimLength|float|False|The length of the death animation.|
<details>
<summary>Derived Fields</summary>

|Field|Type|Readonly|Description|
|---|---|---|---|
|Player|[Player](../objects/Player.md)|True|Player who owns this character.|
|IsAI|bool|True|Is this character AI?|
|ViewID|int|True|Network view ID of the character.|
|IsMine|bool|True|Is this character mine?|
|IsMainCharacter|bool|True|Character belongs to my player and is the main character (the camera-followed player).|
|Transform|[Transform](../objects/Transform.md)|True|Unity transform of the character.|
|Position|[Vector3](../objects/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../objects/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../objects/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../objects/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../objects/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|True|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../objects/Vector3.md)|True|The character's target direction.|
|Team|string|False|Team character belongs to.|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|string|True|Character's current playing animation.|
|Grounded|bool|True|Character's grounded status.|
</details>

## Methods
###### function <mark style="color:yellow;">MoveTo</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, range: <mark style="color:blue;">float</mark>, ignoreEnemies: <mark style="color:blue;">bool</mark>)
> Causes the (AI) shifter to move towards a position. If ignoreEnemies is true, will not engage enemies along the way.

###### function <mark style="color:yellow;">Target</mark>(enemyObj: <mark style="color:blue;">Object</mark>, focus: <mark style="color:blue;">float</mark>)
> Causes the (AI) shifter to target an enemy character or MapTargetable for focusTime seconds. If focusTime is 0 it will use the default focus time.

###### function <mark style="color:yellow;">Idle</mark>(time: <mark style="color:blue;">float</mark>)
> Causes the (AI) shifter to idle for time seconds before beginning to wander. During idle the titan will not react or move at all.

###### function <mark style="color:yellow;">Wander</mark>()
> Causes the (AI) shifter to cancel any move commands and begin wandering randomly.

###### function <mark style="color:yellow;">Blind</mark>()
> Causes the shifter to enter the blind state.

###### function <mark style="color:yellow;">Cripple</mark>(time: <mark style="color:blue;">float</mark>)
> Causes the shifter to enter the cripple state.

###### function <mark style="color:yellow;">GetKilled</mark>(killer: <mark style="color:blue;">string</mark>)
> Kills the character. Callable by non-owners.

###### function <mark style="color:yellow;">GetDamaged</mark>(killer: <mark style="color:blue;">string</mark>, damage: <mark style="color:blue;">int</mark>)
> Damages the character and kills it if its health reaches 0. Callable by non-owners.

###### function <mark style="color:yellow;">Emote</mark>(emote: <mark style="color:blue;">string</mark>)
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.

###### function <mark style="color:yellow;">PlayAnimation</mark>(animation: <mark style="color:blue;">string</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>)
> Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

###### function <mark style="color:yellow;">ForceAnimation</mark>(animation: <mark style="color:blue;">string</mark>, fade: <mark style="color:blue;">float</mark> = <mark style="color:blue;">0.1</mark>)
> Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.

###### function <mark style="color:yellow;">GetAnimationLength</mark>(animation: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">float</mark>
> Gets the length of animation.

###### function <mark style="color:yellow;">PlaySound</mark>(sound: <mark style="color:blue;">string</mark>)
> Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.

###### function <mark style="color:yellow;">StopSound</mark>(sound: <mark style="color:blue;">string</mark>)
> Stops the sound.

###### function <mark style="color:yellow;">LookAt</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>)
> Rotates the character such that it is looking towards a world position.

###### function <mark style="color:yellow;">AddForce</mark>(force: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, mode: <mark style="color:blue;">string</mark> = <mark style="color:blue;">Acceleration</mark>)
> Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.

###### function <mark style="color:yellow;">Reveal</mark>(delay: <mark style="color:blue;">float</mark>)
> Reveal the titan for a set number of seconds.

###### function <mark style="color:yellow;">AddOutline</mark>(color: <mark style="color:blue;">[Color](../objects/Color.md)</mark> = <mark style="color:blue;">null</mark>, mode: <mark style="color:blue;">string</mark> = <mark style="color:blue;">OutlineAll</mark>)
> Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor

###### function <mark style="color:yellow;">RemoveOutline</mark>()
> Removes the outline effect from the character.


---

