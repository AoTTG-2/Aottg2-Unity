# Character
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../Static/String.md)|False|Character's name.|
|Guild|[String](../Static/String.md)|False|Character's guild.|
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
