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
<td>Refill()</td>
<td>bool</td>
<td>Refills the gas of the human</td>
</tr>
<tr>
<td>RefillImmediate()</td>
<td>none</td>
<td>Refills the gas of the human immediately</td>
</tr>
<tr>
<td>ClearHooks()</td>
<td>none</td>
<td>Clears all hooks</td>
</tr>
<tr>
<td>ClearLeftHook()</td>
<td>none</td>
<td>Clears the left hook</td>
</tr>
<tr>
<td>ClearRightHook()</td>
<td>none</td>
<td>Clears the right hook</td>
</tr>
<tr>
<td>MountMapObject(mapObject : [MapObject](../objects/MapObject.md),positionOffset : [Vector3](../objects/Vector3.md),rotationOffset : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Mounts the human on a map object</td>
</tr>
<tr>
<td>MountTransform(transform : [Transform](../objects/Transform.md),positionOffset : [Vector3](../objects/Vector3.md),rotationOffset : [Vector3](../objects/Vector3.md))</td>
<td>none</td>
<td>Mounts the human on a transform</td>
</tr>
<tr>
<td>Unmount()</td>
<td>none</td>
<td>Unmounts the human</td>
</tr>
<tr>
<td>SetSpecial(special : [String](../static/String.md))</td>
<td>none</td>
<td>Sets the special of the human</td>
</tr>
<tr>
<td>ActivateSpecial()</td>
<td>none</td>
<td>Activates the special of the human</td>
</tr>
<tr>
<td>SetWeapon(weapon : [String](../static/String.md))</td>
<td>none</td>
<td>Sets the weapon of the human</td>
</tr>
<tr>
<td>DisablePerks()</td>
<td>none</td>
<td>Disables all perks of the human</td>
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
