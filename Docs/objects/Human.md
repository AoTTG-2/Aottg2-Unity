# Human
Inherits from [Character](../objects/Character.md)
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Name|[String](../static/String.md)|False|The human's name|
|Guild|[String](../static/String.md)|False|The human's guild|
|Weapon|[String](../static/String.md)|False|The weapon the human is using|
|CurrentSpecial|[String](../static/String.md)|False|The current special the human is using|
|SpecialCooldown|float|False|The cooldown of the special|
|ShifterLiveTime|float|False|The live time of the shifter special|
|SpecialCooldownRatio|float|False|The ratio of the special cooldown|
|CurrentGas|float|False|The current gas of the human|
|MaxGas|float|False|The max gas of the human|
|Acceleration|int|False|The acceleration of the human|
|Speed|int|False|The speed of the human|
|HorseSpeed|float|False|The speed of the horse|
|CurrentBladeDurability|float|False|The current blade durability|
|MaxBladeDurability|float|False|The max blade durability|
|CurrentBlade|int|False|The current blade|
|MaxBlade|int|False|The max number of blades held|
|CurrentAmmoRound|int|False|The current ammo round|
|MaxAmmoRound|int|False|The max ammo round|
|CurrentAmmoLeft|int|False|The current ammo left|
|MaxAmmoTotal|int|False|The max total ammo|
|LeftHookEnabled|bool|False|Whether the left hook is enabled|
|RightHookEnabled|bool|False|Whether the right hook is enabled|
|IsMounted|bool|False|Whether the human is mounted|
|MountedMapObject|[MapObject](../objects/MapObject.md)|False|The map object the human is mounted on|
|MountedTransform|[Transform](../objects/Transform.md)|False|The transform the human is mounted on|
|AutoRefillGas|bool|False|Whether the human auto refills gas|
|State|[String](../static/String.md)|False|The state of the human|
|CanDodge|bool|False|Whether the human can dodge|
|IsInvincible|bool|False|Whether the human is invincible|
|InvincibleTimeLeft|float|False|The time left for invincibility|
|IsCarried|bool|False|If the human is carried.|
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
##### bool Refill()
- **Description:** Refills the gas of the human
##### void RefillImmediate()
- **Description:** Refills the gas of the human immediately
##### void ClearHooks()
- **Description:** Clears all hooks
##### void ClearLeftHook()
- **Description:** Clears the left hook
##### void ClearRightHook()
- **Description:** Clears the right hook
##### void MountMapObject([MapObject](../objects/MapObject.md) mapObject, [Vector3](../objects/Vector3.md) positionOffset, [Vector3](../objects/Vector3.md) rotationOffset)
- **Description:** Mounts the human on a map object
##### void MountTransform([Transform](../objects/Transform.md) transform, [Vector3](../objects/Vector3.md) positionOffset, [Vector3](../objects/Vector3.md) rotationOffset)
- **Description:** Mounts the human on a transform
##### void Unmount()
- **Description:** Unmounts the human
##### void SetSpecial([String](../static/String.md) special)
- **Description:** Sets the special of the human
##### void ActivateSpecial()
- **Description:** Activates the special of the human
##### void SetWeapon([String](../static/String.md) weapon)
- **Description:** Sets the weapon of the human
##### void DisablePerks()
- **Description:** Disables all perks of the human
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

