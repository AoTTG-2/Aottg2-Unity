# Character
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../static/String.md)|False|Character's name.|
|Guild|[String](../static/String.md)|False|Character's guild.|
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
##### void GetKilled([String](../static/String.md) killer)
- **Description:** Kills the character. Callable by non-owners.
##### void GetDamaged([String](../static/String.md) killer, int damage)
- **Description:** Damages the character and kills it if its health reaches 0. Callable by non-owners.
##### void Emote([String](../static/String.md) emote)
- **Description:** Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
##### void PlayAnimation([String](../static/String.md) animation, float fade = 0.1)
- **Description:** Causes the character to play an animation.  If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.
##### void ForceAnimation([String](../static/String.md) animation, float fade = 0.1)
- **Description:** Forces the character to play an animation. If the fade parameter is provided, will crossfade the animation by this timestep. Available animations can be found here: Human, Titan, Annie, Eren. Use the right-hand string value for the animation.
##### float GetAnimationLength([String](../static/String.md) animation)
- **Description:** Gets the length of animation.
##### void PlaySound([String](../static/String.md) sound)
- **Description:** Plays a sound if present in the character. Available sound names can be found here: Humans, Shifters, Titans. Note that shifters also have all titan sounds.
##### void StopSound([String](../static/String.md) sound)
- **Description:** Stops the sound.
##### void LookAt([Vector3](../objects/Vector3.md) position)
- **Description:** Rotates the character such that it is looking towards a world position.
##### void AddForce([Vector3](../objects/Vector3.md) force, [String](../static/String.md) mode = Acceleration)
- **Description:** Adds a force to the character with given force vector and optional mode. Valid modes are Force, Acceleration, Impulse, VelocityChange with default being Acceleration.
##### void Reveal(float delay)
- **Description:** Reveaal the titan for a set number of seconds.
##### void AddOutline([Color](../objects/Color.md) color = null, [String](../static/String.md) mode = OutlineAll)
- **Description:** Adds an outline effect with the given color and mode. Valid modes are: OutlineAll, OutlineVisible, OutlineHidden, OutlineAndSilhouette, SilhouetteOnly, OutlineAndLightenColor
##### void RemoveOutline()
- **Description:** Removes the outline effect from the character.

---

